using API.Models.Catalogos;
using API.Models.Metodos;
using API.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [Authorize]
    public class AsignarCuestionarioModeloController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarCuestionarioModelo _objCatalogoAsignarCuestionarioModelo = new CatalogoAsignarCuestionarioModelo();
        Seguridad _seguridad = new Seguridad();


        [HttpPost]
        [Route("api/AsignarCuestionarioModelo_insertar")]
        public object AsignarCuestionarioModelo_insertar(AsignarCuestionarioModelo _objAsignarCuestionarioModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarCuestionarioModelo == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto modelo genérico";
                }
                else if (_objAsignarCuestionarioModelo.IdModeloGenerico == null || string.IsNullOrEmpty(_objAsignarCuestionarioModelo.IdModeloGenerico))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del modelo genérico";
                }
                else if (_objAsignarCuestionarioModelo.IdCuestionarioPublicado == null || string.IsNullOrEmpty(_objAsignarCuestionarioModelo.IdCuestionarioPublicado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del cuestionario genérico";
                }
                else if (_objAsignarCuestionarioModelo.IdAsignarUsuarioTipoUsuario == null || string.IsNullOrEmpty(_objAsignarCuestionarioModelo.IdAsignarUsuarioTipoUsuario))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ingrese el tipo de usuario";
                }
                else
                {
                    var _objAsignarCuestionarioModeloData = _objCatalogoAsignarCuestionarioModelo.ConsultarAsignarCuestionarioModelo().Where(p => _seguridad.DesEncriptar(p.IdModeloGenerico) == _seguridad.DesEncriptar(_objAsignarCuestionarioModelo.IdModeloGenerico) &&  _seguridad.DesEncriptar(p.IdCuestionarioPublicado) == _seguridad.DesEncriptar(_objAsignarCuestionarioModelo.IdCuestionarioPublicado)).FirstOrDefault();
                    if (_objAsignarCuestionarioModeloData == null)
                    {
                        _objAsignarCuestionarioModelo.IdModeloGenerico = _seguridad.DesEncriptar(_objAsignarCuestionarioModelo.IdModeloGenerico);
                        _objAsignarCuestionarioModelo.IdCuestionarioPublicado = _seguridad.DesEncriptar(_objAsignarCuestionarioModelo.IdCuestionarioPublicado);
                        _objAsignarCuestionarioModelo.IdAsignarUsuarioTipoUsuario = _seguridad.DesEncriptar(_objAsignarCuestionarioModelo.IdAsignarUsuarioTipoUsuario);
                        int _idAsignarCuestionarioModelo = _objCatalogoAsignarCuestionarioModelo.InsertarAsignarCuestionarioModelo(_objAsignarCuestionarioModelo);
                        if (_idAsignarCuestionarioModelo == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de ingresar el asignar cuestionario generico.";
                        }
                        else
                        {
                            _objAsignarCuestionarioModeloData = _objCatalogoAsignarCuestionarioModelo.ConsultarAsignarCuestionarioModeloPorId(_idAsignarCuestionarioModelo).FirstOrDefault();
                            _objAsignarCuestionarioModeloData.IdAsignarCuestionarioModelo = 0;
                            _respuesta = _objAsignarCuestionarioModeloData;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }
                    else
                    {
                        _objAsignarCuestionarioModeloData.IdAsignarCuestionarioModelo = 0;
                        _respuesta = _objAsignarCuestionarioModeloData;
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
