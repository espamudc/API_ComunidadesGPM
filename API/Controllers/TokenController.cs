using API.Models.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [Authorize]
    public class TokenController : ApiController
    {
        CatalogoTokens CatTokens = new CatalogoTokens();
        /*
         *  1	INSERTAR
            2	MODIFICAR
            3	ELIMINAR
            4	CONSULTAR
         */
        [HttpGet]
        [Route("api/tokens")]
        public object tokens()
        {

            return CatTokens.GenerarTokens();
        }
    }
}
