using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models.Catalogos;
using API.Models.Entidades;
using API.Models.Metodos;

namespace API.Controllers
{
    [Authorize]
    public class OpcionPreguntaSeleccionController : ApiController
    {
        CatalogoOpcionPreguntaSeleccion _objCatalogoOpcionPreguntaSeleccion = new CatalogoOpcionPreguntaSeleccion();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        Seguridad _seguridad = new Seguridad();
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();

        [HttpPost]
        [Route("api/opcionpreguntaseleccion_insertar")]
        public object opcionpreguntaseleccion_insertar(OpcionPreguntaSeleccion _objOpcionPreguntaSelecccion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objOpcionPreguntaSelecccion == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto opción pregunta de selección";
                }
                else if (_objOpcionPreguntaSelecccion.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objOpcionPreguntaSelecccion.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objOpcionPreguntaSelecccion.Descripcion == null || string.IsNullOrEmpty(_objOpcionPreguntaSelecccion.Descripcion))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción de la opción";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objOpcionPreguntaSelecccion.Pregunta.IdPreguntaEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c=>c.Estado==true).FirstOrDefault();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta en el sistema";
                    }
                    else
                    {
                        _objOpcionPreguntaSelecccion.Pregunta.IdPregunta = _idPregunta;
                        _objOpcionPreguntaSelecccion.Estado = true;
                        int _idOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.InsertarOpcionPreguntaSeleccion(_objOpcionPreguntaSelecccion);
                        if (_idOpcionPreguntaSeleccion == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de ingresar la opción";
                        }
                        else
                        {
                            _objOpcionPreguntaSelecccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorId(_idOpcionPreguntaSeleccion).FirstOrDefault();
                            _objOpcionPreguntaSelecccion.IdOpcionPreguntaSeleccion = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.IdPregunta = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.IdSeccion = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.Componente.IdComponente = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _respuesta = _objOpcionPreguntaSelecccion;
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
        [Route("api/opcionpreguntaseleccion_editar")]
        public object opcionpreguntaseleccion_editar(OpcionPreguntaSeleccion _objOpcionPreguntaSelecccion)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objOpcionPreguntaSelecccion == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto opción pregunta de selección";
                }
                else if (_objOpcionPreguntaSelecccion.Pregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objOpcionPreguntaSelecccion.Pregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objOpcionPreguntaSelecccion.Descripcion == null || string.IsNullOrEmpty(_objOpcionPreguntaSelecccion.Descripcion))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción de la opción";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objOpcionPreguntaSelecccion.Pregunta.IdPreguntaEncriptado));
                    int _idOpcionPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objOpcionPreguntaSelecccion.IdOpcionPreguntaSeleccionEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta en el sistema";
                    }
                    else
                    {
                        _objOpcionPreguntaSelecccion.Pregunta.IdPregunta = _idPregunta;
                        _objOpcionPreguntaSelecccion.Estado = true;
                        _objOpcionPreguntaSelecccion.IdOpcionPreguntaSeleccion = _idOpcionPregunta;
                        int _idOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.EditarOpcionPreguntaSeleccion(_objOpcionPreguntaSelecccion);
                        if (_idOpcionPreguntaSeleccion == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de editar la opción";
                        }
                        else
                        {
                            _objOpcionPreguntaSelecccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorId(_idOpcionPreguntaSeleccion).FirstOrDefault();
                            _objOpcionPreguntaSelecccion.IdOpcionPreguntaSeleccion = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.IdPregunta = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.IdSeccion = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.Componente.IdComponente = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _respuesta = _objOpcionPreguntaSelecccion;
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
        [Route("api/opcionpreguntaseleccion_consultarporidpregunta")]
        public object opcionpreguntaseleccion_consultarporidpregunta(string _idPreguntaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPreguntaEncriptado == null || string.IsNullOrEmpty(_idPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_idPreguntaEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).FirstOrDefault();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta en el sistema";
                    }
                    else if(_objPregunta.TipoPregunta.Identificador!=2 && _objPregunta.TipoPregunta.Identificador != 3 && _objPregunta.TipoPregunta.Identificador != 6)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La pregunta no es de selección única ni de selección múltiple";
                    }
                    else
                    {
                        var _listaOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorIdPregunta(_idPregunta).Where(c => c.Estado == true).ToList();
                        foreach (var _objOpcionPreguntaSelecccion in _listaOpcionPreguntaSeleccion)
                        {
                            _objOpcionPreguntaSelecccion.IdOpcionPreguntaSeleccion = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.IdPregunta = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.IdSeccion = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.Componente.IdComponente = 0;
                            _objOpcionPreguntaSelecccion.Pregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _listaOpcionPreguntaSeleccion;
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
        [Route("api/opcionpreguntaseleccion_eliminar")]
        public object opcionpreguntaseleccion_eliminar(string _idOpcionPreguntaSeleccionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idOpcionPreguntaSeleccionEncriptado == null || string.IsNullOrEmpty(_idOpcionPreguntaSeleccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la opción";
                }
                else
                {
                    int _idOpcionPreguntaSeleccion = Convert.ToInt32(_seguridad.DesEncriptar(_idOpcionPreguntaSeleccionEncriptado));
                    var _objOpcionPreguntaSelecccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorId(_idOpcionPreguntaSeleccion).FirstOrDefault();
                    if (_objOpcionPreguntaSelecccion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la opción en el sistema";
                    }
                    else if(_objOpcionPreguntaSelecccion.Utilizado=="1" || _objOpcionPreguntaSelecccion.Encajonamiento=="1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Esta opción ya ha sido utilizada, por lo tanto no puede ser eliminada.";
                    }
                    else if (_objOpcionPreguntaSelecccion.Utilizado == "0" && _objOpcionPreguntaSelecccion.Encajonamiento == "0")
                    {                        
                        _objCatalogoOpcionPreguntaSeleccion.EliminarOpcionPreguntaSeleccion(_idOpcionPreguntaSeleccion);
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
