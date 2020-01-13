﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Conexion
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ComunidadesGPMEntities : DbContext
    {
        public ComunidadesGPMEntities()
            : base("name=ComunidadesGPMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int Sp_AsignarModuloPrivilegioCambiarEstado(Nullable<int> idAsinarModuloPrivilegio, Nullable<bool> nuevoEstado)
        {
            var idAsinarModuloPrivilegioParameter = idAsinarModuloPrivilegio.HasValue ?
                new ObjectParameter("IdAsinarModuloPrivilegio", idAsinarModuloPrivilegio) :
                new ObjectParameter("IdAsinarModuloPrivilegio", typeof(int));
    
            var nuevoEstadoParameter = nuevoEstado.HasValue ?
                new ObjectParameter("NuevoEstado", nuevoEstado) :
                new ObjectParameter("NuevoEstado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_AsignarModuloPrivilegioCambiarEstado", idAsinarModuloPrivilegioParameter, nuevoEstadoParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> Sp_AsignarModuloPrivilegioInsertar(Nullable<int> idModulo, Nullable<int> idPrivilegio, Nullable<bool> estado)
        {
            var idModuloParameter = idModulo.HasValue ?
                new ObjectParameter("IdModulo", idModulo) :
                new ObjectParameter("IdModulo", typeof(int));
    
            var idPrivilegioParameter = idPrivilegio.HasValue ?
                new ObjectParameter("IdPrivilegio", idPrivilegio) :
                new ObjectParameter("IdPrivilegio", typeof(int));
    
            var estadoParameter = estado.HasValue ?
                new ObjectParameter("Estado", estado) :
                new ObjectParameter("Estado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("Sp_AsignarModuloPrivilegioInsertar", idModuloParameter, idPrivilegioParameter, estadoParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> Sp_AsignarTipoUsuarioModuloPrivilegioInsertar(Nullable<int> idTipoUsuario, Nullable<int> idAsignarModuloPrivilegio, Nullable<bool> estado)
        {
            var idTipoUsuarioParameter = idTipoUsuario.HasValue ?
                new ObjectParameter("IdTipoUsuario", idTipoUsuario) :
                new ObjectParameter("IdTipoUsuario", typeof(int));
    
            var idAsignarModuloPrivilegioParameter = idAsignarModuloPrivilegio.HasValue ?
                new ObjectParameter("IdAsignarModuloPrivilegio", idAsignarModuloPrivilegio) :
                new ObjectParameter("IdAsignarModuloPrivilegio", typeof(int));
    
            var estadoParameter = estado.HasValue ?
                new ObjectParameter("Estado", estado) :
                new ObjectParameter("Estado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("Sp_AsignarTipoUsuarioModuloPrivilegioInsertar", idTipoUsuarioParameter, idAsignarModuloPrivilegioParameter, estadoParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> Sp_AsignarUsuarioTipoUsuario_insertar(Nullable<int> idUsuario, Nullable<int> idTipoUsuario, Nullable<bool> estado)
        {
            var idUsuarioParameter = idUsuario.HasValue ?
                new ObjectParameter("IdUsuario", idUsuario) :
                new ObjectParameter("IdUsuario", typeof(int));
    
            var idTipoUsuarioParameter = idTipoUsuario.HasValue ?
                new ObjectParameter("IdTipoUsuario", idTipoUsuario) :
                new ObjectParameter("IdTipoUsuario", typeof(int));
    
            var estadoParameter = estado.HasValue ?
                new ObjectParameter("Estado", estado) :
                new ObjectParameter("Estado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("Sp_AsignarUsuarioTipoUsuario_insertar", idUsuarioParameter, idTipoUsuarioParameter, estadoParameter);
        }
    
        public virtual int Sp_AsignarUsuarioTipoUsuarioCambiarEstado(Nullable<int> idAsignarUsuarioTipoUsuario, Nullable<bool> nuevoEstado)
        {
            var idAsignarUsuarioTipoUsuarioParameter = idAsignarUsuarioTipoUsuario.HasValue ?
                new ObjectParameter("IdAsignarUsuarioTipoUsuario", idAsignarUsuarioTipoUsuario) :
                new ObjectParameter("IdAsignarUsuarioTipoUsuario", typeof(int));
    
            var nuevoEstadoParameter = nuevoEstado.HasValue ?
                new ObjectParameter("NuevoEstado", nuevoEstado) :
                new ObjectParameter("NuevoEstado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_AsignarUsuarioTipoUsuarioCambiarEstado", idAsignarUsuarioTipoUsuarioParameter, nuevoEstadoParameter);
        }
    
        public virtual ObjectResult<Sp_ModuloConsultar_Result> Sp_ModuloConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_ModuloConsultar_Result>("Sp_ModuloConsultar");
        }
    
        public virtual int Sp_PersonaCambiarEstado(Nullable<int> idPersona, Nullable<bool> nuevoEstado)
        {
            var idPersonaParameter = idPersona.HasValue ?
                new ObjectParameter("IdPersona", idPersona) :
                new ObjectParameter("IdPersona", typeof(int));
    
            var nuevoEstadoParameter = nuevoEstado.HasValue ?
                new ObjectParameter("NuevoEstado", nuevoEstado) :
                new ObjectParameter("NuevoEstado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_PersonaCambiarEstado", idPersonaParameter, nuevoEstadoParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> Sp_PersonaInsertar(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string numeroIdentificacion, Nullable<int> idTipoIdentificacion, string telefono, Nullable<int> idSexo, Nullable<int> idParroquia, string direccion, Nullable<bool> estado)
        {
            var primerNombreParameter = primerNombre != null ?
                new ObjectParameter("PrimerNombre", primerNombre) :
                new ObjectParameter("PrimerNombre", typeof(string));
    
            var segundoNombreParameter = segundoNombre != null ?
                new ObjectParameter("SegundoNombre", segundoNombre) :
                new ObjectParameter("SegundoNombre", typeof(string));
    
            var primerApellidoParameter = primerApellido != null ?
                new ObjectParameter("PrimerApellido", primerApellido) :
                new ObjectParameter("PrimerApellido", typeof(string));
    
            var segundoApellidoParameter = segundoApellido != null ?
                new ObjectParameter("SegundoApellido", segundoApellido) :
                new ObjectParameter("SegundoApellido", typeof(string));
    
            var numeroIdentificacionParameter = numeroIdentificacion != null ?
                new ObjectParameter("NumeroIdentificacion", numeroIdentificacion) :
                new ObjectParameter("NumeroIdentificacion", typeof(string));
    
            var idTipoIdentificacionParameter = idTipoIdentificacion.HasValue ?
                new ObjectParameter("IdTipoIdentificacion", idTipoIdentificacion) :
                new ObjectParameter("IdTipoIdentificacion", typeof(int));
    
            var telefonoParameter = telefono != null ?
                new ObjectParameter("Telefono", telefono) :
                new ObjectParameter("Telefono", typeof(string));
    
            var idSexoParameter = idSexo.HasValue ?
                new ObjectParameter("IdSexo", idSexo) :
                new ObjectParameter("IdSexo", typeof(int));
    
            var idParroquiaParameter = idParroquia.HasValue ?
                new ObjectParameter("IdParroquia", idParroquia) :
                new ObjectParameter("IdParroquia", typeof(int));
    
            var direccionParameter = direccion != null ?
                new ObjectParameter("Direccion", direccion) :
                new ObjectParameter("Direccion", typeof(string));
    
            var estadoParameter = estado.HasValue ?
                new ObjectParameter("Estado", estado) :
                new ObjectParameter("Estado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("Sp_PersonaInsertar", primerNombreParameter, segundoNombreParameter, primerApellidoParameter, segundoApellidoParameter, numeroIdentificacionParameter, idTipoIdentificacionParameter, telefonoParameter, idSexoParameter, idParroquiaParameter, direccionParameter, estadoParameter);
        }
    
        public virtual int Sp_PersonaModificar(Nullable<int> idPersona, string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string numeroIdentificacion, Nullable<int> idTipoIdentificacion, string telefono, Nullable<int> idSexo, Nullable<int> idParroquia, string direccion)
        {
            var idPersonaParameter = idPersona.HasValue ?
                new ObjectParameter("IdPersona", idPersona) :
                new ObjectParameter("IdPersona", typeof(int));
    
            var primerNombreParameter = primerNombre != null ?
                new ObjectParameter("PrimerNombre", primerNombre) :
                new ObjectParameter("PrimerNombre", typeof(string));
    
            var segundoNombreParameter = segundoNombre != null ?
                new ObjectParameter("SegundoNombre", segundoNombre) :
                new ObjectParameter("SegundoNombre", typeof(string));
    
            var primerApellidoParameter = primerApellido != null ?
                new ObjectParameter("PrimerApellido", primerApellido) :
                new ObjectParameter("PrimerApellido", typeof(string));
    
            var segundoApellidoParameter = segundoApellido != null ?
                new ObjectParameter("SegundoApellido", segundoApellido) :
                new ObjectParameter("SegundoApellido", typeof(string));
    
            var numeroIdentificacionParameter = numeroIdentificacion != null ?
                new ObjectParameter("NumeroIdentificacion", numeroIdentificacion) :
                new ObjectParameter("NumeroIdentificacion", typeof(string));
    
            var idTipoIdentificacionParameter = idTipoIdentificacion.HasValue ?
                new ObjectParameter("IdTipoIdentificacion", idTipoIdentificacion) :
                new ObjectParameter("IdTipoIdentificacion", typeof(int));
    
            var telefonoParameter = telefono != null ?
                new ObjectParameter("Telefono", telefono) :
                new ObjectParameter("Telefono", typeof(string));
    
            var idSexoParameter = idSexo.HasValue ?
                new ObjectParameter("IdSexo", idSexo) :
                new ObjectParameter("IdSexo", typeof(int));
    
            var idParroquiaParameter = idParroquia.HasValue ?
                new ObjectParameter("IdParroquia", idParroquia) :
                new ObjectParameter("IdParroquia", typeof(int));
    
            var direccionParameter = direccion != null ?
                new ObjectParameter("Direccion", direccion) :
                new ObjectParameter("Direccion", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_PersonaModificar", idPersonaParameter, primerNombreParameter, segundoNombreParameter, primerApellidoParameter, segundoApellidoParameter, numeroIdentificacionParameter, idTipoIdentificacionParameter, telefonoParameter, idSexoParameter, idParroquiaParameter, direccionParameter);
        }
    
        public virtual ObjectResult<Sp_PrivilegioConsultar_Result> Sp_PrivilegioConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_PrivilegioConsultar_Result>("Sp_PrivilegioConsultar");
        }
    
        public virtual ObjectResult<Sp_SexoConsultar_Result> Sp_SexoConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_SexoConsultar_Result>("Sp_SexoConsultar");
        }
    
        public virtual ObjectResult<Sp_TipoIdentificacionConsultar_Result> Sp_TipoIdentificacionConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_TipoIdentificacionConsultar_Result>("Sp_TipoIdentificacionConsultar");
        }
    
        public virtual int Sp_TipoUsuarioCambiarEstado(Nullable<int> idTipoUsuario, Nullable<bool> nuevoEstado)
        {
            var idTipoUsuarioParameter = idTipoUsuario.HasValue ?
                new ObjectParameter("IdTipoUsuario", idTipoUsuario) :
                new ObjectParameter("IdTipoUsuario", typeof(int));
    
            var nuevoEstadoParameter = nuevoEstado.HasValue ?
                new ObjectParameter("NuevoEstado", nuevoEstado) :
                new ObjectParameter("NuevoEstado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_TipoUsuarioCambiarEstado", idTipoUsuarioParameter, nuevoEstadoParameter);
        }
    
        public virtual ObjectResult<Sp_TipoUsuarioConsultar_Result> Sp_TipoUsuarioConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_TipoUsuarioConsultar_Result>("Sp_TipoUsuarioConsultar");
        }
    
        public virtual ObjectResult<Nullable<decimal>> Sp_TipoUsuarioInsertar(Nullable<int> identificador, string descripcion, Nullable<bool> estado)
        {
            var identificadorParameter = identificador.HasValue ?
                new ObjectParameter("Identificador", identificador) :
                new ObjectParameter("Identificador", typeof(int));
    
            var descripcionParameter = descripcion != null ?
                new ObjectParameter("Descripcion", descripcion) :
                new ObjectParameter("Descripcion", typeof(string));
    
            var estadoParameter = estado.HasValue ?
                new ObjectParameter("Estado", estado) :
                new ObjectParameter("Estado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("Sp_TipoUsuarioInsertar", identificadorParameter, descripcionParameter, estadoParameter);
        }
    
        public virtual int Sp_TipoUsuarioModificar(Nullable<int> idTipoUsuario, Nullable<int> identificador, string descripcion)
        {
            var idTipoUsuarioParameter = idTipoUsuario.HasValue ?
                new ObjectParameter("IdTipoUsuario", idTipoUsuario) :
                new ObjectParameter("IdTipoUsuario", typeof(int));
    
            var identificadorParameter = identificador.HasValue ?
                new ObjectParameter("Identificador", identificador) :
                new ObjectParameter("Identificador", typeof(int));
    
            var descripcionParameter = descripcion != null ?
                new ObjectParameter("Descripcion", descripcion) :
                new ObjectParameter("Descripcion", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_TipoUsuarioModificar", idTipoUsuarioParameter, identificadorParameter, descripcionParameter);
        }
    
        public virtual ObjectResult<Sp_UsuarioBuscar_Result> Sp_UsuarioBuscar(string correo)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_UsuarioBuscar_Result>("Sp_UsuarioBuscar", correoParameter);
        }
    
        public virtual int Sp_UsuarioCambiarEstado(Nullable<int> idUsuario, Nullable<bool> nuevoEstado)
        {
            var idUsuarioParameter = idUsuario.HasValue ?
                new ObjectParameter("idUsuario", idUsuario) :
                new ObjectParameter("idUsuario", typeof(int));
    
            var nuevoEstadoParameter = nuevoEstado.HasValue ?
                new ObjectParameter("NuevoEstado", nuevoEstado) :
                new ObjectParameter("NuevoEstado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_UsuarioCambiarEstado", idUsuarioParameter, nuevoEstadoParameter);
        }
    
        public virtual ObjectResult<Sp_UsuarioConsultar_Result> Sp_UsuarioConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_UsuarioConsultar_Result>("Sp_UsuarioConsultar");
        }
    
        public virtual ObjectResult<Nullable<decimal>> Sp_UsuarioInsertar(Nullable<int> idPersona, string correo, string clave, Nullable<bool> estado)
        {
            var idPersonaParameter = idPersona.HasValue ?
                new ObjectParameter("IdPersona", idPersona) :
                new ObjectParameter("IdPersona", typeof(int));
    
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var claveParameter = clave != null ?
                new ObjectParameter("Clave", clave) :
                new ObjectParameter("Clave", typeof(string));
    
            var estadoParameter = estado.HasValue ?
                new ObjectParameter("Estado", estado) :
                new ObjectParameter("Estado", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("Sp_UsuarioInsertar", idPersonaParameter, correoParameter, claveParameter, estadoParameter);
        }
    
        public virtual int Sp_UsuarioModificar(Nullable<int> idUsuario, Nullable<int> idPersona, string correo, string clave)
        {
            var idUsuarioParameter = idUsuario.HasValue ?
                new ObjectParameter("idUsuario", idUsuario) :
                new ObjectParameter("idUsuario", typeof(int));
    
            var idPersonaParameter = idPersona.HasValue ?
                new ObjectParameter("IdPersona", idPersona) :
                new ObjectParameter("IdPersona", typeof(int));
    
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            var claveParameter = clave != null ?
                new ObjectParameter("Clave", clave) :
                new ObjectParameter("Clave", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_UsuarioModificar", idUsuarioParameter, idPersonaParameter, correoParameter, claveParameter);
        }
    
        public virtual ObjectResult<Sp_TokenConsultar_Result2> Sp_TokenConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_TokenConsultar_Result2>("Sp_TokenConsultar");
        }
    
        public virtual ObjectResult<Sp_AsignarModuloPrivilegioConsultar_Result1> Sp_AsignarModuloPrivilegioConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_AsignarModuloPrivilegioConsultar_Result1>("Sp_AsignarModuloPrivilegioConsultar");
        }
    
        public virtual ObjectResult<Sp_UsuarioValidar_Result1> Sp_UsuarioValidar(string correo)
        {
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_UsuarioValidar_Result1>("Sp_UsuarioValidar", correoParameter);
        }
    
        public virtual ObjectResult<Sp_AsignarTipoUsuarioModuloPrivilegioConsultar_Result1> Sp_AsignarTipoUsuarioModuloPrivilegioConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_AsignarTipoUsuarioModuloPrivilegioConsultar_Result1>("Sp_AsignarTipoUsuarioModuloPrivilegioConsultar");
        }
    
        public virtual ObjectResult<Sp_AsignarUsuarioTipoUsuarioConsultar_Result> Sp_AsignarUsuarioTipoUsuarioConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_AsignarUsuarioTipoUsuarioConsultar_Result>("Sp_AsignarUsuarioTipoUsuarioConsultar");
        }
    
        public virtual ObjectResult<Sp_PersonaConsultar_Result1> Sp_PersonaConsultar()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Sp_PersonaConsultar_Result1>("Sp_PersonaConsultar");
        }
    }
}
