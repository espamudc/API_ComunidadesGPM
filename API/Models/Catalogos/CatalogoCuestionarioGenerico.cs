using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models.Entidades;
using API.Conexion;
using API.Models.Metodos;

namespace API.Models.Catalogos
{
    public class CatalogoCuestionarioGenerico
    {
        ComunidadesGPMEntities db = new ComunidadesGPMEntities();
        Seguridad _seguridad = new Seguridad();


        public int FinalizarEncuesta(int idAsignarEncuestado)
        {
            try
            {
                int result = Convert.ToInt32(db.Sp_FinalizarEncuesta(idAsignarEncuestado).FirstOrDefault());
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int InsertarCuestionarioGenerico(CuestionarioGenerico _objCuestionarioGenerico)
        {
            try
            {
                return int.Parse(db.Sp_CuestionarioGenericoInsertar(_objCuestionarioGenerico.Nombre, _objCuestionarioGenerico.Descripcion, _objCuestionarioGenerico.Estado).Select(x => x.Value.ToString()).FirstOrDefault());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int ModificarCuestionarioGenerico(CuestionarioGenerico _objCuestionarioGenerico)
        {
            try
            {
                db.Sp_CuestionarioGenericoModificar(_objCuestionarioGenerico.IdCuestionarioGenerico, _objCuestionarioGenerico.Nombre, _objCuestionarioGenerico.Descripcion, _objCuestionarioGenerico.Estado);
                return _objCuestionarioGenerico.IdCuestionarioGenerico;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public void EliminarCuestionarioGenerico(int _idCuestionarioGenerico)
        {
            db.Sp_CuestionarioGenericoEliminar(_idCuestionarioGenerico);
        }
        public List<CuestionarioGenerico> ConsultarCuestionarioGenerico()
        {
            List<CuestionarioGenerico> _lista = new List<CuestionarioGenerico>();
            foreach (var item in db.Sp_CuestionarioGenericoConsultar())
            {
                _lista.Add(new CuestionarioGenerico()
                {
                    IdCuestionarioGenerico=item.IdCuestionarioGenerico,
                    IdCuestionarioGenericoEncriptado=_seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                    Nombre=item.Nombre,
                    Descripcion=item.Descripcion,
                    Estado=item.Estado,
                    Utilizado=item.UtilizadoCuestionarioGenerico
                });
            }
            return _lista;
        }

        public List<CuestionarioGenerico> ConsultarCuestionarioGenericoPorId(int _idCuestionarioGenerico)
        {
            List<CuestionarioGenerico> _lista = new List<CuestionarioGenerico>();
            foreach (var item in db.Sp_CuestionarioGenericoConsultar().Where(c=>c.IdCuestionarioGenerico== _idCuestionarioGenerico).ToList())
            {
                _lista.Add(new CuestionarioGenerico()
                {
                    IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                    IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    Utilizado = item.UtilizadoCuestionarioGenerico
                });
            }
            return _lista;
        }

        public List<CuestionarioGenerico> ConsultarCuestionarioGenericoPorIdConComponenteSeccionPregunta(int _idCuestionarioGenerico)
        {
            List<CuestionarioGenerico> _lista = new List<CuestionarioGenerico>();

            foreach (var item in db.Sp_CuestionarioGenericoConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {


                _lista.Add(new CuestionarioGenerico()
                {
                    IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                    IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    Utilizado = item.UtilizadoCuestionarioGenerico
                    ,
                    listaComponente = new CatalogoComponente().ConsultarComponentePorIdCuestionarioGenericoConSeccionPregunta(item.IdCuestionarioGenerico)

                });
            }
            return _lista;
        }
        public List<CuestionarioGenerico> ConsultarCuestionarioGenericoPorIdConComponenteSeccionPreguntaRandom(int _idCuestionarioGenerico)
        {
            List<CuestionarioGenerico> _lista = new List<CuestionarioGenerico>();
           
            foreach (var item in db.Sp_CuestionarioGenericoConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {

               
                _lista.Add(new CuestionarioGenerico()
                {
                    IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                    IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    Utilizado = item.UtilizadoCuestionarioGenerico
                    ,listaComponente = new CatalogoComponente().ConsultarComponentePorIdCuestionarioGenericoConSeccionPreguntaRandom(item.IdCuestionarioGenerico)
                   
                });
            }
            return _lista;
        }
        public List<CuestionarioGenerico> ConsultarCuestionarioGenericoPorIdConComponenteSeccionPreguntaRandom(int _idCuestionarioGenerico)
        {
            List<CuestionarioGenerico> _lista = new List<CuestionarioGenerico>();

            foreach (var item in db.Sp_CuestionarioGenericoConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {


                _lista.Add(new CuestionarioGenerico()
                {
                    IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                    IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    Utilizado = item.UtilizadoCuestionarioGenerico
                    ,
                    listaComponente = new CatalogoComponente().ConsultarComponentePorIdCuestionarioGenericoConSeccionPreguntaRandom(item.IdCuestionarioGenerico)

                });
            }
            return _lista;
        }
        public List<CuestionarioGenerico> ConsultarCuestionarioGenericoPorVersion(int _idCuestionarioGenerico, int _idVersionCuestionario)
        {
            List<CuestionarioGenerico> _lista = new List<CuestionarioGenerico>();
            foreach (var item in db.Sp_CuestionarioGenericoConsultar().Where(c => c.IdCuestionarioGenerico == _idCuestionarioGenerico).ToList())
            {
                _lista.Add(new CuestionarioGenerico()
                {
                    IdCuestionarioGenerico = item.IdCuestionarioGenerico,
                    IdCuestionarioGenericoEncriptado = _seguridad.Encriptar(item.IdCuestionarioGenerico.ToString()),
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado,
                    Utilizado = item.UtilizadoCuestionarioGenerico
                    ,
                    listaComponente = new CatalogoComponente().ConsultarComponentePorIdCuestionarioGenericoConSeccionPreguntaPorVersion(item.IdCuestionarioGenerico, _idVersionCuestionario)

                });
            }
            return _lista;
        }


    }
}