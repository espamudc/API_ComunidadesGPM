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
    public class PreguntaEncajonadaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        Seguridad _seguridad = new Seguridad();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();

        CatalogoOpcionPreguntaSeleccion _objCatalogoOpcionPreguntaSeleccion = new CatalogoOpcionPreguntaSeleccion();
        CatalogoPreguntaEncajonada _objCatalogoPreguntaEncajonada = new CatalogoPreguntaEncajonada();
        [HttpPost]
        [Route("api/preguntaencajonada_insertar")]
        public object preguntaencajonada_insertar(PreguntaEncajonada _objPreguntaEncajonada)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPreguntaEncajonada == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto pregunta encajonada";
                }
                else if (_objPreguntaEncajonada.Pregunta.IdPreguntaEncriptado== null || string.IsNullOrEmpty(_objPreguntaEncajonada.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objPreguntaEncajonada.OpcionPreguntaSeleccion.IdOpcionPreguntaSeleccionEncriptado== null || string.IsNullOrEmpty(_objPreguntaEncajonada.OpcionPreguntaSeleccion.IdOpcionPreguntaSeleccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la opción pregunta de selección";
                }
                else
                {
                    _objPreguntaEncajonada.Pregunta.IdPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objPreguntaEncajonada.Pregunta.IdPreguntaEncriptado));
                    _objPreguntaEncajonada.OpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion = Convert.ToInt32(_seguridad.DesEncriptar(_objPreguntaEncajonada.OpcionPreguntaSeleccion.IdOpcionPreguntaSeleccionEncriptado));
                    _objPreguntaEncajonada.Estado = true;
                   
                    int _idPreguntaEncajonada = _objCatalogoPreguntaEncajonada.InsertarPreguntaEncajonada(_objPreguntaEncajonada);
                    if (_idPreguntaEncajonada == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ingresar la pregunta encajonada";
                    }
                    else
                    {
                        _objPreguntaEncajonada = _objCatalogoPreguntaEncajonada.ConsultarPreguntaEncajonadaPorId(_idPreguntaEncajonada).Where(c => c.Estado == true).FirstOrDefault();
                        _objPreguntaEncajonada.IdPreguntaEncajonada = 0;
                        _objPreguntaEncajonada.OpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion = 0;
                        _objPreguntaEncajonada.Pregunta.IdPregunta = 0;
                        _objPreguntaEncajonada.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                        _objPreguntaEncajonada.Pregunta.Seccion.IdSeccion = 0;
                        _objPreguntaEncajonada.Pregunta.Seccion.Componente.IdComponente = 0;
                        _objPreguntaEncajonada.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objPreguntaEncajonada;
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
        [Route("api/preguntaencajonada_consultarporidopcionpreguntaseleccion")]
        public object preguntaencajonada_consultarporidopcionpreguntaseleccion(string _idOpcionPreguntaSeleccionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idOpcionPreguntaSeleccionEncriptado == null || string.IsNullOrEmpty(_idOpcionPreguntaSeleccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la opción pregunta de selección";
                }
                else
                {
                    int _idOpcionPreguntaSeleccion = Convert.ToInt32(_seguridad.DesEncriptar(_idOpcionPreguntaSeleccionEncriptado));
                    var _objOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorId(_idOpcionPreguntaSeleccion).Where(c => c.Estado == true).FirstOrDefault();
                    if(_objOpcionPreguntaSeleccion==null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto opción pregunta de selección";
                    }
                    else
                    {
                        var _listaPreguntasEncajonadas = _objCatalogoPreguntaEncajonada.ConsultarPreguntaEncajonadaPorIdOpcionSeleccionUnica(_idOpcionPreguntaSeleccion).Where(c => c.Estado == true).ToList();
                        foreach (var _objPreguntaEncajonada in _listaPreguntasEncajonadas)
                        {
                            _objPreguntaEncajonada.IdPreguntaEncajonada = 0;
                            _objPreguntaEncajonada.OpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion = 0;
                            _objPreguntaEncajonada.Pregunta.IdPregunta = 0;
                            _objPreguntaEncajonada.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objPreguntaEncajonada.Pregunta.Seccion.IdSeccion = 0;
                            _objPreguntaEncajonada.Pregunta.Seccion.Componente.IdComponente = 0;
                            _objPreguntaEncajonada.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _listaPreguntasEncajonadas;
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
        [Route("api/preguntaencajonada_eliminar")]
        public object preguntaencajonada_eliminar(string _idPreguntaEncajonadaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPreguntaEncajonadaEncriptado == null || string.IsNullOrEmpty(_idPreguntaEncajonadaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta encajonada";
                }
                else
                {
                    int _idPreguntaEncajonada = Convert.ToInt32(_seguridad.DesEncriptar(_idPreguntaEncajonadaEncriptado));
                    var _objPreguntaEncajonada = _objCatalogoPreguntaEncajonada.ConsultarPreguntaEncajonadaPorId(_idPreguntaEncajonada).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPreguntaEncajonada == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto pregunta encajonada";
                    }
                    //else if(_objPreguntaEncajonada.Utilizado=="1")
                    //{
                    //    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                    //    _http.mensaje = "No se puede eliminar porque ya ha sido utilizada";
                    //}
                    else 
                    {
                        _objCatalogoPreguntaEncajonada.EliminarPreguntaEncajonada(_idPreguntaEncajonada);
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
        [HttpGet]
        [Route("api/pregunta/encajonada")]
        public object preguntaencajonada_ver(string IdOpcionPreguntaSeleccion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (IdOpcionPreguntaSeleccion == null || string.IsNullOrEmpty(IdOpcionPreguntaSeleccion))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la opción de la pregunta";
                    return new { http = _http };
                }
                int _IdOpcionPreguntaSeleccion = int.Parse(_seguridad.DesEncriptar(IdOpcionPreguntaSeleccion));
                var _respuestas = _objCatalogoPreguntaEncajonada.VerPreguntaEncajonada(Convert.ToString(_IdOpcionPreguntaSeleccion));
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
