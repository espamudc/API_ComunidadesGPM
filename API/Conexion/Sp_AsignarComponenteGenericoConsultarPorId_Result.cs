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
    
    public partial class Sp_AsignarComponenteGenericoConsultarPorId_Result
    {
        public int IdAsignarComponenteGenerico { get; set; }
        public int IdComponente { get; set; }
        public int Orden { get; set; }
        public bool EstadoAsignarComponenteGenerico { get; set; }
        public string AsignarComponenteGenericoUtilizado { get; set; }
        public System.DateTime FechaAsignacionAsignarCuestionarioModelo { get; set; }
        public int IdAsignarCuestionarioModelo { get; set; }
        public int IdAsignarUsuarioTipoUsaurioAsignarCuestionarioModelo { get; set; }
        public int IdModeloGenericoAsignarCuestionarioModelo { get; set; }
        public bool EstadoCuestionarioPublicado { get; set; }
        public System.DateTime FechaPublicacionCuestionarioPublicado { get; set; }
        public int IdAsignarUsuarioTipoUsuarioCuestionarioPublicado { get; set; }
        public int IdCabeceraVersionCuestionarioCuestionarioPublicado { get; set; }
        public int IdCuestionarioPublicado { get; set; }
        public int IdPeriodoCuestionarioPublicado { get; set; }
    }
}