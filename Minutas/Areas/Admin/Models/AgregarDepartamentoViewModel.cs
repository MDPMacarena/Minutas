using Minutas.Models;

namespace Minutas.Areas.Admin.Models
{
    public class AgregarDepartamentoViewModel
    {
        // Propiedad para el departamento que se está agregando
        public Departamento Departamento { get; set; } = new Departamento();

        // Lista de empleados para seleccionar un jefe (IdJefe)
        public IEnumerable<Usuarios> Empleados { get; set; } = new List<Usuarios>();

        // Lista de departamentos para seleccionar el departamento superior (IdDeptSuperior)
        public IEnumerable<Departamento> Departamentos { get; set; } = new List<Departamento>();

    }
}
