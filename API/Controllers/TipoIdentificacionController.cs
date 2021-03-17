using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using API.Models.Catalogos;
using API.Models.Entidades;
using API.Models.Metodos;

namespace API.Controllers
{
    [Authorize]
    public class TipoIdentificacionController : ApiController
    {

        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();

        CatalogoTipoIdentificacion _objCatalogoTipoIdentificacion = new CatalogoTipoIdentificacion();

        [HttpPost]
        [Route("api/tipoidentificacion_consultar")]
        public object tipoidentificacion_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaTipoIdentificacion = _objCatalogoTipoIdentificacion.ConsultarTipoIdentificacion().Where(x => x.Estado == true).ToList();
                foreach (var item in _listaTipoIdentificacion)
                {
                    item.IdTipoIdentificacion = 0;
                }
                _respuesta = _listaTipoIdentificacion;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
                return new
                {
                    respuesta = _respuesta,
                    http = _http
                };
            }
            return new
            {
                respuesta = _respuesta,
                http = _http
            };
        }
    }
}
