using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using API.Conexion;
using API.Models.Entidades;
using API.Models.Catalogos;
using API.Models.Metodos;



namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        CatalogoPersona _objCatalogoPersona = new CatalogoPersona();
        CatalogoUsuario _objCatalogoUsuarios = new CatalogoUsuario();
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarUsuarioTipoUsuario _objCatalogoAsignarUsuarioTipoUsuario = new CatalogoAsignarUsuarioTipoUsuario();

        CatalogoTokens catTokens = new CatalogoTokens();
        Seguridad _seguridad = new Seguridad();

        [Authorize]
        [HttpPost]
        [Route("api/usuario_insertar")]
        public object usuario_insertar(Usuario _objUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objUsuario == null || _objUsuario.Persona == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró la persona o usuario para insertar";
                }
                else if (_objUsuario.Persona.IdPersonaEncriptado == null || string.IsNullOrEmpty(_objUsuario.Persona.IdPersonaEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador";
                }
                else if (_objUsuario.Correo == null || string.IsNullOrEmpty(_objUsuario.Correo.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese un usuario";
                }
                else if (_objCatalogoUsuarios.ValidarCorreo(_objUsuario).Count > 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "El usuario existente";
                }
                else if (_objUsuario.Clave == null || string.IsNullOrEmpty(_objUsuario.Clave.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ingrese una contraseña";
                }
                else
                {
                    int _idPersona = Convert.ToInt32(_seguridad.DesEncriptar(_objUsuario.Persona.IdPersonaEncriptado));
                    var _objPersona = _objCatalogoPersona.ConsultarPersona().Where(c => c.IdPersona == _idPersona && c.Estado == true).FirstOrDefault();
                    if (_objPersona == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la persona a la que intenta asignarle un usuario.";
                    }
                    else
                    {
                        _objUsuario.Estado = true;
                        _objUsuario.Persona = _objPersona;
                        _objUsuario.Clave = _seguridad.Encriptar(_objUsuario.Clave);
                        int _idUsuarioIngresado = _objCatalogoUsuarios.InsertarUsuario(_objUsuario);
                        if (_idUsuarioIngresado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar ingresar al usuario, intente nuevamente.";
                        }
                        else
                        {
                            var _objUsuarioIngresado = _objCatalogoUsuarios.ConsultarUsuarioPorId(_idUsuarioIngresado).Where(c => c.Estado == true).FirstOrDefault();
                            _objUsuarioIngresado.IdUsuario = 0;
                            _objUsuarioIngresado.Persona.IdPersona = 0;
                            _objUsuarioIngresado.Persona.Sexo.IdSexo = 0;
                            _objUsuarioIngresado.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            _respuesta = _objUsuarioIngresado;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new
            {
                respuesta = _respuesta,
                http = _http
            };
        }


        [Authorize]
        [HttpPost]
        [Route("api/usuario_modificar")]
        public object usuario_modificar(Usuario _objUsuario)
        {

            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();

            try
            {

                if (_objUsuario == null || _objUsuario.Persona == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró la personao o usuario para modificar";
                }
                else if (_objUsuario.IdUsuarioEncriptado == null || string.IsNullOrEmpty(_objUsuario.IdUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del usuario que desea modificar.";
                }
                else if (_objUsuario.Correo == null || string.IsNullOrEmpty(_objUsuario.Correo.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el correo.";
                }
                else
                {
                    int _idUsuarioDesencriptado = Convert.ToInt32(_seguridad.DesEncriptar(_objUsuario.IdUsuarioEncriptado));
                    _objUsuario.IdUsuario = _idUsuarioDesencriptado; //  se asigna el id del usuario que ha sido desencriptado para su posterior modificacion
                    if (_objCatalogoUsuarios.ValidarCorreo(_objUsuario).Where(c => c.IdUsuario != _objUsuario.IdUsuario).ToList().Count > 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "El correo electrónico ha sido utilizado por otro usuario.";
                    }
                    else if (_objUsuario.Clave == null || string.IsNullOrEmpty(_objUsuario.Clave.Trim()))
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ingrese la contraseña";
                    }
                    else
                    {
                        int _idPersona = Convert.ToInt32(_seguridad.DesEncriptar(_objUsuario.Persona.IdPersonaEncriptado));
                        var _objPersona = _objCatalogoPersona.ConsultarPersona().Where(c => c.IdPersona == _idPersona && c.Estado == true).FirstOrDefault();
                        if (_objPersona == null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró a la persona en el sistema";
                        }
                        else
                        {
                            _objUsuario.Estado = true;

                            _objUsuario.Persona = _objPersona;
                            _objUsuario.Clave = _seguridad.Encriptar(_objUsuario.Clave);
                            int _idUsuarioModificado = _objCatalogoUsuarios.ModificarUsuario(_objUsuario);
                            if (_idUsuarioModificado == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al intentar modificar al usuario, intente nuevamente.";
                            }
                            else
                            {
                                var _objUsuarioModificado = _objCatalogoUsuarios.ConsultarUsuarioPorId(_idUsuarioDesencriptado).FirstOrDefault();
                                _objUsuarioModificado.IdUsuario = 0;
                                _objUsuarioModificado.Persona.IdPersona = 0;
                                _objUsuarioModificado.Persona.Sexo.IdSexo = 0;
                                _objUsuarioModificado.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                                _respuesta = _objUsuarioModificado;
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
            return new
            {
                respuesta = _respuesta,
                http = _http
            };




        }
<<<<<<< HEAD
=======

        [Authorize]
>>>>>>> 38defc51b45a0b8504590836b9f49951a053e090
        [HttpPost]
        [Route("api/usuario_eliminar")]
        public object usuario_eliminar(string _idUsuarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();

            try
            {
                if (_idUsuarioEncriptado == null || string.IsNullOrEmpty(_idUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del usuario a eliminar.";
                }
                else
                {
                    int _idUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_idUsuarioEncriptado));
                    var _objUsuario = _objCatalogoUsuarios.ConsultarUsuario().Where(c => c.IdUsuario == _idUsuario).FirstOrDefault();

                    if (_objUsuario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el usuario que intenta eliminar.";
                    }
                    else if (_objUsuario.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Este usuario ya ha sido utilizado, por lo tanto no lo puede eliminar.";
                    }
                    else
                    {
                        int _idUsuarioEliminado = _objCatalogoUsuarios.EliminarUsuario(_idUsuario);
                        if (_idUsuarioEliminado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar eliminar al usuario, intente nuevamente.";
                        }
                        else
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new
            {
                respuesta = _respuesta,
                http = _http
            };

        }
<<<<<<< HEAD
=======

        [Authorize]
>>>>>>> 38defc51b45a0b8504590836b9f49951a053e090
        [HttpPost]
        [Route("api/usuario_consultar")]
        public object usuario_consultar()
        {

            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();

            try
            {
                var listaUsuarios = _objCatalogoUsuarios.ConsultarUsuario();

                foreach (var item in listaUsuarios)
                {
                    item.IdUsuario = 0;
                    item.Persona.IdPersona = 0;
                    item.Persona.Sexo.IdSexo = 0;
                    item.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                }
                _respuesta = listaUsuarios;
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
<<<<<<< HEAD
=======

        [Authorize]
>>>>>>> 38defc51b45a0b8504590836b9f49951a053e090
        [HttpPost]
        [Route("api/usuario_consultar")]
        public object usuario_consultar(string _idUsuarioEncriptado)
        {

            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();

            try
            {
                if (string.IsNullOrEmpty(_idUsuarioEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                }
                else
                {
                    int _idUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_idUsuarioEncriptado));
                    var _objUsuario = _objCatalogoUsuarios.ConsultarUsuario().Where(c => c.IdUsuario == _idUsuario).FirstOrDefault();

                    if (_objUsuario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                    }
                    else
                    {
                        _objUsuario.IdUsuario = 0;
                        _objUsuario.Persona.IdPersona = 0;
                        _objUsuario.Persona.Sexo.IdSexo = 0;
                        _objUsuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                    }
                    _respuesta = _objUsuario;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };


        }
<<<<<<< HEAD
=======

        [AllowAnonymous]
>>>>>>> 38defc51b45a0b8504590836b9f49951a053e090
        [HttpPost]
        [Route("api/ValidarCorreo")]
        public object ValidarCorreo(Usuario _objUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();

            try
            {
                if (string.IsNullOrEmpty(_objUsuario.Correo.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                }
                else if (_objCatalogoUsuarios.ValidarCorreo(_objUsuario).Count == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                }
                else if (_objCatalogoUsuarios.ValidarCorreo(_objUsuario).Count > 0)
                {
                    _respuesta = _objUsuario.Correo.Trim();
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Login")]
        public object Login(Usuario _objUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            string _token = "";
            try
            {
                // calida el token de la peticion, este es una ruta para consultar asi que el identificador del token debe ser 4
                //Token _token = catTokens.Consultar().Where(x => x.Identificador == 4).FirstOrDefault();
                //string _clave_desencriptada = _seguridad.DecryptStringAES(_objUsuario.Token, _token.objClave.Descripcion);
                if (string.IsNullOrEmpty(_objUsuario.Correo.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                }
                else
                {
                    var _objUsuarioBuscado = _objCatalogoUsuarios.ValidarCorreo(_objUsuario).FirstOrDefault();
                    if (_objUsuarioBuscado!=null)
                    {
                        _objUsuarioBuscado.Clave = _seguridad.DesEncriptar(_objUsuarioBuscado.Clave);
                      //  _objUsuario.Clave = _seguridad.DesEncriptar(_objUsuario.Clave);
                    }
                    if (_objUsuarioBuscado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el correo";
                    }
                    else if (_objUsuarioBuscado.Estado == false)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Este usuario ha sido deshabilitado. Comuníquese con el administrador del sistema.";
                    }
                    else if (_seguridad.DesEncriptar(_objUsuarioBuscado.Clave) != _objUsuario.Clave)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La contraseña es incorrecta";
                    }
                    else
                    {
                        var _listaAsignarUsuarioTipoUsuario = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuario().Where(c => c.Usuario.IdUsuario == _objUsuarioBuscado.IdUsuario && c.Estado == true && c.TipoUsuario.Estado == true).ToList();
                        if (_listaAsignarUsuarioTipoUsuario.Count == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "204").FirstOrDefault();
                            _http.mensaje = "No se encontraron roles asignados a este usuario";
                        }
                        else
                        {
                            foreach (var item in _listaAsignarUsuarioTipoUsuario)
                            {
                                item.IdAsignarUsuarioTipoUsuario = 0;
                                item.TipoUsuario.IdTipoUsuario = 0;
                                item.Usuario.IdUsuario = 0;
                                item.Usuario.Persona.IdPersona = 0;
                                item.Usuario.Persona.Sexo.IdSexo = 0;
                                item.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            }
                            _respuesta = _listaAsignarUsuarioTipoUsuario;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                            _token = TokenGenerator.GenerateTokenJwt(_objUsuario.Correo);
                        }
                    }
                }
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
            if (_token != "")
            {
                return new
                {
                    respuesta = _respuesta,
                    http = _http,
                    token = _token
                };
            }
            else
            {
                return new
                {
                    respuesta = _respuesta,
                    http = _http,
                };
            }

        }
    
        [AllowAnonymous]
        [HttpPost]
        [Route("api/token/update")]
        public object updateToken(Usuario _objUsuario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            string _token = "";
            try
            {
                if (string.IsNullOrEmpty(_objUsuario.Correo.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Revise su conexión a internet";
                }
                else {
                    _token = TokenGenerator.GenerateTokenJwt(_objUsuario.Correo);
                }

                if (_token != "")
                {
                    return new
                    {
                        token = _token
                    };
                }
                else {
                    return new
                    {
                        respuesta = _http.mensaje,
                        http = _http
                    };
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
                return new
                {
                    respuesta = _http.mensaje,
                    http = _http
                };
            }

            //return "fff";
        }

        }
}