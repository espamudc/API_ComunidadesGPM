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
    public class PresidenteJuntaParroquialController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoPresidenteJuntaParroquial _objCatalogoPresidenteJuntaParroquial = new CatalogoPresidenteJuntaParroquial();
        CatalogoParroquia _objCatalogoParroquia = new CatalogoParroquia();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/presidentejuntaparroquial_consultar")]
        public object presidentejuntaparroquial_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaPresidenteJuntaParroquial = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquial().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaPresidenteJuntaParroquial)
                {
                    item.IdPresidenteJuntaParroquial = 0;
                    item.Parroquia.IdParroquia = 0;
                    item.Parroquia.Canton.IdCanton = 0;
                    item.Parroquia.Canton.Provincia.IdProvincia = 0;
                }
                _respuesta = _listaPresidenteJuntaParroquial;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/presidentejuntaparroquial_consultar")]
        public object presidentejuntaparroquial_consultar(string _idPresidenteJuntaParroquialEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPresidenteJuntaParroquialEncriptado == null || string.IsNullOrEmpty(_idPresidenteJuntaParroquialEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del presidente de la junta";
                }
                else
                {
                    int _idPresidenteJuntaParroquial = Convert.ToInt32(_seguridad.DesEncriptar(_idPresidenteJuntaParroquialEncriptado));
                    var _objPresidenteJuntaParroquial = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorId(_idPresidenteJuntaParroquial).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPresidenteJuntaParroquial == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el presidente de la junta parroquial.";
                    }
                    else
                    {
                        _objPresidenteJuntaParroquial.IdPresidenteJuntaParroquial = 0;
                        _objPresidenteJuntaParroquial.Parroquia.IdParroquia = 0;
                        _objPresidenteJuntaParroquial.Parroquia.Canton.IdCanton = 0;
                        _objPresidenteJuntaParroquial.Parroquia.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objPresidenteJuntaParroquial;
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
        [Route("api/presidentejuntaparroquial_consultarporidparroquia")]
        public object presidentejuntaparroquial_consultarporidparroquia(string _idParroquiaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idParroquiaEncriptado == null || string.IsNullOrEmpty(_idParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia";
                }
                else
                {
                    int _idParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_idParroquiaEncriptado));
                    var _objParroquia = _objCatalogoParroquia.ConsultarParroquiaPorId(_idParroquia).Where(c => c.EstadoParroquia == true).FirstOrDefault();
                    if (_objParroquia == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la parroquia en el sistema.";
                    }
                    else
                    {
                        var _listaPresidentesJuntasParroquiales = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorIdParroquia(_idParroquia).Where(c => c.Estado == true && c.Parroquia.EstadoParroquia == true).ToList();
                        foreach (var _objPresidenteJuntaParroquial in _listaPresidentesJuntasParroquiales)
                        {
                            _objPresidenteJuntaParroquial.IdPresidenteJuntaParroquial = 0;
                            _objPresidenteJuntaParroquial.Parroquia.IdParroquia = 0;
                            _objPresidenteJuntaParroquial.Parroquia.Canton.IdCanton = 0;
                            _objPresidenteJuntaParroquial.Parroquia.Canton.Provincia.IdProvincia = 0;
                        }
                        _respuesta = _listaPresidentesJuntasParroquiales;
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
        [Route("api/presidentejuntaparroquial_insertar")]
        public object presidentejuntaparroquial_insertar(PresidenteJuntaParroquial _objPresidenteJuntaParroquial)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPresidenteJuntaParroquial == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto presidente de la junta parroquial";
                }
                else if (_objPresidenteJuntaParroquial.Parroquia.IdParroquiaEncriptado== null || string.IsNullOrEmpty(_objPresidenteJuntaParroquial.Parroquia.IdParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la parroquia";
                }
                else if (_objPresidenteJuntaParroquial.Representante == null || string.IsNullOrEmpty(_objPresidenteJuntaParroquial.Representante))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del representante";
                }
                else if (_objPresidenteJuntaParroquial.FechaIngreso.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de ingreso";
                }
                else if (_objPresidenteJuntaParroquial.FechaSalida != null && (DateTime.Compare(_objPresidenteJuntaParroquial.FechaIngreso, Convert.ToDateTime(_objPresidenteJuntaParroquial.FechaSalida)) == 1 || DateTime.Compare(_objPresidenteJuntaParroquial.FechaIngreso, Convert.ToDateTime(_objPresidenteJuntaParroquial.FechaSalida)) == 0))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de ingreso debe ser menor a la fecha de salida";
                }
                else
                {
                    int _idParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_objPresidenteJuntaParroquial.Parroquia.IdParroquiaEncriptado));
                    var _objUltimoPresidenteSinSalida = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorIdParroquia(_idParroquia).Where(c => c.Estado == true && c.FechaSalida==null).FirstOrDefault();
                    if (_objUltimoPresidenteSinSalida != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No puede ingresar un nuevo presidente, mientras no haya registrado la fecha de salida de " + _objUltimoPresidenteSinSalida.Representante.ToUpper();
                    }
                    else
                    {
                        var _objUltimoPresidenteConSalida = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorIdParroquia(_idParroquia).Where(c => c.Estado == true).OrderByDescending(c => c.FechaSalida).FirstOrDefault();
                        if (_objUltimoPresidenteConSalida != null && (DateTime.Compare(Convert.ToDateTime(_objUltimoPresidenteConSalida.FechaSalida), _objPresidenteJuntaParroquial.FechaIngreso) > 0))
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La fecha de ingreso del nuevo presidente debe ser mayor a la fecha de salida de " + _objUltimoPresidenteConSalida.Representante.ToUpper();
                        }
                        else
                        {
                            _objPresidenteJuntaParroquial.Estado = true;
                            _objPresidenteJuntaParroquial.Parroquia.IdParroquia = _idParroquia;
                            int _idPresidenteJuntaParroquial = _objCatalogoPresidenteJuntaParroquial.InsertarPresidenteJuntaParroquial(_objPresidenteJuntaParroquial);
                            if (_idPresidenteJuntaParroquial == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de ingresar al presidente de la junta parroquial";
                            }
                            else
                            {
                                var _objPresidenteJuntaParroquialIngresado = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorId(_idPresidenteJuntaParroquial).FirstOrDefault();
                                _objPresidenteJuntaParroquialIngresado.IdPresidenteJuntaParroquial = 0;
                                _objPresidenteJuntaParroquialIngresado.Parroquia.IdParroquia = 0;
                                _objPresidenteJuntaParroquialIngresado.Parroquia.Canton.IdCanton = 0;
                                _objPresidenteJuntaParroquialIngresado.Parroquia.Canton.Provincia.IdProvincia = 0;
                                _respuesta = _objPresidenteJuntaParroquialIngresado;
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
        [Route("api/presidentejuntaparroquial_modificar")]
        public object presidentejuntaparroquial_modificar(PresidenteJuntaParroquial _objPresidenteJuntaParroquial)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPresidenteJuntaParroquial == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto presidente de la junta parroquial";
                }
                else if (_objPresidenteJuntaParroquial.IdPresidenteJuntaParroquialEncriptado == null || string.IsNullOrEmpty(_objPresidenteJuntaParroquial.IdPresidenteJuntaParroquialEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del presidente de la junta parroquial";
                }
                else if (_objPresidenteJuntaParroquial.Parroquia.IdParroquiaEncriptado == null || string.IsNullOrEmpty(_objPresidenteJuntaParroquial.Parroquia.IdParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la parroquia";
                }
                else if (_objPresidenteJuntaParroquial.Representante == null || string.IsNullOrEmpty(_objPresidenteJuntaParroquial.Representante))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del representante";
                }
                else if (_objPresidenteJuntaParroquial.FechaIngreso.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de ingreso";
                }
                else if (_objPresidenteJuntaParroquial.FechaSalida != null && (DateTime.Compare(_objPresidenteJuntaParroquial.FechaIngreso, Convert.ToDateTime(_objPresidenteJuntaParroquial.FechaSalida)) == 1 || DateTime.Compare(_objPresidenteJuntaParroquial.FechaIngreso, Convert.ToDateTime(_objPresidenteJuntaParroquial.FechaSalida)) == 0))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de ingreso debe ser menor a la fecha de salida";
                }
                else
                {
                    int _idParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_objPresidenteJuntaParroquial.Parroquia.IdParroquiaEncriptado));
                    int _idPresidenteJuntaParroquial = Convert.ToInt32(_seguridad.DesEncriptar(_objPresidenteJuntaParroquial.IdPresidenteJuntaParroquialEncriptado));
                    var _objUltimoPresidenteSinSalida = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorIdParroquia(_idParroquia).Where(c => c.Estado == true && c.FechaSalida == null && c.IdPresidenteJuntaParroquial!=_idPresidenteJuntaParroquial).FirstOrDefault();
                    if (_objUltimoPresidenteSinSalida != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No puede ingresar un nuevo presidente, mientras no haya registrado la fecha de salida de " + _objUltimoPresidenteSinSalida.Representante.ToUpper();
                    }
                    else
                    {
                        var _objUltimoPresidenteConSalida = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorIdParroquia(_idParroquia).Where(c => c.Estado == true && c.IdPresidenteJuntaParroquial!=_idPresidenteJuntaParroquial).OrderByDescending(c => c.FechaSalida).FirstOrDefault();
                        if (_objUltimoPresidenteConSalida != null && (DateTime.Compare(Convert.ToDateTime(_objUltimoPresidenteConSalida.FechaSalida), _objPresidenteJuntaParroquial.FechaIngreso) > 0))
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La fecha de ingreso del nuevo presidente debe ser mayor a la fecha de salida de " + _objUltimoPresidenteConSalida.Representante.ToUpper();
                        }
                        else
                        {
                            _objPresidenteJuntaParroquial.Estado = true;
                            _objPresidenteJuntaParroquial.IdPresidenteJuntaParroquial = _idPresidenteJuntaParroquial;
                            _objPresidenteJuntaParroquial.Parroquia.IdParroquia = _idParroquia;
                            _idPresidenteJuntaParroquial = _objCatalogoPresidenteJuntaParroquial.ModificarPresidenteJuntaParroquial(_objPresidenteJuntaParroquial);
                            if (_idPresidenteJuntaParroquial == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de modificar al presidente de la junta parroquial";
                            }
                            else
                            {
                                var _objPresidenteJuntaParroquialModificado = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorId(_idPresidenteJuntaParroquial).FirstOrDefault();
                                _objPresidenteJuntaParroquialModificado.IdPresidenteJuntaParroquial = 0;
                                _objPresidenteJuntaParroquialModificado.Parroquia.IdParroquia = 0;
                                _objPresidenteJuntaParroquialModificado.Parroquia.Canton.IdCanton = 0;
                                _objPresidenteJuntaParroquialModificado.Parroquia.Canton.Provincia.IdProvincia = 0;
                                _respuesta = _objPresidenteJuntaParroquialModificado;
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
        [Route("api/presidentejuntaparroquial_eliminar")]
        public object presidentejuntaparroquial_eliminar(string _idPresidenteJuntaParroquialEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPresidenteJuntaParroquialEncriptado == null || string.IsNullOrEmpty(_idPresidenteJuntaParroquialEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del presidente de junta parroquial";
                }
                else
                {
                    int _idPresidenteJuntaParroquial = Convert.ToInt32(_seguridad.DesEncriptar(_idPresidenteJuntaParroquialEncriptado));
                    var _objPresidenteJuntaParroquial = _objCatalogoPresidenteJuntaParroquial.ConsultarPresidenteJuntaParroquialPorId(_idPresidenteJuntaParroquial).FirstOrDefault();
                    if (_objPresidenteJuntaParroquial.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Este presidente de junta parroquial ya ha sido utilizado, por lo tanto no lo puede eliminar.";
                    }
                    else
                    {
                        _objCatalogoPresidenteJuntaParroquial.EliminarPresidenteJuntaParroquial(_idPresidenteJuntaParroquial);
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
