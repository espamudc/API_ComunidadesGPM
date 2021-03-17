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
    public class ModeloGenericoController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoModeloGenerico _objCatalogoModeloGenerico = new CatalogoModeloGenerico();
        Seguridad _seguridad = new Seguridad();


        [HttpPost]
        [Route("api/ModeloGenerico_insertar")]
        public object ModeloGenerico_insertar(ModeloGenerico _objModeloGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloGenerico == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto modelo genérico";
                }
                else if (_objModeloGenerico.Nombre == null || string.IsNullOrEmpty(_objModeloGenerico.Nombre.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del modelo genérico";
                }
                else if (_objModeloGenerico.Descripcion == null || string.IsNullOrEmpty(_objModeloGenerico.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del modelo genérico";
                }
                else
                {
                    _objModeloGenerico.Nombre = _objModeloGenerico.Nombre.Trim();
                    _objModeloGenerico.Estado = true;
                    ModeloGenerico _ModeloGenerico = new ModeloGenerico();
                    _ModeloGenerico = _objCatalogoModeloGenerico.ConsultarModeloGenericoPorNombre(_objModeloGenerico.Nombre).FirstOrDefault();
                    if (_ModeloGenerico == null)
                    {
                        int _idModeloGenerico = _objCatalogoModeloGenerico.InsertarModeloGenerico(_objModeloGenerico);
                        if (_idModeloGenerico == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de ingresar el modelo generico.";
                        }
                        else
                        {
                            var _objModeloGenericoIngresado = _objCatalogoModeloGenerico.ConsultarModeloGenericoPorId(_idModeloGenerico).Where(C => C.Estado == true).FirstOrDefault();
                            _objModeloGenericoIngresado.IdModeloGenerico = 0;
                            _respuesta = _objModeloGenericoIngresado;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }
                    else
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe un modelo con el mismo nombre, verifique en la lista.";
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
        [Route("api/ModeloGenerico_consultar")]
        public object ModeloGenerico_consultar(string _idModeloGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaModeloGenerico = _objCatalogoModeloGenerico.ConsultarModeloGenericoPorId(int.Parse(_seguridad.DesEncriptar(_idModeloGenerico))).FirstOrDefault();
                if (_listaModeloGenerico != null)
                {
                    _listaModeloGenerico.IdModeloGenerico = 0;
                }
                _respuesta = _listaModeloGenerico;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/ModeloGenerico_eliminar")]
        public object ModeloGenerico_eliminar(ModeloGenerico _objModeloGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloGenerico.IdModeloGenericoEncriptado == null || string.IsNullOrEmpty(_objModeloGenerico.IdModeloGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del modelo generico que va a eliminar.";
                }
                else
                {
                    _objModeloGenerico.IdModeloGenericoEncriptado = _seguridad.DesEncriptar(_objModeloGenerico.IdModeloGenericoEncriptado);
                    var _objBuscadoModeloGenerico = _objCatalogoModeloGenerico.ConsultarModeloGenericoPorId(int.Parse(_objModeloGenerico.IdModeloGenericoEncriptado)).FirstOrDefault();
                    if (_objBuscadoModeloGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El modelo que intenta eliminar no existe.";
                    }
                    else if (_objBuscadoModeloGenerico.ModeloGenericoVersionadoUtilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "El modelo generico ya esta utilizado, por la tanto no puede ser eliminado.";
                    }
                    else
                    {
                        _objCatalogoModeloGenerico.EliminarModeloGenerico(int.Parse(_objModeloGenerico.IdModeloGenericoEncriptado));
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
        [Route("api/ModeloGenerico_consultarTodos")]
        public object ModeloGenerico_consultarTodos()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaModeloGenerico = _objCatalogoModeloGenerico.ConsultarModeloGenericoTodos().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaModeloGenerico)
                {
                    item.IdModeloGenerico = 0;
                }
                _respuesta = _listaModeloGenerico;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/ModeloGenerico_modificar")]
        public object ModeloGenerico_modificar(ModeloGenerico _objModeloGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloGenerico == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto modelo genérico";
                }
                else if (_objModeloGenerico.IdModeloGenericoEncriptado == null || string.IsNullOrEmpty(_objModeloGenerico.IdModeloGenericoEncriptado.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el id del modelo genérico";
                }
                else if (_objModeloGenerico.Nombre == null || string.IsNullOrEmpty(_objModeloGenerico.Nombre.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del modelo genérico";
                }
                else if (_objModeloGenerico.Descripcion == null || string.IsNullOrEmpty(_objModeloGenerico.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del modelo genérico";
                }
                else
                {
                    _objModeloGenerico.IdModeloGenericoEncriptado = _seguridad.DesEncriptar(_objModeloGenerico.IdModeloGenericoEncriptado);
                    _objModeloGenerico.IdModeloGenerico = int.Parse(_objModeloGenerico.IdModeloGenericoEncriptado);
                    _objModeloGenerico.Nombre = _objModeloGenerico.Nombre.Trim();
                    _objModeloGenerico.Estado = true;
                    ModeloGenerico _ModeloGenerico = new ModeloGenerico();
                    _ModeloGenerico = _objCatalogoModeloGenerico.ConsultarModeloGenericoPorId(_objModeloGenerico.IdModeloGenerico).FirstOrDefault();
                    if (_ModeloGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La caracterización que intenta modificar no existe.";
                    }
                    else
                    {
                        _objModeloGenerico.IdModeloGenerico = _ModeloGenerico.IdModeloGenerico;
                        if (_ModeloGenerico.Nombre.Trim().ToUpper() != _objModeloGenerico.Nombre.Trim().ToUpper())
                        {
                            _ModeloGenerico = new ModeloGenerico();
                            _ModeloGenerico = _objCatalogoModeloGenerico.ConsultarModeloGenericoPorNombre(_objModeloGenerico.Nombre).FirstOrDefault();
                            if (_ModeloGenerico != null)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                                _http.mensaje = "Ya existe un modelo con el mismo nombre, verifique en la lista.";
                                return new { respuesta = _respuesta, http = _http };
                            }
                        }
                        int _idModeloGenerico = _objCatalogoModeloGenerico.ModificarModeloGenerico(_objModeloGenerico);
                        if (_idModeloGenerico == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de ingresar el modelo generico.";
                        }
                        else
                        {
                            var _objModeloGenericoIngresado = _objCatalogoModeloGenerico.ConsultarModeloGenericoPorId(_idModeloGenerico).Where(C => C.Estado == true).FirstOrDefault();
                            _objModeloGenericoIngresado.IdModeloGenerico = 0;
                            _respuesta = _objModeloGenericoIngresado;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
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
        [Route("api/ConsultarModeloGenericoParaPublicar")]
        public object ConsultarModeloGenericoParaPublicar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaModeloGenerico = _objCatalogoModeloGenerico.ConsultarModeloGenericoParaPublicar();
                _respuesta = _listaModeloGenerico;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
    }
}
