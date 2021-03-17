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
    public class PrefectoController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoPrefecto _objCatalogoPrefecto = new CatalogoPrefecto();
        CatalogoProvincia _objCatalogoProvincia = new CatalogoProvincia();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/prefecto_consultar")]
        public object prefecto_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaPrefecto = _objCatalogoPrefecto.ConsultarPrefecto().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaPrefecto)
                {
                    item.IdPrefecto = 0;
                    item.Provincia.IdProvincia = 0;
                }
                _respuesta = _listaPrefecto;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/prefecto_consultar")]
        public object prefecto_consultar(string _idPrefectoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPrefectoEncriptado == null || string.IsNullOrEmpty(_idPrefectoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del prefecto";
                }
                else
                {
                    int _idPrefecto = Convert.ToInt32(_seguridad.DesEncriptar(_idPrefectoEncriptado));
                    var _objPrefecto = _objCatalogoPrefecto.ConsultarPrefectoPorId(_idPrefecto).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPrefecto == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el prefecto registrado.";
                    }
                    else
                    {
                        _objPrefecto.IdPrefecto = 0;
                        _objPrefecto.Provincia.IdProvincia = 0;
                        _respuesta = _objPrefecto;
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
        [Route("api/prefecto_consultarporidprovincia")]
        public object prefecto_consultarporidprovincia(string _idProvinciaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idProvinciaEncriptado == null || string.IsNullOrEmpty(_idProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia.";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_idProvinciaEncriptado));
                    var _objProvincia = _objCatalogoProvincia.ConsultarProvinciaPorId(_idProvincia).Where(c => c.EstadoProvincia == true).FirstOrDefault();
                    if (_objProvincia == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la provincia.";
                    }
                    else
                    {
                        var _listaPrefectos = _objCatalogoPrefecto.ConsultarPrefectoPorIdProvincia(_idProvincia).Where(c => c.Estado == true && c.Provincia.EstadoProvincia == true).OrderBy(c=>c.FechaIngreso).ToList();
                        foreach (var item in _listaPrefectos)
                        {
                            item.IdPrefecto = 0;
                            item.Provincia.IdProvincia = 0;
                        }
                        _respuesta = _listaPrefectos;
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
        [Route("api/prefecto_insertar")]
        public object prefecto_insertar(Prefecto _objPrefecto)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPrefecto == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto prefecto";
                }
                else if (_objPrefecto.Provincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objPrefecto.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la provincia";
                }
                else if (_objPrefecto.Representante == null || string.IsNullOrEmpty(_objPrefecto.Representante))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del representante";
                }
                else if (_objPrefecto.FechaIngreso.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de ingreso";
                }
                else if (_objPrefecto.FechaSalida != null && (DateTime.Compare(_objPrefecto.FechaIngreso, Convert.ToDateTime(_objPrefecto.FechaSalida)) == 1 || DateTime.Compare(_objPrefecto.FechaIngreso, Convert.ToDateTime(_objPrefecto.FechaSalida)) == 0))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de ingreso debe ser menor a la fecha de salida";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objPrefecto.Provincia.IdProvinciaEncriptado));
                    var _objUltimoPrefectoSinSalida = _objCatalogoPrefecto.ConsultarPrefectoPorIdProvincia(_idProvincia).Where(c => c.Estado == true && c.FechaSalida==null).FirstOrDefault();
                    if (_objUltimoPrefectoSinSalida != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No puede ingresar un nuevo prefecto, mientras no haya registrado la fecha de salida de "+_objUltimoPrefectoSinSalida.Representante.ToUpper();
                    }
                    else
                    {
                        var _objUltimoPrefectoConSalida = _objCatalogoPrefecto.ConsultarPrefectoPorIdProvincia(_idProvincia).Where(c => c.Estado == true).OrderByDescending(c => c.FechaSalida).FirstOrDefault();
                        if (_objUltimoPrefectoConSalida!=null && ( DateTime.Compare(Convert.ToDateTime(_objUltimoPrefectoConSalida.FechaSalida), _objPrefecto.FechaIngreso) > 0))
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La fecha de ingreso del nuevo prefecto debe ser mayor a la fecha de salida de "+_objUltimoPrefectoConSalida.Representante.ToUpper();
                        }
                        else
                        {
                            _objPrefecto.Estado = true;
                            _objPrefecto.Provincia.IdProvincia = _idProvincia;
                            int _idPrefecto = _objCatalogoPrefecto.InsertarPrefecto(_objPrefecto);
                            if (_idPrefecto == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de ingresar al prefecto";
                            }
                            else
                            {
                                var _objProfectoIngresado = _objCatalogoPrefecto.ConsultarPrefectoPorId(_idPrefecto).FirstOrDefault();
                                _objProfectoIngresado.IdPrefecto = 0;
                                _objProfectoIngresado.Provincia.IdProvincia = 0;
                                _respuesta = _objProfectoIngresado;
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
        [Route("api/prefecto_modificar")]
        public object prefecto_modificar(Prefecto _objPrefecto)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPrefecto == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto prefecto";
                }
                else if (_objPrefecto.IdPrefectoEncriptado == null || string.IsNullOrEmpty(_objPrefecto.IdPrefectoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del prefecto";
                }
                else if (_objPrefecto.Provincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objPrefecto.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la provincia";
                }
                else if (_objPrefecto.Representante == null || string.IsNullOrEmpty(_objPrefecto.Representante))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del representante";
                }
                else if (_objPrefecto.FechaIngreso.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de ingreso";
                }
                else if (_objPrefecto.FechaSalida != null && (DateTime.Compare(_objPrefecto.FechaIngreso, Convert.ToDateTime(_objPrefecto.FechaSalida)) == 1 || DateTime.Compare(_objPrefecto.FechaIngreso, Convert.ToDateTime(_objPrefecto.FechaSalida)) == 0))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de ingreso debe ser menor a la fecha de salida";
                }
                else
                {
                    int _idProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objPrefecto.Provincia.IdProvinciaEncriptado));
                    int _idPrefecto = Convert.ToInt32(_seguridad.DesEncriptar(_objPrefecto.IdPrefectoEncriptado));
                    var _objUltimoPrefectoSinSalida = _objCatalogoPrefecto.ConsultarPrefectoPorIdProvincia(_idProvincia).Where(c => c.Estado == true && c.FechaSalida == null && c.IdPrefecto!=_idPrefecto).FirstOrDefault();
                    if (_objUltimoPrefectoSinSalida != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No puede modificar el nuevo prefecto, mientras no haya registrado la fecha de salida de " + _objUltimoPrefectoSinSalida.Representante.ToUpper();
                    }
                    else
                    {
                        var _objUltimoPrefectoConSalida = _objCatalogoPrefecto.ConsultarPrefectoPorIdProvincia(_idProvincia).Where(c => c.Estado == true && c.IdPrefecto!=_idPrefecto).OrderByDescending(c => c.FechaSalida).FirstOrDefault();
                        if (_objUltimoPrefectoConSalida != null && (DateTime.Compare(Convert.ToDateTime(_objUltimoPrefectoConSalida.FechaSalida), _objPrefecto.FechaIngreso) > 0))
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La fecha de ingreso del prefecto debe ser mayor a la fecha de salida de " + _objUltimoPrefectoConSalida.Representante.ToUpper();
                        }
                        else
                        {
                            _objPrefecto.Estado = true;
                            _objPrefecto.IdPrefecto = _idPrefecto;
                            _objPrefecto.Provincia.IdProvincia = _idProvincia;
                            _idPrefecto = _objCatalogoPrefecto.ModificarPrefecto(_objPrefecto);
                            if (_idPrefecto == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de modificar al prefecto";
                            }
                            else
                            {
                                var _objPrefectoModificado = _objCatalogoPrefecto.ConsultarPrefectoPorId(_idPrefecto).FirstOrDefault();
                                _objPrefectoModificado.IdPrefecto = 0;
                                _objPrefectoModificado.Provincia.IdProvincia = 0;
                                _respuesta = _objPrefectoModificado;
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
        [Route("api/prefecto_eliminar")]
        public object prefecto_eliminar(string _idPrefectoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPrefectoEncriptado == null || string.IsNullOrEmpty(_idPrefectoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del prefecto";
                }
                else
                {
                    int _idPrefecto = Convert.ToInt32(_seguridad.DesEncriptar(_idPrefectoEncriptado));
                    var _objPrefecto = _objCatalogoPrefecto.ConsultarPrefectoPorId(_idPrefecto).FirstOrDefault();
                    if (_objPrefecto.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();                    
                        _http.mensaje = "Este prefecto ya ha sido utilizado, por lo tanto no lo puede eliminar.";
                    }
                    else
                    {
                        _objCatalogoPrefecto.EliminarPrefecto(_idPrefecto);
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
