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
    public class RespuestaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarEncuestado _objCatalogoAsignarEncuestado = new CatalogoAsignarEncuestado();
        CatalogoCabeceraRespuesta _objCatalogoCabeceraRespuesta = new CatalogoCabeceraRespuesta();
        CatalogoRespuesta _objCatalogoRespuestas = new CatalogoRespuesta();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        CatalogoOpcionPreguntaSeleccion _objCatalogoOpcionPreguntaSeleccion = new CatalogoOpcionPreguntaSeleccion();
        CatalogoPreguntaEncajonada _objCatalogoPreguntaEncajonada = new CatalogoPreguntaEncajonada();
        CatalogoPreguntaAbierta _objCatalogoPreguntaAbierta = new CatalogoPreguntaAbierta();
        CatalogoConfigurarMatriz _objCatalogoConfigurarMatriz = new CatalogoConfigurarMatriz();
        Seguridad _seguridad = new Seguridad();



        [HttpPost]
        [Route("api/respuesta_consultarporidcabecerarespuesta")]
        public object respuesta_consultarporidcabecerarespuesta(string _idCabeceraRespuestaEncriptado)
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
                        var _listaRespuestas = _objCatalogoRespuestas.ConsultarRespuestaPorIdCabeceraRespuesta(_idCabeceraRespuesta).Where(c => c.Estado == true).ToList();
                        foreach (var _objRespuesta in _listaRespuestas)
                        {
                            _objRespuesta.IdRespuesta = 0;
                            _objRespuesta.IdRespuestaLogica = 0;
                            _objRespuesta.Pregunta.IdPregunta = 0;
                            _objRespuesta.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objRespuesta.CabeceraRespuesta.IdCabeceraRespuesta = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestado = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.Comunidad.IdComunidad = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                            _objRespuesta.CabeceraRespuesta.AsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _listaRespuestas;
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
        [Route("api/respuesta_insertarconfigurarmatriz")]
        public object respuesta_insertarconfigurarmatriz(Respuesta _objRespuesta)
        {

            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                DateTime _fechaActual = DateTime.Now;

                if (_objRespuesta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto respuesta";
                }
                else if (_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la cabecera respuesta";
                }
                else if (_objRespuesta.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objRespuesta.IdRespuestaLogicaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.IdRespuestaLogicaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la respuesta lógica";
                }
                else
                {
                    int _idPregunta = int.Parse(_seguridad.DesEncriptar(_objRespuesta.Pregunta.IdPreguntaEncriptado));
                    int _idCabeceraRespuesta = int.Parse(_seguridad.DesEncriptar(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado));
                    _objRespuesta.IdRespuestaLogica = int.Parse(_seguridad.DesEncriptar(_objRespuesta.IdRespuestaLogicaEncriptado));
                    int _idConfigurarMatriz = _objRespuesta.IdRespuestaLogica;
                    var _listaConfigurarMatrizPorCuestionarioGenericoPregunta = _objCatalogoConfigurarMatriz.ConsultarConfigurarMatrizPorIdPregunta(_idPregunta).Where(c => c.Estado == true).ToList();
                    var _listaConfigurarMatriz = _listaConfigurarMatrizPorCuestionarioGenericoPregunta.Where(c => c.IdConfigurarMatriz == _idConfigurarMatriz).ToList();
                    if (_listaConfigurarMatriz.Count == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La opción seleccionada no pertenece a la pregunta ni al cuestionario";
                    }
                    else
                    {
                        var _objCabeceraRespuesta = _objCatalogoCabeceraRespuesta.ConsultarCabeceraRespuestaPorId(_idCabeceraRespuesta).FirstOrDefault();
                        if (_objCabeceraRespuesta == null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró la cabecera de la respuesta.";
                        }
                        else if (_objCabeceraRespuesta.Finalizado == true)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La ficha ya fue respondida por el usuario";
                        }
                        else
                        {
                            int _idOpcionUnoMatriz = _listaConfigurarMatriz.FirstOrDefault().OpcionUnoMatriz.IdOpcionUnoMatriz;
                            var _listaConfigurarMatrizPorOpcionUno = _listaConfigurarMatrizPorCuestionarioGenericoPregunta.Where(c => c.OpcionUnoMatriz.IdOpcionUnoMatriz == _idOpcionUnoMatriz).ToList();


                            var _listaRespuestasPorCabecera = _objCatalogoRespuestas.ConsultarRespuestaPorIdCabeceraRespuesta(_idCabeceraRespuesta);
                            var _listaRespuestasPorPregunta = _listaRespuestasPorCabecera.Where(c => c.Pregunta.IdPregunta == _idPregunta).ToList();

                            var _listaRespuestas = _listaConfigurarMatrizPorOpcionUno
                            .Join(_listaRespuestasPorPregunta,
                            configurarMatriz => configurarMatriz.IdConfigurarMatriz,
                            respuestas => respuestas.IdRespuestaLogica,
                            (configurarMatriz, respuestas) => new { ConfigurarMatriz = configurarMatriz, Respuestas = respuestas }).ToList();

                            bool _validarRespuesta = false;
                            int _sumadorRespuesta = 0;
                            Respuesta _objEntidadRespuesta = new Respuesta();

                            if (_listaRespuestas.Count == 0)
                            {
                                _objEntidadRespuesta.CabeceraRespuesta = new CabeceraRespuesta() { IdCabeceraRespuesta = _idCabeceraRespuesta };
                                _objEntidadRespuesta.Pregunta = new Pregunta() { IdPregunta = _idPregunta };
                                _objEntidadRespuesta.IdRespuestaLogica = _idConfigurarMatriz;
                                _objEntidadRespuesta.DescripcionRespuestaAbierta = "";
                                _objEntidadRespuesta.FechaRegistro = _fechaActual;
                                _objEntidadRespuesta.Estado = true;
                                int _idRespuesta = _objCatalogoRespuestas.InsertarRespuesta(_objEntidadRespuesta);
                                if (_idRespuesta != 0)
                                {
                                    _sumadorRespuesta = 1;
                                    _validarRespuesta = true;
                                }
                            }
                            else
                            {
                                _objEntidadRespuesta.IdRespuesta = _listaRespuestas.FirstOrDefault().Respuestas.IdRespuesta;
                                _objEntidadRespuesta.CabeceraRespuesta = new CabeceraRespuesta() { IdCabeceraRespuesta = _listaRespuestas.FirstOrDefault().Respuestas.CabeceraRespuesta.IdCabeceraRespuesta };
                                _objEntidadRespuesta.Pregunta = new Pregunta() { IdPregunta = _listaRespuestas.FirstOrDefault().Respuestas.Pregunta.IdPregunta };
                                _objEntidadRespuesta.IdRespuestaLogica = _idConfigurarMatriz;
                                _objEntidadRespuesta.DescripcionRespuestaAbierta = "";
                                _objEntidadRespuesta.FechaRegistro = _fechaActual;
                                _objEntidadRespuesta.Estado = true;
                                int _idRespuesta = _objCatalogoRespuestas.ModificarRespuesta(_objEntidadRespuesta);
                                if (_idRespuesta != 0)
                                {
                                    _validarRespuesta = true;
                                }
                            }

                            if (_validarRespuesta == false)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "No se guardó la respuesta. Por favor, comuníquese con el administrador del sistema";
                            }
                            else
                            {
                                var _listaOpcionUno = _listaConfigurarMatrizPorCuestionarioGenericoPregunta.Select(o => new OpcionUnoMatriz
                                {
                                    IdOpcionUnoMatriz = o.OpcionUnoMatriz.IdOpcionUnoMatriz,
                                })
                               .GroupBy(fl => fl.IdOpcionUnoMatriz).ToList()
                               .Select(fl => new OpcionUnoMatriz
                               {
                                   IdOpcionUnoMatriz = fl.Key,
                               })
                               .ToList();


                                int _totalRespuestas = _listaRespuestasPorPregunta.Count + _sumadorRespuesta;
                                int _totalOpcionUno = _listaOpcionUno.Count;
                                bool _validarMatriz = false;
                                if (_totalRespuestas == _totalOpcionUno)
                                {
                                    _validarMatriz = true;
                                }
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                                return new { respuesta = _respuesta, http = _http, validarMatriz = _validarMatriz };
                            }
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
        [Route("api/respuesta_insertarpreguntaabierta")]
        public object respuesta_insertarpreguntaabierta(Respuesta _objRespuesta)
        {

            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                DateTime _fechaActual = DateTime.Now;

                if (_objRespuesta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto respuesta";
                }
                else if (_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la cabecera respuesta";
                }
                else if (_objRespuesta.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objRespuesta.DescripcionRespuestaAbierta == null || string.IsNullOrEmpty(_objRespuesta.DescripcionRespuestaAbierta))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la respuesta";
                }
                else
                {
                    int _idPregunta = int.Parse(_seguridad.DesEncriptar(_objRespuesta.Pregunta.IdPreguntaEncriptado));
                    int _idCabeceraRespuesta = int.Parse(_seguridad.DesEncriptar(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado));
                    string _descripcionPreguntaAbierta = _objRespuesta.DescripcionRespuestaAbierta;
                    var _objPreguntaAbierta = _objCatalogoPreguntaAbierta.ConsultarPreguntaAbiertaPorIdPregunta(_idPregunta).FirstOrDefault();
                    if (_objPreguntaAbierta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La pregunta seleccionada no es una pregunta abierta";
                    }
                    else
                    {
                        var _objCabeceraRespuesta = _objCatalogoCabeceraRespuesta.ConsultarCabeceraRespuestaPorId(_idCabeceraRespuesta).FirstOrDefault();
                        if (_objCabeceraRespuesta == null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró la cabecera de la respuesta.";
                        }
                        else if (_objCabeceraRespuesta.Finalizado == true)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La ficha ya fue respondida por el usuario";
                        }
                        else
                        {

                            int _identificadorTipoDato = _objPreguntaAbierta.TipoDato.Identificador;
                            string _valorMinimo = _objPreguntaAbierta.ValorMinimo;
                            string _valorMaximo = _objPreguntaAbierta.ValorMaximo;

                            bool _validarRangos = true;
                            if (_identificadorTipoDato == 1)
                            {
                                int _i = 0;
                                if (int.TryParse(_descripcionPreguntaAbierta, out _i) == false)
                                {
                                    _validarRangos = false;
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "Se debe ingresar un número entero";
                                }
                                else
                                {
                                    int _valorPreguntaAbierta = int.Parse(_descripcionPreguntaAbierta);
                                    if (_valorMinimo != null)
                                    {
                                        int _valorMinimoEntero = int.Parse(_valorMinimo);
                                        int _valorMaximoEntero = int.Parse(_valorMaximo);
                                        if (_valorPreguntaAbierta < _valorMinimoEntero || _valorPreguntaAbierta > _valorMaximoEntero)
                                        {
                                            _validarRangos = false;
                                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                            _http.mensaje = "El valor ingresado no se encuentra en el rango " + _valorMinimo + " - " + _valorMaximo;
                                        }
                                    }
                                }
                            }
                            else if (_identificadorTipoDato == 2)
                            {
                                DateTime _i = new DateTime();
                                if (DateTime.TryParse(_descripcionPreguntaAbierta, out _i) == false)
                                {
                                    _validarRangos = false;
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "Se debe ingresar una fecha";
                                }
                                else
                                {
                                    DateTime _valorPreguntaAbierta = DateTime.Parse(_descripcionPreguntaAbierta);
                                    if (_valorMinimo != null)
                                    {
                                        DateTime _valorMinimoDate = DateTime.Parse(_valorMinimo);
                                        DateTime _valorMaximoDate = DateTime.Parse(_valorMaximo);
                                        if (DateTime.Compare(_valorPreguntaAbierta, _valorMinimoDate) < 0 || DateTime.Compare(_valorPreguntaAbierta, _valorMaximoDate) > 0)
                                        {
                                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                            _validarRangos = false;
                                            _http.mensaje = "El valor ingresado no se encuentra en el rango " + _valorMinimo + " - " + _valorMaximo;
                                        }
                                    }
                                }
                            }
                            else if (_identificadorTipoDato == 3)
                            {
                                decimal _i = 0;
                                if (decimal.TryParse(_descripcionPreguntaAbierta, out _i) == false)
                                {
                                    _validarRangos = false;
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "Se debe ingresar un valor decimal";
                                }
                                else
                                {
                                    _descripcionPreguntaAbierta = _descripcionPreguntaAbierta.Replace(".", ",");
                                    decimal _valorPreguntaAbierta = Convert.ToDecimal(_descripcionPreguntaAbierta);
                                    if (_valorMinimo != null)
                                    {
                                        _valorMinimo = _valorMinimo.Replace(".", ",");
                                        _valorMaximo = _valorMaximo.Replace(".", ",");
                                        decimal _valorMinimoDecimal = Convert.ToDecimal(_valorMinimo);
                                        decimal _valorMaximoDecimal = Convert.ToDecimal(_valorMaximo);
                                        if (_valorPreguntaAbierta < _valorMinimoDecimal || _valorPreguntaAbierta > _valorMaximoDecimal)
                                        {
                                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                            _validarRangos = false;
                                            _http.mensaje = "El valor ingresado no se encuentra en el rango " + _valorMinimo + " - " + _valorMaximo;
                                        }
                                    }
                                }
                            }
                            else if (_identificadorTipoDato == 4)
                            {
                                int _tamanoPreguntaAbierta = _descripcionPreguntaAbierta.Trim().Length;
                                if (_valorMinimo != null)
                                {
                                    int _valorMinimoEntero = int.Parse(_valorMinimo);
                                    int _valorMaximoEntero = int.Parse(_valorMaximo);
                                    if (_tamanoPreguntaAbierta < _valorMinimoEntero || _tamanoPreguntaAbierta > _valorMaximoEntero)
                                    {
                                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                        _validarRangos = false;
                                        _http.mensaje = "El valor ingresado no se encuentra en el rango " + _valorMinimo + " - " + _valorMaximo;
                                    }
                                }
                            }

                            if (_validarRangos == true)
                            {
                                var _listaRespuestasPorCabecera = _objCatalogoRespuestas.ConsultarRespuestaPorIdCabeceraRespuesta(_idCabeceraRespuesta);
                                var _listaRespuestas = _listaRespuestasPorCabecera.Where(c => c.Pregunta.IdPregunta == _idPregunta).ToList();
                                bool _validarRespuesta = false;
                                Respuesta _objEntidadRespuesta = new Respuesta();
                                if (_listaRespuestas.Count == 0)
                                {
                                    _objEntidadRespuesta.CabeceraRespuesta = new CabeceraRespuesta() { IdCabeceraRespuesta = _idCabeceraRespuesta };
                                    _objEntidadRespuesta.Pregunta = new Pregunta() { IdPregunta = _idPregunta };
                                    _objEntidadRespuesta.IdRespuestaLogica = _objPreguntaAbierta.IdPreguntaAbierta;
                                    _objEntidadRespuesta.DescripcionRespuestaAbierta = _descripcionPreguntaAbierta;
                                    _objEntidadRespuesta.FechaRegistro = _fechaActual;
                                    _objEntidadRespuesta.Estado = true;
                                    int _idRespuesta = _objCatalogoRespuestas.InsertarRespuesta(_objEntidadRespuesta);
                                    if (_idRespuesta != 0)
                                    {
                                        _validarRespuesta = true;
                                    }
                                }
                                else
                                {
                                    _objEntidadRespuesta.IdRespuesta = _listaRespuestas.FirstOrDefault().IdRespuesta;
                                    _objEntidadRespuesta.CabeceraRespuesta = new CabeceraRespuesta() { IdCabeceraRespuesta = _listaRespuestas.FirstOrDefault().CabeceraRespuesta.IdCabeceraRespuesta };
                                    _objEntidadRespuesta.Pregunta = new Pregunta() { IdPregunta = _listaRespuestas.FirstOrDefault().Pregunta.IdPregunta };
                                    _objEntidadRespuesta.IdRespuestaLogica = _listaRespuestas.FirstOrDefault().IdRespuestaLogica;
                                    _objEntidadRespuesta.DescripcionRespuestaAbierta = _descripcionPreguntaAbierta;
                                    _objEntidadRespuesta.FechaRegistro = _fechaActual;
                                    _objEntidadRespuesta.Estado = true;
                                    int _idRespuesta = _objCatalogoRespuestas.ModificarRespuesta(_objEntidadRespuesta);
                                    if (_idRespuesta != 0)
                                    {
                                        _validarRespuesta = true;
                                    }
                                }

                                if (_validarRespuesta == false)
                                {
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "No se guardó la respuesta. Por favor, comuníquese con el administrador del sistema";
                                }
                                else
                                {
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                                    return new { respuesta = _respuesta, http = _http };
                                }
                            }
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
        [Route("api/respuesta_insertaropcionseleccionmultiple")]
        public object respuesta_insertaropcionseleccionmultiple(Respuesta _objRespuesta)
        {

            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                DateTime _fechaActual = DateTime.Now;

                if (_objRespuesta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto respuesta";
                }
                else if (_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la cabecera respuesta";
                }
                else if (_objRespuesta.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objRespuesta.IdRespuestaLogicaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.IdRespuestaLogicaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la respuesta lógica";
                }
                else
                {
                    int _idPregunta = int.Parse(_seguridad.DesEncriptar(_objRespuesta.Pregunta.IdPreguntaEncriptado));
                    var _objPreguntaPorRespuestaLogica = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true && c.TipoPregunta.Identificador == 3).FirstOrDefault();
                    if (_objPreguntaPorRespuestaLogica == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La pregunta seleccionada no es del tipo selección múltiple";
                    }
                    else
                    {
                        int _idCabeceraRespuesta = int.Parse(_seguridad.DesEncriptar(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado));
                        _objRespuesta.IdRespuestaLogica = int.Parse(_seguridad.DesEncriptar(_objRespuesta.IdRespuestaLogicaEncriptado));
                        int _idOpccionSeleccionMultiple = _objRespuesta.IdRespuestaLogica;

                        var _listaOpcionSeleccionMultiple = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorIdPregunta(_idPregunta).Where(c => c.Estado == true).ToList();
                        var _listaOpcionSeleccionada = _listaOpcionSeleccionMultiple.Where(c => c.IdOpcionPreguntaSeleccion == _idOpccionSeleccionMultiple).ToList();
                        if (_listaOpcionSeleccionada.Count == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La opción seleccionada no pertenece a la pregunta ni al cuestionario";
                        }
                        else
                        {
                            var _listaCabeceraRespuesta = _objCatalogoCabeceraRespuesta.ConsultarCabeceraRespuestaPorId(_idCabeceraRespuesta).FirstOrDefault();
                            if (_listaCabeceraRespuesta.Finalizado == true)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "La ficha ya fue respondida por el usuario";
                            }
                            else
                            {
                                var _listaRespuestasPorCabecera = _objCatalogoRespuestas.ConsultarRespuestaPorIdCabeceraRespuesta(_idCabeceraRespuesta);
                                var _listaRespuestaPorPregunta = _listaRespuestasPorCabecera.Where(c => c.Pregunta.IdPregunta == _idPregunta).ToList();
                                var _listaRespuestas = _listaRespuestaPorPregunta.Where(c => c.IdRespuestaLogica == _idOpccionSeleccionMultiple).ToList();
                                bool _validarRespuesta = false;
                                Respuesta _objEntidadRespuesta = new Respuesta();
                                if (_listaRespuestas.Count == 0)
                                {
                                    _objEntidadRespuesta.CabeceraRespuesta = new CabeceraRespuesta() { IdCabeceraRespuesta = _idCabeceraRespuesta };
                                    _objEntidadRespuesta.Pregunta = new Pregunta() { IdPregunta = _idPregunta };
                                    _objEntidadRespuesta.IdRespuestaLogica = _idOpccionSeleccionMultiple;
                                    _objEntidadRespuesta.DescripcionRespuestaAbierta = "";
                                    _objEntidadRespuesta.FechaRegistro = _fechaActual;
                                    _objEntidadRespuesta.Estado = true;
                                    int _idRespuesta = _objCatalogoRespuestas.InsertarRespuesta(_objEntidadRespuesta);
                                    if (_idRespuesta != 0)
                                    {
                                        _validarRespuesta = true;
                                    }
                                }
                                else
                                {
                                    _objCatalogoRespuestas.EliminarRespuesta(_listaRespuestas.FirstOrDefault().IdRespuesta);
                                    _validarRespuesta = true;
                                }

                                if (_validarRespuesta == false)
                                {
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "No se guardó la respuesta. Por favor, comuníquese con el administrador del sistema";
                                }
                                else
                                {
                                    var _listaRespuestasPorCabecera2 = _objCatalogoRespuestas.ConsultarRespuestaPorIdCabeceraRespuesta(_idCabeceraRespuesta).Where(c => c.Pregunta.IdPregunta == _idPregunta).ToList();
                                    bool _validarPreguntaVacia = true;
                                    if (_listaRespuestasPorCabecera2.Count > 0)
                                    {
                                        _validarPreguntaVacia = false;
                                    }
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                                    return new { respuesta = _respuesta, http = _http, validarPreguntaVacia = _validarPreguntaVacia };
                                }
                            }
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
        [Route("api/respuesta_insertaropcionseleccionunica")]
        public object respuesta_insertaropcionseleccionunica(Respuesta _objRespuesta)
        {
            int bandera;
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                DateTime _fechaActual = DateTime.Now;

                if (_objRespuesta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto respuesta";
                }
                if (_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado))
                {
                    bandera = 0;
                }
                else
                {
                    bandera = int.Parse(_seguridad.DesEncriptar(_objRespuesta.CabeceraRespuesta.IdCabeceraRespuestaEncriptado));
                }
                if (_objRespuesta.CabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestadoEncriptado == null || string.IsNullOrEmpty(_objRespuesta.CabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objRespuesta.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objRespuesta.IdRespuestaLogicaEncriptado == null || string.IsNullOrEmpty(_objRespuesta.IdRespuestaLogicaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la respuesta lógica";
                }
                else
                {
                    int _idPregunta = int.Parse(_seguridad.DesEncriptar(_objRespuesta.Pregunta.IdPreguntaEncriptado));
                    var _objPreguntaPorRespuestaLogica = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPreguntaPorRespuestaLogica == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La pregunta no existe";
                    }
                    else
                    {
                        int _IdAsignarEncuestado = int.Parse(_seguridad.DesEncriptar(_objRespuesta.CabeceraRespuesta.AsignarEncuestado.IdAsignarEncuestadoEncriptado));
                        int _IdRespuestaLogicaEncriptado = Convert.ToInt32(_seguridad.DesEncriptar(_objRespuesta.IdRespuestaLogicaEncriptado));
                        _objRespuesta.CabeceraRespuesta = new CabeceraRespuesta() { IdCabeceraRespuesta = bandera };
                        _objRespuesta.CabeceraRespuesta.FechaRegistro = _fechaActual;
                        _objRespuesta.CabeceraRespuesta.AsignarEncuestado = new AsignarEncuestado() { IdAsignarEncuestado = _IdAsignarEncuestado };
                        _objRespuesta.CabeceraRespuesta.Finalizado = false;
                        _objRespuesta.Estado = true;
                        int _identificadorPregunta = _objRespuesta.Pregunta.TipoPregunta.Identificador;
                        _objRespuesta.Pregunta = new Pregunta() { IdPregunta = _idPregunta };
                        _objRespuesta.Pregunta.TipoPregunta = new TipoPregunta { Identificador = _identificadorPregunta };
                        _objRespuesta.IdRespuestaLogica = _IdRespuestaLogicaEncriptado;
                        _objRespuesta.FechaRegistro = _fechaActual;
                        _objRespuesta.Estado = true;
                        int _idRespuesta = _objCatalogoRespuestas.InsertarRespuesta2(_objRespuesta);
                        if (_idRespuesta != 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                            return new { respuesta = _respuesta, http = _http };
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
        public void EliminarRespuestasEncajonadas(int _idOpcionPreguntaSeleccionAnterior, List<PreguntaEncajonada> _listaPreguntasEncajonadasPorOpcionAnterior, List<Respuesta> _listaRespuestasPorCabecera)
        {
            var _listaRespuestasEliminar = _listaRespuestasPorCabecera
                .Join(_listaPreguntasEncajonadasPorOpcionAnterior,
                respuestasPorCabecera => respuestasPorCabecera.Pregunta.IdPregunta,
                preguntasEncajonadasPorOpcionAnterior => preguntasEncajonadasPorOpcionAnterior.Pregunta.IdPregunta,
                (respuestasPorCabecera, preguntasEncajonadasPorOpcionAnterior) => new { RespuestasPorCabecera = respuestasPorCabecera, PreguntasEncajonadasPorOpcionAnterior = preguntasEncajonadasPorOpcionAnterior }).ToList();
            foreach (var itemRespuestasEliminar in _listaRespuestasEliminar)
            {
                EliminarRespuestasEncajonadas(itemRespuestasEliminar.RespuestasPorCabecera.IdRespuestaLogica, _listaPreguntasEncajonadasPorOpcionAnterior, _listaRespuestasPorCabecera);
                _objCatalogoRespuestas.EliminarRespuesta(itemRespuestasEliminar.RespuestasPorCabecera.IdRespuesta);
            }
        }
        [HttpGet]
        [Route("api/respuestas/pregunta")]
        public object respuestasPorPreguntas(string _IdAsignarEncuestado, string _IdPregunta)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_IdAsignarEncuestado == null || string.IsNullOrEmpty(_IdAsignarEncuestado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Algunos campos están vacíos";
                    return new { http = _http };
                }

                if (_IdPregunta == null || string.IsNullOrEmpty(_IdPregunta))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Algunos campos están vacíos";
                    return new { http = _http };
                }
                int idPregunta = int.Parse(_seguridad.DesEncriptar(_IdPregunta));
                var _objPreguntaPorRespuestaLogica = _objCatalogoPregunta.ConsultarPreguntaPorId(idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                if (_objPreguntaPorRespuestaLogica == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La pregunta no existe";
                    return new { http = _http };
                }
                int IdAsignarEncuestado = int.Parse(_seguridad.DesEncriptar(_IdAsignarEncuestado));
                var objCatalogoAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(IdAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                if (objCatalogoAsignarEncuestado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "El asignar encuestado no existe";
                    return new { http = _http };
                }

                var _respuestas = _objCatalogoRespuestas.mostrarRespuestas(Convert.ToString(IdAsignarEncuestado), Convert.ToString(idPregunta));
                if (_respuestas != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                    return new { respuesta = _respuestas, http = _http };
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpGet]
        [Route("api/respuestas/pregunta/seleccion")]
        public object respuestasPorPreguntaseleccion(string _IdPregunta, string _IdAsignarEncuestado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_IdAsignarEncuestado == null || string.IsNullOrEmpty(_IdAsignarEncuestado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Algunos campos están vacíos";
                    return new { http = _http };
                }

                if (_IdPregunta == null || string.IsNullOrEmpty(_IdPregunta))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Algunos campos están vacíos";
                    return new { http = _http };
                }
                int idPregunta = int.Parse(_seguridad.DesEncriptar(_IdPregunta));
                var _objPreguntaPorRespuestaLogica = _objCatalogoPregunta.ConsultarPreguntaPorId(idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                if (_objPreguntaPorRespuestaLogica == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La pregunta no existe";
                    return new { http = _http };
                }
                int IdAsignarEncuestado = int.Parse(_seguridad.DesEncriptar(_IdAsignarEncuestado));
                var objCatalogoAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(IdAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                if (objCatalogoAsignarEncuestado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "El asignar encuestado no existe";
                    return new { http = _http };
                }

                var _respuestas = _objCatalogoRespuestas.mostrarPreguntaRespuestasPorSeleccion(Convert.ToString(idPregunta), Convert.ToString(IdAsignarEncuestado));
                if (_respuestas != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                    return new { respuesta = _respuestas, http = _http };
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpGet]
        [Route("api/respuestas/pregunta/abierta")]
        public object respuestasPreguntaAbierta(string _IdPregunta, string _IdAsignarEncuestado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_IdAsignarEncuestado == null || string.IsNullOrEmpty(_IdAsignarEncuestado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Algunos campos están vacíos";
                    return new { http = _http };
                }

                if (_IdPregunta == null || string.IsNullOrEmpty(_IdPregunta))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Algunos campos están vacíos";
                    return new { http = _http };
                }
                int idPregunta = int.Parse(_seguridad.DesEncriptar(_IdPregunta));
                var _objPreguntaPorRespuestaLogica = _objCatalogoPregunta.ConsultarPreguntaPorId(idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                if (_objPreguntaPorRespuestaLogica == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La pregunta no existe";
                    return new { http = _http };
                }
                int IdAsignarEncuestado = int.Parse(_seguridad.DesEncriptar(_IdAsignarEncuestado));
                var objCatalogoAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(IdAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                if (objCatalogoAsignarEncuestado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "El asignar encuestado no existe";
                    return new { http = _http };
                }

                var _respuestas = _objCatalogoRespuestas.mostrarRespuestasAbierta(Convert.ToString(idPregunta), Convert.ToString(IdAsignarEncuestado));
                if (_respuestas != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                    return new { respuesta = _respuestas, http = _http };
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
