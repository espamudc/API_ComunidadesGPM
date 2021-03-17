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
    public class ParroquiaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoParroquia _objCatalogoParroquia = new CatalogoParroquia();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/parroquia_consultar")]
        public object parroquia_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaParroquias = _objCatalogoParroquia.ConsultarParroquia().Where(c => c.EstadoParroquia == true && c.Canton.EstadoCanton == true && c.Canton.Provincia.EstadoProvincia == true).OrderBy(n => n.IdParroquia).ToList();
                foreach (var item in _listaParroquias)
                {
                    item.IdParroquia = 0;
                    item.Canton.IdCanton = 0;
                    item.Canton.Provincia.IdProvincia = 0;
                }
                _respuesta = _listaParroquias;
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
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/parroquia_consultar")]
        public object parroquia_consultar(string _idParroquiaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idParroquiaEncriptado) || _idParroquiaEncriptado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia.";
                }
                else
                {
                    int _idParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_idParroquiaEncriptado).ToString());
                    var _objParroquia = _objCatalogoParroquia.ConsultarParroquiaPorId(_idParroquia).Where(c => c.EstadoParroquia == true && c.Canton.EstadoCanton == true && c.Canton.Provincia.EstadoProvincia).FirstOrDefault();

                    _objParroquia.IdParroquia = 0;
                    _objParroquia.Canton.IdCanton = 0;
                    _objParroquia.Canton.Provincia.IdProvincia = 0;
                    _respuesta = _objParroquia;
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
        [Route("api/parroquia_consultarporidcanton")]
        public object parroquia_consultarporidcanton(string _idCantonEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idCantonEncriptado) || _idCantonEncriptado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón.";
                }
                else
                {
                    int _idCanton = Convert.ToInt32(_seguridad.DesEncriptar(_idCantonEncriptado).ToString());
                    var _listaParroquias = _objCatalogoParroquia.ConsultarParroquiaPorIdCanton(_idCanton).Where(c => c.EstadoParroquia == true && c.Canton.EstadoCanton == true && c.Canton.Provincia.EstadoProvincia).ToList();
                    foreach (var item in _listaParroquias)
                    {
                        item.IdParroquia = 0;
                        item.Canton.IdCanton = 0;
                        item.Canton.Provincia.IdProvincia = 0;
                    }
                    _respuesta = _listaParroquias;
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
        [Route("api/parroquia_insertar")]
        public object parroquia_insertar(Parroquia _objParroquia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objParroquia == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto parroquia.";
                }
                else if (_objParroquia.Canton.IdCantonEncriptado == null || string.IsNullOrEmpty(_objParroquia.Canton.IdCantonEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón al que pertenece la parroquia.";
                }
                else if (_objParroquia.Canton.Provincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objParroquia.Canton.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia.";
                }
                else if (_objParroquia.NombreParroquia == null || string.IsNullOrEmpty(_objParroquia.NombreParroquia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre de la parroquia.";
                }
                else if (_objCatalogoParroquia.ConsultarParroquia().Where(c => c.NombreParroquia == _objParroquia.NombreParroquia.Trim() && c.Canton.IdCanton == Convert.ToInt32(_seguridad.DesEncriptar(_objParroquia.Canton.IdCantonEncriptado))).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe una parroquia con el mismo nombre, por favor verifique en la lista.";
                }
                else if (_objParroquia.CodigoParroquia == null || string.IsNullOrEmpty(_objParroquia.CodigoParroquia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo de la parroquia.";
                }
                else if (_objCatalogoParroquia.ConsultarParroquia().Where(c => c.CodigoParroquia == _objParroquia.CodigoParroquia.Trim()).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe una parroquia con el mismo código, por favor verifique en la lista.";
                }
                else
                {
                    _objParroquia.NombreParroquia = _objParroquia.NombreParroquia.Trim();
                    _objParroquia.CodigoParroquia = _objParroquia.CodigoParroquia.Trim(); ;
                    _objParroquia.EstadoParroquia = true;
                    _objParroquia.Canton.IdCanton = Convert.ToInt32(_seguridad.DesEncriptar(_objParroquia.Canton.IdCantonEncriptado));
                    _objParroquia.Canton.Provincia.IdProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objParroquia.Canton.Provincia.IdProvinciaEncriptado));
                    int _idParroquia = _objCatalogoParroquia.InsertarParroquia(_objParroquia);
                    if (_idParroquia == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al intentar ingresar la parroquia.";
                    }
                    else
                    {
                        var _objParroquiaInsertada = _objCatalogoParroquia.ConsultarParroquiaPorId(_idParroquia).Where(c => c.EstadoParroquia == true).FirstOrDefault();
                        _objParroquiaInsertada.IdParroquia = 0;
                        _objParroquiaInsertada.Canton.IdCanton = 0;
                        _objParroquiaInsertada.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objParroquiaInsertada;
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
        [Route("api/parroquia_modificar")]
        public object parroquia_modificar(Parroquia _objParroquia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objParroquia == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto parroquia.";
                }
                else if (_objParroquia.IdParroquiaEncriptado == null || string.IsNullOrEmpty(_objParroquia.IdParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia.";
                }
                else if (_objParroquia.Canton.IdCantonEncriptado == null || string.IsNullOrEmpty(_objParroquia.Canton.IdCantonEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón.";
                }
                else if (_objParroquia.Canton.Provincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objParroquia.Canton.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia.";
                }
                else if (_objParroquia.NombreParroquia == null || string.IsNullOrEmpty(_objParroquia.NombreParroquia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre de la parroquia.";
                }
                else if (_objParroquia.CodigoParroquia == null || string.IsNullOrEmpty(_objParroquia.CodigoParroquia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo de la parroquia.";
                }
                else
                {
                    int _idParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_objParroquia.IdParroquiaEncriptado));
                    var _objParroquiaConsultada = _objCatalogoParroquia.ConsultarParroquiaPorId(_idParroquia).FirstOrDefault();
                    if (_objParroquiaConsultada == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La parroquia que intenta modificar no existe.";
                    }
                    else if (_objCatalogoParroquia.ConsultarParroquia().Where(c => c.NombreParroquia == _objParroquia.NombreParroquia.Trim() && c.IdParroquia != _idParroquia && c.Canton.IdCanton == Convert.ToInt32(_seguridad.DesEncriptar(_objParroquia.Canton.IdCantonEncriptado))).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe una parroquia con el mismo nombre, por favor verifique en la lista.";
                    }
                    else if (_objCatalogoParroquia.ConsultarParroquia().Where(c => c.CodigoParroquia == _objParroquia.CodigoParroquia.Trim() && c.IdParroquia != _idParroquia).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe una parroquia con el mismo código, por favor verifique en la lista.";
                    }
                    else
                    {
                        _objParroquia.NombreParroquia = _objParroquia.NombreParroquia.Trim();
                        _objParroquia.CodigoParroquia = _objParroquia.CodigoParroquia.Trim();
                        _objParroquia.IdParroquia = _idParroquia;
                        _objParroquia.Canton.IdCanton = Convert.ToInt32(_seguridad.DesEncriptar(_objParroquia.Canton.IdCantonEncriptado));
                        _objParroquia.Canton.Provincia.IdProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objParroquia.Canton.Provincia.IdProvinciaEncriptado));
                        _objParroquia.EstadoParroquia = true;
                        int? _idParroquiaModificado = _objCatalogoParroquia.ModificarParroquia(_objParroquia);
                        if (_idParroquiaModificado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar modificar la parroquia.";
                        }
                        else
                        {
                            var _objParroquiaModificaa = _objCatalogoParroquia.ConsultarParroquiaPorId(_idParroquia).FirstOrDefault();
                            _objParroquiaModificaa.IdParroquia = 0;
                            _objParroquiaModificaa.Canton.IdCanton = 0;
                            _objParroquiaModificaa.Canton.Provincia.IdProvincia = 0;
                            _respuesta = _objParroquiaModificaa;
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
        [Route("api/parroquia_eliminar")]
        public object parroquia_eliminar(string _idParroquiaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idParroquiaEncriptado == null || string.IsNullOrEmpty(_idParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia que va a eliminar.";
                }
                else
                {
                    int _idParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_idParroquiaEncriptado));
                    var _objParroquiaConsultada = _objCatalogoParroquia.ConsultarParroquiaPorId(_idParroquia).FirstOrDefault();
                    if (_objParroquiaConsultada == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La parroquia que intenta eliminar no existe.";
                    }
                    else if (_objParroquiaConsultada.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La parroquia ya ha sido utilizada, por la tanto no puede ser eliminado.";
                    }
                    else
                    {
                        _objCatalogoParroquia.EliminarParroquia(_idParroquia);
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
