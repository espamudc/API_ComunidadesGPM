using API.Models.Catalogos;
using API.Models.Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace API.Controllers
{
    [System.Web.Http.Authorize]
    public class CoordenadasController : ApiController
    {
        // GET: Coordenadas
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoCoordenadas _objCatalogoCoordenadas = new CatalogoCoordenadas();
        Seguridad _seguridad = new Seguridad();
        CatalogoParroquia _objCatalogoParroquia = new CatalogoParroquia();

        [System.Web.Mvc.HttpPost]
        [System.Web.Http.Route("api/update/coordenadas")]
        public object coordenadas_insertar(string idComunidad,string latitud, string longitud)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (latitud == null || string.IsNullOrEmpty(latitud))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la latitud";
                    return new { respuesta = _http.mensaje, http = _http.codigo };
                }
                if (longitud == null || string.IsNullOrEmpty(longitud))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la longitud";
                    return new { respuesta = _http.mensaje, http = _http.codigo };
                }
                if (Convert.ToString(idComunidad) == null)
                { 
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el id de la comunidad";
                    return new { respuesta = _http.mensaje, http = _http.codigo };
                }
                var idcomunidad= _seguridad.DesEncriptar(idComunidad);
                idComunidad = idcomunidad;
                var coordenada = _objCatalogoCoordenadas.ModificarCoordenadas(idComunidad,  latitud,  longitud);
                if (coordenada == 1)
                {
                    return new { respuesta = "Coordenadas actualizadas", http = 200 };
                }
                else {
                    return new { respuesta = "Error al actualizar las coordenadas", http = 406 };
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/comunidades/coordenadas")]
        public object comunidadesPorCoordenadas(float latitud , float longitud )
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (latitud == 0 )
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No existe una latitud";
                    return new { respuesta = _http.mensaje, http = _http.codigo };
                }
                if (longitud == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No existe una longitud";
                    return new { respuesta = _http.mensaje, http = _http.codigo };
                }
                        
                var coordenada = _objCatalogoCoordenadas.ConsultarCanton(latitud, longitud);
                if (coordenada != null)
                {
                    return new { respuesta = coordenada, http = 200 };
                }
                else
                {
                    return new { respuesta = "Error, no se pudo obetner las coordenadas", http = 406 };
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