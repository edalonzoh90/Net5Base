using System;
using System.ComponentModel;

namespace BOT.WEB.Models
{
    public class ResponseModel
    {

    }

    public enum EstadoMensaje
    {
        Exitoso,
        Error,
        Informacion,
        Advertencia
    }


    /// <summary>
    /// Clase para mandar los mensajes 
    /// </summary>
    public class Mensaje
    {
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public EstadoMensaje Estado { get; set; }
        public Mensaje()
        {
            Titulo = "Info";
            Contenido = "";
            Estado = EstadoMensaje.Exitoso;
        }

        public Mensaje(String Titulo, String Contenido, EstadoMensaje Estado)
        {
            this.Titulo = Titulo;
            this.Contenido = Contenido;
            this.Estado = Estado;
        }
    }

    public enum Mensajes
    {
        [Description("Los datos se grabaron correctamente.")]
        Exitoso,
        [Description("Los datos no se grabaron. Respuesta del sistema: ")]
        Error,
        [Description("Datos inconsistentes")]
        Advertencia,
        [Description("El registro se dio de baja correctamente")]
        Eliminar
    }

    public enum titulo
    {
        [Description("Operación exitosa")]
        Exitoso,
        [Description("Operación erronea")]
        Error,
        [Description("Operación anulada")]
        Advertencia
    }

    /// <summary>
    /// Clase para enviar respuesta a la view
    /// </summary>
    public class RespuestaJson
    {
        public RespuestaJson()
        {
        }
        public Mensaje Mensaje { get; set; }
        public object DatoExtra { get; set; }
        public object Data { get; set; }
        public string Url { get; set; }
        public bool Exitoso = true;
    }
}
