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
    public class AsignarResponsableModeloPublicadoController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarResponsableModeloPublicado _objCatalogoAsignarResponsableModeloPublicado = new CatalogoAsignarResponsableModeloPublicado();
        CatalogoModeloPublicado _objCatalogoModeloPublicado = new CatalogoModeloPublicado();
        Seguridad _seguridad = new Seguridad();


        [HttpPost]
        [Route("api/asignarresponsablemodelopublicado_insertar")]
        public object asignarresponsablemodelopublicado_insertar(AsignarResponsableModeloPublicado _objAsignarResponsableModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarResponsableModeloPublicado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto asignar responsable modelo generico";
                }
                else if (_objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario == null || string.IsNullOrEmpty(_objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del autor de la asignación";
                }
                else if (_objAsignarResponsableModeloPublicado.ModeloPublicado == null || string.IsNullOrEmpty(_objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del modelo publicado que va a asignar";
                }
                else if (_objAsignarResponsableModeloPublicado.Parroquia == null || string.IsNullOrEmpty(_objAsignarResponsableModeloPublicado.Parroquia.IdParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquial";
                }
                else if (_objAsignarResponsableModeloPublicado.FechaInicio == null || _objAsignarResponsableModeloPublicado.FechaInicio.ToShortDateString() == "01/01/0001" || _objAsignarResponsableModeloPublicado.FechaInicio.ToShortDateString() == "1/1/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de inicio.";
                }
                else if (_objAsignarResponsableModeloPublicado.FechaFin == null || _objAsignarResponsableModeloPublicado.FechaFin.ToShortDateString() == "01/01/0001" || _objAsignarResponsableModeloPublicado.FechaFin.ToShortDateString() == "1/1/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de finalización.";
                }
                else if (DateTime.Compare(Convert.ToDateTime(_objAsignarResponsableModeloPublicado.FechaInicio), Convert.ToDateTime(_objAsignarResponsableModeloPublicado.FechaFin )) >= 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de inicio debe ser menor a la fecha fin.";
                }
                else
                {
                    _objAsignarResponsableModeloPublicado.Parroquia.IdParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarResponsableModeloPublicado.Parroquia.IdParroquiaEncriptado));
                    _objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicadoEncriptado));
                    _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado));
                    _objAsignarResponsableModeloPublicado.FechaAsignacion = DateTime.Now;
                    _objAsignarResponsableModeloPublicado.Estado = true;
                    int _idAsignarResponsableInsertado = _objCatalogoAsignarResponsableModeloPublicado.InsertarAsignarResponsableModeloPublicado(_objAsignarResponsableModeloPublicado);
                    if (_idAsignarResponsableInsertado == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar el asignar responsable.";
                    }
                    else
                    {
                        _objAsignarResponsableModeloPublicado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsableInsertado).FirstOrDefault();
                        _objAsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado=0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objAsignarResponsableModeloPublicado;
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
        [Route("api/asignarresponsablemodelopublicado_modificar")]
        public object asignarresponsablemodelopublicado_modificar(AsignarResponsableModeloPublicado _objAsignarResponsableModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarResponsableModeloPublicado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto asignar responsable modelo generico";
                }
                else if (_objAsignarResponsableModeloPublicado == null || string.IsNullOrEmpty(_objAsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto asignar responsable modelo publicado";
                }
                else if (_objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario == null || string.IsNullOrEmpty(_objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del autor de la asignación";
                }
                else if (_objAsignarResponsableModeloPublicado.FechaFin == null || _objAsignarResponsableModeloPublicado.FechaFin.ToShortDateString() == "01/01/0001" || _objAsignarResponsableModeloPublicado.FechaFin.ToShortDateString() == "1/1/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de finalización.";
                }
                else
                {
                    int _idAsignarResponsable = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado));
                    var _objAsignarResponsableConsultado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsable).FirstOrDefault();
                    if (_objAsignarResponsableConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar responsable que intenta modificar";
                    }
                    else
                    {
                        var _validarFechaUtilizado = true;
                        DateTime _fechaInicioPreviamenteRegistrada = _objAsignarResponsableConsultado.FechaInicio;
                        if (DateTime.Compare(Convert.ToDateTime(_fechaInicioPreviamenteRegistrada), Convert.ToDateTime(_objAsignarResponsableModeloPublicado.FechaFin)) >= 0)
                        {
                            _validarFechaUtilizado = false;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "La nueva fecha de finalización debe ser mayor a la fecha de inicio registrada: "+_fechaInicioPreviamenteRegistrada.ToShortDateString();
                        }
                        else
                        {
                            if (_objAsignarResponsableConsultado.Utilizado == "1")
                            {
                                DateTime _fechaFinAnterior = _objAsignarResponsableConsultado.FechaFin;
                                if (DateTime.Compare(Convert.ToDateTime(_fechaFinAnterior), Convert.ToDateTime(_objAsignarResponsableModeloPublicado.FechaFin)) >= 0)
                                {
                                    _validarFechaUtilizado = false;
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "La nueva fecha de finalización debe ser mayor a la fecha de finalización anterior: " + _fechaFinAnterior.ToShortDateString();
                                }
                            }
                        }
                        if(_validarFechaUtilizado==true)
                        {
                            _objAsignarResponsableConsultado.FechaFin = _objAsignarResponsableModeloPublicado.FechaFin;
                            _objAsignarResponsableConsultado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado));
                            _objAsignarResponsableConsultado.FechaAsignacion = DateTime.Now;
                            _objAsignarResponsableConsultado.Estado = true;
                            _idAsignarResponsable = _objCatalogoAsignarResponsableModeloPublicado.ModificarAsignarResponsableModeloPublicado(_objAsignarResponsableConsultado);
                            if (_idAsignarResponsable == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "Ocurrió un error al tratar de modificar el asignar responsable.";
                            }
                            else
                            {
                                _objAsignarResponsableModeloPublicado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsable).FirstOrDefault();
                                _objAsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                                _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                                _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                                _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                                _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                                _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                                _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                                _objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = 0;
                                _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                                _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                                _objAsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                                _objAsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                                _objAsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                                _respuesta = _objAsignarResponsableModeloPublicado;
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
        [Route("api/asignarresponsablemodelopublicado_eliminar")]
        public object asignarresponsablemodelopublicado_eliminar(string _idAsignarResponsableModeloPublicadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarResponsableModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_idAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto asignar responsable modelo publicado";
                }
                else
                {
                    int _idAsignarResponsable = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarResponsableModeloPublicadoEncriptado));
                    var _objAsignarResponsableConsultado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsable).FirstOrDefault();
                    if (_objAsignarResponsableConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar responsable que intenta eliminar";
                    }
                    else
                    {
                        _objCatalogoAsignarResponsableModeloPublicado.EliminarAsignarResponsableModeloPublicado(_idAsignarResponsable);
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
        [Route("api/asignarresponsablemodelopublicado_consultar")]
        public object asignarresponsablemodelopublicado_consultar(string _idAsignarResponsableModeloPublicadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarResponsableModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_idAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto asignar responsable modelo publicado";
                }
                else
                {
                    int _idAsignarResponsable = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarResponsableModeloPublicadoEncriptado));
                    var _objAsignarResponsableConsultado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsable).FirstOrDefault();
                    if (_objAsignarResponsableConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar responsable";
                    }
                    else
                    {
                        var _objAsignarResponsableModeloPublicado = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(_idAsignarResponsable).FirstOrDefault();
                        _objAsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objAsignarResponsableModeloPublicado;
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
        [Route("api/asignarresponsablemodelopublicado_consultarporidmodelopublicado")]
        public object asignarresponsablemodelopublicado_consultarporidmodelopublicado(string _idModeloPublicadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_idModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto modelo publicado";
                }
                else
                {
                    int _idModeloPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_idModeloPublicadoEncriptado));
                    //var _objModeloPublicado = _objCatalogoModeloPublicado.ConsultarModeloPublicadoPorIdDHBD(_idModeloPublicado).FirstOrDefault();
                    var _objModeloPublicado = _objCatalogoModeloPublicado.ConsultarModeloPublicadoPorId(_idModeloPublicado).FirstOrDefault();
                    if (_objModeloPublicado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto modelo publicado";
                    }
                    else
                    {
                        var _listaAsignarResponsable = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorIdModeloPublicado(_idModeloPublicado).ToList();
                        foreach (var _objAsignarResponsableModeloPublicado in _listaAsignarResponsable)
                        {
                            _objAsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                            _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = 0;
                            _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                            _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                            _objAsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                            _objAsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                            _objAsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                        }
                        _respuesta = _listaAsignarResponsable;
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
        [Route("api/asignarresponsablemodelopublicado_consultarporparroquia")]
        public object asignarresponsablemodelopublicado_consultarporparroquia(string _idParroquia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idParroquia == null || string.IsNullOrEmpty(_idParroquia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del objeto modelo publicado";
                }
                else
                {
                    int _idParroquias = Convert.ToInt32(_seguridad.DesEncriptar(_idParroquia));
                    var _listaAsignarResponsable = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorParroquia(_idParroquias).ToList();
                    foreach (var _objAsignarResponsableModeloPublicado in _listaAsignarResponsable)
                    {
                        _objAsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicado = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        _objAsignarResponsableModeloPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.IdModeloPublicado = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.IdCabeceraVersionModelo = 0;
                        _objAsignarResponsableModeloPublicado.ModeloPublicado.CabeceraVersionModelo.ModeloGenerico.IdModeloGenerico = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.IdParroquia = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.Canton.IdCanton = 0;
                        _objAsignarResponsableModeloPublicado.Parroquia.Canton.Provincia.IdProvincia = 0;
                    }
                    _respuesta = _listaAsignarResponsable;
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
        [Route("api/HabilitarAsignarResponsableModeloPublicado")]
        public object HabilitarAsignarResponsableModeloPublicado(AsignarResponsableModeloPublicado _AsignarResponsableModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la asignacion parroquia que va a habilitar.";
                }
                else
                {
                    var _dataModeloGenericoParroquia = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(int.Parse(_seguridad.DesEncriptar(_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado))).FirstOrDefault();
                    if (_dataModeloGenericoParroquia == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La asignacion parroquia que intenta deshabilitar no existe.";
                    }
                    else
                    {
                        _objCatalogoAsignarResponsableModeloPublicado.HabilitarAsignarResponsableModeloPublicado(int.Parse(_seguridad.DesEncriptar(_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado)));
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
        [Route("api/DesHabilitarAsignarResponsableModeloPublicado")]
        public object DesHabilitarAsignarResponsableModeloPublicado(AsignarResponsableModeloPublicado _AsignarResponsableModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia de la version que va a eliminar.";
                }
                else
                {
                    var _dataModeloGenericoParroquia = _objCatalogoAsignarResponsableModeloPublicado.ConsultarAsignarResponsableModeloPublicadoPorId(int.Parse(_seguridad.DesEncriptar(_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado))).FirstOrDefault();
                    if (_dataModeloGenericoParroquia == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La asignacion que intenta eliminar no existe.";
                    }
                    else
                    {
                        _objCatalogoAsignarResponsableModeloPublicado.DesHabilitarAsignarResponsableModeloPublicado(int.Parse(_seguridad.DesEncriptar(_AsignarResponsableModeloPublicado.IdAsignarResponsableModeloPublicadoEncriptado)));
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
        [Route("api/obtenerCarpeta")]
        public object obtenerCarpeta()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                
                _respuesta = _objCatalogoAsignarResponsableModeloPublicado.obtenerCarpeta();
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
