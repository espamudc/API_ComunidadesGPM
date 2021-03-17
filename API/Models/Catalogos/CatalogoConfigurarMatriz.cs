using API.Conexion;
using API.Models.Metodos;
using API.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Catalogos
{
    public class CatalogoConfigurarMatriz
    {
        ComunidadesGPMEntities db = new ComunidadesGPMEntities();
        Seguridad _seguridad = new Seguridad();
        public int InsertarConfigurarMatriz(ConfigurarMatriz _objConfigurarMatriz)
        {
            try
            {
                return int.Parse(db.Sp_ConfigurarMatrizInsertar(_objConfigurarMatriz.OpcionUnoMatriz.IdOpcionUnoMatriz, _objConfigurarMatriz.OpcionDosMatriz.IdOpcionDosMatriz, _objConfigurarMatriz.Estado).Select(x => x.Value.ToString()).FirstOrDefault());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void EliminarConfigurarMatriz(int _idConfigurarMatriz)
        {
            db.Sp_ConfigurarMatrizEliminar(_idConfigurarMatriz);
        }
        public List<ConfigurarMatriz> ConsultarConfigurarMatrizPorIdOpcionDosMatriz(int _idOpcionDosMatriz)
        {
            List<ConfigurarMatriz> _lista = new List<ConfigurarMatriz>();
            foreach (var item in db.Sp_ConfigurarMatrizConsultar().Where(c => c.IdOpcionDosMatriz == _idOpcionDosMatriz).ToList())
            {
                _lista.Add(new ConfigurarMatriz()
                {
                    IdConfigurarMatriz = item.IdConfigurarMatriz,
                    IdConfigurarMatrizEncriptado = _seguridad.Encriptar(item.IdConfigurarMatriz.ToString()),
                    Estado = item.EstadoConfigurarMatriz,
                    OpcionDosMatriz = new OpcionDosMatriz()
                    {
                        IdOpcionDosMatriz = item.IdOpcionDosMatriz,
                        IdOpcionDosMatrizEncriptado = _seguridad.Encriptar(item.IdOpcionDosMatriz.ToString()),
                        Descripcion = item.DescripcionOpcionDosMatriz,
                        Estado = item.EstadoOpcionOpcionDosMatriz
                    },
                    OpcionUnoMatriz =
                    new OpcionUnoMatriz()
                    {
                        IdOpcionUnoMatriz = item.IdOpcionUnoMatriz,
                        IdOpcionUnoMatrizEncriptado = _seguridad.Encriptar(item.IdOpcionUnoMatriz.ToString()),
                        Descripcion = item.DescripcionOpcionUnoMatriz,
                        Estado = item.EstadoOpcionOpcionUnoMatriz,
                        Utilizado = item.UtilizadoOpcionUnoMatriz,
                        Encajonamiento = item.EncajonamientoOpcionUnoMatriz,
                        Pregunta = new Pregunta()
                        {
                            IdPregunta = item.IdPregunta,
                            IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                            Descripcion = item.DescripcionPregunta,
                            Estado = item.EstadoPregunta,
                            Obligatorio = item.ObligatorioPregunta,
                            Orden = item.OrdenPregunta,
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
                        }
                    }
                });
            }
            return _lista;
        }
        public List<ConfigurarMatriz> ConsultarConfigurarMatrizPorIdPregunta(int _idPregunta)
        {
            List<ConfigurarMatriz> _lista = new List<ConfigurarMatriz>();
            foreach (var item in db.Sp_ConfigurarMatrizConsultar2(_idPregunta, 0).ToList())
            {
                _lista.Add(new ConfigurarMatriz()
                {
                    IdConfigurarMatriz = item.IdConfigurarMatriz,
                    IdConfigurarMatrizEncriptado = _seguridad.Encriptar(item.IdConfigurarMatriz.ToString()),
                    Estado = item.EstadoConfigurarMatriz,
                    OpcionDosMatriz = new OpcionDosMatriz()
                    {
                        IdOpcionDosMatriz = item.IdOpcionDosMatriz,
                        IdOpcionDosMatrizEncriptado = _seguridad.Encriptar(item.IdOpcionDosMatriz.ToString()),
                        Descripcion = item.DescripcionOpcionDosMatriz,
                        Estado = item.EstadoOpcionOpcionDosMatriz
                    },
                    OpcionUnoMatriz =
                    new OpcionUnoMatriz()
                    {
                        IdOpcionUnoMatriz = item.IdOpcionUnoMatriz,
                        IdOpcionUnoMatrizEncriptado = _seguridad.Encriptar(item.IdOpcionUnoMatriz.ToString()),
                        Descripcion = item.DescripcionOpcionUnoMatriz,
                        Estado = item.EstadoOpcionOpcionUnoMatriz,
                        Utilizado = item.UtilizadoOpcionUnoMatriz,
                        Encajonamiento = item.EncajonamientoOpcionUnoMatriz,
                        Pregunta = new Pregunta()
                        {
                            IdPregunta = item.IdPregunta,
                            IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                            Descripcion = item.DescripcionPregunta,
                            Estado = item.EstadoPregunta,
                            Obligatorio = item.ObligatorioPregunta,
                            Orden = item.OrdenPregunta,
<<<<<<< HEAD
                            leyendaLateral= item.leyendaLateral,
                            leyendaSuperior=item.leyendaSuperior,
                            Observacion=Convert.ToBoolean(item.observacion),
                           
=======
                            leyendaLateral = item.leyendaLateral,
                            leyendaSuperior = item.leyendaSuperior,
                            Observacion = Convert.ToBoolean(item.observacion),

>>>>>>> 38defc51b45a0b8504590836b9f49951a053e090

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
                        }
                    }
                });
            }
            return _lista;
        }
        public List<ConfigurarMatriz> ConsultarConfigurarMatrizPorIdPregunta2(int _idPregunta, int _idAsignarEncuestado)
        {
            List<ConfigurarMatriz> _lista = new List<ConfigurarMatriz>();
            foreach (var item in db.Sp_ConfigurarMatrizConsultar2(_idPregunta, _idAsignarEncuestado).ToList())
            {
                _lista.Add(new ConfigurarMatriz()
                {
                    IdConfigurarMatriz = item.IdConfigurarMatriz,
                    IdConfigurarMatrizEncriptado = _seguridad.Encriptar(item.IdConfigurarMatriz.ToString()),
                    Estado = item.EstadoConfigurarMatriz,
                    IdRespuestaLogica = _seguridad.Encriptar(item.IdRespuestaLogica.ToString()),
                    DescripcionRespuestaAbierta = item.DescripcionRespuestaAbierta,
                    IdAsignarEncuestado = _seguridad.Encriptar(item.IdAsignarEncuestado.ToString()),
                    datoRespuestaMatriz = item.datos,
                    OpcionDosMatriz = new OpcionDosMatriz()
                    {
                        IdOpcionDosMatriz = item.IdOpcionDosMatriz,
                        IdOpcionDosMatrizEncriptado = _seguridad.Encriptar(item.IdOpcionDosMatriz.ToString()),
                        Descripcion = item.DescripcionOpcionDosMatriz,
                        Estado = item.EstadoOpcionOpcionDosMatriz
                    },
                    OpcionUnoMatriz =
                    new OpcionUnoMatriz()
                    {
                        IdOpcionUnoMatriz = item.IdOpcionUnoMatriz,
                        IdOpcionUnoMatrizEncriptado = _seguridad.Encriptar(item.IdOpcionUnoMatriz.ToString()),
                        Descripcion = item.DescripcionOpcionUnoMatriz,
                        Estado = item.EstadoOpcionOpcionUnoMatriz,
                        Utilizado = item.UtilizadoOpcionUnoMatriz,
                        Encajonamiento = item.EncajonamientoOpcionUnoMatriz,
                        Pregunta = new Pregunta()
                        {
                            IdPregunta = item.IdPregunta,
                            IdPreguntaEncriptado = _seguridad.Encriptar(item.IdPregunta.ToString()),
                            Descripcion = item.DescripcionPregunta,
                            Estado = item.EstadoPregunta,
                            Obligatorio = item.ObligatorioPregunta,
                            Orden = item.OrdenPregunta,
                            leyendaLateral = item.leyendaLateral,
                            leyendaSuperior = item.leyendaSuperior,
                            Observacion = Convert.ToBoolean(item.observacion),

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
                        }
                    }
                });
            }
            return _lista;
        }
    }
}