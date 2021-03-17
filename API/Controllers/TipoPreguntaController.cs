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
    public class TipoPreguntaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        Seguridad _seguridad = new Seguridad();
        CatalogoTipoPregunta _objCatalogoTipoPregunta = new CatalogoTipoPregunta();
        [HttpPost]
        [Route("api/tipopregunta_consultar")]
        public object tipopregunta_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _lista = _objCatalogoTipoPregunta.ConsultarTipoPregunta().Where(c => c.Estado == true).ToList();
                foreach (var item in _lista)
                {
                    item.IdTipoPregunta = 0;
                }
                _respuesta = _lista;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
    }
}
