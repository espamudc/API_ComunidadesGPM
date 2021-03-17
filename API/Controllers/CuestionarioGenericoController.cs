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
    public class CuestionarioGenericoController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoCuestionarioGenerico _objCatalogoCuestionarioGenerico = new CatalogoCuestionarioGenerico();
        Seguridad _seguridad = new Seguridad();
        CatalogoAsignarEncuestado _objCatalogoAsignarEncuestado = new CatalogoAsignarEncuestado();
        [HttpGet]
        [Route("api/cuestionario/finalizar")]
        public object cuestionarioFinalizar(string _IdAsignarEncuestado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_IdAsignarEncuestado == null || string.IsNullOrEmpty(_IdAsignarEncuestado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Algunos campos están vacíos";
                    return new { http = _http };
                }
                int IdAsignarEncuestado = int.Parse(_seguridad.DesEncriptar(_IdAsignarEncuestado));
                var objCatalogoAsignarEncuestado = _objCatalogoAsignarEncuestado.ConsultarAsignarEncuestadoPorId(IdAsignarEncuestado).Where(c => c.Estado == true).FirstOrDefault();
                if (objCatalogoAsignarEncuestado == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "El asignar encuestado no existe";
                    return new { http = _http };
                }else{
                    var _respuestas = _objCatalogoCuestionarioGenerico.FinalizarEncuesta(IdAsignarEncuestado);
                    if (_respuestas >=0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        return new { respuesta = _respuestas, http = _http };
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
        [Route("api/cuestionariogenerico_insertar")]
        public object cuestionariogenerico_insertar(CuestionarioGenerico _objCuestionarioGenerico)
        {

            
object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if(_objCuestionarioGenerico==null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto cuestionario genérico";
                }
                else if(_objCuestionarioGenerico.Nombre == null || string.IsNullOrEmpty(_objCuestionarioGenerico.Nombre.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del cuestionario genérico";
                }
                else if(_objCuestionarioGenerico.Descripcion == null || string.IsNullOrEmpty(_objCuestionarioGenerico.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del cuestionario genérico";
                }
                else if (_objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenerico().Where(c=>c.Estado==true && c.Nombre==_objCuestionarioGenerico.Nombre.Trim()).ToList().Count>0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                    _http.mensaje = "Ya existe un cuestionario con el mismo nombre, verifique en la lista.";
                }
                else
                {
                    _objCuestionarioGenerico.Nombre = _objCuestionarioGenerico.Nombre.Trim();
                    _objCuestionarioGenerico.Estado = true;
                    int _idCuestionarioGenerico = _objCatalogoCuestionarioGenerico.InsertarCuestionarioGenerico(_objCuestionarioGenerico);
                    if(_idCuestionarioGenerico==0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un error al tratar de ingresar el cuestionario.";
                    }
                    else
                    {
                        var _objCuestionarioGenericoIngresado = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(C => C.Estado == true).FirstOrDefault();
                        _objCuestionarioGenericoIngresado.IdCuestionarioGenerico = 0;
                        _respuesta = _objCuestionarioGenericoIngresado;
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
        [Route("api/cuestionariogenerico_modificar")]
        public object cuestionariogenerico_modificar(CuestionarioGenerico _objCuestionarioGenerico)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objCuestionarioGenerico == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto cuestionario genérico";
                }
                else if (_objCuestionarioGenerico.IdCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_objCuestionarioGenerico.IdCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario genérico";
                }
                else if (_objCuestionarioGenerico.Nombre == null || string.IsNullOrEmpty(_objCuestionarioGenerico.Nombre.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el nombre del cuestionario genérico";
                }
                else if (_objCuestionarioGenerico.Descripcion == null || string.IsNullOrEmpty(_objCuestionarioGenerico.Descripcion.Trim()))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción del cuestionario genérico";
                }
                else
                {
                    int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_objCuestionarioGenerico.IdCuestionarioGenericoEncriptado));
                    if (_objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenerico().Where(c => c.Estado == true && c.Nombre == _objCuestionarioGenerico.Nombre.Trim() && c.IdCuestionarioGenerico!= _idCuestionarioGenerico).ToList().Count > 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "406").FirstOrDefault();
                        _http.mensaje = "Ya existe un cuestionario con el mismo nombre, verifique en la lista.";
                    }
                    else
                    {
                        _objCuestionarioGenerico.IdCuestionarioGenerico = _idCuestionarioGenerico;
                        _objCuestionarioGenerico.Nombre = _objCuestionarioGenerico.Nombre.Trim();
                        _objCuestionarioGenerico.Estado = true;
                        _idCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ModificarCuestionarioGenerico(_objCuestionarioGenerico);
                        if (_idCuestionarioGenerico == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un error al tratar de modificar el cuestionario.";
                        }
                        else
                        {
                            var _objCuestionarioGenericoModificado = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(C => C.Estado == true).FirstOrDefault();
                            _objCuestionarioGenericoModificado.IdCuestionarioGenerico = 0;
                            _respuesta = _objCuestionarioGenericoModificado;
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
        [Route("api/cuestionariogenerico_consultar")]
        public object cuestionariogenerico_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaCuestionariosGenericos = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenerico().Where(c => c.Estado == true).ToList();
                foreach (var item in _listaCuestionariosGenericos)
                {
                    item.IdCuestionarioGenerico = 0;
                }
                _respuesta = _listaCuestionariosGenericos;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/cuestionariogenerico_eliminar")]
        public object cuestionariogenerico_eliminar(string _idCuestionarioGenericoEncriptado)
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
                int _idCuestionarioGenerico = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                var _objCuestionarioGenerico = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorId(_idCuestionarioGenerico).Where(C => C.Estado == true).FirstOrDefault();
                if (_objCuestionarioGenerico == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                    _http.mensaje = "No se encontró el cuestionario que intenta eliminar";

                }
                else if (_objCuestionarioGenerico.Utilizado == "1")
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "No se puede eliminar el cuestionario porque ya ha sido utilizado";
                }
                else
                {
                    _objCatalogoCuestionarioGenerico.EliminarCuestionarioGenerico(_idCuestionarioGenerico);
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
        [Route("api/cuestionariogenerico_consultarporidconcomponenteconseccionconpreguntas")]
        public object cuestionario_generico_consultar(string _idCuestionarioGenericoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            //var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorIdConComponenteSeccionPregunta(int.Parse(_idCuestionarioGenerico)).Where(c => c.Estado == true).FirstOrDefault();

            try
            {
                if (_idCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario";
                }
                else
                {
                    int _idCuestionario = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                    var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorIdConComponenteSeccionPregunta(_idCuestionario).Where(c => c.Estado == true).FirstOrDefault();

                    _objCuestionario.IdCuestionarioGenerico = 0;

                    foreach (var componente in _objCuestionario.listaComponente)
                    {
                        componente.IdComponente = 0;
                        foreach (var seccion in componente.listaSeccion)
                        {
                            seccion.IdSeccion = 0;
                            foreach (var pregunta in seccion.listaPregunta)
                            {
                                pregunta.IdPregunta = 0;
                                pregunta.TipoPregunta.IdTipoPregunta = 0;
                            }
                        }
                    }
                    _respuesta = _objCuestionario;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }
                
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }

            //var _http = "g";
            return new { respuesta = _respuesta, http = _http };
        }


        [HttpPost]
        [Route("api/cuestionariogenerico_consultarporidconcomponenteconseccionconpreguntasRandom")]
        public object cuestionario_generico_consultarRandom(string _idCuestionarioGenericoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            //var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorIdConComponenteSeccionPregunta(int.Parse(_idCuestionarioGenerico)).Where(c => c.Estado == true).FirstOrDefault();

            try
            {
                if (_idCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario";
                }
                else
                {
                    int _idCuestionario = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                    var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorIdConComponenteSeccionPreguntaRandom(_idCuestionario).Where(c => c.Estado == true).FirstOrDefault();

                    _objCuestionario.IdCuestionarioGenerico = 0;

                    foreach (var componente in _objCuestionario.listaComponente)
                    {
                        componente.IdComponente = 0;
                        foreach (var seccion in componente.listaSeccion)
                        {
                            seccion.IdSeccion = 0;
                            foreach (var pregunta in seccion.listaPregunta)
                            {
                                pregunta.IdPregunta = 0;
                                pregunta.TipoPregunta.IdTipoPregunta = 0;
                            }
                        }
                    }
                    _respuesta = _objCuestionario;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }

            //var _http = "g";
            return new { respuesta = _respuesta, http = _http };
        }


        [HttpPost]
        [Route("api/cuestionariogenerico_consultarporidconcomponenteconseccionconpreguntasRandom")]
        public object cuestionario_generico_consultarRandom(string _idCuestionarioGenericoEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            //var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorIdConComponenteSeccionPregunta(int.Parse(_idCuestionarioGenerico)).Where(c => c.Estado == true).FirstOrDefault();

            try
            {
                if (_idCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario";
                }
                else
                {
                    int _idCuestionario = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                    var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorIdConComponenteSeccionPreguntaRandom(_idCuestionario).Where(c => c.Estado == true).FirstOrDefault();

                    _objCuestionario.IdCuestionarioGenerico = 0;

                    foreach (var componente in _objCuestionario.listaComponente)
                    {
                        componente.IdComponente = 0;
                        foreach (var seccion in componente.listaSeccion)
                        {
                            seccion.IdSeccion = 0;
                            foreach (var pregunta in seccion.listaPregunta)
                            {
                                pregunta.IdPregunta = 0;
                                pregunta.TipoPregunta.IdTipoPregunta = 0;
                            }
                        }
                    }
                    _respuesta = _objCuestionario;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }

            //var _http = "g";
            return new { respuesta = _respuesta, http = _http };
        }


        [HttpPost]
        [Route("api/cuestionariogenerico_consultarporversion")]
        public object cuestionariogenerico_consultarporversion(string _idCuestionarioGenericoEncriptado, string IdCabeceraVersionCuestionarioEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            //var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorIdConComponenteSeccionPregunta(int.Parse(_idCuestionarioGenerico)).Where(c => c.Estado == true).FirstOrDefault();

            try
            {
                if (_idCuestionarioGenericoEncriptado == null || string.IsNullOrEmpty(_idCuestionarioGenericoEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del cuestionario";
                }
                else if (IdCabeceraVersionCuestionarioEncriptado == null || string.IsNullOrEmpty(IdCabeceraVersionCuestionarioEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la version del cuestionario";
                }
                else
                {
                    int _idCuestionario = Convert.ToInt32(_seguridad.DesEncriptar(_idCuestionarioGenericoEncriptado));
                    int _idVersionCuestionario = Convert.ToInt32(_seguridad.DesEncriptar(IdCabeceraVersionCuestionarioEncriptado));
                    var _objCuestionario = _objCatalogoCuestionarioGenerico.ConsultarCuestionarioGenericoPorVersion(_idCuestionario, _idVersionCuestionario).Where(c => c.Estado == true).FirstOrDefault();

                    _objCuestionario.IdCuestionarioGenerico = 0;

                    foreach (var componente in _objCuestionario.listaComponente)
                    {
                        componente.IdComponente = 0;
                        foreach (var seccion in componente.listaSeccion)
                        {
                            seccion.IdSeccion = 0;
                            foreach (var pregunta in seccion.listaPregunta)
                            {
                                pregunta.IdPregunta = 0;
                                pregunta.TipoPregunta.IdTipoPregunta = 0;
                            }
                        }
                    }
                    _respuesta = _objCuestionario;
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }

            //var _http = "g";
            return new { respuesta = _respuesta, http = _http };
        }

    }
}
