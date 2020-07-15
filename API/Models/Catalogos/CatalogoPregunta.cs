﻿using System;
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
        public int InsertarPregunta(Pregunta _objPregunta)
        {
            try
            {
                return int.Parse(db.Sp_PreguntaInsertar(_objPregunta.TipoPregunta.IdTipoPregunta,_objPregunta.Seccion.IdSeccion,_objPregunta.Descripcion,_objPregunta.Orden,_objPregunta.Obligatorio,_objPregunta.Estado).Select(x=>x.Value.ToString()).FirstOrDefault());
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
                db.Sp_PreguntaModificar(_objPregunta.IdPregunta, _objPregunta.TipoPregunta.IdTipoPregunta, _objPregunta.Seccion.IdSeccion, _objPregunta.Descripcion, _objPregunta.Orden, _objPregunta.Obligatorio, _objPregunta.Estado);
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
                    Utilizado=item.UtilizadoPregunta,
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
            foreach (var item in db.Sp_PreguntaConsultar().Where(c=>c.IdPregunta== _idPregunta).ToList())
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

        public List<Pregunta> ConsultarPreguntaPorIdSeccionPorIdentificadorTipoPregunta(int _idSeccion, int _identificadorTipoPregunta)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultar().Where(c => c.IdSeccion == _idSeccion && c.IdentificadorTipoPregunta== _identificadorTipoPregunta).ToList())
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

        public List<Pregunta> ConsultarPreguntaNoEncajonadasPorOpcionPreguntaSeleccion(int _idOpcionPreguntaSeleccion, int _idSeccion, int _idPregunta)
        {
            List<Pregunta> _lista = new List<Pregunta>();
            foreach (var item in db.Sp_PreguntaConsultarNoEncajonadasPorOpcionPreguntaSeleccion(_idOpcionPreguntaSeleccion,_idSeccion, _idPregunta).ToList())
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


    }
}