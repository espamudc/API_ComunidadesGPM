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
    public class TipoUsuarioController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoTipoUsuarios _objCatalogoTipoUsuarios = new CatalogoTipoUsuarios();
        CatalogoUsuario _objCatalogoUsuario = new CatalogoUsuario();
        CatalogoTokens _objCatalogoTokens = new CatalogoTokens();
        Seguridad _seguridad = new Seguridad();
    
        [HttpPost]
        [Route("api/tipousuario_consultar")]
        public object tipousuario_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();

            try
            {
                var _listaTiposUsuarios = _objCatalogoTipoUsuarios.ConsultarTipoUsuarios().Where(c=>c.Estado==true).ToList();
                foreach (var item in _listaTiposUsuarios)
                {
                    item.IdTipoUsuario = 0;
                }
                _respuesta = _listaTiposUsuarios;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " +ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }


        [HttpPost]
        [Route("api/tipousuario_consultarnoasignadosporusuario")]
        public object tipousuario_consultarnoasignadosporusuario(string _idUsuarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();

            try
            {
                if(_idUsuarioEncriptado == null || string.IsNullOrEmpty(_idUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del usuario";
                }
                else
                {
                    int _idUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_idUsuarioEncriptado));
                    var _objUsuario = _objCatalogoUsuario.ConsultarUsuarioPorId(_idUsuario).FirstOrDefault();
                    if (_objUsuario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el usuario registrado en el sistema";
                    }
                    else
                    {
                        var _listaTiposUsuarios = _objCatalogoTipoUsuarios.ConsultarTipoUsuarioNoAsignadosPorUsuario(_idUsuario).Where(c => c.Estado == true).ToList();
                        foreach (var item in _listaTiposUsuarios)
                        {
                            item.IdTipoUsuario = 0;
                        }
                        _respuesta = _listaTiposUsuarios;
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
