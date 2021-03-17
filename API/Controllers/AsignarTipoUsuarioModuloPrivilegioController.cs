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
    public class AsignarTipoUsuarioModuloPrivilegioController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarTipoUsuarioModuloPrivilegio _objCatalogoAsignarTipoUsuarioModuloPrivilegio = new CatalogoAsignarTipoUsuarioModuloPrivilegio();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/asignartipousuariomoduloprivilegio_consultar")]
        public object asignartipousuariomoduloprivilegio_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _lista = _objCatalogoAsignarTipoUsuarioModuloPrivilegio.ConsultarAsignarTipoUsuarioModuloPrivilegio().Where(c => c.Estado == true && c.AsignarModuloPrivilegio.Estado==true && c.AsignarModuloPrivilegio.Modulo.Estado==true && c.AsignarModuloPrivilegio.Privilegio.Estado==true).ToList();
                foreach (var item in _lista)
                {
                    item.IdAsignarTipoUsuarioModuloPrivilegio = 0;
                    item.TipoUsuario.IdTipoUsuario = 0;
                    item.AsignarModuloPrivilegio.IdAsignarModuloPrivilegio = 0;
                    item.AsignarModuloPrivilegio.Modulo.IdModulo = 0;
                    item.AsignarModuloPrivilegio.Privilegio.IdPrivilegio = 0;
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
