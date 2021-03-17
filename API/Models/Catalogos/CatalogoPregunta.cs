using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Conexion;
using API.Models.Entidades;
using API.Models.Metodos;

namespace API.Models.Catalogos
{
    public class CatalogoPregunta
    {
        ComunidadesGPMEntities db = new ComunidadesGPMEntities();
        Seguridad _seguridad = new Seguridad();

        public List<PreguntaRestante> totalPreguntasRestantes(int idAsignarEncuestado)
        {
            List<PreguntaRestante> _lista = new List<PreguntaRestante>();
            foreach (var item in db.Sp_TotalPreguntaRestantes(idAsignarEncuestado))
            {
                _lista.Add(new PreguntaRestante(

                  _seguridad.Encriptar(Convert.ToString(item.IdPregunta)),
                   item.Descripcion,
                   item.Componente,
                   item.Orden,
                   item.Obligatorio
                ));
            }
            return _lista;
        }

        public int InsertarPregunta(Pregunta _objPregunta)
        {
            try
            {
<<<<<<< HEAD
                return int.Parse(db.Sp_PreguntaInsertar(_objPregunta.TipoPregunta.IdTipoPregunta, _objPregunta.Seccion.IdSeccion, _objPregunta.Descripcion, _objPregunta.Orden, _objPregunta.Obligatorio, _objPregunta.Estado, _objPregunta.leyendaSuperior, _objPregunta.leyendaLateral, _objPregunta.Observacion, _objPregunta.campo_observacion, _objPregunta.Reporte).Select(x => x.Value.ToString()).FirstOrDefault());
=======
                return int.Parse(db.Sp_PreguntaInsertar(_objPregunta.TipoPregunta.IdTipoPregunta, _objPregunta.Seccion.IdSeccion, _objPregunta.Descripcion, _objPregunta.Orden, _objPregunta.Obligatorio, _objPregunta.Estado, _objPregunta.leyendaSuperior, _objPregunta.leyendaLateral, _objPregunta.Observacion, _objPregunta.campo_observacion,_objPregunta.Reporte).Select(x => x.Value.ToString()).FirstOrDefault());
>>>>>>> 38defc51b45a0b8504590836b9f49951a053e090
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int ModificarPregunta(Pregunta _objPregunta)
        {
            try
            {
                db.Sp_PreguntaModificar(_objPregunta.IdPregunta, _objPregunta.TipoPregunta.IdTipoPregunta, _objPregunta.Seccion.IdSeccion, _objPregunta.Descripcion, _objPregunta.leyendaSuperior, _objPregunta.leyendaLateral, _objPregunta.Orden, _objPregunta.Obligatorio, _objPregunta.Estado, _objPregunta.Observacion, _objPregunta.campo_observacion);
                return _objPregunta.IdPregunta;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int SubirPregunta(Pregunta _objPregunta)
        {
            try
            {
                db.Sp_SubirPregunta(_objPregunta.IdPregunta, _objPregunta.Estado);
                return _objPregunta.IdPregunta;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int BajarPregunta(Pregunta _objPregunta)
        {
            try
            {
                db.Sp_BajarPregunta(_objPregunta.IdPregunta, _objPregunta.Estado);
                return _objPregunta.IdPregunta;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public void EliminarPregunta(int _idPregunta)
        {
            db.Sp_PreguntaEliminar(_idPregunta);
        }
        public List<Pregunta> ConsultarPregunta()
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultar())
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },
                    Seccion = new Seccion()
                    {
                        IdSeccion = item.IdSeccion,
                        IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.DescripcionSeccion,
                        Estado = item.EstadoSeccion,
                        Orden = item.OrdenSeccion,
                        Componente = new Componente()
                        {
                            IdComponente = item.IdComponente,
                            IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                            Descripcion = item.DescripcionComponente,
                            Estado = item.EstadoComponente,
                            Orden = item.OrdenComponente,
                            CuestionarioGenerico = new CuestionarioGenerico()
                            {
                                IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                                IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                                Descripcion = item.DescripcionCuestionarioGenerico,
                                Estado = item.EstadoCuestionarioGenerico,
                                Nombre = item.NombreCuestionarioGenerico
                            }
                        }
                    }
                });
            }
            return _lista;
        }
        public List<Pregunta> ConsultarPreguntaPorId(int _idPregunta)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultar().Where(c => c.IdPregunta == _idPregunta).ToList())
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    leyendaSuperior = item.leyendaSuperior,
                    leyendaLateral = item.leyendaLateral,
                    Observacion = Convert.ToBoolean(item.ObservacionPregunta),
                    campo_observacion = item.campo_observacion,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },
                    Seccion = new Seccion()
                    {
                        IdSeccion = item.IdSeccion,
                        IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.DescripcionSeccion,
                        Estado = item.EstadoSeccion,
                        Orden = item.OrdenSeccion,
                        Componente = new Componente()
                        {
                            IdComponente = item.IdComponente,
                            IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                            Descripcion = item.DescripcionComponente,
                            Estado = item.EstadoComponente,
                            Orden = item.OrdenComponente,
                            CuestionarioGenerico = new CuestionarioGenerico()
                            {
                                IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                                IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                                Descripcion = item.DescripcionCuestionarioGenerico,
                                Estado = item.EstadoCuestionarioGenerico,
                                Nombre = item.NombreCuestionarioGenerico
                            }
                        }
                    }
                });
            }
            return _lista;
        }
        public List<Pregunta> ConsultarPreguntaPorIdSeccion(int _idSeccion)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultar().Where(c => c.IdSeccion == _idSeccion).ToList())
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    leyendaSuperior = item.leyendaSuperior,
                    leyendaLateral = item.leyendaLateral,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Observacion = Convert.ToBoolean(item.ObservacionPregunta),
                    campo_observacion = item.campo_observacion,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },
                    Seccion = new Seccion()
                    {
                        IdSeccion = item.IdSeccion,
                        IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.DescripcionSeccion,
                        Estado = item.EstadoSeccion,
                        Orden = item.OrdenSeccion,
                        Componente = new Componente()
                        {
                            IdComponente = item.IdComponente,
                            IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                            Descripcion = item.DescripcionComponente,
                            Estado = item.EstadoComponente,
                            Orden = item.OrdenComponente,
                            CuestionarioGenerico = new CuestionarioGenerico()
                            {
                                IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                                IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                                Descripcion = item.DescripcionCuestionarioGenerico,
                                Estado = item.EstadoCuestionarioGenerico,
                                Nombre = item.NombreCuestionarioGenerico
                            }
                        }
                    }
                });
            }
            return _lista;
        }

        public List<Pregunta> ConsultarPreguntaPorIdSeccionFiltrado(int _idSeccion, int _idTipoPregunta)
        {
            List<Pregunta> _lista = new List<Pregunta>();

            foreach (var item in db.Sp_PreguntaConsultarFiltrado(_idTipoPregunta).Where(c => c.IdSeccion == _idSeccion).ToList())
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    leyendaSuperior = item.leyendaSuperior,
                    leyendaLateral = item.leyendaLateral,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Observacion = Convert.ToBoolean(item.ObservacionPregunta),
                    campo_observacion = item.campo_observacion,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },
                    Seccion = new Seccion()
                    {
                        IdSeccion = item.IdSeccion,
                        IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.DescripcionSeccion,
                        Estado = item.EstadoSeccion,
                        Orden = item.OrdenSeccion,
                        Componente = new Componente()
                        {
                            IdComponente = item.IdComponente,
                            IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                            Descripcion = item.DescripcionComponente,
                            Estado = item.EstadoComponente,
                            Orden = item.OrdenComponente,
                            CuestionarioGenerico = new CuestionarioGenerico()
                            {
                                IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                                IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                                Descripcion = item.DescripcionCuestionarioGenerico,
                                Estado = item.EstadoCuestionarioGenerico,
                                Nombre = item.NombreCuestionarioGenerico
                            }
                        }
                    }
                });
            }
            return _lista;
        }

        public List<Pregunta> ConsultarPreguntaPorIdSeccionPorIdentificadorTipoPregunta(int _idSeccion, int _identificadorTipoPregunta)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultar().Where(c => c.IdSeccion == _idSeccion && c.IdentificadorTipoPregunta == _identificadorTipoPregunta).ToList())
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },
                    Seccion = new Seccion()
                    {
                        IdSeccion = item.IdSeccion,
                        IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.DescripcionSeccion,
                        Estado = item.EstadoSeccion,
                        Orden = item.OrdenSeccion,
                        Componente = new Componente()
                        {
                            IdComponente = item.IdComponente,
                            IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                            Descripcion = item.DescripcionComponente,
                            Estado = item.EstadoComponente,
                            Orden = item.OrdenComponente,
                            CuestionarioGenerico = new CuestionarioGenerico()
                            {
                                IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                                IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                                Descripcion = item.DescripcionCuestionarioGenerico,
                                Estado = item.EstadoCuestionarioGenerico,
                                Nombre = item.NombreCuestionarioGenerico
                            }
                        }
                    }
                });
            }
            return _lista;
        }

        public List<Pregunta> ConsultarPreguntaPorIdCuestionarioGenerico(int _idCuestionarioGenerico)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Reporte = item.ReportePregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    opcionSeleccion = item.opcionSeleccion,
                    opcionMatriz = item.opcionMatriz,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },
                    Seccion = new Seccion()
                    {
                        IdSeccion = item.IdSeccion,
                        IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.DescripcionSeccion,
                        Estado = item.EstadoSeccion,
                        Orden = item.OrdenSeccion,
                        Componente = new Componente()
                        {
                            IdComponente = item.IdComponente,
                            IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                            Descripcion = item.DescripcionComponente,
                            Estado = item.EstadoComponente,
                            Orden = item.OrdenComponente,
                            CuestionarioGenerico = new CuestionarioGenerico()
                            {
                                IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                                IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                                Descripcion = item.DescripcionCuestionarioGenerico,
                                Estado = item.EstadoCuestionarioGenerico,
                                Nombre = item.NombreCuestionarioGenerico
                            }
                        }
                    }
                });
            }
            return _lista;
        }

        public List<Pregunta> ConsultarPreguntaNoEncajonadasPorOpcionPreguntaSeleccion(int _idOpcionPreguntaSeleccion, int _idSeccion, int _idPregunta)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultarNoEncajonadasPorOpcionPreguntaSeleccion(_idOpcionPreguntaSeleccion, _idSeccion, _idPregunta).ToList())
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },
                    Seccion = new Seccion()
                    {
                        IdSeccion = item.IdSeccion,
                        IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.DescripcionSeccion,
                        Estado = item.EstadoSeccion,
                        Orden = item.OrdenSeccion,
                        Componente = new Componente()
                        {
                            IdComponente = item.IdComponente,
                            IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                            Descripcion = item.DescripcionComponente,
                            Estado = item.EstadoComponente,
                            Orden = item.OrdenComponente,
                            CuestionarioGenerico = new CuestionarioGenerico()
                            {
                                IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                                IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                                Descripcion = item.DescripcionCuestionarioGenerico,
                                Estado = item.EstadoCuestionarioGenerico,
                                Nombre = item.NombreCuestionarioGenerico
                            }
                        }
                    }
                });
            }
            return _lista;
        }

        public List<Pregunta> ConsultarPreguntaPorIdSeccionConTipoPregunta(int _idSeccion)
        {

            List<Pregunta> _lista = new List<Pregunta>();
         
            foreach (var item in db.Sp_PreguntaConsultarNoEncajonadasPorSeccion(_idSeccion))
            {
                
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },

                });
            }
            return _lista;
        }

        public List<Pregunta> ConsultarPreguntaPorIdSeccionConTipoPreguntaPorVersion(int _idSeccion, int _idVersionCuestionario)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultarNoEncajonadasPorSeccionPorVersion(_idSeccion, _idVersionCuestionario))
            {
                _lista.Add(new Pregunta()
                {
                    IdPregunta = item.IdPregunta,
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.DescripcionPregunta,
                    Estado = item.EstadoPregunta,
                    Obligatorio = item.ObligatorioPregunta,
                    Orden = item.OrdenPregunta,
                    Utilizado = item.UtilizadoPregunta,
                    Encajonamiento = item.EncajonamientoPregunta,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPregunta = item.IdTipoPregunta,
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.DescripcionTipoPregunta,
                        Estado = item.EstadoTipoPregunta,
                        Identificador = item.IdentificadorTipoPregunta
                    },

                });
            }
            return _lista;
        }

        public List<Pregunta> preguntasPorCompenente(int idcomponente, int idusuariotecnico)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_ListadoPreguntasPorComponente(idcomponente, idusuariotecnico))
            {
                _lista.Add(new Pregunta()
                {
                    IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                    Descripcion = item.Descripcion,
                    TipoPregunta = new TipoPregunta()
                    {
                        IdTipoPreguntaEncriptado = _seguridad.Encriptar(item.IdTipoPregunta.ToString()),
                        Descripcion = item.TipoPregunta,
                        Estado = item.EstadoPregunta,
                        Identificador = item.Identificador
                    },
                    CabeceraVersionCuestionario = new CabeceraVersionCuestionario()
                    {
                        IdCabeceraVersionCuestionario = item.IdCabeceraVersionCuestionario
                    },
                    PreguntaAbierta = new PreguntaAbierta()
                    {
                        IdPreguntaAbiertaEncriptado = _seguridad.Encriptar(item.IdPreguntaAbierta.ToString()),
                        TipoDato = new TipoDato()
                        {
                            TipoHTML = _seguridad.Encriptar(item.TipoHTML.ToString())
                        }
                    },
                    Seccion = new Seccion()
                    {
                        SeccionId = _seguridad.Encriptar(item.IdSeccion.ToString()),
                        Descripcion = item.Seccion
                    },
                    IdPreguntaEncajonada = item.PreguntaEncajonada,
                    IdComponente = _seguridad.Encriptar(item.IdComponente.ToString()),
                    IdOpcionPreguntaSeleccion = _seguridad.Encriptar(item.IdOpcionPreguntaSeleccion.ToString())

                });
            }
            return _lista;
        }
    }
}