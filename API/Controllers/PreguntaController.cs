﻿using System;
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
    public class PreguntaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        Seguridad _seguridad = new Seguridad();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        CatalogoTipoPregunta _objCatalogoTipoPregunta = new CatalogoTipoPregunta();
        CatalogoSeccion _objCatalogoSeccion = new CatalogoSeccion();
        CatalogoPreguntaAbierta _objCatalogoPreguntaAbierta = new CatalogoPreguntaAbierta();
        CatalogoOpcionPreguntaSeleccion _objCatalogoOpcionPreguntaSeleccion = new CatalogoOpcionPreguntaSeleccion();
        CatalogoOpcionUnoMatriz _objCatalogoOpcionUnoMatriz = new CatalogoOpcionUnoMatriz();
        CatalogoConfigurarMatriz _objCatalogoConfigurarMatriz = new CatalogoConfigurarMatriz();
        CatalogoOpcionDosMatriz _objCatalogoOpcionDosMatriz = new CatalogoOpcionDosMatriz();
        [HttpPost]
        [Route("api/pregunta_insertar")]
        public object pregunta_insertar(Pregunta _objPregunta)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPregunta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto pregunta";
                }
                else if (_objPregunta.Seccion.IdSeccionEncriptado == null || string.IsNullOrEmpty(_objPregunta.Seccion.IdSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }
                else if (_objPregunta.TipoPregunta.IdTipoPreguntaEncriptado == null || string.IsNullOrEmpty(_objPregunta.TipoPregunta.IdTipoPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del tipo de pregunta";
                }
                else if (_objPregunta.Descripcion == null || string.IsNullOrEmpty(_objPregunta.Descripcion))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción de la pregunta";
                }
                else if (_objPregunta.Orden == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el orden de la pregunta";
                }
                else
                {
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_objPregunta.Seccion.IdSeccionEncriptado));
                    int _idTipoPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objPregunta.TipoPregunta.IdTipoPreguntaEncriptado));
                    _objPregunta.Seccion.IdSeccion = _idSeccion;
                    _objPregunta.TipoPregunta.IdTipoPregunta = _idTipoPregunta;
                    _objPregunta.Estado = true;
                    int _idPregunta = _objCatalogoPregunta.InsertarPregunta(_objPregunta);
                    if (_idPregunta == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ingresar la pregunta";
                    }
                    else
                    {
                        _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                        _objPregunta.IdPregunta = 0;
                        _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                        _objPregunta.Seccion.IdSeccion = 0;
                        _objPregunta.Seccion.Componente.IdComponente = 0;
                        _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objPregunta;
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
        [Route("api/pregunta_modificar")]
        public object pregunta_modificar(Pregunta _objPregunta)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPregunta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto pregunta";
                }
                else if (_objPregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objPregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else if (_objPregunta.Seccion.IdSeccionEncriptado == null || string.IsNullOrEmpty(_objPregunta.Seccion.IdSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }
                else if (_objPregunta.TipoPregunta.IdTipoPreguntaEncriptado == null || string.IsNullOrEmpty(_objPregunta.TipoPregunta.IdTipoPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador del tipo de pregunta";
                }
                else if (_objPregunta.Descripcion == null || string.IsNullOrEmpty(_objPregunta.Descripcion))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese la descripción de la pregunta";
                }
                else if (_objPregunta.Orden == 0)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el orden de la pregunta";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objPregunta.IdPreguntaEncriptado));
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_objPregunta.Seccion.IdSeccionEncriptado));
                    int _idTipoPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objPregunta.TipoPregunta.IdTipoPreguntaEncriptado));
                    _objPregunta.IdPregunta = _idPregunta;
                    _objPregunta.Seccion.IdSeccion = _idSeccion;
                    _objPregunta.TipoPregunta.IdTipoPregunta = _idTipoPregunta;
                    _objPregunta.Estado = true;
                    _idPregunta = _objCatalogoPregunta.ModificarPregunta(_objPregunta);
                    if (_idPregunta == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar modificar la pregunta";
                    }
                    else
                    {
                        _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                        _objPregunta.IdPregunta = 0;
                        _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                        _objPregunta.Seccion.IdSeccion = 0;
                        _objPregunta.Seccion.Componente.IdComponente = 0;
                        _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objPregunta;
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
        [Route("api/subir_pregunta")]
        public object subir_pregunta(Pregunta _objPregunta)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPregunta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto pregunta";
                }
                else if (_objPregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objPregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
               
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objPregunta.IdPreguntaEncriptado));
                  
                 
                    _objPregunta.IdPregunta = _idPregunta;
                    _objPregunta.Estado = true;
                    _idPregunta = _objCatalogoPregunta.SubirPregunta(_objPregunta);
                    if (_idPregunta == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ordenar la pregunta";
                    }
                    else
                    {
                        _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                        _objPregunta.IdPregunta = 0;
                        _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                        _objPregunta.Seccion.IdSeccion = 0;
                        _objPregunta.Seccion.Componente.IdComponente = 0;
                        _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objPregunta;
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
        [Route("api/bajar_pregunta")]
        public object bajar_pregunta(Pregunta _objPregunta)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_objPregunta == null)
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el objeto pregunta";
                }
                else if (_objPregunta.IdPreguntaEncriptado == null || string.IsNullOrEmpty(_objPregunta.IdPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }

                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_objPregunta.IdPreguntaEncriptado));


                    _objPregunta.IdPregunta = _idPregunta;
                    _objPregunta.Estado = true;
                    _idPregunta = _objCatalogoPregunta.BajarPregunta(_objPregunta);
                    if (_idPregunta == 0)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "Ocurrió un problema al intentar ordenar la pregunta";
                    }
                    else
                    {
                        _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                        _objPregunta.IdPregunta = 0;
                        _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                        _objPregunta.Seccion.IdSeccion = 0;
                        _objPregunta.Seccion.Componente.IdComponente = 0;
                        _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        _respuesta = _objPregunta;
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
        [Route("api/pregunta_cambiarestado")]
        public object pregunta_cambiarestado(string _idPreguntaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPreguntaEncriptado == null || string.IsNullOrEmpty(_idPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_idPreguntaEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).FirstOrDefault();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta que intenta modificar";
                    }
                    else
                    {
                        bool _nuevoEstado = false;
                        if(_objPregunta.Estado==false)
                        {
                            _nuevoEstado = true;
                        }
                        _objPregunta.Estado = _nuevoEstado;
                        _idPregunta = _objCatalogoPregunta.ModificarPregunta(_objPregunta);
                        if (_idPregunta == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un problema al intentar modificar la pregunta";
                        }
                        else
                        {
                            _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).FirstOrDefault();
                            _objPregunta.IdPregunta = 0;
                            _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objPregunta.Seccion.IdSeccion = 0;
                            _objPregunta.Seccion.Componente.IdComponente = 0;
                            _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                            _respuesta = _objPregunta;
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
        [Route("api/pregunta_eliminar")]
        public object pregunta_eliminar(string _idPreguntaEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idPreguntaEncriptado == null || string.IsNullOrEmpty(_idPreguntaEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la pregunta";
                }
                else
                {
                    int _idPregunta = Convert.ToInt32(_seguridad.DesEncriptar(_idPreguntaEncriptado));
                    var _objPregunta = _objCatalogoPregunta.ConsultarPreguntaPorId(_idPregunta).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objPregunta == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró la pregunta que intenta eliminar";
                    }
                    else if (_objPregunta.Encajonamiento == "1")
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No se puede eliminar preguntas que han sido encajonadas";
                    }
                    else if (_objPregunta.Utilizado == "1")
                    {
                        bool _nuevoEstado = false;
                        if (_objPregunta.Estado == false)
                        {
                            _nuevoEstado = true;
                        }
                        _objPregunta.Estado = _nuevoEstado;
                        _idPregunta = _objCatalogoPregunta.ModificarPregunta(_objPregunta);
                        if (_idPregunta == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                            _http.mensaje = "Ocurrió un problema al intentar cambiar el estado de la pregunta";
                        }
                        else
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
                        }
                    }
                    else
                    {
                        if(_objPregunta.TipoPregunta.Identificador==1)
                        {
                            var _objPreguntaAbierta = _objCatalogoPreguntaAbierta.ConsultarPreguntaAbiertaPorIdPregunta(_idPregunta).FirstOrDefault();
                            if (_objPreguntaAbierta != null)
                            {
                                _objCatalogoPreguntaAbierta.EliminarPreguntaAbierta(_objPreguntaAbierta.IdPreguntaAbierta);
                            }
                        }
                        else if(_objPregunta.TipoPregunta.Identificador==2)
                        {
                            var _listaOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorIdPregunta(_idPregunta).ToList();
                            foreach (var item in _listaOpcionPreguntaSeleccion)
                            {
                                _objCatalogoOpcionPreguntaSeleccion.EliminarOpcionPreguntaSeleccion(item.IdOpcionPreguntaSeleccion);
                            }
                        }
                        else if (_objPregunta.TipoPregunta.Identificador == 3)
                        {
                            var _listaOpcionPreguntaSeleccionMultiple = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorIdPregunta(_idPregunta).ToList();
                            foreach (var item in _listaOpcionPreguntaSeleccionMultiple)
                            {
                                _objCatalogoOpcionPreguntaSeleccion.EliminarOpcionPreguntaSeleccion(item.IdOpcionPreguntaSeleccion);
                            }
                        }
                        else if (_objPregunta.TipoPregunta.Identificador == 4)
                        {
                            var _listaConfigurarMatriz = _objCatalogoConfigurarMatriz.ConsultarConfigurarMatrizPorIdPregunta(_idPregunta).ToList();
                            if (_listaConfigurarMatriz.Count > 0)
                            {
                                var _listaOpcionDosAgrupada = _listaConfigurarMatriz.GroupBy(c => c.OpcionDosMatriz.IdOpcionDosMatriz).ToList();
                                var _listaOpcionUnoMatriz = _objCatalogoOpcionUnoMatriz.ConsultarOpcionUnoMatrizPorIdPregunta(_idPregunta).ToList();
                                foreach (var itemOpcionUno in _listaOpcionUnoMatriz)
                                {
                                    var _listaConfigurarMatrizPorOpcionUno = _listaConfigurarMatriz.Where(c => c.OpcionUnoMatriz.IdOpcionUnoMatriz == itemOpcionUno.IdOpcionUnoMatriz).ToList();
                                    foreach (var itemConfMatriz in _listaConfigurarMatrizPorOpcionUno)
                                    {
                                        _objCatalogoConfigurarMatriz.EliminarConfigurarMatriz(itemConfMatriz.IdConfigurarMatriz);
                                    }
                                }
                                foreach (var item in _listaOpcionDosAgrupada)
                                {
                                    _objCatalogoOpcionDosMatriz.EliminarOpcionDosMatriz(item.Key);
                                }
                                foreach (var itemOpcionUno in _listaOpcionUnoMatriz)
                                {
                                    _objCatalogoOpcionUnoMatriz.EliminarOpcionUnoMatriz(itemOpcionUno.IdOpcionUnoMatriz);
                                }
                            }
                        }
                        _objCatalogoPregunta.EliminarPregunta(_idPregunta);
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
        [Route("api/pregunta_consultar")]
        public object pregunta_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _lista = _objCatalogoPregunta.ConsultarPregunta().Where(c => c.Estado == true).ToList();
                foreach (var _objPregunta in _lista)
                {
                    _objPregunta.IdPregunta = 0;
                    _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                    _objPregunta.Seccion.IdSeccion = 0;
                    _objPregunta.Seccion.Componente.IdComponente = 0;
                    _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                }
                _respuesta = _lista;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
            }
            return new { respuesta = _respuesta, http = _http };
        }
        [HttpPost]
        [Route("api/pregunta_consultarporidseccion")]
        public object pregunta_consultarporidseccion(string _idSeccionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idSeccionEncriptado == null || string.IsNullOrEmpty(_idSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }
                else
                {
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_idSeccionEncriptado));
                    var _objSeccion = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objSeccion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto sección";
                    }
                    else
                    {
                        var _lista = _objCatalogoPregunta.ConsultarPreguntaPorIdSeccion(_idSeccion).Where(c => c.Estado == true).OrderBy(c => c.Orden).ToList();
                        foreach (var _objPregunta in _lista)
                        {
                            _objPregunta.IdPregunta = 0;
                            _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objPregunta.Seccion.IdSeccion = 0;
                            _objPregunta.Seccion.Componente.IdComponente = 0;
                            _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _lista;
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
        [Route("api/pregunta_consultarseleccionunicaporidseccion")]
        public object pregunta_consultarseleccionunicaporidseccion(string _idSeccionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idSeccionEncriptado == null || string.IsNullOrEmpty(_idSeccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la sección";
                }
                else
                {
                    int _idSeccion = Convert.ToInt32(_seguridad.DesEncriptar(_idSeccionEncriptado));
                    var _objSeccion = _objCatalogoSeccion.ConsultarSeccionPorId(_idSeccion).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objSeccion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto sección";
                    }
                    else
                    {
                        int _identificadorTipoPregunta = 2;
                        var _lista = _objCatalogoPregunta.ConsultarPreguntaPorIdSeccionPorIdentificadorTipoPregunta(_idSeccion, _identificadorTipoPregunta).Where(c => c.Estado == true).OrderBy(c => c.Orden).ToList();
                        foreach (var _objPregunta in _lista)
                        {
                            _objPregunta.IdPregunta = 0;
                            _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objPregunta.Seccion.IdSeccion = 0;
                            _objPregunta.Seccion.Componente.IdComponente = 0;
                            _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _lista;
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
        [Route("api/pregunta_consultarpornoencajonadasporopcionpreguntaseleccion")]
        public object pregunta_consultarpornoencajonadasporopcionpreguntaseleccion(string _idOpcionPreguntaSeleccionEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idOpcionPreguntaSeleccionEncriptado == null || string.IsNullOrEmpty(_idOpcionPreguntaSeleccionEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la opción pregunta selección";
                }
                else
                {
                    int _idOpcionPreguntaSeleccion = Convert.ToInt32(_seguridad.DesEncriptar(_idOpcionPreguntaSeleccionEncriptado));
                    var _objOpcionPreguntaSeleccion = _objCatalogoOpcionPreguntaSeleccion.ConsultarOpcionPreguntaSeleccionPorId(_idOpcionPreguntaSeleccion).Where(c => c.Estado == true).FirstOrDefault();
                    if (_objOpcionPreguntaSeleccion == null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                        _http.mensaje = "No se encontró el objeto opción pregunta selección";
                    }
                    else
                    {
                        var _lista = _objCatalogoPregunta.ConsultarPreguntaNoEncajonadasPorOpcionPreguntaSeleccion(_objOpcionPreguntaSeleccion.IdOpcionPreguntaSeleccion,_objOpcionPreguntaSeleccion.Pregunta.Seccion.IdSeccion,_objOpcionPreguntaSeleccion.Pregunta.IdPregunta).Where(c=>c.Estado == true).ToList();
                        foreach (var _objPregunta in _lista)
                        {
                            _objPregunta.IdPregunta = 0;
                            _objPregunta.TipoPregunta.IdTipoPregunta = 0;
                            _objPregunta.Seccion.IdSeccion = 0;
                            _objPregunta.Seccion.Componente.IdComponente = 0;
                            _objPregunta.Seccion.Componente.CuestionarioGenerico.IdCuestionarioGenerico = 0;
                        }
                        _respuesta = _lista;
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