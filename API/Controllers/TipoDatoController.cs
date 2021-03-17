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
    public class TipoDatoController : ApiController
    {

        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoTipoDato _objCatalogoTipoDato = new CatalogoTipoDato();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/tipodato_consultar")]
        public object tipodato_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _lista = _objCatalogoTipoDato.ConsultarTipoDato().Where(c => c.Estado == true).ToList();
                foreach (var item in _lista)
                {
                    item.IdTipoDato = 0;
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
