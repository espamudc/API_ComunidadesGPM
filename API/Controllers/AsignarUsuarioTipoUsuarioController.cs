using API.Models.Catalogos;
using API.Models.Entidades;
using API.Models.Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [Authorize]
    public class AsignarUsuarioTipoUsuarioController : ApiController
    {

        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarUsuarioTipoUsuario _objCatalogoAsignarUsuarioTipoUsuario = new CatalogoAsignarUsuarioTipoUsuario();
        CatalogoUsuario _objCatalogoUsuario = new CatalogoUsuario();
        CatalogoTipoUsuarios _objCatalogoTipoUsuario = new CatalogoTipoUsuarios();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/asignarusuariotipousuario_consultar")]
        public object asignarusuariotipousuario_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaAsignarUsuarioTipoUsuario = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuario().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaAsignarUsuarioTipoUsuario)
                {
                    item.IdAsignarUsuarioTipoUsuario = 0;
                    item.Usuario.Persona.IdPersona = 0;
                    item.Usuario.IdUsuario = 0;
                    item.Usuario.Persona.Sexo.IdSexo = 0;
                    item.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                    item.TipoUsuario.IdTipoUsuario = 0;
                }
                _respuesta = _listaAsignarUsuarioTipoUsuario;
               _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }

            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/asignarusuariotipousuario_consultar")]
        public object asignarusuariotipousuario_consultar(string _idUsuarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idUsuarioEncriptado==null || string.IsNullOrEmpty(_idUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el usuario que intenta consultar";
                }
                else
                {
                    int _idUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_idUsuarioEncriptado));
                    var _objUsuario = _objCatalogoUsuario.ConsultarUsuarioPorId(_idUsuario).FirstOrDefault();
                    var lista = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuario().Where(c => c.Estado == true && c.Usuario.IdUsuario == _objUsuario.IdUsuario).ToList();
                    foreach (var item in lista)
                    {
                        item.IdAsignarUsuarioTipoUsuario = 0;
                        item.Usuario.IdUsuario = 0;
                        item.TipoUsuario.IdTipoUsuario = 0;
                        item.Usuario.Persona.IdPersona = 0;
                        item.Usuario.Persona.Sexo.IdSexo = 0;
                        item.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                    }
                    _respuesta = lista;
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
        [Route("api/asignarusuariotipousuario_consultarporidentificadortipousuario")]
        public object asignarusuariotipousuario_consultarporidentificadortipousuario (int _identificadorTipoUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_identificadorTipoUsuario == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del tipo usuario que intenta consultar";
                }
                else
                {
                    var _tipoUsuario = _objCatalogoTipoUsuario.ConsultarTipoUsuarios().Where(c => c.Identificador == _identificadorTipoUsuario && c.Estado == true).FirstOrDefault();
                    if (_tipoUsuario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El identificador ingresado no fue encontrado para un tipo usuario";
                    }
                    else
                    {
                        var lista = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuarioPorIdentificadorTipoUsuario(_identificadorTipoUsuario).Where(c => c.Estado == true).ToList();
                        foreach (var item in lista)
                        {
                            item.IdAsignarUsuarioTipoUsuario = 0;
                            item.Usuario.IdUsuario = 0;
                            item.TipoUsuario.IdTipoUsuario = 0;
                            item.Usuario.Persona.IdPersona = 0;
                            item.Usuario.Persona.Sexo.IdSexo = 0;
                            item.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        }
                        _respuesta = lista;
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
        [Route("api/asignarusuariotipousuario_consultarnoasignadosresponsablesporcuestionariogenericoporidentificadortipousuario")]
        public object asignarusuariotipousuario_consultarnoasignadosresponsablesporcuestionariogenericoporidentificadortipousuario(string _idCuestionarioGenericoEncriptado, int _identificadorTipoUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenericoEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico";
                }
                else if (_identificadorTipoUsuario==0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del tipo usuario";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                    var lista = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuarioNoAsignadosResponsablePorCuestionarioGenericoPorIdentificadorTipoUsuario(_idCuestionarioGenerico,_identificadorTipoUsuario).Where(c => c.Estado == true && c.Usuario.Estado==true).ToList();
                    foreach (var item in lista)
                    {
                        item.IdAsignarUsuarioTipoUsuario = 0;
                        item.Usuario.IdUsuario = 0;
                        item.Usuario.Persona.IdPersona = 0;
                        item.Usuario.Persona.Sexo.IdSexo = 0;
                        item.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                    }
                    _respuesta = lista;
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
        [Route("api/asignarusuariotipousuario_insertar")]
        public object asignarusuariotipousuario_insertar(AsignarUsuarioTipoUsuario _objAsignarUsuarioTipoUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarUsuarioTipoUsuario == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto";
                }
                else if (_objAsignarUsuarioTipoUsuario.Usuario.IdUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarUsuarioTipoUsuario.Usuario.IdUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el id encriptado del usuario";
                }
                else if (_objAsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el id encriptado del tipo de usuario";
                }
                else
                {
                    _objAsignarUsuarioTipoUsuario.Usuario.IdUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarUsuarioTipoUsuario.Usuario.IdUsuarioEncriptado));
                    _objAsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuarioEncriptado));
                    _objAsignarUsuarioTipoUsuario.Estado = true;

                    var _objAsignarUsuarioTipoUsuarioExistente = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuario().Where(x => x.TipoUsuario.IdTipoUsuario == _objAsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario && x.Usuario.IdUsuario == _objAsignarUsuarioTipoUsuario.Usuario.IdUsuario).FirstOrDefault();

                    if (_objAsignarUsuarioTipoUsuarioExistente != null)
                    {
                        if (_objAsignarUsuarioTipoUsuarioExistente.Estado == true)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ya existe una asignación válida para este usuario y este tipo de usuario ";
                        }
                        else
                        {
                            int _idAsignarUsuarioTipoUsuarioModificado =_objCatalogoAsignarUsuarioTipoUsuario.CambiarEstadoAsignarUsuarioTipoUsuario(_objAsignarUsuarioTipoUsuarioExistente.IdAsignarUsuarioTipoUsuario, true);
                            if (_idAsignarUsuarioTipoUsuarioModificado == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al intentar asignar el usuario con el tipo de usuario seleccionado, intente nuevamente.";
                            }
                            else
                            {
                                var _objAsignarUsuarioTipoUsuarioActualizado = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuario().Where(c => c.IdAsignarUsuarioTipoUsuario == _objAsignarUsuarioTipoUsuarioExistente.IdAsignarUsuarioTipoUsuario).FirstOrDefault();
                                _objAsignarUsuarioTipoUsuarioActualizado.IdAsignarUsuarioTipoUsuario = 0;
                                _objAsignarUsuarioTipoUsuarioActualizado.Usuario.Persona.IdPersona = 0;
                                _objAsignarUsuarioTipoUsuarioActualizado.Usuario.IdUsuario = 0;
                                _objAsignarUsuarioTipoUsuarioActualizado.Usuario.Persona.Sexo.IdSexo = 0;
                                _objAsignarUsuarioTipoUsuarioActualizado.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                                _respuesta = _objAsignarUsuarioTipoUsuarioActualizado;
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "200").FirstOrDefault();
                            }
                        }
                    }
                    else
                    {
                        int _idAsignarUsuarioTipoUsuarioIngresado = _objCatalogoAsignarUsuarioTipoUsuario.InsertarAsignarUsuarioTipoUsuario(_objAsignarUsuarioTipoUsuario);
                        if (_idAsignarUsuarioTipoUsuarioIngresado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar asignar el usuario con el tipo de usuario seleccionado, intente nuevamente.";
                        }
                        else
                        {
                            var _objAsignarUsuarioTipoUsuarioIngresado = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuario().Where(c => c.IdAsignarUsuarioTipoUsuario == _idAsignarUsuarioTipoUsuarioIngresado).FirstOrDefault();
                            _objAsignarUsuarioTipoUsuarioIngresado.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarUsuarioTipoUsuarioIngresado.Usuario.Persona.IdPersona = 0;
                            _objAsignarUsuarioTipoUsuarioIngresado.Usuario.IdUsuario = 0;
                            _objAsignarUsuarioTipoUsuarioIngresado.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarUsuarioTipoUsuarioIngresado.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            _respuesta = _objAsignarUsuarioTipoUsuarioIngresado;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "200").FirstOrDefault();
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
        [Route("api/asignarusuariotipousuario_modificar")]
        public object asignarusuariotipousuario_modificar(AsignarUsuarioTipoUsuario _objAsignarUsuarioTipoUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el id encriptado del asignar usuario tipo usuario";
                }
                else
                {
                    int _idAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado));
                    var _objAsignarUsuarioTipoUsuarioConsultado = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuarioPorId(_idAsignarUsuarioTipoUsuario).FirstOrDefault();
                    bool _nuevoEstado = false;
                    if(_objAsignarUsuarioTipoUsuarioConsultado.Estado==false)
                    {
                        _nuevoEstado = true;
                    }
                    int _idAsignarUsuarioTipoUsuarioModificado = _objCatalogoAsignarUsuarioTipoUsuario.CambiarEstadoAsignarUsuarioTipoUsuario(_objAsignarUsuarioTipoUsuarioConsultado.IdAsignarUsuarioTipoUsuario,_nuevoEstado);
                    if (_idAsignarUsuarioTipoUsuarioModificado == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al intentar cambiar el estado del usuario con el tipo de usuario seleccionado, intente nuevamente.";
                    }
                    else
                    {
                        var _objAsignarUsuarioTipoUsuarioModificado = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuario().Where(c => c.IdAsignarUsuarioTipoUsuario == _idAsignarUsuarioTipoUsuarioModificado).FirstOrDefault();
                        _objAsignarUsuarioTipoUsuarioModificado.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarUsuarioTipoUsuarioModificado.Usuario.Persona.IdPersona = 0;
                        _objAsignarUsuarioTipoUsuarioModificado.Usuario.IdUsuario = 0;
                        _objAsignarUsuarioTipoUsuarioModificado.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarUsuarioTipoUsuarioModificado.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _respuesta = _objAsignarUsuarioTipoUsuarioModificado;
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "200").FirstOrDefault();
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
        [Route("api/asignarusuariotipousuario_eliminar")]
        public object asignarusuariotipousuario_eliminar(string _idAsignarUsuarioTipoUsuarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarUsuarioTipoUsuarioEncriptado== null || string.IsNullOrEmpty(_idAsignarUsuarioTipoUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el id encriptado del asignar usuario tipo usuario";
                }
                else
                {
                    int _idAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarUsuarioTipoUsuarioEncriptado).ToString());
                    var _objAsignarUsuarioTipoUsuario = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuarioPorId(_idAsignarUsuarioTipoUsuario).FirstOrDefault();
                    bool _nuevoEstado= false;
                    if (_objAsignarUsuarioTipoUsuario.Estado == false)
                        _nuevoEstado = true;
                    int _idAsignarUsuarioTipoUsuarioModificado = _objCatalogoAsignarUsuarioTipoUsuario.CambiarEstadoAsignarUsuarioTipoUsuario(_idAsignarUsuarioTipoUsuario, _nuevoEstado);
                    if (_idAsignarUsuarioTipoUsuarioModificado == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al intentar cambiar el estado del usuario con el tipo de usuario seleccionado, intente nuevamente.";
                    }
                    else
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(c => c.codigo == "200").FirstOrDefault();
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
