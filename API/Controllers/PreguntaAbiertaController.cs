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
    public class PreguntaAbiertaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        CatalogoPreguntaAbierta _objCatalogoPreguntaAbierta = new CatalogoPreguntaAbierta();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/preguntaabierta_insertar")]
        public object preguntaabierta_insertar(PreguntaAbierta _objPreguntaAbierta)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPreguntaAbierta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto pregunta abierta";
                }
                else if (_objPreguntaAbierta.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objPreguntaAbierta.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objPreguntaAbierta.TipoDato.IdTipoDatoEncriptado == null || string.IsNullOrEmpty(_objPreguntaAbierta.TipoDato.IdTipoDatoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del tipo de dato";
                }
                else if (_objPreguntaAbierta.EspecificaRango == true && (_objPreguntaAbierta.ValorMinimo == "" || _objPreguntaAbierta.ValorMaximo == ""))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Es necesario que ingrese el valor mínimo y el valor máximo";
                }
                else
                {
                    _objPreguntaAbierta.Estado = true;
                    _objPreguntaAbierta.Pregunta.IdPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objPreguntaAbierta.Pregunta.IdPreguntaEncriptado));
                    _objPreguntaAbierta.TipoDato.IdTipoDato = Convert.ToInt32(_seguridad.DesEncriptar(_objPreguntaAbierta.TipoDato.IdTipoDatoEncriptado));
                    int _idPreguntaAbierta = _objCatalogoPreguntaAbierta.InsertarPreguntaAbierta(_objPreguntaAbierta);
                    if(_idPreguntaAbierta==0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar la pregunta abierta";
                    }
                    else
                    {
                        _objPreguntaAbierta = _objCatalogoPreguntaAbierta.ConsultarPreguntaAbiertaPorId(_idPreguntaAbierta).FirstOrDefault();
                        _objPreguntaAbierta.IdPreguntaAbierta = 0;
                        _objPreguntaAbierta.TipoDato.IdTipoDato = 0;
                        _objPreguntaAbierta.Pregunta.IdPregunta = 0;
                        _objPreguntaAbierta.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                        _objPreguntaAbierta.Pregunta.Seccion.IdSeccion = 0;
                        _objPreguntaAbierta.Pregunta.Seccion.Componente.IdComponente = 0;
                        _objPreguntaAbierta.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objPreguntaAbierta;
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
        [Route("api/preguntaabierta_consultarporidpregunta")]
        public object preguntaabierta_consultarporidpregunta(string _idPreguntaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPreguntaEncriptado == null || string.IsNullOrEmpty(_idPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_idPreguntaEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).ToList();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta en el sistema";
                    }
                    else
                    {
                        var _objPreguntaAbierta = _objCatalogoPreguntaAbierta.ConsultarPreguntaAbiertaPorIdPregunta(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                        _objPreguntaAbierta.IdPreguntaAbierta = 0;
                        _objPreguntaAbierta.TipoDato.IdTipoDato = 0;
                        _objPreguntaAbierta.Pregunta.IdPregunta = 0;
                        _objPreguntaAbierta.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                        _objPreguntaAbierta.Pregunta.Seccion.IdSeccion = 0;
                        _objPreguntaAbierta.Pregunta.Seccion.Componente.IdComponente = 0;
                        _objPreguntaAbierta.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objPreguntaAbierta;
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
        [Route("api/preguntaabierta_eliminar")]
        public object preguntaabierta_eliminar(string _idPreguntaAbiertaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPreguntaAbiertaEncriptado == null || string.IsNullOrEmpty(_idPreguntaAbiertaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta abierta";
                }
                else
                {
                    int _idPreguntaAbierta = Convert.ToInt32(_seguridad.DesEncriptar(_idPreguntaAbiertaEncriptado));
                    var _objPreguntaAbierta = _objCatalogoPreguntaAbierta.ConsultarPreguntaAbiertaPorId(_idPreguntaAbierta).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPreguntaAbierta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta abierta en el sistema";
                    }
                    else if(_objPreguntaAbierta.Utilizado == "1" || _objPreguntaAbierta.Encajonamiento == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje ="La pregunta ya ha sido utilizada, por lo tanto no puede ser eliminada";
                    }
                    else if (_objPreguntaAbierta.Utilizado == "0" || _objPreguntaAbierta.Encajonamiento == "0")
                    {
                        _objCatalogoPreguntaAbierta.EliminarPreguntaAbierta(_idPreguntaAbierta);
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
