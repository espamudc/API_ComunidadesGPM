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
    
    public partial class Sp_CuestionarioPublicadoConsultar_Result
    {
        public int IdCuestionarioPublicado { get; set; }
        public bool EstadoCuestionarioPublicado { get; set; }
        public System.DateTime FechaPublicacionCuestionarioPublicado { get; set; }
        public string UtilizadoCuestionarioPublicado { get; set; }
        public int ASIGNARUSUARIOTIPOUSUARIOPUBLICADO_IdAsignarUsuarioTipoUsuario { get; set; }
        public bool ASIGNARUSUARIOTIPOUSUARIOPUBLICADO_Estado { get; set; }
        public int USUARIOPUBLICADO_IdUsuario { get; set; }
        public int USUARIOPUBLICADO_IdPersona { get; set; }
        public string USUARIOPUBLICADO_Correo { get; set; }
        public string USUARIOPUBLICADO_Clave { get; set; }
        public bool USUARIOPUBLICADO_Estado { get; set; }
        public int PERSONAPUBLICADO_IdPersona { get; set; }
        public string PERSONAPUBLICADO_PrimerNombre { get; set; }
        public string PERSONAPUBLICADO_SegundoNombre { get; set; }
        public string PERSONAPUBLICADO_PrimerApellido { get; set; }
        public string PERSONAPUBLICADO_SegundoApellido { get; set; }
        public string PERSONAPUBLICADO_NumeroIdentificacion { get; set; }
        public string PERSONAPUBLICADO_Telefono { get; set; }
        public int PERSONAPUBLICADO_IdParroquia { get; set; }
        public string PERSONAPUBLICADO_Direccion { get; set; }
        public bool PERSONAPUBLICADO_Estado { get; set; }
        public int SEXOPUBLICADO_IdSexo { get; set; }
        public int SEXOPUBLICADO_Identificador { get; set; }
        public string SEXOPUBLICADO_Descripcion { get; set; }
        public bool SEXOPUBLICADO_Estado { get; set; }
        public int TIPOIDENTIFICACIONPUBLICADO_IdTipoIdentificacion { get; set; }
        public int TIPOIDENTIFICACIONPUBLICADO_Identificador { get; set; }
        public string TIPOIDENTIFICACIOPUBLICADON_Descripcion { get; set; }
        public bool TIPOIDENTIFICACIONPUBLICADO_Estado { get; set; }
        public int TIPOUSUARIOPUBLICADO_IdTipoUsuario { get; set; }
        public int TIPOUSUARIOPUBLICADO_Identificador { get; set; }
        public string TIPOUSUARIOPUBLICADO_Descripcion { get; set; }
        public bool TIPOUSUARIOPUBLICADO_Estado { get; set; }
        public int IdPeriodo { get; set; }
        public System.DateTime FechaInicioPeriodo { get; set; }
        public System.DateTime FechaFinPeriodo { get; set; }
        public string DescripcionPeriodo { get; set; }
        public bool EstadoPeriodo { get; set; }
        public int IdCabeceraVersionCuestionario { get; set; }
        public string CaracteristicaCabeceraVersionCuestionario { get; set; }
        public bool EstadoCabeceraVersionCuestionario { get; set; }
        public System.DateTime FechaCreacionCabeceraVersionCuestionario { get; set; }
        public int VersionCabeceraVersionCuestionario { get; set; }
        public string UtilizadoCabeceraVersionCuestionario { get; set; }
        public int ASIGNARRESPONSABLE_IdAsignarResponsable { get; set; }
        public System.DateTime ASIGNARRESPONSABLE_FechaAsignacion { get; set; }
        public bool ASIGNARRESPONSABLE_Estado { get; set; }
        public int CUESTIONARIOGENERICO_IdCuestionarioGenerico { get; set; }
        public string CUESTIONARIOGENERICO_Descripcion { get; set; }
        public string CUESTIONARIOGENERICO_Nombre { get; set; }
        public bool CUESTIONARIOGENERICO_Estado { get; set; }
        public int ASIGNARUSUARIOTIPOUSUARIO_IdAsignarUsuarioTipoUsuario { get; set; }
        public bool ASIGNARUSUARIOTIPOUSUARIO_Estado { get; set; }
        public int USUARIO_IdUsuario { get; set; }
        public int USUARIO_IdPersona { get; set; }
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
        public int TIPOUSUARIO_IdTipoUsuario { get; set; }
        public int TIPOUSUARIO_Identificador { get; set; }
        public string TIPOUSUARIO_Descripcion { get; set; }
        public bool TIPOUSUARIO_Estado { get; set; }
    }
}
