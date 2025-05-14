using MinutasManage.Models;

namespace MinutasManage.Areas.Admin.Models
{
    public class AgregarEmpleadoViewModel
    {
        public Usuarios Usuario { get; set; }

        public IEnumerable<Departamento> Departamento { get; set; }
    }

}
