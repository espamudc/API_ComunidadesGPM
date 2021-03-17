using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models.Entidades;
using API.Models.Catalogos;
using API.Models.Metodos;

namespace API.Controllers
{
    [Authorize]
    public class AsignarResponsableController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarResponsable _objCatalogoAsignarResponsable = new CatalogoAsignarResponsable();
        CatalogoCuestionarioGenerico _objCatalogoCuestionarioGenerico = new CatalogoCuestionarioGenerico();
        CatalogoAsignarUsuarioTipoUsuario _objCatalogoAsignarUsuarioTipoUsuario = new CatalogoAsignarUsuarioTipoUsuario();
        Seguridad _seguridad = new Seguridad();
        [HttpPost]
        [Route("api/asignarresponsable_insertar")]
        public object asignarresponsable_insertar(AsignarResponsable _objAsignarResponsable)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarResponsable == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto asignar responsable.";
                }
                else if (_objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico.";
                }
                else if (_objAsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar usuario tipo usuario.";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenericoEncriptado));
                    var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCuestionarioGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el cuestionario genérico.";
                    }
                    else
                    {
                        int _idAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado));
                        var _objAsignarUsuarioTipoUsuario = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuarioPorId(_idAsignarUsuarioTipoUsuario).FirstOrDefault();
                        if (_objAsignarUsuarioTipoUsuario == null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró el asignar usuario tipo usuario.";
                        }
                        else
                        {
                            _objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = _idCuestionarioGenerico;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = _idAsignarUsuarioTipoUsuario;
                            _objAsignarResponsable.Estado = true;
                            //_objAsignarResponsable.FechaAsignacion = DateTime.Now;
                            int _idAsignarResponsable = _objCatalogoAsignarResponsable.InsertarAsignarResponsable(_objAsignarResponsable);
                            if (_idAsignarResponsable == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un problema al tratar de ingresar el asignar responsable.";
                            }
                            else
                            {
                                var _objAsignarResponsableInsertado = _objCatalogoAsignarResponsable.ConsultarAsignarResponsablePorIdCuestionarioGenerico(_idCuestionarioGenerico).Where(c => c.IdAsignarResponsable == _idAsignarResponsable && c.Estado == true).FirstOrDefault();
                                _objAsignarResponsableInsertado.IdAsignarResponsable = 0;
                                _objAsignarResponsableInsertado.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                                _objAsignarResponsableInsertado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                                _objAsignarResponsableInsertado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                                _objAsignarResponsableInsertado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                                _objAsignarResponsableInsertado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                                _objAsignarResponsableInsertado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                                _objAsignarResponsableInsertado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                                _respuesta = _objAsignarResponsableInsertado;
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                            }
                        }
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
        [Route("api/asignarresponsable_consultarporidcuestionariogenerico")]
        public object asignarresponsable_consultarporidcuestionariogenerico(string _idCuestionarioGenericoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico.";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                    var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCuestionarioGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el cuestionario genérico.";
                    }
                    else
                    {                                                                                                                                       // c.Estado == true &&
                        var _lista = _objCatalogoAsignarResponsable.ConsultarAsignarResponsablePorIdCuestionarioGenerico(_idCuestionarioGenerico).Where(c => c.AsignarUsuarioTipoUsuario.Estado == true && c.AsignarUsuarioTipoUsuario.Usuario.Estado == true).ToList();
                        foreach (var _objAsignarResponsable in _lista)
                        {
                            _objAsignarResponsable.IdAsignarResponsable = 0;
                            _objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        }
                        _respuesta = _lista;
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
        [Route("api/asignarresponsable_consultarporidasignarusuariotipousuario")]
        public object asignarresponsable_consultarporidasignarusuariotipousuario(string _idAsignarUsuarioTipoUsuarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_idAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar usuario tipo usuario.";
                }
                else
                {
                    int _idAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarUsuarioTipoUsuarioEncriptado));
                    var _objAsignarUsuarioTipoUsuario = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuarioPorId(_idAsignarUsuarioTipoUsuario).FirstOrDefault();
                    if (_objAsignarUsuarioTipoUsuario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar usuario tipo usuario.";
                    }
                    else
                    {
                        var _lista = _objCatalogoAsignarResponsable.ConsultarAsignarResponsablePorIdAsignarUsuarioTipoUsuario(_idAsignarUsuarioTipoUsuario).Where(c => c.Estado == true && c.AsignarUsuarioTipoUsuario.Estado == true && c.AsignarUsuarioTipoUsuario.Usuario.Estado == true).ToList();
                        foreach (var _objAsignarResponsable in _lista)
                        {
                            _objAsignarResponsable.IdAsignarResponsable = 0;
                            _objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            _objAsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        }
                        _respuesta = _lista;
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
        [Route("api/asignarresponsable_cambiarestado")]
        public object asignarresponsable_cambiarestado(string _idAsignarResponsableEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarResponsableEncriptado == null || string.IsNullOrEmpty(_idAsignarResponsableEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar responsable.";
                }
                else
                {
                    int _idAsignarResponsable = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarResponsableEncriptado));
                    var _objAsignarResponsable = _objCatalogoAsignarResponsable.ConsultarAsignarResponsablePorId(_idAsignarResponsable).FirstOrDefault();
                    if(_objAsignarResponsable == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el asignar responsable en el sistema.";
                    }
                    else
                    {
                        bool _nuevoEstado = false;
                        if(_objAsignarResponsable.Estado==false)
                        {
                            _nuevoEstado = true;
                        }
                        _objCatalogoAsignarResponsable.CambiarEstadoAsignarResponsable(_idAsignarResponsable, _nuevoEstado);
                        _objAsignarResponsable = _objCatalogoAsignarResponsable.ConsultarAsignarResponsablePorId(_idAsignarResponsable).FirstOrDefault();
                        _objAsignarResponsable.IdAsignarResponsable = 0;
                        _objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _objAsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _objAsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _respuesta = _objAsignarResponsable;
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
        [Route("api/asignarresponsable_eliminar")]
        public object asignarresponsable_eliminar(string _idAsignarResponsableEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarResponsableEncriptado == null || string.IsNullOrEmpty(_idAsignarResponsableEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar responsable.";
                }
                else
                {
                    int _idAsignarResponsable = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarResponsableEncriptado));
                    var _objAsignarResponsable = _objCatalogoAsignarResponsable.ConsultarAsignarResponsablePorId(_idAsignarResponsable).FirstOrDefault();
                    if (_objAsignarResponsable == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el asignar responsable en el sistema.";
                    }
                    else
                    {
                        _objCatalogoAsignarResponsable.EliminarAsignarResponsable(_idAsignarResponsable);
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
