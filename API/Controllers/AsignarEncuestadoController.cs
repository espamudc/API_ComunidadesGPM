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
    public class AsignarEncuestadoController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarEncuestado _objCatalogoAsignarEncuestado = new CatalogoAsignarEncuestado();
        CatalogoCuestionarioPublicado _objCatalogoCuestionarioPublicado = new CatalogoCuestionarioPublicado();
        CatalogoAsignarUsuarioTipoUsuario _objCatalogoAsignarUsuarioTipoUsuario = new CatalogoAsignarUsuarioTipoUsuario();
        Seguridad _seguridad = new Seguridad();
        [HttpPost]
        [Route("api/asignarencuestado_insertar")]
        public object asignarencuestado_insertar(AsignarEncuestado _objAsignarEncuestado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarEncuestado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto asignar encuestado";
                }
                else if (_objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar usuario tipo usuario.";
                }
                else if (_objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar usuario técnico.";
                }
                else if (_objAsignarEncuestado.Comunidad.IdComunidadEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.Comunidad.IdComunidadEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad.";
                }
                else if (_objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicadoEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario publicado.";
                }
                else if (_objAsignarEncuestado.FechaInicio == null || _objAsignarEncuestado.FechaInicio.ToShortDateString()=="01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de inicio.";
                }
                else if (_objAsignarEncuestado.FechaFin == null || _objAsignarEncuestado.FechaFin.ToShortDateString() == "01/01/0001") 
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de finalización.";
                }
                else if (DateTime.Compare(Convert.ToDateTime(_objAsignarEncuestado.FechaInicio), Convert.ToDateTime(_objAsignarEncuestado.FechaFin)) >= 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de inicio debe ser menor a la fecha fin.";
                }
                else
                {
                    _objAsignarEncuestado.Estado = true;
                    _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado));
                    _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuarioEncriptado));
                    _objAsignarEncuestado.Comunidad.IdComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.Comunidad.IdComunidadEncriptado));
                    _objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicadoEncriptado));
                    int _idAsignarEncuestado = _objCatalogoAsignarEncuestado.InsertarAsignarEncuestado(_objAsignarEncuestado);
                    if (_idAsignarEncuestado == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar al encuestado";
                    }
                    else
                    {
                        _objAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(_idAsignarEncuestado).Where(c=>c.Estado==true).FirstOrDefault();
                        _objAsignarEncuestado.IdAsignarEncuestado = 0;

                        _objAsignarEncuestado.Comunidad.IdComunidad = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                         _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _respuesta = _objAsignarEncuestado;
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
        [Route("api/cuestionarios/nuevos/tecnico")]
        public object cuestionariosnuevosporusuariotecnico(string idUsuarioTipoUsuarioTecnico)
        {
            object _respuesta = new object();
            try
            {
                int _idUsuarioTipoUsuarioTecnico = Convert.ToInt32(_seguridad.DesEncriptar(Convert.ToString(idUsuarioTipoUsuarioTecnico)));
                if (Convert.ToString(_idUsuarioTipoUsuarioTecnico) == null || string.IsNullOrEmpty(Convert.ToString(_idUsuarioTipoUsuarioTecnico)))
                {
                    return new { respuesta = "No se permiten valores vacíos", http = 406 };
                }
                _respuesta = _objCatalogoAsignarEncuestado.cuestionariosNuevoPorTecnico(_idUsuarioTipoUsuarioTecnico).ToList();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = 200 };
        }
        [HttpPost]
        [Route("api/asignarencuestado_editar")]
        public object asignarencuestado_editar(AsignarEncuestado _objAsignarEncuestado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarEncuestado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto asignar encuestado";
                }
                else if (_objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar usuario tipo usuario.";
                }
                else if (_objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuarioEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar usuario técnico.";
                }
                else if (_objAsignarEncuestado.Comunidad.IdComunidadEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.Comunidad.IdComunidadEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad.";
                }
                else if (_objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicadoEncriptado == null || string.IsNullOrEmpty(_objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario publicado.";
                }
                else if (_objAsignarEncuestado.FechaInicio == null || _objAsignarEncuestado.FechaInicio.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de inicio.";
                }
                else if (_objAsignarEncuestado.FechaFin == null || _objAsignarEncuestado.FechaFin.ToShortDateString() == "01/01/0001")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la fecha de finalización.";
                }
                else if (DateTime.Compare(Convert.ToDateTime(_objAsignarEncuestado.FechaInicio), Convert.ToDateTime(_objAsignarEncuestado.FechaFin)) >= 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "La fecha de inicio debe ser menor a la fecha fin.";
                }
                else
                {
                    _objAsignarEncuestado.Estado = true;
                    _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado));
                    _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuarioEncriptado));
                    _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuarioEncriptado));
                    _objAsignarEncuestado.Comunidad.IdComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.Comunidad.IdComunidadEncriptado));
                    _objAsignarEncuestado.IdEncuestadoEncriptado =_seguridad.DesEncriptar(_objAsignarEncuestado.IdEncuestadoEncriptado);
                    _objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicadoEncriptado));
                    int _idAsignarEncuestado = _objCatalogoAsignarEncuestado.EditarAsignarEncuestado(_objAsignarEncuestado);
                    if (_idAsignarEncuestado == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar al encuestado";
                    }
                    else
                    {
                        _objAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(_idAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                        _objAsignarEncuestado.IdAsignarEncuestado = 0;

                        _objAsignarEncuestado.Comunidad.IdComunidad = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _respuesta = _objAsignarEncuestado;
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
        [Route("api/asignarencuestado_eliminar")]
        public object asignarencuestado_eliminar(string _idAsignarEncuestadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarEncuestadoEncriptado == null || string.IsNullOrEmpty(_idAsignarEncuestadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar encuestado.";
                }
                else
                {
                    int _idAsignarEncuestado = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarEncuestadoEncriptado));
                    var _objAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(_idAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objAsignarEncuestado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto que intenta eliminar";
                    }
                    else if(_objAsignarEncuestado.Utilizado=="1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No es posible eliminar el objeto porque ya ha sido utilizado";
                    }
                    else
                    {
                        _objCatalogoAsignarEncuestado.EliminarAsignarEncuestado(_idAsignarEncuestado);
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
        [Route("api/asignarencuestado_consultar")]
        public object asignarencuestado_consultar(string _idAsignarEncuestadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarEncuestadoEncriptado == null || string.IsNullOrEmpty(_idAsignarEncuestadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar encuestado.";
                }
                else
                {
                    int _idAsignarEncuestado = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarEncuestadoEncriptado));
                    var _objAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(_idAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objAsignarEncuestado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto que intenta consultar";
                    }
                    else
                    {
                        _objAsignarEncuestado.IdAsignarEncuestado = 0;

                        _objAsignarEncuestado.Comunidad.IdComunidad = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                        _objAsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                        _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                        _respuesta = _objAsignarEncuestado;
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
        [Route("api/asignarencuestado_consultarporidcuestionariopublicado")]
        public object asignarencuestado_consultarporidcuestionariopublicado(string _idCuestionarioPublicadoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCuestionarioPublicadoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario publicado.";
                }
                else
                {
                    int _idCuestionarioPublicado = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioPublicadoEncriptado));
                    var _objCuestionarioPulicado = _objCatalogoCuestionarioPublicado.ConsultarCuestionarioPublicadoPorId(_idCuestionarioPublicado).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCuestionarioPulicado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el cuestionario publicado";
                    }
                    else
                    {
                        var _listaAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadPorIdCuestionarioPublicado(_idCuestionarioPublicado).Where(c => c.Estado == true).ToList();
                        foreach (var _objAsignarEncuestado in _listaAsignarEncuestado)
                        {


                            //_objAsignarEncuestado.IdAsignarEncuestado = 0;

                            _objAsignarEncuestado.Comunidad.IdComunidad = 0;
                            _objAsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                            _objAsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objAsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;

                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;

                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.CabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objAsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;

                            _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarEncuestado.CuestionarioPublicado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                        }
                        _respuesta = _listaAsignarEncuestado;
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
        [Route("api/asignarencuestado_consultarporidasignarusuariotipousuariotecnico")]
        public object asignarencuestado_consultarporidasignarusuariotipousuariotecnico(string _idAsignarUsuarioTipoUsuarioTecnicoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idAsignarUsuarioTipoUsuarioTecnicoEncriptado == null || string.IsNullOrEmpty(_idAsignarUsuarioTipoUsuarioTecnicoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del técnico.";
                }
                else
                {
                    int _idAsignarUsuarioTipoUsuarioTecnico = Convert.ToInt32(_seguridad.DesEncriptar(_idAsignarUsuarioTipoUsuarioTecnicoEncriptado));
                    var _objAsignarUsuarioTipoUsuario = _objCatalogoAsignarUsuarioTipoUsuario.ConsultarAsignarUsuarioTipoUsuarioPorId(_idAsignarUsuarioTipoUsuarioTecnico).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objAsignarUsuarioTipoUsuario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar usuario tipo usuario";
                    }
                    else if (_objAsignarUsuarioTipoUsuario.TipoUsuario.Identificador != 2)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El usuario no está registrado como técnico, por lo tanto no puede realizarse la consulta";
                    }
                    else
                    {
                        var _listaAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorIdAsignarUsuarioTipoUsuario(_idAsignarUsuarioTipoUsuarioTecnico).Where(c => c.Estado == true).ToList();
                        foreach (var _objAsignarEncuestado in _listaAsignarEncuestado)
                        {


                            _objAsignarEncuestado.IdAsignarEncuestado = 0;

                            _objAsignarEncuestado.Comunidad.IdComunidad = 0;
                            _objAsignarEncuestado.Comunidad.Parroquia.IdParroquia = 0;
                            _objAsignarEncuestado.Comunidad.Parroquia.Canton.IdCanton = 0;
                            _objAsignarEncuestado.Comunidad.Parroquia.Canton.Provincia.IdProvincia = 0;

                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.IdAsignarUsuarioTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.IdUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.TipoUsuario.IdTipoUsuario = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.IdPersona = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.Sexo.IdSexo = 0;
                            _objAsignarEncuestado.AsignarUsuarioTipoUsuarioTecnico.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;

                            _objAsignarEncuestado.CuestionarioPublicado.IdCuestionarioPublicado = 0;


                            _objAsignarEncuestado.CuestionarioPublicado.Periodo.IdPeriodo = 0;
                        }
                        _respuesta = _listaAsignarEncuestado;
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
