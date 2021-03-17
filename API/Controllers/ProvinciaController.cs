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
    public class ProvinciaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoProvincia _objCatalogoProvincia = new CatalogoProvincia();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/provincia_consultar")]
        public object provincia_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaProvincias = _objCatalogoProvincia.ConsultarProvincia().Where(c=>c.EstadoProvincia==true).ToList();
                foreach (var item in _listaProvincias)
                {
                    item.IdProvincia = 0;
                }
                _respuesta = _listaProvincias;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/provincia_consultar")]
        public object provincia_consultar(string _idProvinciaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pronvicia.";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_idProvinciaEncriptado));
                    var _objProvincia = _objCatalogoProvincia.ConsultarProvinciaPorId(_idProvincia).Where(c => c.EstadoProvincia == true).FirstOrDefault();
                    _objProvincia.IdProvincia = 0;
                    _respuesta = _objProvincia;
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
        [Route("api/provincia_insertar")]
        public object provincia_insertar(Provincia _objProvincia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objProvincia == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto provincia.";
                }
                else if (_objProvincia.NombreProvincia == null || string.IsNullOrEmpty(_objProvincia.NombreProvincia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre de la provincia.";
                }
                else if (_objCatalogoProvincia.ConsultarProvincia().Where(c => c.NombreProvincia == _objProvincia.NombreProvincia).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe una provincia con el mismo nombre, por favor verifique en la lista.";
                }
                else if (_objProvincia.CodigoProvincia==null || string.IsNullOrEmpty(_objProvincia.CodigoProvincia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo de la provincia.";
                }
                else if (_objCatalogoProvincia.ConsultarProvincia().Where(c => c.CodigoProvincia == _objProvincia.CodigoProvincia).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe una provincia con el mismo código, por favor verifique en la lista.";
                }
                else
                {
                    {
                        _objProvincia.EstadoProvincia = true;
                        int _idProvincia = _objCatalogoProvincia.InsertarProvincia(_objProvincia);
                        if (_idProvincia == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar ingresar la provincia.";
                        }
                        else
                        {
                            var _objProvinciaInsertado = _objCatalogoProvincia.ConsultarProvinciaPorId(_idProvincia).Where(c => c.EstadoProvincia == true).FirstOrDefault();
                            _objProvinciaInsertado.IdProvincia = 0;
                            _respuesta = _objProvinciaInsertado;
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
        [Route("api/provincia_modificar")]
        public object provincia_modificar(Provincia _objProvincia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objProvincia == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto provincia.";
                }
                else if (_objProvincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objProvincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia que va a modificar.";
                }
                else if (_objProvincia.NombreProvincia == null || string.IsNullOrEmpty(_objProvincia.NombreProvincia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre de la provincia.";
                }
                else if (_objProvincia.CodigoProvincia == null || string.IsNullOrEmpty(_objProvincia.CodigoProvincia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo de la provincia.";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objProvincia.IdProvinciaEncriptado));
                    if (_objCatalogoProvincia.ConsultarProvincia().Where(c => c.NombreProvincia == _objProvincia.NombreProvincia && c.IdProvincia!=_idProvincia).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe una provincia con el mismo nombre, por favor verifique en la lista.";
                    }
                    else if (_objCatalogoProvincia.ConsultarProvincia().Where(c => c.CodigoProvincia == _objProvincia.CodigoProvincia && c.IdProvincia != _idProvincia).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe una provincia con el mismo código, por favor verifique en la lista.";
                    }
                    else
                    {
                        _objProvincia.IdProvincia = _idProvincia;
                        _objProvincia.EstadoProvincia = true;
                        int _idProvinciaModficado = _objCatalogoProvincia.ModificarProvincia(_objProvincia);
                        if (_idProvinciaModficado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar modificar la provincia.";
                        }
                        else
                        {
                            var _objProvinciaModificado = _objCatalogoProvincia.ConsultarProvinciaPorId(_idProvincia).Where(c => c.EstadoProvincia == true).FirstOrDefault();
                            _objProvinciaModificado.IdProvincia = 0;
                            _respuesta = _objProvinciaModificado;
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
        [Route("api/provincia_cambiarestado")]
        public object provincia_cambiarestado(Provincia _objProvincia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objProvincia == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto provincia.";
                }
                else if (_objProvincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objProvincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia que va a modificar.";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objProvincia.IdProvinciaEncriptado));
                    var _objProvinciaConsultado= _objCatalogoProvincia.ConsultarProvinciaPorId(_idProvincia).FirstOrDefault();
                    if (_objProvinciaConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La provincia que intenta modfiicar no existe";
                    }
                    else
                    {
                        bool _nuevoEstado = false;
                        if (_objProvinciaConsultado.EstadoProvincia == false)
                        {
                            _nuevoEstado = true;
                        }
                        _objProvinciaConsultado.EstadoProvincia = _nuevoEstado;
                        int _idProvinciaModficado = _objCatalogoProvincia.ModificarProvincia(_objProvinciaConsultado);
                        if (_idProvinciaModficado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar cambiar el estado de la provincia.";
                        }
                        else
                        {
                            var _objProvinciaModificado = _objCatalogoProvincia.ConsultarProvinciaPorId(_idProvincia).FirstOrDefault();
                            _objProvinciaModificado.IdProvincia = 0;
                            _respuesta = _objProvinciaModificado;
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
        [Route("api/provincia_eliminar")]
        public object provincia_eliminar(string _idProvinciaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idProvinciaEncriptado == null || string.IsNullOrEmpty(_idProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia que va a eliminar.";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_idProvinciaEncriptado));
                    var _objProvinciaConsultado = _objCatalogoProvincia.ConsultarProvinciaPorId(_idProvincia).FirstOrDefault();
                    if (_objProvinciaConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la provincia que intenta eliminar.";
                    }
                    else if (_objProvinciaConsultado.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La provincia ya es utilizada, por la tanto no puede ser eliminada.";
                    }
                    else
                    {
                        _objCatalogoProvincia.EliminarProvincia(_idProvincia);
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