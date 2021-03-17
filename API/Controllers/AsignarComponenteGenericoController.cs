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
    public class AsignarComponenteGenericoController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarComponenteGenerico AsignarComponenteGenerico = new CatalogoAsignarComponenteGenerico();
        Seguridad _seguridad = new Seguridad();
        CatalogoAsignarCuestionarioModelo _objAsignarCuestionarioModelo = new CatalogoAsignarCuestionarioModelo();


        [HttpPost]
        [Route("api/AsignarComponenteGenerico_insertar")]
        public object AsignarComponenteGenerico_insertar(AsignarComponenteGenerico _objAsignarComponenteGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarComponenteGenerico == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto AsignarComponenteGenerico";
                }
                else if (_objAsignarComponenteGenerico.IdAsignarCuestionarioModelo == null || string.IsNullOrEmpty(_objAsignarComponenteGenerico.IdAsignarCuestionarioModelo))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del CuestionarioModelo";
                }
                else if (_objAsignarComponenteGenerico.IdComponente == null || string.IsNullOrEmpty(_objAsignarComponenteGenerico.IdComponente))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del Componente";
                }
                else if (_objAsignarComponenteGenerico.Orden.ToString() == null || string.IsNullOrWhiteSpace(_objAsignarComponenteGenerico.Orden.ToString().Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ingrese el Orden";
                }
                else
                {
                    _objAsignarComponenteGenerico.IdAsignarCuestionarioModelo = _seguridad.DesEncriptar(_objAsignarComponenteGenerico.IdAsignarCuestionarioModelo);
                    _objAsignarComponenteGenerico.IdComponente = _seguridad.DesEncriptar(_objAsignarComponenteGenerico.IdComponente);
                    int _idAsignarComponenteGenerico = AsignarComponenteGenerico.InsertarAsignarComponenteGenerico(_objAsignarComponenteGenerico);
                    if (_idAsignarComponenteGenerico == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar el asignar cuestionario generico.";
                    }
                    else
                    {
                        _objAsignarComponenteGenerico = AsignarComponenteGenerico.ConsultarAsignarComponenteGenericoPorId(_idAsignarComponenteGenerico).FirstOrDefault();
                        _objAsignarComponenteGenerico.IdAsignarComponenteGenerico = 0;
                        _respuesta = _objAsignarComponenteGenerico;
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
        [Route("api/ComponenteDeUnModelo_consultar")]
        public object componente_consultar(AsignarCuestionarioModelo _AsignarCuestionarioModelo)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_AsignarCuestionarioModelo.IdModeloGenerico == null || string.IsNullOrEmpty(_AsignarCuestionarioModelo.IdModeloGenerico))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del ModeloGenerico";
                }
                else if (_AsignarCuestionarioModelo.IdCuestionarioPublicado == null || string.IsNullOrEmpty(_AsignarCuestionarioModelo.IdCuestionarioPublicado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del CuestionarioGenerico";
                }
                else
                {
                    _AsignarCuestionarioModelo.IdModeloGenerico = _seguridad.DesEncriptar(_AsignarCuestionarioModelo.IdModeloGenerico);
                    _AsignarCuestionarioModelo.IdCuestionarioPublicado = _seguridad.DesEncriptar(_AsignarCuestionarioModelo.IdCuestionarioPublicado);
                    var _objListaComponentes = _objAsignarCuestionarioModelo.ConsultarComponenteDeUnModeloGenerico(_AsignarCuestionarioModelo).ToList();
                    _respuesta = _objListaComponentes;
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
        [Route("api/AsignarComponenteGenerico_eliminar")]
        public object canton_eliminar(AsignarComponenteGenerico _objAsignarComponenteGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado == null || string.IsNullOrEmpty(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón que va a eliminar.";
                }
                else
                {
                    _objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado = _seguridad.DesEncriptar(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado);
                    var _objAsignarComponenteGenericoConsultado = AsignarComponenteGenerico.ConsultarAsignarComponenteGenericoPorId(int.Parse(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado)).FirstOrDefault();
                    if (_objAsignarComponenteGenericoConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El AsignarComponenteGenerico que intenta eliminar no existe.";
                    }
                    else if (_objAsignarComponenteGenericoConsultado.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "El AsignarComponenteGenerico ya es utilizado, por la tanto no puede ser eliminado.";
                    }
                    else
                    {
                        AsignarComponenteGenerico.EliminarAsignarComponenteGenerico(int.Parse(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado));
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
        [Route("api/AsignarComponenteGenerico_CambiarPosicion")]
        public object AsignarComponenteGenerico_CambiarPosicion(AsignarComponenteGenerico _objAsignarComponenteGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado == null || string.IsNullOrEmpty(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del componente que va a cambiar de posicion.";
                }
                else if (_objAsignarComponenteGenerico.Orden <= 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la nueva posicion correcta";
                }
                else
                {
                    _objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado = _seguridad.DesEncriptar(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado);
                    var _objAsignarComponenteGenericoConsultado = AsignarComponenteGenerico.ConsultarAsignarComponenteGenericoPorId(int.Parse(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado)).FirstOrDefault();
                    if (_objAsignarComponenteGenericoConsultado == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "El AsignarComponenteGenerico que intenta cambiar de posicion no existe.";
                    }
                    else
                    {
                        _objAsignarComponenteGenerico.IdAsignarComponenteGenerico = int.Parse(_objAsignarComponenteGenerico.IdAsignarComponenteGenericoEncriptado);
                        if (AsignarComponenteGenerico.SubirAsignarComponenteGenerico(_objAsignarComponenteGenerico) == true)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                        else
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "Ocurrio un error al cambiar de posicion";
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
    }
}
