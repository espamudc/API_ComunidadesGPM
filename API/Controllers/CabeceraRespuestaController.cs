using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models.Catalogos;
using API.Models.Metodos;
using API.Models.Entidades;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [Authorize]
    public class CabeceraRespuestaController : ApiController
    {
        Seguridad _seguridad = new Seguridad();
        CatalogoCabeceraRespuesta _objCatalogoCabeceraRespuesta = new CatalogoCabeceraRespuesta();
        CatalogoRespuesta _objCatalogoRespuestas = new CatalogoRespuesta();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        CatalogoVersionamientoPregunta _objCatalogoVersionamientoPregunta = new CatalogoVersionamientoPregunta();
        CatalogoAsignarEncuestado _objCatalogoAsignarEncuestado = new CatalogoAsignarEncuestado();
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoOpcionPreguntaSeleccion _objCatalogoOpcionPreguntaSeleccion = new CatalogoOpcionPreguntaSeleccion();
        CatalogoPreguntaEncajonada _objCatalogoPreguntaEncajonada = new CatalogoPreguntaEncajonada();


        [HttpPost]
        [Route("api/cabecerarespuesta_validarfinalizar")]
        public object cabecerarespuesta_validarfinalizar(string _idCabeceraRespuestaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idCabeceraRespuestaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la cabecera respuesta";
                }
                else
                {

                    int _idCabeceraRespuesta = int.Parse(_seguridad.DesEncriptar(_idCabeceraRespuestaEncriptado));
                    var _objCabeceraRespuesta = _objCatalogoCabeceraRespuesta.ConsultarCabeceraRespuestaPorId(_idCabeceraRespuesta).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCabeceraRespuesta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto cabecera respuesta";
                    }
                    else
                    {
                        int _idCabeceraVersionCuestionario = _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario;
                        var _listaTodasPreguntasVersionamiento = _objCatalogoVersionamientoPregunta.ConsultarVersionamientoPreguntaCompletoPorIdCabeceraVersion(_idCabeceraVersionCuestionario).ToList();
                        var _listaPreguntasObligatorias = _listaTodasPreguntasVersionamiento.Where(c => c.Pregunta.Obligatorio == true).ToList();
                        var _listaRespuestas = _objCatalogoRespuestas.ConsultarRespuestaPorIdCabeceraRespuesta(_idCabeceraRespuesta).Where(c => c.Estado == true).ToList();

                        if (_listaPreguntasObligatorias.Count == 0)
                        {
                            if (_listaRespuestas.Count == 0)
                            {
                                _respuesta = QuitarIdentificadoresListaVersionamiento(_listaTodasPreguntasVersionamiento);
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "202").FirstOrDefault();
                                _http.mensaje = "Todas las preguntas NO obligatorias de la ficha están sin responder. ¿Está seguro que desea finalizar el cuestionario?";
                            }
                        }
                        else
                        {
                            string _idAsignarEncuestadoEncriptado = _objCabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestadoEncriptado;
                            string _idCabeceraVersionCuestionarioEncriptado = _objCabeceraRespuesta.IdCabeceraRespuestaEncriptado;
                            var _listaOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccion().Where(c => c.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico == _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico).ToList();
                            var _listaRespuestasPreguntasAbiertas = _listaRespuestas.Where(c => c.Pregunta.TipoPregunta.Identificador == 1).ToList();
                            var _listaRespuestasSeleccion = _listaRespuestas.Where(c => c.Pregunta.TipoPregunta.Identificador == 2 || c.Pregunta.TipoPregunta.Identificador == 3).ToList();
                            var _listaRespuestasMatrizSeleccion = _listaRespuestas.Where(c => c.Pregunta.TipoPregunta.Identificador == 4).ToList();
                            List<VersionamientoPregunta> _listaPreguntasNoRespondidas = new List<VersionamientoPregunta>();
                            foreach (var itemPregunta in _listaTodasPreguntasVersionamiento)
                            {
                                if (itemPregunta.Pregunta.TipoPregunta.Identificador == 1)
                                {
                                    var _preguntaRespondida = _listaRespuestasPreguntasAbiertas.Where(c => c.Pregunta.IdPregunta == itemPregunta.Pregunta.IdPregunta).FirstOrDefault();
                                    if (_preguntaRespondida == null)
                                    {
                                        _listaPreguntasNoRespondidas.Add(itemPregunta);
                                    }
                                }
                                else if (itemPregunta.Pregunta.TipoPregunta.Identificador == 2 || itemPregunta.Pregunta.TipoPregunta.Identificador == 3)
                                {
                                    var _preguntaRespondida = _listaRespuestasSeleccion.Where(c => c.Pregunta.IdPregunta == itemPregunta.Pregunta.IdPregunta).ToList();
                                    if (_preguntaRespondida.Count == 0)
                                    {
                                        _listaPreguntasNoRespondidas.Add(itemPregunta);
                                    }
                                    else
                                    {
                                        if (itemPregunta.Pregunta.TipoPregunta.Identificador == 2)
                                        {
                                            var _listaOpcionPreguntaSeleccionPorPregunta = _listaOpcionPreguntaSeleccion.Where(c => c.Pregunta.IdPregunta == itemPregunta.Pregunta.IdPregunta).ToList();
                                            foreach (var itemOpcionPreguntaSeleccion in _listaOpcionPreguntaSeleccionPorPregunta)
                                            {
                                                var _listaRespuestaPorOpcionPreguntaSeleccion = _listaRespuestasSeleccion.Where(c => c.IdRespuestaLogica == itemOpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion).ToList();
                                                string _idOpcionPreguntaSeleccionEncriptado2 = _seguridad.Encriptar(itemOpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion.ToString());
                                                if (_listaRespuestaPorOpcionPreguntaSeleccion.Count > 0)
                                                {
                                                    var _listaPreguntasEncajonadas = _objCatalogoPreguntaEncajonada.ConsultarPreguntaEncajonadaPorIdOpcionSeleccionUnica(itemOpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion).ToList();
                                                    List<Pregunta> _listaPreguntas = new List<Pregunta>();
                                                    var _listaPreguntasNoRespondidasPorOpcion = PreguntasEncajonadas(_listaPreguntas, _listaPreguntasEncajonadas, _listaOpcionPreguntaSeleccion, _listaRespuestas);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        public List<Pregunta> PreguntasEncajonadas(List<Pregunta> _listaPreguntasSinResponder, List<PreguntaEncajonada> _listaPreguntasEncajonadas, List<OpcionPreguntaSeleccion> _listaOpcionesCuestionario, List<Respuesta> _listaRespuestasEncuestado)
        {
            foreach (var itemPreguntasEncajonadas in _listaPreguntasEncajonadas)
            {
                bool _validarPreguntaRespondida = false;
                var _opcionesPreguntaActual = _listaOpcionesCuestionario.Where(c => c.Pregunta.IdPregunta == itemPreguntasEncajonadas.Pregunta.IdPregunta).ToList();
                foreach (var itemOpcion in _opcionesPreguntaActual)
                {
                    var _opcionSeleccionadaPorUsuario = _listaRespuestasEncuestado.Where(c => c.IdRespuestaLogica == itemOpcion.IdOpcionPreguntaSeleccion).FirstOrDefault();
                    if (_opcionSeleccionadaPorUsuario != null)
                    {
                        _validarPreguntaRespondida = true;
                        var _listaPreguntasEncajonadasPorOpcionSeleccionada = _objCatalogoPreguntaEncajonada.ConsultarPreguntaEncajonadaPorIdOpcionSeleccionUnica(itemOpcion.IdOpcionPreguntaSeleccion).ToList();
                        if (_listaPreguntasEncajonadasPorOpcionSeleccionada.Count > 0)
                           PreguntasEncajonadas(_listaPreguntasSinResponder,_listaPreguntasEncajonadasPorOpcionSeleccionada, _listaOpcionesCuestionario, _listaRespuestasEncuestado);
                    }
                }
                if (_validarPreguntaRespondida == false)
                    _listaPreguntasSinResponder.Add(itemPreguntasEncajonadas.Pregunta);
            }
            return _listaPreguntasSinResponder;
        }

        public List<VersionamientoPregunta> QuitarIdentificadoresListaVersionamiento(List<VersionamientoPregunta> _lista)
        {
            foreach (var _objVersionamiento in _lista)
            {
                _objVersionamiento.IdVersionamientoPregunta = 0;
                _objVersionamiento.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                _objVersionamiento.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                _objVersionamiento.Pregunta.IdPregunta = 0;
                _objVersionamiento.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                _objVersionamiento.Pregunta.Seccion.IdSeccion = 0;
                _objVersionamiento.Pregunta.Seccion.Componente.IdComponente = 0;
            }
            return _lista;
        }



        [HttpPost]
        [Route("api/cabecerarespuesta_insertar")]
        public object cabecerarespuesta_insertar(CabeceraRespuesta _objCabeceraRespuesta)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if(_objCabeceraRespuesta==null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto cabecera respuesta";
                }
                else if(_objCabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestadoEncriptado==null || string.IsNullOrEmpty(_objCabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar encuestado";
                }
                else
                {
                    int _idAsignarEncuestado = Convert.ToInt32(_seguridad.DesEncriptar(_objCabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestadoEncriptado));
                    var _objAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(_idAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objAsignarEncuestado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar encuestado";
                    }
                    else
                    {
                        _objCabeceraRespuesta.FechaRegistro = DateTime.Now;
                        _objCabeceraRespuesta.Estado = true;
                        _objCabeceraRespuesta.FechaFinalizado = null;
                        _objCabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestado = _idAsignarEncuestado;
                        int _idCabeceraRespuesta = _objCatalogoCabeceraRespuesta.InsertarCabeceraRespuesta(_objCabeceraRespuesta);
                        if(_idCabeceraRespuesta==0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de ingresar la cabecera respuesta";
                        }
                        else
                        {
                            _objCabeceraRespuesta = _objCatalogoCabeceraRespuesta.ConsultarCabeceraRespuestaPorId(_idCabeceraRespuesta).FirstOrDefault();
                            _objCabeceraRespuesta.IdCabeceraRespuesta = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.IdComunidad = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.IdComunidad = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _respuesta = _objCabeceraRespuesta;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }
                }           
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }



        [HttpPost]
        [Route("api/cabecerarespuesta_consultarporidasignarencuestado")]
        public object cabecerarespuesta_consultarporidasignarencuestado(string _idAsignarEncuestadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarEncuestadoEncriptado == null || string.IsNullOrEmpty(_idAsignarEncuestadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar encuestado";
                }
                else
                {
                    int _idAsignarEncuestado = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarEncuestadoEncriptado));
                    var _objAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(_idAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objAsignarEncuestado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar encuestado";
                    }
                    else
                    {
                        var _objCabeceraRespuesta = _objCatalogoCabeceraRespuesta.ConsultarCabeceraRespuestaPorIdAsignarEncuestado(_idAsignarEncuestado).FirstOrDefault();
                            _objCabeceraRespuesta.IdCabeceraRespuesta = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.IdComunidad = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.IdComunidad = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _respuesta = _objCabeceraRespuesta;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
    }
}
