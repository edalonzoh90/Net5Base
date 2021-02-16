using System;

namespace BOT.DATA.Models
{
    public class EmpleadoModel
    {
        public int IdEmpresa { get; set; }
        public string CodigoEmpresa { get; set; }
        public string Ficha { get; set; }
        public int idPersona { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string IMSS { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string LugarNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Calle { get; set; }
        public string Referencia { get; set; }
        public string Referencia2 { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public string CiudadResidencia { get; set; }
        public string EstadoResidencia { get; set; }
        public string TelefonoCelular { get; set; }
        public string TelefonoDomicilio { get; set; }
        public string EmailPersonal { get; set; }
        public string Turno { get; set; }
        public string EMAILTRABAJO { get; set; }
        public string UrlImagen { get; set; }
        public byte[] Imagen { get; set; }
        public string Marcaje { get; set; }
        public int? DisponiblePaseLista { get; set; }
        public int? DisponibleCalendario { get; set; }
    }

    //Datos de area
    public class DatosAreaEmpleadoModelo
    {
        public string Puesto { get; set; }
        public string Ficha { get; set; }
        public string Departamento { get; set; }
        public string Area { get; set; }
        public string CentroCosto { get; set; }
        public string Gerencia { get; set; }
        public string Email { get; set; }
        public string CodigoEmpresa { get; set; }
        public string Superintendencia { get; set; }
        public string Subdireccion { get; set; }
    }

    //Entidad Empleado IDM

    public class DatosEmpleadoIDM
    {
        public string Ficha { get; set; }

        public string Nombre { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string Email { get; set; }

        public string OtroEmail { get; set; }

        public string CelPersonal { get; set; }

        public string CelOficina { get; set; }





    }
}
