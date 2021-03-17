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
    public class SeccionController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoComponente _objCatalogoComponente = new CatalogoComponente();
        CatalogoSeccion _objCatalogoSeccion = new CatalogoSeccion();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/seccion_insertar")]
        public object seccion_insertar(Seccion _objSeccion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objSeccion == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto Seccion";
                }
                else if (_objSeccion.Componente.IdComponenteEncriptado== null || string.IsNullOrEmpty(_objSeccion.Componente.IdComponenteEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente";
                }
                else if (_objSeccion.Descripcion == null || string.IsNullOrEmpty(_objSeccion.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción de la Sección";
                }
                else if (_objSeccion.Orden == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el orden de la Sección";
                }
                else
                {
                    int _idComponente = Convert.ToInt32(_seguridad.DesEncriptar(_objSeccion.Componente.IdComponenteEncriptado));
                    var _objComponente = _objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objComponente == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el componente en el sistema";
                    }
                    else
                    {
                        if (_objCatalogoSeccion.ConsultarSeccionPorIdComponente(_idComponente).Where(c => c.Estado == true && c.Descripcion == _objSeccion.Descripcion.Trim()).ToList().Count > 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                            _http.mensaje = "Ya existe una sección en este cuestionario con la misma descripción, verifique en la lista.";
                        }
                        else
                        {
                            _objSeccion.Descripcion = _objSeccion.Descripcion.Trim();
                            _objSeccion.Componente.IdComponente= _idComponente;
                            _objSeccion.Estado = true;
                            int _idSeccion = _objCatalogoSeccion.InsertarSeccion(_objSeccion);
                            if (_idSeccion == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de ingresar la sección.";
                            }
                            else
                            {
                                var _objSeccionIngresado = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(C => C.Estado == true).FirstOrDefault();
                                _objSeccionIngresado.IdSeccion = 0;
                                _objSeccionIngresado.Componente.IdComponente= 0;
                                _objSeccionIngresado.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                                _respuesta = _objSeccionIngresado;
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
        [Route("api/seccion_modificar")]
        public object seccion_modificar(Seccion _objSeccion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objSeccion == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto Seccion";
                }
                else if (_objSeccion.IdSeccionEncriptado == null || string.IsNullOrEmpty(_objSeccion.IdSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del Seccion";
                }
                else if (_objSeccion.Componente.IdComponenteEncriptado== null || string.IsNullOrEmpty(_objSeccion.Componente.IdComponenteEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente";
                }
                else if (_objSeccion.Descripcion == null || string.IsNullOrEmpty(_objSeccion.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción de la sección";
                }
                else if (_objSeccion.Orden == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el orden de la sección";
                }
                else
                {
                    int _idComponente = Convert.ToInt32(_seguridad.DesEncriptar(_objSeccion.Componente.IdComponenteEncriptado));
                    var _objComponente = _objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objComponente == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el componente en el sistema";
                    }
                    else
                    {
                        int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_objSeccion.IdSeccionEncriptado));
                        if (_objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(c => c.Estado == true).FirstOrDefault() == null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró el Seccion que intenta modificar";
                        }
                        else
                        {
                            if (_objCatalogoSeccion.ConsultarSeccionPorIdComponente(_idComponente).Where(c => c.Estado == true && c.IdSeccion != _idSeccion && c.Descripcion == _objSeccion.Descripcion.Trim()).ToList().Count > 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                                _http.mensaje = "Ya existe un Seccion en este cuestionario con la misma descripción, verifique en la lista.";
                            }
                            else
                            {
                                _objSeccion.IdSeccion = _idSeccion;
                                _objSeccion.Descripcion = _objSeccion.Descripcion.Trim();
                                _objSeccion.Componente.IdComponente= _idComponente;
                                _objSeccion.Estado = true;
                                _idSeccion = _objCatalogoSeccion.ModificarSeccion(_objSeccion);
                                if (_idSeccion == 0)
                                {
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "Ocurrió un error al tratar de modificar el Seccion.";
                                }
                                else
                                {
                                    var _objSeccionModificado = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(C => C.Estado == true).FirstOrDefault();
                                    _objSeccionModificado.IdSeccion = 0;
                                    _objSeccionModificado.Componente.IdComponente = 0;
                                    _objSeccionModificado.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                                    _respuesta = _objSeccionModificado;
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                                }
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
        [Route("api/subir_seccion")]
        public object subir_seccion(Seccion _objSeccion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objSeccion == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto sección";
                }
                else if (_objSeccion.IdSeccionEncriptado == null || string.IsNullOrEmpty(_objSeccion.IdSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }

                else
                {
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_objSeccion.IdSeccionEncriptado));


                    _objSeccion.IdSeccion = _idSeccion;
                    _objSeccion.Estado = true;
                    _idSeccion = _objCatalogoSeccion.SubirSeccion(_objSeccion);
                    if (_idSeccion == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ordenar la sección";
                    }
                    else
                    {
                        _objSeccion = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(c => c.Estado == true).FirstOrDefault();



                        _respuesta = _objSeccion;
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
        [Route("api/bajar_seccion")]
        public object bajar_seccion(Seccion _objSeccion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objSeccion == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto sección";
                }
                else if (_objSeccion.IdSeccionEncriptado == null || string.IsNullOrEmpty(_objSeccion.IdSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }

                else
                {
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_objSeccion.IdSeccionEncriptado));


                    _objSeccion.IdSeccion = _idSeccion;
                    _objSeccion.Estado = true;
                    _idSeccion = _objCatalogoSeccion.BajarSeccion(_objSeccion);
                    if (_idSeccion == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ordenar la sección";
                    }
                    else
                    {
                        _objSeccion = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(c => c.Estado == true).FirstOrDefault();



                        _respuesta = _objSeccion;
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
        [Route("api/seccion_consultarporidcomponente")]
        public object seccion_consultarporidcomponente(string _idComponenteEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idComponenteEncriptado == null || string.IsNullOrEmpty(_idComponenteEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente";
                }
                else
                {
                    int _idComponente = Convert.ToInt32(_seguridad.DesEncriptar(_idComponenteEncriptado));
                    var _objComponente = _objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objComponente == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el componente en el sistema";
                    }
                    else
                    {
                        var _listaSecciones = _objCatalogoSeccion.ConsultarSeccionPorIdComponente(_idComponente).Where(c => c.Estado == true).ToList();
                        foreach (var _objSeccion in _listaSecciones)
                        {
                            _objSeccion.IdSeccion = 0;
                            _objSeccion.Componente.IdComponente = 0;
                            _objSeccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _listaSecciones;
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
        [Route("api/seccion_consultar")]
        public object seccion_consultar(string _idSeccionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idSeccionEncriptado == null || string.IsNullOrEmpty(_idSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }
                else
                {
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_idSeccionEncriptado));
                    var _objSeccion = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objSeccion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la sección en el sistema";
                    }
                    else
                    {
                        _objSeccion.IdSeccion = 0;
                        _objSeccion.Componente.IdComponente = 0;
                        _objSeccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objSeccion;
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
        [Route("api/Seccion_eliminar")]
        public object Seccion_eliminar(string _idSeccionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idSeccionEncriptado == null || string.IsNullOrEmpty(_idSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }
                else
                {
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_idSeccionEncriptado));
                    var _objSeccion = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objSeccion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la sección en el sistema";
                    }
                    else if(_objSeccion.Utilizado=="1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No se puede eliminar la sección porque ya ha sido utilizada";
                    }
                    else
                    {
                        _objCatalogoSeccion.EliminarSeccion(_idSeccion);
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
