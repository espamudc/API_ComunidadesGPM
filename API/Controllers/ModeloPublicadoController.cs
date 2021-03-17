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
    public class ModeloPublicadoController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoModeloPublicado _objModeloPublicados = new CatalogoModeloPublicado();
        Seguridad _seguridad = new Seguridad();


        [HttpPost]
        [Route("api/modeloPublicado_insertar")]
        public object modeloPublicado_insertar(ModeloPublicado _objModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloPublicado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el Modelo a publicar";
                }
                else if (_objModeloPublicado.IdCabeceraVersionModelo == null || string.IsNullOrEmpty(_objModeloPublicado.IdCabeceraVersionModelo))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la version a guardar";
                }
                else if (_objModeloPublicado.IdPeriodo == null || string.IsNullOrEmpty(_objModeloPublicado.IdPeriodo))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el periodo";
                }
                else if (_objModeloPublicado.IdAsignarUsuarioTipoUsuario == null || string.IsNullOrEmpty(_objModeloPublicado.IdAsignarUsuarioTipoUsuario))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la asignacion tipo usuario";
                }
                else
                {
                    _objModeloPublicado.IdCabeceraVersionModelo = _seguridad.DesEncriptar(_objModeloPublicado.IdCabeceraVersionModelo);
                    _objModeloPublicado.IdPeriodo = _seguridad.DesEncriptar(_objModeloPublicado.IdPeriodo);
                    _objModeloPublicado.IdAsignarUsuarioTipoUsuario = _seguridad.DesEncriptar(_objModeloPublicado.IdAsignarUsuarioTipoUsuario);
                    int _idModeloPublicado = _objModeloPublicados.InsertarModeloPublicado(_objModeloPublicado);
                    if (_idModeloPublicado == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar al el modelo publicado";
                    }
                    else
                    {
                        var DataModeloPublicado = _objModeloPublicados.ConsultarModeloPublicadoPorId(_idModeloPublicado).FirstOrDefault();
                        DataModeloPublicado.IdModeloPublicado = 0;
                        _respuesta = DataModeloPublicado;
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
        [Route("api/modeloPublicado_eliminar")]
        public object modeloPublicado_eliminar(ModeloPublicado _objModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloPublicado.IdModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_objModeloPublicado.IdModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del modelo publicado que va a eliminar.";
                }
                else
                {
                    _objModeloPublicado.IdModeloPublicadoEncriptado = _seguridad.DesEncriptar(_objModeloPublicado.IdModeloPublicadoEncriptado);
                    var _objModeloPublicadoConsultado = _objModeloPublicados.ConsultarModeloPublicadoPorId(int.Parse(_objModeloPublicado.IdModeloPublicadoEncriptado)).FirstOrDefault();
                    if (_objModeloPublicadoConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El modelo publicado que intenta eliminar no existe.";
                    }
                    else if (_objModeloPublicadoConsultado.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "El modelo publicado ya esta utilizado, por la tanto no puede ser eliminado.";
                    }
                    else
                    {
                        _objModeloPublicados.EliminarModeloPublicado(int.Parse(_objModeloPublicado.IdModeloPublicadoEncriptado));
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
        [Route("api/HabilitarModeloPublicado")]
        public object HabilitarModeloPublicado(ModeloPublicado _objModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloPublicado.IdModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_objModeloPublicado.IdModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del modelo publicado de la version que va habilitar";
                }
                else
                {
                    var _dataModeloPublicado = _objModeloPublicados.ConsultarModeloPublicadoPorId(int.Parse(_seguridad.DesEncriptar(_objModeloPublicado.IdModeloPublicadoEncriptado))).FirstOrDefault();
                    if (_dataModeloPublicado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El modelo publicado que intenta habilitar no existe.";
                    }
                    else
                    {
                        _objModeloPublicados.HabilitarModeloPublicado(int.Parse(_seguridad.DesEncriptar(_objModeloPublicado.IdModeloPublicadoEncriptado)));
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
        [Route("api/DesHabilitarModeloPublicado")]
        public object DesHabilitarModeloPublicado(ModeloPublicado _objModeloPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloPublicado.IdModeloPublicadoEncriptado == null || string.IsNullOrEmpty(_objModeloPublicado.IdModeloPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del modelo publicado de la version que va deshabilitar";
                }
                else
                {
                    var _dataModeloPublicado = _objModeloPublicados.ConsultarModeloPublicadoPorId(int.Parse(_seguridad.DesEncriptar(_objModeloPublicado.IdModeloPublicadoEncriptado))).FirstOrDefault();
                    if (_dataModeloPublicado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El modelo publicado que intenta deshabilitar no existe.";
                    }
                    else
                    {
                        _objModeloPublicados.DesHabilitarModeloPublicad(int.Parse(_seguridad.DesEncriptar(_objModeloPublicado.IdModeloPublicadoEncriptado)));
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
        [Route("api/modeloPublicado_consultar")]
        public object modeloPublicado_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaCabeceraVersion = _objModeloPublicados.ConsultarModeloPublicado();
                _respuesta = _listaCabeceraVersion;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/modeloPublicadoActivo_consultar")]
        public object modeloPublicadoActivo_consultar(ModeloGenerico _objModeloGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objModeloGenerico.IdModeloGenericoEncriptado == null || string.IsNullOrEmpty(_objModeloGenerico.IdModeloGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Seleccione el modelo generico";
                }
                else
                {
                    var _listaCabeceraVersion = _objModeloPublicados.ConsultarVersionesPublicadoActivos(int.Parse(_seguridad.DesEncriptar(_objModeloGenerico.IdModeloGenericoEncriptado)));
                    _respuesta = _listaCabeceraVersion;
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
        [Route("api/ModeloGenericoConVersionesActivas_Consultar")]
        public object ModeloGenericoConVersionesActivas_Consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaCabeceraVersion = _objModeloPublicados.ModeloGenericoConVersionesActivas();
                _respuesta = _listaCabeceraVersion;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/CuestionarioFinalizado_consultar")]
        public object CuestionarioFinalizado_consultar(CuestionarioPublicado _objCuestionarioPublicado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objCuestionarioPublicado.IdCuestionarioPublicadoEncriptado == null || string.IsNullOrEmpty(_objCuestionarioPublicado.IdCuestionarioPublicadoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Seleccione el cuestionario publicado";
                }
                else
                {
                    _objCuestionarioPublicado.IdCuestionarioPublicado = int.Parse(_seguridad.DesEncriptar(_objCuestionarioPublicado.IdCuestionarioPublicadoEncriptado));
                    var _listaCuestionarioPublicado = _objModeloPublicados.ConsultarEncuestasFinalizadas(_objCuestionarioPublicado);
                    _respuesta = _listaCuestionarioPublicado;
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
        [Route("api/ConsultarCaracterizacionComunidad")]
        public object ConsultarCaracterizacionComunidad(CaracterizacionComunidad _CaracterizacionComunidad)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_CaracterizacionComunidad.IdComunidad))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la comunidad";
                }
                else
                {
                    _CaracterizacionComunidad.IdComunidad = _seguridad.DesEncriptar(_CaracterizacionComunidad.IdComunidad);
                    CaracterizacionComunidad _Caracterizacion = new CaracterizacionComunidad();
                    _Caracterizacion = _objModeloPublicados.getCaracterizacionPorComunidad(int.Parse(_CaracterizacionComunidad.IdComunidad)).FirstOrDefault();
                    if (_Caracterizacion != null)
                    {
                        if (_Caracterizacion.idAsignarEncuestado == "0" || _Caracterizacion.idModeloPublicado =="0")
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "No existe una encuesta finalizada o ninguna caracterizacion para generar";
                        }
                        else
                        {
                            _respuesta = _Caracterizacion;
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }
                    else
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No existe una caracterizacion para generar";
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
