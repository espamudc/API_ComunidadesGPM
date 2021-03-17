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
    public class SexoController : ApiController
    {
        CatalogoSexo _objCatalogoSexo = new CatalogoSexo();
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();

        [HttpPost]
        [Route("api/sexo_consultar")]
        public object sexo_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var listaSexo = _objCatalogoSexo.ConsultarSexos().Where(x => x.Estado == true).ToList();
                foreach (var item in listaSexo)
                {
                    item.IdSexo = 0;
                }
                _respuesta = listaSexo;
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
