using MinutasManage.Models;

namespace MinutasManage.Areas.Admin.Models
{
    public class AgregarMinutaViewModel
    {

        public Minutas Minuta { get; set; } = new Minutas();
        public List<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
        public List<string> Externos { get; set; } = new List<string>();


    }
}
