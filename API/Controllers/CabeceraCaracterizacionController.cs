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
    public class CabeceraCaracterizacionController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoCabeceraCaracterizacion _objCatalogoCabeceraCaracterizacion = new CatalogoCabeceraCaracterizacion();
        CatalogoAsignarResponsableModeloPublicado _objCatalogoAsignarResponsableModeloPublicado = new CatalogoAsignarResponsableModeloPublicado();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/cabeceracaracterizacion_insertar")]
        public object cabeceracaracterizacion_insertar(CabeceraCaracterizacion _objCabeceraCaracterizacion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objCabeceraCaracterizacion == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto cabecera caracterización";
                }
                else if (_objCabeceraCaracterizacion.AsignarResponsableModeloPublicado == null || string.IsNullOrEmpty(_objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto asignar modelo publicado";
                }
                else
                {
                    _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado));
                    _objCabeceraCaracterizacion.FechaRegistro = DateTime.Now;
                    _objCabeceraCaracterizacion.Finalizado = false;
                    _objCabeceraCaracterizacion.FechaFinalizado = null;
                    _objCabeceraCaracterizacion.Estado = true;
                    int _idCabeceraCaracterizacion = _objCatalogoCabeceraCaracterizacion.InsertarCabeceraCaracterizacion(_objCabeceraCaracterizacion);
                    if (_idCabeceraCaracterizacion == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al intentar ingresar la cabecera de la caracterización";
                    }
                    else
                    {
                        _objCabeceraCaracterizacion = _objCatalogoCabeceraCaracterizacion.ConsultarCabeceraCaracterizacionPorId(_idCabeceraCaracterizacion).FirstOrDefault();
                        _objCabeceraCaracterizacion.IdCabeceraCaracterizacion = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objCabeceraCaracterizacion;
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
        [Route("api/cabeceracaracterizacion_consultar")]
        public object cabeceracaracterizacion_consultar(string _idCabeceraCaracterizacionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idCabeceraCaracterizacionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto cabecera caracterización";
                }
                else
                {
                    int _idCabeceraCaracterizacion = Convert.ToInt32(_seguridad.DesEncriptar(_idCabeceraCaracterizacionEncriptado));
                     var   _objCabeceraCaracterizacion = _objCatalogoCabeceraCaracterizacion.ConsultarCabeceraCaracterizacionPorId(_idCabeceraCaracterizacion).FirstOrDefault();
                        _objCabeceraCaracterizacion.IdCabeceraCaracterizacion = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                        _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objCabeceraCaracterizacion;
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
        [Route("api/cabeceracaracterizacion_consultarporidasignarresponsablemodelopublicado")]
        public object cabeceracaracterizacion_consultarporidasignarresponsablemodelopublicado(string _idAsignarResponsableModeloPublicadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto asignar responsable modelo publicado";
                }
                else
                {
                    int _idAsignarResponsableModeloPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarResponsableModeloPublicadoEncriptado));
                    var _objAsignarResponsableModeloPublicado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsableModeloPublicado).FirstOrDefault();
                    if (_objAsignarResponsableModeloPublicado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar responsable modelo publicado";
                    }
                    else
                    {
                        var _objCabeceraCaracterizacion = _objCatalogoCabeceraCaracterizacion.ConsultarCabeceraCaracterizacionPorIdAsignarResponsableModeloPublicado(_idAsignarResponsableModeloPublicado).FirstOrDefault();
                        if(_objCabeceraCaracterizacion != null)
                        {
                            _objCabeceraCaracterizacion.IdCabeceraCaracterizacion = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                            _objCabeceraCaracterizacion.AsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                        }
                        _respuesta = _objCabeceraCaracterizacion;
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
