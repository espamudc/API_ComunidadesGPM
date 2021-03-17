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
    public class CabeceraVersionCuestionarioController : ApiController
    {
        Seguridad _seguridad = new Seguridad();
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarResponsable _objCatalogoAsignarResponsable = new CatalogoAsignarResponsable();
        CatalogoCabeceraVersionCuestionario _objCatalogoCabeceraVersionCuestionario = new CatalogoCabeceraVersionCuestionario();
        CatalogoVersionamientoPregunta _objCatalogoVersionamientoPregunta = new CatalogoVersionamientoPregunta();
        CatalogoCuestionarioGenerico _objCatalogoCuestionarioGenerico = new CatalogoCuestionarioGenerico();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();

        [HttpPost]
        [Route("api/cabeceraversioncuestionario_insertar")]
        public object cabeceraversioncuestionario_insertar(CabeceraVersionCuestionario _objCabeceraVersionCuestionario)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objCabeceraVersionCuestionario == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto cabecera versión cuestionario";
                }
                else if (_objCabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsableEncriptado == null || string.IsNullOrEmpty(_objCabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsableEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del asignar responsable";
                }
                else if (string.IsNullOrEmpty(_objCabeceraVersionCuestionario.Caracteristica))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la característica";
                }
                else if (_objCabeceraVersionCuestionario.Version == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la versión";
                }
                else
                {
                    _objCabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = Convert.ToInt32(_seguridad.DesEncriptar(_objCabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsableEncriptado));
                    var _objAsignarResponsable = _objCatalogoAsignarResponsable.ConsultarAsignarResponsablePorId(_objCabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objAsignarResponsable == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto asignar responsable";
                    }
                    else
                    {
                        var _listaCabeceraVersionCuestionario = _objCatalogoCabeceraVersionCuestionario.ConsultarCabeceraVersionCuestionario().Where(c => c.Estado == true && c.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico == _objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico).ToList();
                        var _mismaVersion = _listaCabeceraVersionCuestionario.Where(c => c.Version == _objCabeceraVersionCuestionario.Version).FirstOrDefault();
                        if (_mismaVersion != null)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                            _http.mensaje = "Esta versión ya fue creada para el cuestionario seleccionado";
                        }
                        else
                        {
                            var _listadoPreguntas = _objCatalogoPregunta.ConsultarPreguntaPorIdCuestionarioGenerico(_objAsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico).Where(c => c.Estado == true && c.Seccion.Estado == true && c.Seccion.Componente.Estado == true).ToList();
                            if (_listadoPreguntas.Count == 0)
                            {
                                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                _http.mensaje = "No se han ingresado preguntas en este cuestionario";
                            }
                            else
                            {
                                _objCabeceraVersionCuestionario.Estado = true;
                                _objCabeceraVersionCuestionario.FechaCreacion = DateTime.Now;
                                int _idCabeceraVersionCuestionario = _objCatalogoCabeceraVersionCuestionario.InsertarCabeceraVersionCuestionario(_objCabeceraVersionCuestionario);
                                if (_idCabeceraVersionCuestionario == 0)
                                {
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                                    _http.mensaje = "Ocurrió un error al tratar de ingresar la cabecera versión cuestionario";
                                }
                                else
                                {
                                    _objCabeceraVersionCuestionario.IdCabeceraVersionCuestionario = _idCabeceraVersionCuestionario;
                                    foreach (var item in _listadoPreguntas)
                                    {
                                        int _idVersionamientoPregunta = _objCatalogoVersionamientoPregunta.InsertarVersionamientoPregunta(new VersionamientoPregunta() { Estado = true, Pregunta = item, CabeceraVersionCuestionario = _objCabeceraVersionCuestionario });
                                    }
                                    _objCabeceraVersionCuestionario = _objCatalogoCabeceraVersionCuestionario.ConsultarCabeceraVersionCuestionarioPorId(_idCabeceraVersionCuestionario).Where(c => c.Estado == true).FirstOrDefault();
                                    _objCabeceraVersionCuestionario.IdCabeceraVersionCuestionario = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.IdAsignarResponsable = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                                    _objCabeceraVersionCuestionario.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                                    _respuesta = _objCabeceraVersionCuestionario;
                                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                                }
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
        [Route("api/cabeceraversioncuestionario_eliminar")]
        public object cabeceraversioncuestionario_eliminar(string _idCabeceraVersionCuestionarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCabeceraVersionCuestionarioEncriptado == null || string.IsNullOrEmpty(_idCabeceraVersionCuestionarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la cabecera versión cuestionario";
                }
                else
                {
                    int _idCabeceraVersionCuestionario = Convert.ToInt32(_seguridad.DesEncriptar(_idCabeceraVersionCuestionarioEncriptado));
                    var _objCabeceraVersionCuestionario = _objCatalogoCabeceraVersionCuestionario.ConsultarCabeceraVersionCuestionarioPorId(_idCabeceraVersionCuestionario).FirstOrDefault();
                    if (_objCabeceraVersionCuestionario == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto cabecera versión cuestionario";
                    }
                    else if (_objCabeceraVersionCuestionario.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No se puede eliminar la versión porque ya ha sido utilizada";
                    }
                    else
                    {
                        var _listaVersionamientoPregunta = _objCatalogoVersionamientoPregunta.ConsultarVersionamientoPreguntaPorIdCabeceraVersionCuestionario(_idCabeceraVersionCuestionario).ToList();
                        foreach (var item in _listaVersionamientoPregunta)
                        {
                            _objCatalogoVersionamientoPregunta.EliminarVersionamientoPregunta(item.IdVersionamientoPregunta);
                        }
                        _objCatalogoCabeceraVersionCuestionario.EliminarCabeceraVersionCuestionario(_idCabeceraVersionCuestionario);
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
        [Route("api/cabeceraversioncuestionario_consultar")]
        public object cabeceraversioncuestionario_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaCabeceraVersionCuestionario = _objCatalogoCabeceraVersionCuestionario.ConsultarCabeceraVersionCuestionario().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaCabeceraVersionCuestionario)
                {
                    item.IdCabeceraVersionCuestionario = 0;
                    item.AsignarResponsable.IdAsignarResponsable = 0;
                    item.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                    item.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                    item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                    item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                    item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                    item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                    item.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                }
                _respuesta = _listaCabeceraVersionCuestionario;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }

        [HttpPost]
        [Route("api/cabeceraversioncuestionario_consultarporidcuestionariogenerico")]
        public object cabeceraversioncuestionario_consultarporidcuestionariogenerico(string _idCuestionarioGenericoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                    var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objCuestionarioGenerico == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el cuestionario genérico";
                    }
                    else
                    {
                        var _listaCabeceraVersionCuestionario = _objCatalogoCabeceraVersionCuestionario.ConsultarCabeceraVersionCuestionarioPorIdCuestionarioGenerico(_idCuestionarioGenerico).Where(c => c.Estado == true).ToList();
                        foreach (var item in _listaCabeceraVersionCuestionario)
                        {
                            item.IdCabeceraVersionCuestionario = 0;
                            item.AsignarResponsable.IdAsignarResponsable = 0;
                            item.AsignarResponsable.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            item.AsignarResponsable.AsignarUsuarioTipoUsuario.IdAsignarUsuarioTipoUsuario = 0;
                            item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.IdUsuario = 0;
                            item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.IdPersona = 0;
                            item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.Sexo.IdSexo = 0;
                            item.AsignarResponsable.AsignarUsuarioTipoUsuario.Usuario.Persona.TipoIdentificacion.IdTipoIdentificacion = 0;
                            item.AsignarResponsable.AsignarUsuarioTipoUsuario.TipoUsuario.IdTipoUsuario = 0;
                        }
                        _respuesta = _listaCabeceraVersionCuestionario;
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