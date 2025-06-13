namespace MinutasManage.Areas.Admin.Models
{
    public class ConfiguracionCorreo
    {
        public string Host { get; set; }
        public int Puerto { get; set; }
        public string Encriptacion { get; set; }
        public string Remitente { get; set; }
        public string Token { get; set; }

    }
}
