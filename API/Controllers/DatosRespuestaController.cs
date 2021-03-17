using API.Models.Catalogos;
using API.Models.Entidades;
using API.Models.Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [Authorize]
    public class DatosRespuestaController : ApiController
    {
        CatalogoDatosRespuesta objCatalogoDatosRespuesta = new CatalogoDatosRespuesta();
        Seguridad _seguridad = new Seguridad();
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        CatalogoAsignarEncuestado _objCatalogoAsignarEncuestado = new CatalogoAsignarEncuestado();
        [HttpPost]
        [Route("api/datosrespuesta")]
        public object datosrespuesta_insertar(DatosRespuesta _objDatos)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objDatos == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto de datos.";
                }
                else if (_objDatos.datos == "null" || string.IsNullOrEmpty(_objDatos.datos))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el dato";
                }
                else if (_objDatos.IdPregunta == "null" || string.IsNullOrEmpty(_objDatos.IdPregunta))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la respuesta";
                }
                else
                {

                    int idPregunta = int.Parse(_seguridad.DesEncriptar(_objDatos.IdPregunta));
                    var _objPreguntaPorRespuestaLogica = _objCatalogoPregunta.ConsultarPreguntaPorId(idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPreguntaPorRespuestaLogica == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La pregunta no existe";
                        return new { http = _http };
                    }
                    int IdAsignarEncuestado = int.Parse(_seguridad.DesEncriptar(_objDatos.IdAsignarEncuestado));
                    var objCatalogoAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(IdAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                    if (objCatalogoAsignarEncuestado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "El asignar encuestado no existe";
                        return new { http = _http };
                    }
                    _objDatos.IdAsignarEncuestado = Convert.ToString(IdAsignarEncuestado);
                    _objDatos.IdPregunta = Convert.ToString(idPregunta);
                    var _respuestas = objCatalogoDatosRespuesta.InsertarDatosRespuesta(_objDatos);
               
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                    if (_respuestas != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        return new { respuesta = _respuestas, http = _http };
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
