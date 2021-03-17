using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Conexion;
using API.Models.Entidades;
using API.Models.Metodos;
namespace API.Models.Catalogos
{
    public class CatalogoSeccion
    {
        ComunidadesGPMEntities db = new ComunidadesGPMEntities();
        Seguridad _seguridad = new Seguridad();
        public int InsertarSeccion(Seccion _objSeccion)
        {
            try
            {
                return int.Parse(db.Sp_SeccionInsertar(_objSeccion.Componente.IdComponente, _objSeccion.Descripcion, _objSeccion.Orden, _objSeccion.Estado).Select(x => x.Value.ToString()).FirstOrDefault());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int ModificarSeccion(Seccion _objSeccion)
        {
            try
            {
                db.Sp_SeccionModificar(_objSeccion.IdSeccion, _objSeccion.Componente.IdComponente, _objSeccion.Descripcion, _objSeccion.Orden, _objSeccion.Estado);
                return _objSeccion.IdSeccion;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int SubirSeccion(Seccion _objSeccion)
        {
            try
            {
                db.Sp_SubirSeccion(_objSeccion.IdSeccion, _objSeccion.Estado);
                return _objSeccion.IdSeccion;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int BajarSeccion(Seccion _objSeccion)
        {
            try
            {
                db.Sp_BajarSeccion(_objSeccion.IdSeccion, _objSeccion.Estado);
                return _objSeccion.IdSeccion;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void EliminarSeccion(int _idSeccion)
        {
            db.Sp_SeccionEliminar(_idSeccion);
        }
        public List<Seccion> ConsultarSeccion()
        {
            List<Seccion> _lista = new List<Seccion>();
            foreach (var item in db.Sp_SeccionConsultar())
            {
                _lista.Add(new Seccion()
                {
                    IdSeccion = item.IdSeccion,
                    IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                    Descripcion = item.DescripcionSeccion,
                    Estado = item.EstadoSeccion,
                    Orden = item.OrdenSeccion,
                    Utilizado = item.UtilizadoSeccion,
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
                });
            }
            return _lista;
        }
        public List<Seccion> ConsultarSeccionPorId(int _idSeccion)
        {
            List<Seccion> _lista = new List<Seccion>();
            foreach (var item in db.Sp_SeccionConsultar().Where(c => c.IdSeccion == _idSeccion).ToList())
            {
                _lista.Add(new Seccion()
                {
                    IdSeccion = item.IdSeccion,
                    IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                    Descripcion = item.DescripcionSeccion,
                    Estado = item.EstadoSeccion,
                    Orden = item.OrdenSeccion,
                    Utilizado = item.UtilizadoSeccion,
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
                });
            }
            return _lista;
        }
        public List<Seccion> ConsultarSeccionPorIdComponente(int _idComponente)
        {
            List<Seccion> _lista = new List<Seccion>();
            foreach (var item in db.Sp_SeccionConsultar().Where(c => c.IdComponente == _idComponente).ToList())
            {
                _lista.Add(new Seccion()
                {
                    IdSeccion = item.IdSeccion,
                    IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                    Descripcion = item.DescripcionSeccion,
                    Estado = item.EstadoSeccion,
                    Orden = item.OrdenSeccion,
                    Utilizado = item.UtilizadoSeccion,
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
                });
            }
            return _lista;
        }

        public List<Seccion> ConsultarSeccionPorIdComponenteConPregunta(int _idComponente)
        {
           
            List<Seccion> _lista = new List<Seccion>();
           
            foreach (var item in db.Sp_SeccionConsultar().Where(c => c.IdComponente == _idComponente).ToList())
            {
               
                _lista.Add(new Seccion()
                {
                    IdSeccion = item.IdSeccion,
                    IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                    Descripcion = item.DescripcionSeccion,
                    Estado = item.EstadoSeccion,
                    Orden = item.OrdenSeccion,
                    Utilizado = item.UtilizadoSeccion,
                    listaPregunta = new CatalogoPregunta().ConsultarPreguntaPorIdSeccionConTipoPregunta(item.IdSeccion)
                });
            }
            return _lista;
        }

        public List<Seccion> ConsultarSeccionPorIdComponenteConPreguntaRandom(int _idComponente, ref List<int> numerosRandom)
        {

            List<Seccion> _lista = new List<Seccion>();
            Random _r = new Random();
            int _x = 0;
            foreach (var item in db.Sp_SeccionConsultar().Where(c => c.IdComponente == _idComponente).ToList())
            {
                _x = _r.Next(10000, 99999);
                while (numerosRandom.Contains(_x))
                {
                    _x = _r.Next(10000, 99999);
                }
                numerosRandom.Add(_x);
                _lista.Add(new Seccion()
                {
                    IdSeccion = item.IdSeccion,
                    IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                    Descripcion = _x + ". " + item.DescripcionSeccion,
                    Estado = item.EstadoSeccion,
                    Orden = item.OrdenSeccion,
                    Utilizado = item.UtilizadoSeccion,
                    listaPregunta = new CatalogoPregunta().ConsultarPreguntaPorIdSeccionConTipoPregunta(item.IdSeccion)
                });
            }
            return _lista;
        }

        public List<Seccion> ConsultarSeccionPorIdComponenteConPreguntaPorVersion(int _idComponente, int _idVersionCuestionario)
        {
            List<Seccion> _lista = new List<Seccion>();
            foreach (var item in db.Sp_SeccionConsultarPorVersion(_idVersionCuestionario).Where(c => c.IdComponente == _idComponente).ToList())
            {
                _lista.Add(new Seccion()
                {
                    IdSeccion = item.IdSeccion,
                    IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                    Descripcion = item.DescripcionSeccion,
                    Estado = item.EstadoSeccion,
                    Orden = item.OrdenSeccion,
                    Utilizado = item.UtilizadoSeccion,
                    listaPregunta = new CatalogoPregunta().ConsultarPreguntaPorIdSeccionConTipoPreguntaPorVersion(item.IdSeccion, _idVersionCuestionario)
                });
            }
            return _lista;
        }
        public List<Seccion> ConsultarSeccionPorIdComponenteConPreguntaRandom(int _idComponente, ref List<int> numerosRandom)
        {

            List<Seccion> _lista = new List<Seccion>();
            Random _r = new Random();
            int _x = 0;
            foreach (var item in db.Sp_SeccionConsultar().Where(c => c.IdComponente == _idComponente).ToList())
            {
                _x = _r.Next(10000, 99999);
                while (numerosRandom.Contains(_x))
                {
                    _x = _r.Next(10000, 99999);
                }
                numerosRandom.Add(_x);
                _lista.Add(new Seccion()
                {
                    IdSeccion = item.IdSeccion,
                    IdSeccionEncriptado = _seguridad.Encriptar(item.IdSeccion.ToString()),
                    Descripcion = _x + ". " + item.DescripcionSeccion,
                    Estado = item.EstadoSeccion,
                    Orden = item.OrdenSeccion,
                    Utilizado = item.UtilizadoSeccion,
                    listaPregunta = new CatalogoPregunta().ConsultarPreguntaPorIdSeccionConTipoPregunta(item.IdSeccion)
                });
            }
            return _lista;
        }

    }
}