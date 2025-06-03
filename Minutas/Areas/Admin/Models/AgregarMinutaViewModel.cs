using MinutasManage.Models;

namespace MinutasManage.Areas.Admin.Models
{
    public class AgregarMinutaViewModel
    {
        public string Contenido { get; set; }
        public int[] AsistentesIds { get; set; }
        public sbyte Privada { get; set; }
    }
}
