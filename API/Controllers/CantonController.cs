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
    public class CantonController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoCanton _objCatalogoCanton = new CatalogoCanton();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/canton_consultar")]
        public object canton_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaCantones = _objCatalogoCanton.ConsultarCanton().Where(c=>c.EstadoCanton==true).ToList();
                foreach (var item in _listaCantones)
                {
                    item.IdCanton = 0;
                    item.Provincia.IdProvincia = 0;
                }
                _respuesta = _listaCantones;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/canton_consultar")]
        public object canton_consultar(string _idCantonEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idCantonEncriptado) || _idCantonEncriptado==null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón";
                }
                else
                {
                    int _idCanton = Convert.ToInt32(_seguridad.DesEncriptar(_idCantonEncriptado));
                    var _objCanton = _objCatalogoCanton.ConsultarCantonPorId(_idCanton).Where(c => c.EstadoCanton == true && c.Provincia.EstadoProvincia==true).FirstOrDefault();
                    _objCanton.IdCanton = 0;
                    _objCanton.Provincia.IdProvincia = 0;
                    _respuesta = _objCanton;
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
        [Route("api/canton_consultarporidprovincia")]
        public object canton_consultarporidprovincia(string _idProvinciaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idProvinciaEncriptado) || _idProvinciaEncriptado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_idProvinciaEncriptado));
                    var _listaCantones = _objCatalogoCanton.ConsultarCantonPorIdProvincia(_idProvincia).Where(c => c.EstadoCanton == true).ToList();
                    foreach (var _objCanton in _listaCantones)
                    {
                        _objCanton.IdCanton = 0;
                        _objCanton.Provincia.IdProvincia = 0;
                    }
                    _respuesta = _listaCantones;
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
        [Route("api/canton_insertar")]
        public object canton_insertar(Canton _objCanton)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objCanton == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto canton.";
                }
                else if (_objCanton.Provincia.IdProvinciaEncriptado == "null" || string.IsNullOrEmpty(_objCanton.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Seleccione un cantón.";
                }
                else if (_objCanton.NombreCanton == "null" || string.IsNullOrEmpty(_objCanton.NombreCanton))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del cantón.";
                }
                else if (_objCatalogoCanton.ConsultarCanton().Where(c => c.NombreCanton == _objCanton.NombreCanton.Trim() && c.Provincia.IdProvincia == Convert.ToInt32(_seguridad.DesEncriptar(_objCanton.Provincia.IdProvinciaEncriptado))).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe un cantón con el mismo nombre, por favor verifique en la lista.";
                }
                else if (_objCanton.CodigoCanton == "null" || string.IsNullOrEmpty(_objCanton.CodigoCanton))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo del cantón.";
                }
                else if (_objCatalogoCanton.ConsultarCanton().Where(c => c.CodigoCanton == _objCanton.CodigoCanton.Trim()).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe un cantón con el mismo código, por favor verifique en la lista.";
                }
                else
                {
                    _objCanton.NombreCanton = _objCanton.NombreCanton.Trim();
                    _objCanton.CodigoCanton = _objCanton.CodigoCanton.Trim();
                    _objCanton.EstadoCanton = true;
                    _objCanton.Provincia.IdProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objCanton.Provincia.IdProvinciaEncriptado));
                    int _idCanton = _objCatalogoCanton.InsertarCanton(_objCanton);
                    if (_idCanton == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al intentar ingresar el cantón.";
                    }
                    else
                    {
                        var _objCantonInsertado = _objCatalogoCanton.ConsultarCantonPorId(_idCanton).Where(c => c.EstadoCanton == true).FirstOrDefault();
                        _objCantonInsertado.IdCanton = 0;
                        _objCantonInsertado.Provincia.IdProvincia = 0;
                        _respuesta = _objCantonInsertado;
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
        [Route("api/canton_modificar")]
        public object canton_modificiar(Canton _objCanton)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objCanton == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto canton.";
                }
                else if (_objCanton.IdCantonEncriptado == "null" || string.IsNullOrEmpty(_objCanton.IdCantonEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón.";
                }
                else if (_objCanton.Provincia.IdProvinciaEncriptado == "null" || string.IsNullOrEmpty(_objCanton.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia a la que pertenece el cantón.";
                }
                else if (_objCanton.NombreCanton == "null" || string.IsNullOrEmpty(_objCanton.NombreCanton))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del cantón.";
                }
                else if (_objCanton.CodigoCanton == "null" || string.IsNullOrEmpty(_objCanton.CodigoCanton))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo del cantón.";
                }
                else
                {
                    int _idCanton = Convert.ToInt32(_seguridad.DesEncriptar(_objCanton.IdCantonEncriptado));
                    var _objCantonConsultado= _objCatalogoCanton.ConsultarCantonPorId(_idCanton).FirstOrDefault();
                    if (_objCantonConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El cantón que intenta modificar no existe.";
                    }
                    else if (_objCatalogoCanton.ConsultarCanton().Where(c => c.NombreCanton == _objCanton.NombreCanton.Trim() && c.IdCanton!=_idCanton && c.Provincia.IdProvincia == Convert.ToInt32(_seguridad.DesEncriptar(_objCanton.Provincia.IdProvinciaEncriptado))).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe un cantón con el mismo nombre, por favor verifique en la lista.";
                    }
                    else if (_objCatalogoCanton.ConsultarCanton().Where(c => c.CodigoCanton == _objCanton.CodigoCanton.Trim() && c.IdCanton != _idCanton).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe un cantón con el mismo código, por favor verifique en la lista.";
                    }
                    else
                    {
                        _objCanton.NombreCanton = _objCanton.NombreCanton.Trim();
                        _objCanton.CodigoCanton = _objCanton.CodigoCanton.Trim();
                        _objCanton.IdCanton = _idCanton;
                        _objCanton.Provincia.IdProvincia= Convert.ToInt32(_seguridad.DesEncriptar(_objCanton.Provincia.IdProvinciaEncriptado));
                        _objCanton.EstadoCanton = true;
                        int _idCantonModificado = _objCatalogoCanton.ModificarCanton(_objCanton);
                        if (_idCantonModificado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar modificar el cantón.";
                        }
                        else
                        {
                            var _objCantonModificado = _objCatalogoCanton.ConsultarCantonPorId(_idCanton).FirstOrDefault();
                            _objCantonModificado.IdCanton = 0;
                            _objCantonModificado.Provincia.IdProvincia = 0;
                            _respuesta = _objCantonModificado;
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
        [Route("api/canton_cambiarestado")]
        public object canton_cambiarestado(Canton _objCanton)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objCanton == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto cantón.";
                }
                else if (_objCanton.IdCantonEncriptado == null || string.IsNullOrEmpty(_objCanton.IdCantonEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón que va a modificar.";
                }
                else
                {
                    int _idCanton = Convert.ToInt32(_seguridad.DesEncriptar(_objCanton.IdCantonEncriptado));
                    var _objCantonConsultado = _objCatalogoCanton.ConsultarCantonPorId(_idCanton).FirstOrDefault();
                    if (_objCantonConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El cantón que intenta modificar no existe.";
                    }
                    else
                    {
                        bool _nuevoEstado = false;
                        if (_objCantonConsultado.EstadoCanton == false)
                        {
                            _nuevoEstado = true;
                        }
                        _objCantonConsultado.EstadoCanton = _nuevoEstado;
                        int _idCantonModificado = _objCatalogoCanton.ModificarCanton(_objCantonConsultado);
                        if (_idCantonModificado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar cambiar el estado del cantón.";
                        }
                        else
                        {
                            var _objCantonModificado = _objCatalogoCanton.ConsultarCantonPorId(_idCanton).FirstOrDefault();
                            _objCantonModificado.IdCanton = 0;
                            _objCantonModificado.Provincia.IdProvincia = 0;
                            _respuesta = _objCantonModificado;
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
        [Route("api/canton_eliminar")]
        public object canton_eliminar(string _idCantonEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCantonEncriptado == null || string.IsNullOrEmpty(_idCantonEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón que va a eliminar.";
                }
                else
                {
                    int _idCanton = Convert.ToInt32(_seguridad.DesEncriptar(_idCantonEncriptado));
                    var _objCantonConsultado = _objCatalogoCanton.ConsultarCantonPorId(_idCanton).FirstOrDefault();
                    if (_objCantonConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El cantón que intenta eliminar no existe.";
                    }
                    else if(_objCantonConsultado.Utilizado=="1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "El cantón ya es utilizado, por la tanto no puede ser eliminado.";
                    }
                    else
                    {
                        _objCatalogoCanton.EliminarCanton(_idCanton);
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