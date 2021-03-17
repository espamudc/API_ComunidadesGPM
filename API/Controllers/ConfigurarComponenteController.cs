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
    public class ConfigurarComponenteController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoConfigurarComponente _objConfigurarComponente = new CatalogoConfigurarComponente();
        Seguridad _seguridad = new Seguridad();
        [HttpPost]
        [Route("api/configurarComponente_insertar")]
        public object configurarComponente_insertar(ConfigurarComponente _ConfigurarComponente)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_ConfigurarComponente == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto del componente";
                }
                else if (_ConfigurarComponente.IdAsignacionTU == null || string.IsNullOrEmpty(_ConfigurarComponente.IdAsignacionTU))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la persona logueada";
                }
                else if (_ConfigurarComponente.IdAsignarComponenteGenerico == null || string.IsNullOrEmpty(_ConfigurarComponente.IdAsignarComponenteGenerico))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el componente";
                }
                else if (_ConfigurarComponente.Contenido == null || string.IsNullOrEmpty(_ConfigurarComponente.Contenido))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el contenido";
                }
                else
                {
                    ConfigurarComponente ConfigurarComponenteDato = new ConfigurarComponente();
                    _ConfigurarComponente.IdAsignacionTU = _seguridad.DesEncriptar(_ConfigurarComponente.IdAsignacionTU);
                    _ConfigurarComponente.IdAsignarComponenteGenerico = _seguridad.DesEncriptar(_ConfigurarComponente.IdAsignarComponenteGenerico);
                    ConfigurarComponenteDato = _objConfigurarComponente.ConsultarConfigurarComponentePorIdAsignarComponente(_ConfigurarComponente.IdAsignarComponenteGenerico).FirstOrDefault();
                    if (ConfigurarComponenteDato == null)
                    {
                        ConfigurarComponenteDato = _objConfigurarComponente.InsertarConfigurarComponente(_ConfigurarComponente);
                    }
                    else
                    {
                        _ConfigurarComponente.IdConfigurarComponente = _seguridad.DesEncriptar(ConfigurarComponenteDato.IdConfigurarComponente);
                        ConfigurarComponenteDato = _objConfigurarComponente.ModificarConfigurarComponente(_ConfigurarComponente);
                    }

                    if (ConfigurarComponenteDato.IdConfigurarComponente == "0")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar la configuracion";
                    }
                    else
                    {
                        _respuesta = ConfigurarComponenteDato;
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
        [Route("api/consultarConfigurarComponentePorComponente")]
        public object consultarConfigurarComponentePorComponente(ConfigurarComponente _ConfigurarComponente)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_ConfigurarComponente.IdAsignarComponenteGenerico == null || string.IsNullOrEmpty(_ConfigurarComponente.IdAsignarComponenteGenerico))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el componente";
                }
                else
                {
                    ConfigurarComponente _ConfigurarComponenteDato = new ConfigurarComponente();
                    _ConfigurarComponente.IdAsignarComponenteGenerico = _seguridad.DesEncriptar(_ConfigurarComponente.IdAsignarComponenteGenerico);
                    _ConfigurarComponenteDato = _objConfigurarComponente.ConsultarConfigurarComponentePorIdAsignarComponente(_ConfigurarComponente.IdAsignarComponenteGenerico).FirstOrDefault();
                    _respuesta = _ConfigurarComponenteDato;
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
