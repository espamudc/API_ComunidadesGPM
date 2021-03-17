using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models.Catalogos;
using API.Models.Metodos;
using API.Models.Entidades;

namespace API.Controllers
{
    [Authorize]
    public class PeriodoController : ApiController
    {
        Seguridad _seguridad = new Seguridad();
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoPeriodo _objCatalogoPeriodo = new CatalogoPeriodo();
        [HttpPost]
        [Route("api/periodo_consultar")]
        public object periodo_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaPeriodos = _objCatalogoPeriodo.ConsultarPeriodo().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaPeriodos)
                {
                    item.IdPeriodo = 0;
                }
                _respuesta = _listaPeriodos;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/periodo_insertar")]
        public object periodo_insertar(Periodo _objPeriodo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPeriodo == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró nada para insertar";
                }
                else if (_objPeriodo.Descripcion == null || string.IsNullOrWhiteSpace(_objPeriodo.Descripcion))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del periodo.";
                }
                else if (_objPeriodo.FechaInicio == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha inicio del periodo.";
                }
                else if (_objPeriodo.FechaFin == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha fin del periodo.";
                }
               
                else
                {

                    _objPeriodo.Estado = true;

                    int _idPeriodoIngresado = _objCatalogoPeriodo.InsertarPeriodo(_objPeriodo);
                    if (_idPeriodoIngresado == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al intentar ingresar el periodo, intente nuevamente.";
                    }
                    else
                    {
                        var _personaIngresada = _objCatalogoPeriodo.ConsultarPeriodo().Where(c => c.IdPeriodo == _idPeriodoIngresado && c.Estado == true).FirstOrDefault();
                        _personaIngresada.IdPeriodo = 0;
                        _respuesta = _personaIngresada;
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
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


        [HttpPost]
        [Route("api/periodo_modificar")]
        public object periodo_modificar(Periodo _objPeriodo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPeriodo.IdPeriodoEncriptado == null || string.IsNullOrWhiteSpace(_objPeriodo.IdPeriodoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del periodo que intenta modificar";
                }
                else
                {
                    int _idPeriodo = Convert.ToInt32(_seguridad.DesEncriptar(_objPeriodo.IdPeriodoEncriptado));
                    if (_objCatalogoPeriodo.ConsultarPeriodo().Where(c => c.IdPeriodo == _idPeriodo).FirstOrDefault() == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "El periodo que intenta modificar no es válido.";
                    }
                    else if (_objPeriodo.Descripcion == null || string.IsNullOrWhiteSpace(_objPeriodo.Descripcion))
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ingrese la descripción del periodo.";
                    }
                    else if (_objPeriodo.FechaInicio == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ingrese la fecha inicio del periodo.";
                    }
                    else if (_objPeriodo.FechaFin == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ingrese la fecha fin del periodo.";
                    }

                    else
                    {

                        _objPeriodo.Estado = true;
                        _objPeriodo.IdPeriodo = _idPeriodo;

                        int _idPeriodoModificado = _objCatalogoPeriodo.ModificarPeriodo(_objPeriodo);
                        if (_idPeriodoModificado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar modificar el periodo, intente nuevamente.";
                        }
                        else
                        {
                            var _periodoModificado = _objCatalogoPeriodo.ConsultarPeriodo().Where(c => c.IdPeriodo == _idPeriodoModificado && c.Estado == true).FirstOrDefault();
                            _periodoModificado.IdPeriodo = 0;
                            _respuesta = _periodoModificado;
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


        [HttpPost]
        [Route("api/periodo_eliminar")]
        public object periodo_eliminar(string _idPeriodoEncriptado)
        {

            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrWhiteSpace(_idPeriodoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del periodo que intenta eliminar.";
                }
                else
                {
                    int _idPeriodo = Convert.ToInt32(_seguridad.DesEncriptar(_idPeriodoEncriptado));
                    var _objPeriodo = _objCatalogoPeriodo.ConsultarPeriodo().Where(c => c.IdPeriodo == _idPeriodo).FirstOrDefault();

                    if (_objPeriodo == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el periodo que intenta eliminar.";
                    }
                    else if (_objPeriodo.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "El periodo ya ha sido asignado, no se puede eliminar.";
                    }
                    else
                    {
                        _objCatalogoPeriodo.EliminarPeriodo(_idPeriodo);
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
