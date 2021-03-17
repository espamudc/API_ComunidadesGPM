using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Conexion;
using API.Models.Entidades;
using API.Models.Metodos;

namespace API.Models.Catalogos
{
    public class CatalogoComponente
    {
        ComunidadesGPMEntities db = new ComunidadesGPMEntities();
        Seguridad _seguridad = new Seguridad();
        public int InsertarComponente (Componente _objComponente)
        {
            try
            {
                return int.Parse(db.Sp_ComponenteInsertar(_objComponente.CuestionarioGenerico.IdCuestionarioGenerico,_objComponente.Descripcion,_objComponente.Orden,_objComponente.Estado).Select(x=>x.Value.ToString()).FirstOrDefault());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int ModificarComponente(Componente _objComponente)
        {
            try
            {
                db.Sp_ComponenteModificar(_objComponente.IdComponente, _objComponente.CuestionarioGenerico.IdCuestionarioGenerico, _objComponente.Descripcion, _objComponente.Orden, _objComponente.Estado);
                return _objComponente.IdComponente;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int SubirComponente(Componente _objComponente)
        {
            try
            {
                db.Sp_SubirComponente(_objComponente.IdComponente, _objComponente.Estado);
                return _objComponente.IdComponente;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int BajarComponente(Componente _objComponente)
        {
            try
            {
                db.Sp_BajarComponente(_objComponente.IdComponente, _objComponente.Estado);
                return _objComponente.IdComponente;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public void EliminarComponente(int _idComponente)
        {
            db.Sp_ComponenteEliminar(_idComponente);
        }
        public List<Componente> ConsultarComponente()
        {
            List<Componente> _lista = new List<Componente>();
            foreach (var item in db.Sp_ComponenteConsultar())
            {
                _lista.Add(new Componente()
                {
                    IdComponente=item.IdComponente,
                    IdComponenteEncriptado=_seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion=item.DescripcionComponente,
                    Estado=item.EstadoComponente,
                    Orden=item.OrdenComponente,
                    Utilizado=item.UtilizadoComponente,
                    CuestionarioGenerico =new CuestionarioGenerico()
                    {
                        IdCuestionarioGenerico=item.IdCuestionarioGenerico,
                        IdCuestionarioGenericoEncriptado=_seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                        Descripcion=item.DescripcionCuestionarioGenerico,
                        Estado=item.EstadoCuestionarioGenerico,
                        Nombre=item.NombreCuestionarioGenerico
                    }
                });
            }
            return _lista;
        }
        public List<Componente> ConsultarComponentePorId(int _idComponente)
        {
            List<Componente> _lista = new List<Componente>();
            foreach (var item in db.Sp_ComponenteConsultar().Where(c=>c.IdComponente== _idComponente).ToList())
            {
                _lista.Add(new Componente()
                {
                    IdComponente = item.IdComponente,
                    IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion = item.DescripcionComponente,
                    Estado = item.EstadoComponente,
                    Orden = item.OrdenComponente,
                    Utilizado = item.UtilizadoComponente,
                    CuestionarioGenerico = new CuestionarioGenerico()
                    {
                        IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                        IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                        Descripcion = item.DescripcionCuestionarioGenerico,
                        Estado = item.EstadoCuestionarioGenerico,
                        Nombre = item.NombreCuestionarioGenerico
                    }
                });
            }
            return _lista;
        }
        public List<Componente> ConsultarComponentePorIdCuestionarioGenerico(int _idCuestionarioGenerico)
        {
            List<Componente> _lista = new List<Componente>();
            foreach (var item in db.Sp_ComponenteConsultar()
                .Where(c=>c.IdCuestionarioGenerico==_idCuestionarioGenerico)
                .Where(c => c.EstadoComponente == true)
                .ToList())
            {
                _lista.Add(new Componente()
                {
                    IdComponente = item.IdComponente,
                    IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion = item.DescripcionComponente,
                    Estado = item.EstadoComponente,
                    Orden = item.OrdenComponente,
                    Utilizado = item.UtilizadoComponente,
                    CuestionarioGenerico = new CuestionarioGenerico()
                    {
                        IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                        IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                        Descripcion = item.DescripcionCuestionarioGenerico,
                        Estado = item.EstadoCuestionarioGenerico,
                        Nombre = item.NombreCuestionarioGenerico
                    }
                });
            }
            return _lista;
        }
        public List<Componente> ConsultarComponentePorIdCuestionarioGenericoConSeccionPregunta(int _idCuestionarioGenerico)
        {
            List<Componente> _lista = new List<Componente>();
           
            
            foreach (var item in db.Sp_ComponenteConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {
              
               
                _lista.Add(new Componente()
                {
                    IdComponente = item.IdComponente,
                    IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion = item.DescripcionComponente,
                    Estado = item.EstadoComponente,
                    Orden = item.OrdenComponente,
                    Utilizado = item.UtilizadoComponente
                    ,listaSeccion = new CatalogoSeccion().ConsultarSeccionPorIdComponenteConPregunta(item.IdComponente)
                });
            }
            return _lista;
        }

        public List<Componente> ConsultarComponentePorIdCuestionarioGenericoConSeccionPreguntaRandom(int _idCuestionarioGenerico)
        {
            List<Componente> _lista = new List<Componente>();
            int x = 0;

            Random r = new Random();
            List<int> numerosRandom = new List<int>();            
            foreach (var item in db.Sp_ComponenteConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {
                x = r.Next(10000, 99999);
                while (numerosRandom.Contains(x))
                {
                    x = r.Next(10000, 99999);
                }
                numerosRandom.Add(x);
                _lista.Add(new Componente()
                {
                    IdComponente = item.IdComponente,
                    IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion = x + ". " + item.DescripcionComponente,
                    Estado = item.EstadoComponente,
                    Orden = item.OrdenComponente,
                    Utilizado = item.UtilizadoComponente
                    ,
                    listaSeccion = new CatalogoSeccion().ConsultarSeccionPorIdComponenteConPreguntaRandom(item.IdComponente, ref numerosRandom)
                });
            }
            return _lista;
        }

        public List<Componente> ConsultarComponentePorIdCuestionarioGenericoConSeccionPreguntaPorVersion(int _idCuestionarioGenerico, int _idVersionCuestionario)
        {
            List<Componente> _lista = new List<Componente>();
            foreach (var item in db.Sp_ComponenteConsultarPorVersion(_idVersionCuestionario).Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {
                _lista.Add(new Componente()
                {
                    IdComponente = item.IdComponente,
                    IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion = item.DescripcionComponente,
                    Estado = item.EstadoComponente,
                    Orden = item.OrdenComponente,
                    Utilizado = item.UtilizadoComponente
                    ,
                    listaSeccion = new CatalogoSeccion().ConsultarSeccionPorIdComponenteConPreguntaPorVersion(item.IdComponente, _idVersionCuestionario)
                });
            }
            return _lista;
        }

        public List<Componente> ComponentePorIdCuestionario(int _idCuestionarioGenerico)
        {
            List<Componente> _lista = new List<Componente>();
            foreach (var item in db.Sp_ComponentesConsultarPorCuestionario(_idCuestionarioGenerico).ToList())
            {
                _lista.Add(new Componente()
                {
                   // IdComponente = item.IdComponente,
                    IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    Orden = item.Orden,
                    CuestionarioGenerico = new CuestionarioGenerico()
                    {
                        IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                    }
                });
            }
            return _lista;
        }
        public List<Componente> ConsultarComponentePorIdCuestionarioGenericoConSeccionPreguntaRandom(int _idCuestionarioGenerico)
        {
            List<Componente> _lista = new List<Componente>();
            int x = 0;

            Random r = new Random();
            List<int> numerosRandom = new List<int>();
            foreach (var item in db.Sp_ComponenteConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {
                x = r.Next(10000, 99999);
                while (numerosRandom.Contains(x))
                {
                    x = r.Next(10000, 99999);
                }
                numerosRandom.Add(x);
                _lista.Add(new Componente()
                {
                    IdComponente = item.IdComponente,
                    IdComponenteEncriptado = _seguridad.Encriptar(item.IdComponente.ToString()),
                    Descripcion = x + ". " + item.DescripcionComponente,
                    Estado = item.EstadoComponente,
                    Orden = item.OrdenComponente,
                    Utilizado = item.UtilizadoComponente
                    ,
                    listaSeccion = new CatalogoSeccion().ConsultarSeccionPorIdComponenteConPreguntaRandom(item.IdComponente, ref numerosRandom)
                });
            }
            return _lista;
        }
    }
}