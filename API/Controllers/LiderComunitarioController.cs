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
    public class LiderComunitarioController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoLiderComunitario _objCatalogoLiderComunitario = new CatalogoLiderComunitario();
        CatalogoComunidad _objCatalogoComunidad = new CatalogoComunidad();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/lidercomunitario_consultar")]
        public object lidercomunitario_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaLiderComunitario = _objCatalogoLiderComunitario.ConsultarLiderComunitario().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaLiderComunitario)
                {
                    item.IdLiderComunitario = 0;
                    item.Comunidad.IdComunidad = 0;
                    item.Comunidad.Parroquia.IdParroquia = 0;
                    item.Comunidad.Parroquia.Canton.IdCanton = 0;
                    item.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;
                }
                _respuesta = _listaLiderComunitario;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/lidercomunitario_consultar")]
        public object lidercomunitario_consultar(string _idLiderComunitarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idLiderComunitarioEncriptado == null || string.IsNullOrEmpty(_idLiderComunitarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del lider de la comunidad.";
                }
                else
                {
                    int _idLiderComunitario = Convert.ToInt32(_seguridad.DesEncriptar(_idLiderComunitarioEncriptado));
                    var _objLiderComunitario = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorId(_idLiderComunitario).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objLiderComunitario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el lider de la comunidad.";
                    }
                    else
                    {
                        _objLiderComunitario.IdLiderComunitario = 0;
                        _objLiderComunitario.Comunidad.IdComunidad = 0;
                        _objLiderComunitario.Comunidad.Parroquia.IdParroquia = 0;
                        _objLiderComunitario.Comunidad.Parroquia.Canton.IdCanton = 0;
                        _objLiderComunitario.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objLiderComunitario;
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
        [Route("api/lidercomunitario_consultarporidcomunidad")]
        public object lidercomunitario_consultarporidcomunidad(string _idComunidadEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idComunidadEncriptado == null || string.IsNullOrEmpty(_idComunidadEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad.";
                }
                else
                {
                    int _idComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_idComunidadEncriptado));
                    var _objComunidad = _objCatalogoComunidad.ConsultarComunidadPorId(_idComunidad).Where(c => c.EstadoComunidad == true).FirstOrDefault();
                    if (_objComunidad == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la comunidad.";
                    }
                    else
                    {
                        var _listaLideresComunitarios = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorIdComunidad(_idComunidad).Where(c => c.Estado==true && c.Comunidad.EstadoComunidad==true).ToList();
                        foreach (var _objLiderComunitario in _listaLideresComunitarios)
                        {
                            _objLiderComunitario.IdLiderComunitario = 0;
                            _objLiderComunitario.Comunidad.IdComunidad = 0;
                            _objLiderComunitario.Comunidad.Parroquia.IdParroquia = 0;
                            _objLiderComunitario.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objLiderComunitario.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;
                        }
                        _respuesta = _listaLideresComunitarios;
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
        [Route("api/lidercomunitario_insertar")]
        public object lidercomunitario_insertar(LiderComunitario _objLiderComunitario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objLiderComunitario == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto líder comunitario";
                }
                else if (_objLiderComunitario.Comunidad.IdComunidadEncriptado == null || string.IsNullOrEmpty(_objLiderComunitario.Comunidad.IdComunidadEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la comunidad";
                }
                else if (_objLiderComunitario.Representante == null || string.IsNullOrEmpty(_objLiderComunitario.Representante))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del representante";
                }
                else if (_objLiderComunitario.FechaIngreso.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de ingreso";
                }
                else if (_objLiderComunitario.FechaSalida !=null && (DateTime.Compare(_objLiderComunitario.FechaIngreso, Convert.ToDateTime(_objLiderComunitario.FechaSalida)) == 1 || DateTime.Compare(_objLiderComunitario.FechaIngreso, Convert.ToDateTime(_objLiderComunitario.FechaSalida)) == 0))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de ingreso debe ser menor a la fecha de salida";
                }
                else
                {
                    int _idComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_objLiderComunitario.Comunidad.IdComunidadEncriptado));
                    var _objUltimoLiderComunitarioSinSalida = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorIdComunidad(_idComunidad).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objUltimoLiderComunitarioSinSalida != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No puede ingresar un nuevo líder comunitario, mientras no haya registrado la fecha de salida de " + _objUltimoLiderComunitarioSinSalida.Representante.ToUpper();
                    }
                    else
                    {
                        var _objUltimoLiderComunitarioConSalida = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorIdComunidad(_idComunidad).Where(c => c.Estado == true).OrderByDescending(c => c.FechaSalida).FirstOrDefault();
                        if (_objUltimoLiderComunitarioConSalida != null && (DateTime.Compare(Convert.ToDateTime(_objUltimoLiderComunitarioConSalida.FechaSalida), _objLiderComunitario.FechaIngreso) > 0))
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La fecha de ingreso del nuevo líder comunitario debe ser mayor a la fecha de salida de " + _objUltimoLiderComunitarioConSalida.Representante.ToUpper();
                        }
                        else
                        {
                            _objLiderComunitario.Estado = true;
                            _objLiderComunitario.Comunidad.IdComunidad = _idComunidad;
                            int _idLiderComunitario = _objCatalogoLiderComunitario.InsertarLiderComunitario(_objLiderComunitario);
                            if (_idLiderComunitario == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de ingresar al líder comunitario";
                            }
                            else
                            {
                                var _objLiderComunitarioIngresado = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorId(_idLiderComunitario).FirstOrDefault();
                                _objLiderComunitarioIngresado.IdLiderComunitario = 0;
                                _objLiderComunitarioIngresado.Comunidad.IdComunidad = 0;
                                _objLiderComunitarioIngresado.Comunidad.Parroquia.IdParroquia = 0;
                                _objLiderComunitarioIngresado.Comunidad.Parroquia.Canton.IdCanton = 0;
                                _objLiderComunitarioIngresado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;
                                _respuesta = _objLiderComunitarioIngresado;
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
        [Route("api/lidercomunitario_modificar")]
        public object lidercomunitario_modificar(LiderComunitario _objLiderComunitario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objLiderComunitario == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto líder comunitario";
                }
                else if (_objLiderComunitario.IdLiderComunitarioEncriptado == null || string.IsNullOrEmpty(_objLiderComunitario.IdLiderComunitarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad";
                }
                else if (_objLiderComunitario.Comunidad.IdComunidadEncriptado == null || string.IsNullOrEmpty(_objLiderComunitario.Comunidad.IdComunidadEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la comunidad";
                }
                else if (_objLiderComunitario.Representante == null || string.IsNullOrEmpty(_objLiderComunitario.Representante))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del representante";
                }
                else if (_objLiderComunitario.FechaIngreso.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de ingreso";
                }
                else if (_objLiderComunitario.FechaSalida != null && (DateTime.Compare(_objLiderComunitario.FechaIngreso, Convert.ToDateTime(_objLiderComunitario.FechaSalida)) == 1 || DateTime.Compare(_objLiderComunitario.FechaIngreso, Convert.ToDateTime(_objLiderComunitario.FechaSalida)) == 0))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de ingreso debe ser menor a la fecha de salida";
                }
                else
                {
                    int _idComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_objLiderComunitario.Comunidad.IdComunidadEncriptado));
                    int _idLiderComunitario = Convert.ToInt32(_seguridad.DesEncriptar(_objLiderComunitario.IdLiderComunitarioEncriptado));
                    var _objUltimoLiderComunitarioSinSalida = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorIdComunidad(_idComunidad).Where(c => c.Estado == true && c.FechaSalida.ToString() == "01/01/0001 0:00:00").FirstOrDefault();
                    if (_objUltimoLiderComunitarioSinSalida != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No puede modificar al líder comunitario, mientras no haya registrado la fecha de salida de " + _objUltimoLiderComunitarioSinSalida.Representante.ToUpper();
                    }
                    else
                    {
                        var _objUltimoLiderComunitarioConSalida = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorIdComunidad(_idComunidad).Where(c => c.Estado == true).OrderByDescending(c => c.FechaSalida).FirstOrDefault();
                        if (_objUltimoLiderComunitarioConSalida != null && (DateTime.Compare(Convert.ToDateTime(_objUltimoLiderComunitarioConSalida.FechaSalida), _objLiderComunitario.FechaIngreso) > 0))
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La fecha de ingreso del líder comunitario debe ser mayor a la fecha de salida de " + _objUltimoLiderComunitarioConSalida.Representante.ToUpper();
                        }
                        else
                        {
                            _objLiderComunitario.Estado = true;
                            _objLiderComunitario.Comunidad.IdComunidad = _idComunidad;
                            _objLiderComunitario.IdLiderComunitario = _idLiderComunitario;
                            _idLiderComunitario = _objCatalogoLiderComunitario.ModificarLiderComunitario(_objLiderComunitario);
                            if (_idLiderComunitario == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de modificar al lider Comunitario";
                            }
                            else
                            {
                                var _objLiderComunitarioModificado = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorId(_idLiderComunitario).FirstOrDefault();
                                _objLiderComunitarioModificado.IdLiderComunitario = 0;
                                _objLiderComunitarioModificado.Comunidad.IdComunidad = 0;
                                _objLiderComunitarioModificado.Comunidad.Parroquia.IdParroquia = 0;
                                _objLiderComunitarioModificado.Comunidad.Parroquia.Canton.IdCanton = 0;
                                _objLiderComunitarioModificado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;
                                _respuesta = _objLiderComunitarioModificado;
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
        [Route("api/lidercomunitario_eliminar")]
        public object lidercomunitario_eliminar(string _idLiderComunitarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idLiderComunitarioEncriptado == null || string.IsNullOrEmpty(_idLiderComunitarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del líder comunitario";
                }
                else
                {
                    int _idLiderComunitario = Convert.ToInt32(_seguridad.DesEncriptar(_idLiderComunitarioEncriptado));
                    var _objPresidenteJuntaParroquial = _objCatalogoLiderComunitario.ConsultarLiderComunitarioPorId(_idLiderComunitario).FirstOrDefault();
                    if (_objPresidenteJuntaParroquial.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Este líder comunitario ya ha sido utilizado, por lo tanto no lo puede eliminar.";
                    }
                    else
                    {
                        _objCatalogoLiderComunitario.EliminarLiderComunitario(_idLiderComunitario);
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
