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
    public class OpcionDosMatrizController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoOpcionUnoMatriz _objCatalogoOpcionUnoMatriz = new CatalogoOpcionUnoMatriz();
        CatalogoOpcionDosMatriz _objCatalogoOpcionDosMatriz = new CatalogoOpcionDosMatriz();
        CatalogoConfigurarMatriz _objCatalogoConfigurarMatriz = new CatalogoConfigurarMatriz();
        CatalogoPregunta _objCatalogoPregunta = new CatalogoPregunta();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/opciondosmatriz_eliminar")]
        public object opciondosmatriz_eliminar(string _idOpcionDosMatrizEncriptado)
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                if (_idOpcionDosMatrizEncriptado == null || string.IsNullOrEmpty(_idOpcionDosMatrizEncriptado))
                {
                    _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
                    _http.mensaje = "Ingrese el identificador de la opción dos matriz";
                }
                else
                {
                    int _idOpcionDosMatriz = Convert.ToInt32(_seguridad.DesEncriptar(_idOpcionDosMatrizEncriptado));
                    var _listaConfigurarMatriz = _objCatalogoConfigurarMatriz.ConsultarConfigurarMatrizPorIdOpcionDosMatriz(_idOpcionDosMatriz).Where(c => c.Estado == true).ToList();
                    var _opcionUno = _listaConfigurarMatriz.Where(c => c.OpcionUnoMatriz.Utilizado == "1" || c.OpcionUnoMatriz.Encajonamiento == "1").FirstOrDefault();
                    if (_opcionUno != null)
                    {
                        _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "400").FirstOrDefault();
                        _http.mensaje = "No se puede eliminar porque la pregunta ya ha sido versionada o encajonada.";
                    }
                    else
                    {
                        if (_listaConfigurarMatriz.Count == 0)
                        {
                            _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "404").FirstOrDefault();
                            _http.mensaje = "No se encontró el objeto opción dos matriz";
                        }
                        else
                        {
                            foreach (var itemConfMatriz in _listaConfigurarMatriz)
                            {
                                _objCatalogoConfigurarMatriz.EliminarConfigurarMatriz(itemConfMatriz.IdConfigurarMatriz);
                            }
                            _objCatalogoOpcionDosMatriz.EliminarOpcionDosMatriz(_idOpcionDosMatriz);
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
    }
}
