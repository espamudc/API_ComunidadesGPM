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
    public class OpcionUnoMatrizController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoOpcionUnoMatriz _objCatalogoOpcionUnoMatriz = new CatalogoOpcionUnoMatriz();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/opcionunomatriz_insertar")]
        public object opcionunomatriz_insertar(OpcionUnoMatriz _objOpcionUnoMatriz)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objOpcionUnoMatriz == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto opción uno matriz";
                }
                else if (_objOpcionUnoMatriz.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objOpcionUnoMatriz.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objOpcionUnoMatriz.Descripcion == null || string.IsNullOrEmpty(_objOpcionUnoMatriz.Descripcion))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción de la opción uno matriz";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objOpcionUnoMatriz.Pregunta.IdPreguntaEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta en el sistema";
                    }
                    else
                    {
                        _objOpcionUnoMatriz.Pregunta.IdPregunta = _idPregunta;
                        _objOpcionUnoMatriz.Estado = true;
                        int _idOpcionUnoMatriz = _objCatalogoOpcionUnoMatriz.InsertarOpcionUnoMatriz(_objOpcionUnoMatriz);
                        if (_idOpcionUnoMatriz == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de ingresar la opción uno matriz";
                        }
                        else
                        {
                            _objOpcionUnoMatriz = _objCatalogoOpcionUnoMatriz.ConsultarOpcionUnoMatrizPorId(_idOpcionUnoMatriz).FirstOrDefault();
                            _objOpcionUnoMatriz.IdOpcionUnoMatriz = 0;
                            _objOpcionUnoMatriz.Pregunta.IdPregunta = 0;
                            _objOpcionUnoMatriz.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objOpcionUnoMatriz.Pregunta.Seccion.IdSeccion = 0;
                            _objOpcionUnoMatriz.Pregunta.Seccion.Componente.IdComponente = 0;
                            _objOpcionUnoMatriz.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _respuesta = _objOpcionUnoMatriz;
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
        [Route("api/opcionunomatriz_consultarporidpregunta")]
        public object opcionunomatriz_consultarporidpregunta(string _idPreguntaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPreguntaEncriptado == null || string.IsNullOrEmpty(_idPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_idPreguntaEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).FirstOrDefault();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta en el sistema";
                    }
                    else
                    {
                        var _listaOpcionUnoMatriz = _objCatalogoOpcionUnoMatriz.ConsultarOpcionUnoMatrizPorIdPregunta(_idPregunta).Where(c => c.Estado == true).ToList();
                        foreach (var _opcionUnoMatriz in _listaOpcionUnoMatriz)
                        {
                            _opcionUnoMatriz.IdOpcionUnoMatriz = 0;
                            _opcionUnoMatriz.Pregunta.IdPregunta = 0;
                            _opcionUnoMatriz.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                            _opcionUnoMatriz.Pregunta.Seccion.IdSeccion = 0;
                            _opcionUnoMatriz.Pregunta.Seccion.Componente.IdComponente = 0;
                            _opcionUnoMatriz.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _listaOpcionUnoMatriz;
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
        [Route("api/opcionunomatriz_eliminar")]
        public object opcionunomatriz_eliminar(string _idOpcionUnoMatrizEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idOpcionUnoMatrizEncriptado == null || string.IsNullOrEmpty(_idOpcionUnoMatrizEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la opción uno matriz";
                }
                else
                {
                    int _idOpcionUnoMatriz = Convert.ToInt32(_seguridad.DesEncriptar(_idOpcionUnoMatrizEncriptado));
                    var _objOpcionUnoMatriz = _objCatalogoOpcionUnoMatriz.ConsultarOpcionUnoMatrizPorId(_idOpcionUnoMatriz).FirstOrDefault();
                    if (_objOpcionUnoMatriz == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la opción uno matriz en el sistema";
                    }
                    else if (_objOpcionUnoMatriz.Utilizado == "1" || _objOpcionUnoMatriz.Encajonamiento == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No es posible eliminar la opción uno matriz porque está siendo utilizada";
                    }
                    else if (_objOpcionUnoMatriz.Utilizado == "0" || _objOpcionUnoMatriz.Encajonamiento == "0")
                    {
                        _objCatalogoOpcionUnoMatriz.EliminarOpcionUnoMatriz(_idOpcionUnoMatriz);
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