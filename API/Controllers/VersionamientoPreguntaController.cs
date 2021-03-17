using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models.Catalogos;
using API.Models.Metodos;
using API.Models.Entidades;

namespace API.Controllers
{
    [Authorize]
    public class VersionamientoPreguntaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoVersionamientoPregunta _objCatalogoVersionamientoPregunta = new CatalogoVersionamientoPregunta();
        CatalogoCabeceraVersionCuestionario _objCatalogoCabeceraVersionCuestionario = new CatalogoCabeceraVersionCuestionario();
        CatalogoAsignarComponenteGenerico _objCatalogoAsignarComponenteGenerico = new CatalogoAsignarComponenteGenerico();
        CatalogoAsignarResponsableModeloPublicado _objCatalogoAsignarResponsableModeloPublicado = new CatalogoAsignarResponsableModeloPublicado();
        CatalogoComunidad _objCatalogoComunidad = new CatalogoComunidad();
        CatalogoRespuesta _objCatalogoRespuesta = new CatalogoRespuesta();
        CatalogoAsignarEncuestado _objCatalogoAsignarEncuestado = new CatalogoAsignarEncuestado();
        CatalogoCabeceraRespuesta _objCatalogoCabeceraRespuesta = new CatalogoCabeceraRespuesta();
        CatalogoOpcionPreguntaSeleccion _objCatalogoOpcionPreguntaSeleccion = new CatalogoOpcionPreguntaSeleccion();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/versionamientopregunta_consultarporcabeceraversion")]
        public object versionamientopregunta_consultarporcabeceraversion(string _idCabeceraVersionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCabeceraVersionEncriptado == null || string.IsNullOrEmpty(_idCabeceraVersionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la cabecera versión";
                }
                else
                {
                    int _idCabeceraVersion = Convert.ToInt32(_seguridad.DesEncriptar(_idCabeceraVersionEncriptado));
                    var _objCabeceraVersion = _objCatalogoCabeceraVersionCuestionario.ConsultarCabeceraVersionCuestionarioPorId(_idCabeceraVersion).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCabeceraVersion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto cabecera versión";
                    }
                    else
                    {
                        var _listaVersionamiento = _objCatalogoVersionamientoPregunta.ConsultarVersionamientoPreguntaCompletoPorIdCabeceraVersion(_idCabeceraVersion).ToList();
                        foreach (var _objVersionamiento in _listaVersionamiento)
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
                        _respuesta = _listaVersionamiento;
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
        [HttpPost]
        [Route("api/versionamientopregunta_consultarporidasignarcomponentegenericoporidasignarresponsablemodelpublicado")]
        public object versionamientopregunta_consultarporidasignarcomponentegenericoporidasignarresponsablemodelpublicado(string _idAsignarComponenteGenericoEncriptado, string _idAsignarResponsableModeloPublicadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarComponenteGenericoEncriptado == null || string.IsNullOrEmpty(_idAsignarComponenteGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar componente genérico";
                }
                else if (_idAsignarResponsableModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_idAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar responsable modelo publicado";
                }
                else
                {
                    int _idAsignarComponenteGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarComponenteGenericoEncriptado));
                    var _objAsignarComponenteGenerico = _objCatalogoAsignarComponenteGenerico.ConsultarAsignarComponenteGenericoPorIdDHBD(_idAsignarComponenteGenerico).FirstOrDefault();
                    if (_objAsignarComponenteGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar componente genérico";
                    }
                    else
                    {
                        int _idAsignarResponsableModeloPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarResponsableModeloPublicadoEncriptado));
                        var _objAsignarResponsableModeloPublicado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsableModeloPublicado).FirstOrDefault();
                        if (_objAsignarResponsableModeloPublicado == null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró el objeto asignar responsable del modelo publicado";
                        }
                        else
                        {
                            var _listaComunidades = _objCatalogoComunidad.ConsultarComunidadPorIdParroquia(_objAsignarResponsableModeloPublicado.Parroquia.IdParroquia.Value).Where(c => c.EstadoComunidad == true).ToList();
                            int _idCuestionarioPublicado = _objAsignarComponenteGenerico.AsignarCuestionarioModelo.CuestionarioPublicado.IdCuestionarioPublicado;
                            var _listaAsignarEncuestadoPorCuestionarioPublicado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadPorIdCuestionarioPublicado(_idCuestionarioPublicado).Where(c => c.Estado == true).ToList();
                            List<Respuesta> _listaRespuestasTodosCuestionariosComunidades = new List<Respuesta>();
                            foreach (var itemComunidad in _listaComunidades)
                            {
                                var _objAsignarEncuestadoPorComunidad = _listaAsignarEncuestadoPorCuestionarioPublicado.Where(c => c.Comunidad.IdComunidad == itemComunidad.IdComunidad).FirstOrDefault();
                                if (_objAsignarEncuestadoPorComunidad != null)
                                {
                                    var _objCabeceraRespuesta = _objCatalogoCabeceraRespuesta.ConsultarCabeceraRespuestaPorIdAsignarEncuestado(_objAsignarEncuestadoPorComunidad.IdAsignarEncuestado).Where(c => c.Finalizado == true && c.Estado == true).FirstOrDefault();
                                    if (_objCabeceraRespuesta != null)
                                    {
                                        var _listaRespuestasPorCabeceraPorComunidad = _objCatalogoRespuesta.ConsultarRespuestaPorIdCabeceraRespuesta(_objCabeceraRespuesta.IdCabeceraRespuesta).ToList();
                                        _listaRespuestasTodosCuestionariosComunidades.AddRange(_listaRespuestasPorCabeceraPorComunidad);
                                    }
                                }
                            }
                            var _listaOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccion().Where(c => c.Estado == true).ToList();
                            var _listaVersionamiento = _objCatalogoVersionamientoPregunta.ConsultarVersionamientoPreguntaCompletoPorIdCabeceraVersionPorIdComponente(_objAsignarComponenteGenerico.AsignarCuestionarioModelo.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario, _objAsignarComponenteGenerico.Componente.IdComponente).ToList();
                            _listaVersionamiento = _listaVersionamiento.Where(c => c.Pregunta.TipoPregunta.Identificador != 1).ToList();
                            foreach (var itemVersionamiento in _listaVersionamiento)
                            {
                                List<RespuestasCaracterizacion> _listaRespuestasCaracterizacion = new List<RespuestasCaracterizacion>();
                                if (itemVersionamiento.Pregunta.TipoPregunta.Identificador == 2 || itemVersionamiento.Pregunta.TipoPregunta.Identificador == 3)
                                {
                                    var _listasRespuestasPorPregunta = _listaRespuestasTodosCuestionariosComunidades.Where(c => c.Pregunta.IdPregunta == itemVersionamiento.Pregunta.IdPregunta).ToList();
                                    var _listaOpcionPreguntaSeleccionPorPregunta = _listaOpcionPreguntaSeleccion.Where(c => c.Pregunta.IdPregunta == itemVersionamiento.Pregunta.IdPregunta).ToList();
                                    foreach (var itemOpcionPreguntaSeleccion in _listaOpcionPreguntaSeleccionPorPregunta)
                                    {
                                        var _totalRespuestasPorOpcion = _listasRespuestasPorPregunta.Where(c => c.IdRespuestaLogica == itemOpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion).ToList().Count;
                                        _listaRespuestasCaracterizacion.Add(new RespuestasCaracterizacion()
                                        {
                                            IdRespuestasCaracterizacion = itemOpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion,
                                            IdRespuestasCaracterizacionEncriptado = _seguridad.Encriptar(itemOpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion.ToString()),
                                            Descripcion = itemOpcionPreguntaSeleccion.Descripcion,
                                            Total = _totalRespuestasPorOpcion
                                        });
                                    }
                                    itemVersionamiento.Pregunta.ListaRespuestasCaracterizacion = _listaRespuestasCaracterizacion;
                                }
                            }

                            foreach (var _objVersionamiento in _listaVersionamiento)
                            {
                                _objVersionamiento.IdVersionamientoPregunta = 0;
                                _objVersionamiento.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;
                                _objVersionamiento.Pregunta.IdPregunta = 0;
                                _objVersionamiento.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                                _objVersionamiento.Pregunta.Seccion.IdSeccion = 0;
                                _objVersionamiento.Pregunta.Seccion.Componente.IdComponente = 0;
                            }
                            _respuesta = _listaVersionamiento;
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
        [Route("api/ConsultarPreguntasPorCuestionarioPublicadoComponente")]
        public object ConsultarPreguntasPorCuestionarioPublicadoComponente(AsignarCuestionarioModelo _AsignarCuestionarioModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_AsignarCuestionarioModelo.IdCuestionarioPublicado == null || string.IsNullOrEmpty(_AsignarCuestionarioModelo.IdCuestionarioPublicado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario publicado";
                }
                else if (_AsignarCuestionarioModelo.AsignarComponenteGenerico[0].IdComponente == null || string.IsNullOrEmpty(_AsignarCuestionarioModelo.AsignarComponenteGenerico[0].IdComponente))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente";
                }
                else
                {
                    _AsignarCuestionarioModelo.IdCuestionarioPublicado = _seguridad.DesEncriptar(_AsignarCuestionarioModelo.IdCuestionarioPublicado);
                    _AsignarCuestionarioModelo.AsignarComponenteGenerico[0].IdComponente = _seguridad.DesEncriptar(_AsignarCuestionarioModelo.AsignarComponenteGenerico[0].IdComponente);
                    var _listaVersionamiento = _objCatalogoVersionamientoPregunta.ConsultarPreguntasPorCuestionarioPublicadoComponente(_AsignarCuestionarioModelo).ToList();
                    for (int i = 0; i < _listaVersionamiento.Count; i++)
                    {
                        _listaVersionamiento[i].IdVersionamientoPregunta = 0;
                        _listaVersionamiento[i].CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        //_listaVersionamiento[i].CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _listaVersionamiento[i].Pregunta.IdPregunta = 0;
                        _listaVersionamiento[i].Pregunta.TipoPregunta.IdTipoPregunta = 0;
                        _listaVersionamiento[i].Pregunta.Seccion.IdSeccion = 0;
                        _listaVersionamiento[i].Pregunta.Seccion.Componente.IdComponente = 0;
                    }
                    _respuesta = _listaVersionamiento;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
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
