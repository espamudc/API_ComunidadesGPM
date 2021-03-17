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
    public class ComponenteController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoCuestionarioGenerico _objCatalogoCuestionarioGenerico = new CatalogoCuestionarioGenerico();
        CatalogoComponente _objCatalogoComponente = new CatalogoComponente();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/componente_insertar")]
        public object componente_insertar(Componente _objComponente)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objComponente == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto componente";
                }
                else if (_objComponente.CuestionarioGenerico.IdCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_objComponente.CuestionarioGenerico.IdCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico";
                }
                else if (_objComponente.Descripcion == null || string.IsNullOrEmpty(_objComponente.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del componente";
                }
                else if (_objComponente.Orden == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el orden del componente";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_objComponente.CuestionarioGenerico.IdCuestionarioGenericoEncriptado));
                    var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCuestionarioGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el cuestionario genérico en el sistema";
                    }
                    else
                    {
                        if (_objCatalogoComponente.ConsultarComponentePorIdCuestionarioGenerico(_idCuestionarioGenerico).Where(c => c.Estado == true && c.Descripcion == _objComponente.Descripcion.Trim()).ToList().Count > 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                            _http.mensaje = "Ya existe un componente en este cuestionario con la misma descripción, verifique en la lista.";
                        }
                        else
                        {
                            _objComponente.Descripcion = _objComponente.Descripcion.Trim();
                            _objComponente.CuestionarioGenerico.IdCuestionarioGenerico = _idCuestionarioGenerico;
                            _objComponente.Estado = true;
                            int _idComponente = _objCatalogoComponente.InsertarComponente(_objComponente);
                            if (_idComponente == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de ingresar el componente.";
                            }
                            else
                            {
                                var _objComponenteIngresado = _objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(C => C.Estado == true).FirstOrDefault();
                                _objComponenteIngresado.IdComponente = 0;
                                _objComponenteIngresado.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                                _respuesta = _objComponenteIngresado;
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
        [Route("api/componente_modificar")]
        public object componente_modificar(Componente _objComponente)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objComponente == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto componente";
                }
                else if (_objComponente.IdComponenteEncriptado == null || string.IsNullOrEmpty(_objComponente.IdComponenteEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente";
                }
                else if (_objComponente.CuestionarioGenerico.IdCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_objComponente.CuestionarioGenerico.IdCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico";
                }
                else if (_objComponente.Descripcion == null || string.IsNullOrEmpty(_objComponente.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del componente";
                }
                else if (_objComponente.Orden == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el orden del componente";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_objComponente.CuestionarioGenerico.IdCuestionarioGenericoEncriptado));
                    var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCuestionarioGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el cuestionario genérico en el sistema";
                    }
                    else
                    {
                        int _idComponente = Convert.ToInt32(_seguridad.DesEncriptar(_objComponente.IdComponenteEncriptado));
                        if (_objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(c => c.Estado == true).FirstOrDefault()==null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró el componente que intenta modificar";
                        }
                        else
                        {
                            if (_objCatalogoComponente.ConsultarComponentePorIdCuestionarioGenerico(_idCuestionarioGenerico).Where(c => c.Estado == true && c.IdComponente!=_idComponente && c.Descripcion == _objComponente.Descripcion.Trim()).ToList().Count > 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                                _http.mensaje = "Ya existe un componente en este cuestionario con la misma descripción, verifique en la lista.";
                            }
                            else
                            {
                                _objComponente.IdComponente = _idComponente;
                                _objComponente.Descripcion = _objComponente.Descripcion.Trim();
                                _objComponente.CuestionarioGenerico.IdCuestionarioGenerico = _idCuestionarioGenerico;
                                _objComponente.Estado = true;
                                _idComponente = _objCatalogoComponente.ModificarComponente(_objComponente);
                                if (_idComponente == 0)
                                {
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "Ocurrió un error al tratar de modificar el componente.";
                                }
                                else
                                {
                                    var _objComponenteModificado = _objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(C => C.Estado == true).FirstOrDefault();
                                    _objComponenteModificado.IdComponente = 0;
                                    _objComponenteModificado.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                                    _respuesta = _objComponenteModificado;
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
        [Route("api/subir_componente")]
        public object subir_componente(Componente _objComponente)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objComponente == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto componente";
                }
                else if (_objComponente.IdComponenteEncriptado == null || string.IsNullOrEmpty(_objComponente.IdComponenteEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente";
                }

                else
                {
                    int _idComponente = Convert.ToInt32(_seguridad.DesEncriptar(_objComponente.IdComponenteEncriptado));


                    _objComponente.IdComponente = _idComponente;
                    _objComponente.Estado = true;
                    _idComponente = _objCatalogoComponente.SubirComponente(_objComponente);
                    if (_idComponente == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ordenar el componente";
                    }
                    else
                    {
                        _objComponente = _objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(c => c.Estado == true).FirstOrDefault();
                      
                    
                      
                        _respuesta = _objComponente;
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
        [Route("api/bajar_componente")]
        public object bajar_componente(Componente _objComponente)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objComponente == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto componente";
                }
                else if (_objComponente.IdComponenteEncriptado == null || string.IsNullOrEmpty(_objComponente.IdComponenteEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente";
                }

                else
                {
                    int _idComponente = Convert.ToInt32(_seguridad.DesEncriptar(_objComponente.IdComponenteEncriptado));


                    _objComponente.IdComponente = _idComponente;
                    _objComponente.Estado = true;
                    _idComponente = _objCatalogoComponente.BajarComponente(_objComponente);
                    if (_idComponente == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ordenar el componente";
                    }
                    else
                    {
                        _objComponente = _objCatalogoComponente.ConsultarComponentePorId(_idComponente).Where(c => c.Estado == true).FirstOrDefault();



                        _respuesta = _objComponente;
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
        [Route("api/componente_consultarporidcuestionariogenerico")]
        public object componente_consultarporidcuestionariogenerico(string _idCuestionarioGenerioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCuestionarioGenerioEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenerioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenerioEncriptado));
                    var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCuestionarioGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el cuestionario genérico en el sistema";
                    }
                    else
                    {
                        var _listaComponentes = _objCatalogoComponente.ConsultarComponentePorIdCuestionarioGenerico(_idCuestionarioGenerico).Where(c => c.Estado == true).ToList();
                        foreach (var _objComponente in _listaComponentes)
                        {
                            _objComponente.IdComponente = 0;
                            _objComponente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _listaComponentes;
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
        [Route("api/componente_consultar")]
        public object componente_consultar(string _idComponenteEncriptado)
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
                        _objComponente.IdComponente = 0;
                        _objComponente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objComponente;
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
        [Route("api/componente_eliminar")]
        public object componente_eliminar(string _idComponenteEncriptado)
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
                    else if(_objComponente.Utilizado=="1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No se puede eliminar el componente porque ya ha sido utilizado";
                    }
                    else
                    {
                        _objCatalogoComponente.EliminarComponente(_idComponente);
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
        [HttpGet]
        [Route("api/componente/cuestionario")]
        public object componente_consultarporidcuestionario(string _idCuestionarioGenerioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCuestionarioGenerioEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenerioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenerioEncriptado));
                    //var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(c => c.Estado == true).FirstOrDefault();
                    //if (_objCuestionarioGenerico == null)
                    //{
                    //    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                    //    _http.mensaje = "No se encontró el cuestionario genérico en el sistema";
                    //}
                    //else
                    //{
                        var _listaComponentes = _objCatalogoComponente.ComponentePorIdCuestionario(_idCuestionarioGenerico).ToList();
                        _respuesta = _listaComponentes;
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                    //}
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
