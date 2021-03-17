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
    public class CabeceraVersionModeloController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoCabeceraVersionModelo _objCabeceraVersionModelo = new CatalogoCabeceraVersionModelo();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/cabeceraVersionModelo_insertar")]
        public object cabeceraVersionModelo_insertar(CabeceraVersionModelo CabeceraVersionModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (CabeceraVersionModelo == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto CabeceraVersionModelo";
                }
                else if (CabeceraVersionModelo.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(CabeceraVersionModelo.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el Asignar tipo usuario";
                }
                else if (CabeceraVersionModelo.IdModeloGenerico == null || string.IsNullOrEmpty(CabeceraVersionModelo.IdModeloGenerico))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el modelo generico";
                }
                else if (CabeceraVersionModelo.Caracteristica == null || string.IsNullOrEmpty(CabeceraVersionModelo.Caracteristica))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la caracteristica";
                }
                else
                {
                    CabeceraVersionModelo.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado = _seguridad.DesEncriptar(CabeceraVersionModelo.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado);
                    CabeceraVersionModelo.IdModeloGenerico = _seguridad.DesEncriptar(CabeceraVersionModelo.IdModeloGenerico);
                    if (_objCabeceraVersionModelo.ConsultarCabeceraVersionModelo().Where(p => p.Version == CabeceraVersionModelo.Version && _seguridad.DesEncriptar(p.IdModeloGenerico) == CabeceraVersionModelo.IdModeloGenerico).FirstOrDefault() == null)
                    {
                        int _idCabeceraVersionModelo = _objCabeceraVersionModelo.InsertarCabeceraVersionModelo(CabeceraVersionModelo);
                        if (_idCabeceraVersionModelo == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de ingresar la cabecera version modelo";
                        }
                        else
                        {
                            var dataCabeceraVersionModelo = _objCabeceraVersionModelo.ConsultarCabeceraVersionModeloPorId(_idCabeceraVersionModelo).FirstOrDefault();
                            dataCabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                            _respuesta = dataCabeceraVersionModelo;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }
                    else
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ya existe esta version";
                    }
                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/cabeceraVersionModelo_consultar")]
        public object cabeceraVersionModelo_consultar(CabeceraVersionModelo CabeceraVersionModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (CabeceraVersionModelo.IdModeloGenerico == null || string.IsNullOrEmpty(CabeceraVersionModelo.IdModeloGenerico))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el modelo generico";
                }
                else
                {
                    var _listaCabeceraVersion = _objCabeceraVersionModelo.ConsultarVersionCaracterizacionPorModeloGenerico(int.Parse(_seguridad.DesEncriptar(CabeceraVersionModelo.IdModeloGenerico))).ToList();
                    _respuesta = _listaCabeceraVersion;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/cabeceraVersionModeloParaPublicar_consultar")]
        public object cabeceraVersionModeloParaPublicar_consultar(CabeceraVersionModelo CabeceraVersionModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (CabeceraVersionModelo.IdModeloGenerico == null || string.IsNullOrEmpty(CabeceraVersionModelo.IdModeloGenerico))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el modelo generico";
                }
                else
                {
                    var _listaCabeceraVersion = _objCabeceraVersionModelo.ConsultarVersionCaracterizacionPorPublicar(int.Parse(_seguridad.DesEncriptar(CabeceraVersionModelo.IdModeloGenerico))).ToList();
                    _respuesta = _listaCabeceraVersion;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/cabeceraVersionModelo_eliminar")]
        public object cabeceraVersionModelo_eliminar(CabeceraVersionModelo CabeceraVersionModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado == null || string.IsNullOrEmpty(CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la version del modelo que va a eliminar.";
                }
                else
                {
                    CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado = _seguridad.DesEncriptar(CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado);
                    var _objCaberceraVersion = _objCabeceraVersionModelo.ConsultarCabeceraVersionModeloPorId(int.Parse(CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado)).FirstOrDefault();
                    if (_objCaberceraVersion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La version que intenta eliminar no existe.";
                    }
                    else if (_objCaberceraVersion.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La version ya esta utilizada, por la tanto no puede ser eliminado.";
                    }
                    else
                    {
                        _objCabeceraVersionModelo.EliminarCabeceraVersionModelo(int.Parse(CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado));
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
        [HttpPost]
        [Route("api/cabeceraVersionModeloPorVersion_consultar")]
        public object cabeceraVersionModeloPorVersion_consultar(CabeceraVersionModelo CabeceraVersionModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado == null || string.IsNullOrEmpty(CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la version del modelo generico";
                }
                else
                {
                    var _listaCabeceraVersion = _objCabeceraVersionModelo.ConsultarInformacionVersion(int.Parse(_seguridad.DesEncriptar(CabeceraVersionModelo.IdCabeceraVersionModeloEncriptado)), null);
                    _respuesta = _listaCabeceraVersion;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
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
