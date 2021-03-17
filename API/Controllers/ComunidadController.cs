using API.Models.Catalogos;
using API.Models.Entidades;
using API.Models.Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [Authorize]
    public class ComunidadController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoComunidad _objCatalogoComunidad = new CatalogoComunidad();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/comunidad_consultar")]
        public object comunidad_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaComunidades = _objCatalogoComunidad.ConsultarComunidad().Where(c=>c.EstadoComunidad==true && c.Parroquia.EstadoParroquia == true && c.Parroquia.Canton.EstadoCanton == true && c.Parroquia.Canton.Provincia.EstadoProvincia==true).ToList();
                foreach (var item in _listaComunidades)
                {
                    item.IdComunidad = 0;
                    item.Parroquia.IdParroquia = 0;
                    item.Parroquia.Canton.IdCanton = 0;
                    item.Parroquia.Canton.Provincia.IdProvincia = 0;
                }
                _respuesta = _listaComunidades;
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
        [Route("api/comunidad_consultar")]
        public object comunidad_consultar(string _idComunidadEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (string.IsNullOrEmpty(_idComunidadEncriptado) || _idComunidadEncriptado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad.";
                }
                else
                {
                    int _idComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_idComunidadEncriptado).ToString());

                    var _objComunidad = _objCatalogoComunidad.ConsultarComunidadPorId(_idComunidad).Where(c => c.EstadoComunidad == true && c.Parroquia.EstadoParroquia == true && c.Parroquia.Canton.EstadoCanton == true && c.Parroquia.Canton.Provincia.EstadoProvincia == true).FirstOrDefault();

                    _objComunidad.IdComunidad = 0;
                    _objComunidad.Parroquia.IdParroquia = 0;
                    _objComunidad.Parroquia.Canton.IdCanton = 0;
                    _objComunidad.Parroquia.Canton.Provincia.IdProvincia = 0;
                    _respuesta = _objComunidad;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
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
        [Route("api/comunidad_consultarporidparroquia")]
        public object comunidad_consultarporidparroquia(string _idParroquiaEncriptado)
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
                    var _listaComunidades = _objCatalogoComunidad.ConsultarComunidadPorIdParroquia(_idParroquia).Where(c => c.EstadoComunidad == true && c.Parroquia.EstadoParroquia == true && c.Parroquia.Canton.EstadoCanton == true && c.Parroquia.Canton.Provincia.EstadoProvincia == true).ToList();
                    foreach (var item in _listaComunidades)
                    {
                        item.IdComunidad = 0;
                        item.Parroquia.IdParroquia = 0;
                        item.Parroquia.Canton.IdCanton = 0;
                        item.Parroquia.Canton.Provincia.IdProvincia = 0;
                    }
                    _respuesta = _listaComunidades;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
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

        //[HttpPost]
        //[Route("api/comunidad_insertar")]
        //public object comunidad_insertar(Comunidad _objComunidad)
        //{
        //    object _respuesta = new object();
        //    RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
        //    try
        //    {
        //        if (_objComunidad == null)
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
        //            _http.mensaje = "No se encontró el objeto comunidad.";
        //        }
        //        else if (_objComunidad.Parroquia.IdParroquiaEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.IdParroquiaEncriptado))
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
        //            _http.mensaje = "Ingrese el identificador de la parroquia a la que pertenece la comunidad.";
        //        }
        //        else if (_objComunidad.Parroquia.Canton.IdCantonEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.Canton.IdCantonEncriptado))
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
        //            _http.mensaje = "Ingrese el identificador del cantón al que pertenece la comunidad.";
        //        }
        //        else if (_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado))
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
        //            _http.mensaje = "Ingrese el identificador de la provincia a la que pertenece la comunidad.";
        //        }
        //        else if (_objComunidad.NombreComunidad == null || string.IsNullOrEmpty(_objComunidad.NombreComunidad))
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
        //            _http.mensaje = "Ingrese el nombre de la comunidad.";
        //        }
        //        else if (_objCatalogoComunidad.ConsultarComunidad().Where(c => c.NombreComunidad == _objComunidad.NombreComunidad.Trim() && c.Parroquia.IdParroquia == Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.IdParroquiaEncriptado))).FirstOrDefault() != null)
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
        //            _http.mensaje = "Ya existe una comunidad con el mismo nombre, por favor verifique en la lista.";
        //        }
        //        else if (_objComunidad.CodigoComunidad == null || string.IsNullOrEmpty(_objComunidad.CodigoComunidad))
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
        //            _http.mensaje = "Ingrese el codigo de la comunidad.";
        //        }
        //        else if (_objCatalogoComunidad.ConsultarComunidad().Where(c => c.CodigoComunidad == _objComunidad.CodigoComunidad.Trim()).FirstOrDefault() != null)
        //        {
        //            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
        //            _http.mensaje = "Ya existe una comunidad con el mismo código, por favor verifique en la lista.";
        //        }
        //        else
        //        {
        //            _objComunidad.CodigoComunidad = _objComunidad.CodigoComunidad.Trim();
        //            _objComunidad.NombreComunidad = _objComunidad.NombreComunidad.Trim();
        //            _objComunidad.EstadoComunidad = true;
        //            _objComunidad.Parroquia.IdParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.IdParroquiaEncriptado));
        //            _objComunidad.Parroquia.Canton.IdCanton = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.Canton.IdCantonEncriptado));
        //            _objComunidad.Parroquia.Canton.Provincia.IdProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado));
        //            int _idComunidad = _objCatalogoComunidad.InsertarComunidad(_objComunidad);
        //            if (_idComunidad == 0)
        //            {
        //                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
        //                _http.mensaje = "Ocurrió un error al intentar ingresar la comunidad.";
        //            }
        //            else
        //            {
        //                var _objComunidadInsertada = _objCatalogoComunidad.ConsultarComunidadPorId(_idComunidad).Where(c => c.EstadoComunidad == true).FirstOrDefault();
        //                _objComunidadInsertada.IdComunidad = 0;
        //                _objComunidadInsertada.Parroquia.IdParroquia = 0;
        //                _objComunidadInsertada.Parroquia.Canton.IdCanton = 0;
        //                _objComunidadInsertada.Parroquia.Canton.Provincia.IdProvincia = 0;
        //                _respuesta = _objComunidadInsertada;
        //                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
        //    }
        //    return new { respuesta = _respuesta, http = _http };
        //}

        [HttpPost]
        [Route("api/comunidad_insertar")]
        public object comunidad_insertar(HttpRequestMessage request)
        {
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            object _respuesta = new object();
            string rutaImagen = "";
            string CodigoComunidad = HttpContext.Current.Request.Form["CodigoComunidad"];
            var NombreComunidad = HttpContext.Current.Request.Form["NombreComunidad"];
            Comunidad _objComunidad = new Comunidad();
            //Comienza proceso imagen
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var httpRequest = HttpContext.Current.Request;

            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                if (postedFile != null && postedFile.ContentLength > 0)
                {

                    int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {
                        var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                        dict.Add("error", message);
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Tipo de imagen no permitido.";
                        return new { respuesta = _respuesta, http = _http };
                    }
                    //else if (postedFile.ContentLength > MaxContentLength)
                    //{
                    //    var message = string.Format("Please Upload a file upto 1 mb.");
                    //    dict.Add("error", message);
                    //    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    //    _http.mensaje = "Tamaño de imagen ha superado el límite permitido.";
                    //    return new { respuesta = _respuesta, http = _http };
                    //}
                    else
                    {
                        rutaImagen = "Imagenes/" + CodigoComunidad + ext;
                        var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/" + CodigoComunidad+ ext);
                        postedFile.SaveAs(filePath);
                    }
                }
            }
                //Finaliza proceso imagen

                string IdParroquiaEncriptado = HttpContext.Current.Request.Form["IdParroquiaEncriptado"];
                string IdCantonEncriptado = HttpContext.Current.Request.Form["IdCantonEncriptado"];
                string IdProvinciaEncriptado = HttpContext.Current.Request.Form["IdProvinciaEncriptado"];
                string DescripcionComunidad = HttpContext.Current.Request.Form["DescripcionComunidad"];

            _objComunidad.Parroquia = new Parroquia()
            {
                IdParroquiaEncriptado = IdParroquiaEncriptado,
                Canton = new Canton()
                {
                    IdCantonEncriptado = IdCantonEncriptado,
                    Provincia= new Provincia()
                    {
                        IdProvinciaEncriptado=IdProvinciaEncriptado,
                    }
                },
            };
                
                _objComunidad.CodigoComunidad = CodigoComunidad;
                _objComunidad.NombreComunidad = NombreComunidad;
                _objComunidad.DescripcionComunidad = DescripcionComunidad;
                _objComunidad.RutaLogoComunidad = rutaImagen;


            try
            {
                if (_objComunidad == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto comunidad.";
                }
                else if (_objComunidad.Parroquia.IdParroquiaEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.IdParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia a la que pertenece la comunidad.";
                }
                else if (_objComunidad.Parroquia.Canton.IdCantonEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.Canton.IdCantonEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón al que pertenece la comunidad.";
                }
                else if (_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia a la que pertenece la comunidad.";
                }
                else if (_objComunidad.CodigoComunidad == null || string.IsNullOrEmpty(_objComunidad.CodigoComunidad))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo de la comunidad.";
                }
                else if (_objComunidad.NombreComunidad == null || string.IsNullOrEmpty(_objComunidad.NombreComunidad))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre de la comunidad.";
                }
                else if (_objCatalogoComunidad.ConsultarComunidad().Where(c => c.CodigoComunidad == _objComunidad.CodigoComunidad.Trim()).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe una comunidad con el mismo código, por favor verifique en la lista.";
                }
                else if (_objCatalogoComunidad.ConsultarComunidad().Where(c => c.NombreComunidad == _objComunidad.NombreComunidad.Trim() && c.Parroquia.IdParroquia == Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.IdParroquiaEncriptado))).FirstOrDefault() != null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe una comunidad con el mismo nombre, por favor verifique en la lista.";
                }
                
                
                else
                {
                    _objComunidad.CodigoComunidad = _objComunidad.CodigoComunidad.Trim();
                    _objComunidad.NombreComunidad = _objComunidad.NombreComunidad.Trim();
                    _objComunidad.EstadoComunidad = true;
                    _objComunidad.Parroquia.IdParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.IdParroquiaEncriptado));
                    _objComunidad.Parroquia.Canton.IdCanton = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.Canton.IdCantonEncriptado));
                    _objComunidad.Parroquia.Canton.Provincia.IdProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado));
                    int _idComunidad = _objCatalogoComunidad.InsertarComunidad(_objComunidad);
                    if (_idComunidad == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al intentar ingresar la comunidad.";
                    }
                    else
                    {
                        var _objComunidadInsertada = _objCatalogoComunidad.ConsultarComunidadPorId(_idComunidad).Where(c => c.EstadoComunidad == true).FirstOrDefault();
                        _objComunidadInsertada.IdComunidad = 0;
                        _objComunidadInsertada.Parroquia.IdParroquia = 0;
                        _objComunidadInsertada.Parroquia.Canton.IdCanton = 0;
                        _objComunidadInsertada.Parroquia.Canton.Provincia.IdProvincia = 0;
                        _respuesta = _objComunidadInsertada;
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
        [Route("api/comunidad_modificar")]
        public object comunidad_modificar(HttpRequestMessage request)
        {
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            object _respuesta = new object();
            string rutaImagen = "";
            string CodigoComunidad = HttpContext.Current.Request.Form["CodigoComunidad"];
            var NombreComunidad = HttpContext.Current.Request.Form["NombreComunidad"];
            Comunidad _objComunidad = new Comunidad();
            //Comienza proceso imagen
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var httpRequest = HttpContext.Current.Request;

            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                if (postedFile != null && postedFile.ContentLength > 0)
                {

                    int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {
                        var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                        dict.Add("error", message);
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Tipo de imagen no permitido.";
                        return new { respuesta = _respuesta, http = _http };
                    }
                    //else if (postedFile.ContentLength > MaxContentLength)
                    //{
                    //    var message = string.Format("Please Upload a file upto 1 mb.");
                    //    dict.Add("error", message);
                    //    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    //    _http.mensaje = "Tamaño de imagen ha superado el límite permitido.";
                    //    return new { respuesta = _respuesta, http = _http };
                    //}
                    else
                    {
                        rutaImagen = "Imagenes/" + CodigoComunidad + ext;
                        var filePath = HttpContext.Current.Server.MapPath("~/Imagenes/" + CodigoComunidad + ext);
                        postedFile.SaveAs(filePath);
                    }
                }
            }
            //Finaliza proceso imagen
            string IdComunidadEncriptado = HttpContext.Current.Request.Form["IdComunidadEncriptado"];
            string IdParroquiaEncriptado = HttpContext.Current.Request.Form["IdParroquiaEncriptado"];
            string IdCantonEncriptado = HttpContext.Current.Request.Form["IdCantonEncriptado"];
            string IdProvinciaEncriptado = HttpContext.Current.Request.Form["IdProvinciaEncriptado"];
            string DescripcionComunidad = HttpContext.Current.Request.Form["DescripcionComunidad"];

            _objComunidad.Parroquia = new Parroquia()
            {
                IdParroquiaEncriptado = IdParroquiaEncriptado,
                Canton = new Canton()
                {
                    IdCantonEncriptado = IdCantonEncriptado,
                    Provincia = new Provincia()
                    {
                        IdProvinciaEncriptado = IdProvinciaEncriptado,
                    }
                },
            };
            _objComunidad.IdComunidadEncriptado = IdComunidadEncriptado;
            _objComunidad.CodigoComunidad = CodigoComunidad;
            _objComunidad.NombreComunidad = NombreComunidad;
            _objComunidad.DescripcionComunidad = DescripcionComunidad;
            _objComunidad.RutaLogoComunidad = rutaImagen;

            try
            {
                if (_objComunidad == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se encontró el objeto comunidad.";
                }
                else if (_objComunidad.IdComunidadEncriptado == null || string.IsNullOrEmpty(_objComunidad.IdComunidadEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad.";
                }
                else if (_objComunidad.Parroquia.IdParroquiaEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.IdParroquiaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la parroquia.";
                }
                else if (_objComunidad.Parroquia.Canton.IdCantonEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.Canton.IdCantonEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cantón.";
                }
                else if (_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado == null || string.IsNullOrEmpty(_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la provincia.";
                }
                else if (_objComunidad.CodigoComunidad == null || string.IsNullOrEmpty(_objComunidad.CodigoComunidad))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el codigo de la comunidad.";
                }
                else if (_objComunidad.NombreComunidad == null || string.IsNullOrEmpty(_objComunidad.NombreComunidad))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre de la comunidad.";
                }
                
                else
                {
                    int _idComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.IdComunidadEncriptado));
                    var _objComunidadConsultada = _objCatalogoComunidad.ConsultarComunidadPorId(_idComunidad).FirstOrDefault();
                    if (_objComunidadConsultada == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La comunidad que intenta modificar no existe.";
                    }
                    else if (_objCatalogoComunidad.ConsultarComunidad().Where(c => c.CodigoComunidad == _objComunidad.CodigoComunidad.Trim() && c.IdComunidad != _idComunidad).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe una comunidad con el mismo código, por favor verifique en la lista.";
                    }
                    else if (_objCatalogoComunidad.ConsultarComunidad().Where(c => c.NombreComunidad == _objComunidad.NombreComunidad.Trim() && c.IdComunidad != _idComunidad).FirstOrDefault() != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe una comunidad con el mismo nombre, por favor verifique en la lista.";
                    }
                    
                    else
                    {
                        _objComunidad.CodigoComunidad = _objComunidad.CodigoComunidad.Trim();
                        _objComunidad.NombreComunidad = _objComunidad.NombreComunidad.Trim();
                        _objComunidad.IdComunidad = _idComunidad;
                        _objComunidad.Parroquia.IdParroquia = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.IdParroquiaEncriptado));
                        _objComunidad.Parroquia.Canton.IdCanton = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.Canton.IdCantonEncriptado));
                        _objComunidad.Parroquia.Canton.Provincia.IdProvincia = Convert.ToInt32(_seguridad.DesEncriptar(_objComunidad.Parroquia.Canton.Provincia.IdProvinciaEncriptado));
                        _objComunidad.EstadoComunidad = true;
                        int _idComunidadModificado = _objCatalogoComunidad.ModificarComunidad(_objComunidad);
                        if (_idComunidadModificado == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al intentar modificar la comunidad.";
                        }
                        else
                        {
                            var _objComunidadModificada = _objCatalogoComunidad.ConsultarComunidadPorId(_idComunidad).FirstOrDefault();
                            _objComunidadModificada.IdComunidad = 0;
                            _objComunidadModificada.Parroquia.IdParroquia = 0;
                            _objComunidadModificada.Parroquia.Canton.IdCanton = 0;
                            _objComunidadModificada.Parroquia.Canton.Provincia.IdProvincia = 0;
                            _respuesta = _objComunidadModificada;
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
        [Route("api/comunidad_eliminar")]
        public object comunidad_eliminar(string _idComunidadEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idComunidadEncriptado == null || string.IsNullOrEmpty(_idComunidadEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la comunidad que va a eliminar.";
                }
                else
                {
                    int _idComunidad = Convert.ToInt32(_seguridad.DesEncriptar(_idComunidadEncriptado));
                    var _objComunidadConsultada = _objCatalogoComunidad.ConsultarComunidadPorId(_idComunidad).FirstOrDefault();
                    if (_objComunidadConsultada == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "La comunidad que intenta eliminar no existe.";
                    }
                    else if (_objComunidadConsultada.Utilizado == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "La comunidad ya ha sido utilizada, por la tanto no puede ser eliminado.";
                    }
                    else
                    {
                        _objCatalogoComunidad.EliminarComunidad(_idComunidad);
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
