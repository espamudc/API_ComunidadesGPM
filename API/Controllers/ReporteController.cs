using API.Models.Catalogos;
using API.Models.Metodos;
using System.Web.Http;
using System;
using System.Linq;
namespace API.Controllers
{
    [Authorize]
    public class ReporteController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        Seguridad _seguridad = new Seguridad();
        CatalogoReporteEjecutivo _objCatalogoReporteEjecutivo = new CatalogoReporteEjecutivo();

        [HttpGet]
        [Route("api/reporte/ejecutivo")]
        public object reporteComunidad(string _idComunidad)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idComunidad))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad";
                }
                else
                {
                    string idComunidad = Convert.ToString(_seguridad.DesEncriptar(_idComunidad));
                    var _objComunidad = _objCatalogoReporteEjecutivo.ConsultarReporteEjecutivo(idComunidad);
                 
                    _respuesta = _objComunidad;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpGet]
        [Route("api/listado/comunidad/parroquia")]
        public object ListadoComunidadAndParroquia()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _objComunidad = _objCatalogoReporteEjecutivo.listadoComunidadesAndParroquias();

                if (_objComunidad != null)
                {
                    _respuesta = _objComunidad;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
                else {
                    _respuesta = "No hay comunidad para mostrar";
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
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