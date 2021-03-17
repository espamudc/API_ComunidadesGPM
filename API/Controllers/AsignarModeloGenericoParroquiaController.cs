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
    public class AsignarModeloGenericoParroquiaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoAsignarModeloGenericoParroquia _objAsignarModeloGenericoParroquia = new CatalogoAsignarModeloGenericoParroquia();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/asignarModeloGenericoParroquia_insertar")]
        public object asignarModeloGenericoParroquia_insertar(AsignarModeloGenericoParroquia _AsignarModeloGenericoParroquia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_AsignarModeloGenericoParroquia == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto alcalde";
                }
                else if (_AsignarModeloGenericoParroquia.IdModeloPublicado == null || string.IsNullOrEmpty(_AsignarModeloGenericoParroquia.IdModeloPublicado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el modelo generico";
                }
                else if (_AsignarModeloGenericoParroquia.IdParroquia == null || string.IsNullOrEmpty(_AsignarModeloGenericoParroquia.IdParroquia))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la parroquia";
                }
                else if (_objAsignarModeloGenericoParroquia.ConsultarAsignarModeloGenericoParroquia().Where(c => c.IdModeloPublicado == _AsignarModeloGenericoParroquia.IdModeloPublicado.Trim() && c.IdParroquia == _AsignarModeloGenericoParroquia.IdParroquia.Trim() && c.Estado == true).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe una parroquia ingresada, por favor verifique en la lista.";
                }
                else
                {
                    _AsignarModeloGenericoParroquia.IdParroquia = _seguridad.DesEncriptar(_AsignarModeloGenericoParroquia.IdParroquia);
                    _AsignarModeloGenericoParroquia.IdModeloPublicado = _seguridad.DesEncriptar(_AsignarModeloGenericoParroquia.IdModeloPublicado);
                    int _idAsignarModeloGenericoParroquia = _objAsignarModeloGenericoParroquia.InsertarAsignarModeloGenericoParroquia(_AsignarModeloGenericoParroquia);
                    if (_idAsignarModeloGenericoParroquia == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar el asignar modelo generico parroquia.";
                    }
                    else
                    {
                        _AsignarModeloGenericoParroquia = _objAsignarModeloGenericoParroquia.ConsultarAsignarModeloGenericoParroquiaPorId(_idAsignarModeloGenericoParroquia).FirstOrDefault();
                        _AsignarModeloGenericoParroquia.IdAsignarModeloGenericoParroquia = 0;
                        _respuesta = _AsignarModeloGenericoParroquia;
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
        [Route("api/asignarModeloGenericoParroquia_eliminar")]
        public object asignarModeloGenericoParroquia_eliminar(AsignarModeloGenericoParroquia _AsignarModeloGenericoParroquia)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_AsignarModeloGenericoParroquia.IdAsignarModeloGenericoParroquiaEncriptado == null || string.IsNullOrEmpty(_AsignarModeloGenericoParroquia.IdAsignarModeloGenericoParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia de la version que va a eliminar.";
                }
                else
                {
                    _AsignarModeloGenericoParroquia.IdAsignarModeloGenericoParroquiaEncriptado = _seguridad.DesEncriptar(_AsignarModeloGenericoParroquia.IdAsignarModeloGenericoParroquiaEncriptado);
                    _objAsignarModeloGenericoParroquia.EliminarModeloGenericoParroquia(int.Parse(_AsignarModeloGenericoParroquia.IdAsignarModeloGenericoParroquiaEncriptado));
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                    /*var _objModeloPublicadoConsultado = _objModeloPublicados.ConsultarModeloPublicadoPorId(int.Parse(_objModeloPublicado.IdModeloPublicadoEncriptado)).FirstOrDefault();
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
                    }*/
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
