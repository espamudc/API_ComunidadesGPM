//------------------------------------------------------------------------------
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
    
    public partial class Sp_CabeceraCaracterizacionConsultarPorAsignarResponsableModeloPublicado_Result
    {
        public int IdCabeceraCaracterizacion { get; set; }
        public Nullable<System.DateTime> FechaFinalizadoCabeceraCaracterizacion { get; set; }
        public System.DateTime FechaRegistroCabeceraCaracterizacion { get; set; }
        public bool FinalizadoCabeceraCaracterizacion { get; set; }
        public int IdAsignarResponsableModeloPublicado { get; set; }
        public bool EstadoAsignarResponsableModeloPublicado { get; set; }
        public System.DateTime FechaAsignacionAsignarResponsableModeloPublicado { get; set; }
        public System.DateTime FechaInicioAsignarResponsableModeloPublicado { get; set; }
        public System.DateTime FechaFinAsignarResponsableModeloPublicado { get; set; }
        public int IdModeloPublicado { get; set; }
        public bool EstadoModeloPublicado { get; set; }
        public System.DateTime FechaPublicacionModeloPublicado { get; set; }
        public int IdCabeceraVersionModelo { get; set; }
        public string CaracteristicaCabeceraVersionModelo { get; set; }
        public bool EstadoCabeceraVersionModelo { get; set; }
        public System.DateTime FechaCreacionCabeceraVersionModelo { get; set; }
        public int VersionCabeceraVersionModelo { get; set; }
        public int IdModeloGenerico { get; set; }
        public bool EstadoModeloGenerico { get; set; }
        public string DescripcionModeloGenerico { get; set; }
        public string NombreModeloGenerico { get; set; }
        public int IdAsignarUsuarioTipoUsuario { get; set; }
        public bool EstadoAsignarUsuarioTipoUsuario { get; set; }
        public int USUARIO_IdUsuario { get; set; }
        public string USUARIO_Correo { get; set; }
        public string USUARIO_Clave { get; set; }
        public bool USUARIO_Estado { get; set; }
        public int PERSONA_IdPersona { get; set; }
        public string PERSONA_PrimerNombre { get; set; }
        public string PERSONA_SegundoNombre { get; set; }
        public string PERSONA_PrimerApellido { get; set; }
        public string PERSONA_SegundoApellido { get; set; }
        public string PERSONA_NumeroIdentificacion { get; set; }
        public string PERSONA_Telefono { get; set; }
        public int PERSONA_IdParroquia { get; set; }
        public string PERSONA_Direccion { get; set; }
        public bool PERSONA_Estado { get; set; }
        public int SEXO_IdSexo { get; set; }
        public int SEXO_Identificador { get; set; }
        public string SEXO_Descripcion { get; set; }
        public bool SEXO_Estado { get; set; }
        public int TIPOIDENTIFICACION_IdTipoIdentificacion { get; set; }
        public int TIPOIDENTIFICACION_Identificador { get; set; }
        public string TIPOIDENTIFICACION_Descripcion { get; set; }
        public bool TIPOIDENTIFICACION_Estado { get; set; }
        public int ASIGNARUSUARIOTIPOUSUARIO_IdAsignarUsuarioTipoUsuario { get; set; }
        public bool ASIGNARUSUARIOTIPOUSUARIO_Estado { get; set; }
        public int TIPOUSUARIO_IdTipoUsuario { get; set; }
        public int TIPOUSUARIO_Identificador { get; set; }
        public string TIPOUSUARIO_Descripcion { get; set; }
        public bool TIPOUSUARIO_Estado { get; set; }
        public int IdPresidenteJuntaParroquial { get; set; }
        public bool EstadoPresidenteJuntaParroquial { get; set; }
        public System.DateTime FechaIngresoPresidenteJuntaParroquial { get; set; }
        public Nullable<System.DateTime> FechaSalidaPresidenteJuntaParroquial { get; set; }
        public string RepresentantePresidenteJuntaParroquial { get; set; }
        public int IdParroquia { get; set; }
        public string CodigoParroquia { get; set; }
        public string NombreParroquia { get; set; }
        public string DescripcionParroquia { get; set; }
        public string RutaLogoParroquia { get; set; }
        public bool EstadoParroquia { get; set; }
        public int IdCanton { get; set; }
        public string CodigoCanton { get; set; }
        public string NombreCanton { get; set; }
        public string DescripcionCanton { get; set; }
        public string RutaLogoCanton { get; set; }
        public bool EstadoCanton { get; set; }
        public int IdProvincia { get; set; }
        public string CodigoProvincia { get; set; }
        public string NombreProvincia { get; set; }
        public string DescripcionProvincia { get; set; }
        public string RutaLogoProvincia { get; set; }
        public bool EstadoProvincia { get; set; }
    }
}
