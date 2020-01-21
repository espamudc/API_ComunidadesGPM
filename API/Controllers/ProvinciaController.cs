﻿using API.Models.Catalogos;
using API.Models.Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ProvinciaController : ApiController
    {
        CatalogoRespuestasHTTP _objCatalogoRespuestasHTTP = new CatalogoRespuestasHTTP();
        CatalogoProvincia _objCatalogoProvincia = new CatalogoProvincia();
        Seguridad _seguridad = new Seguridad();

        [HttpPost]
        [Route("api/provincia_consultar")]
        public object provincia_consultar()
        {
            object _respuesta = new object();
            RespuestaHTTP _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "500").FirstOrDefault();
            try
            {
                var _listaProvincias = _objCatalogoProvincia.ConsultarProvincia().Where(c=>c.EstadoProvincia==true).ToList();
                foreach (var item in _listaProvincias)
                {
                    item.IdProvincia = 0;
                }
                _respuesta = _listaProvincias;
                _http = _objCatalogoRespuestasHTTP.consultar().Where(x => x.codigo == "200").FirstOrDefault();
            }
            catch (Exception ex)
            {

                _http.mensaje = _http.mensaje + " " + ex.Message.ToString();
                return new
                {
                    respuesta = _respuesta, http = _http
                };

            }
            return new { respuesta = _respuesta, http = _http };
            }
        }
    }
