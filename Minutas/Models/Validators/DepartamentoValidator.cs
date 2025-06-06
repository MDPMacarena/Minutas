using MinutasManage.Repositories;
using System.Text;

namespace MinutasManage.Models.Validators
{
    public class DepartamentoValidator
    {
        private readonly Repository<Departamento> repository;

        public DepartamentoValidator(Repository<Departamento> repository)
        {
            this.repository = repository;
        }

        public bool ValidarDepartamento(
    Departamento dep,
    out string errores,
    out string avisos,
    List<Departamento> departamentosExistentes,
    bool esEliminacion = false)
        {
            var sbErrores = new StringBuilder();
            var sbAvisos = new StringBuilder();

            if (!esEliminacion)
            {
                if (string.IsNullOrWhiteSpace(dep.Nombre))
                    sbErrores.AppendLine("El campo 'Nombre del departamento' es obligatorio. Por favor, ingrese un nombre válido.");

                // Nueva validación para longitud del nombre
                if (!string.IsNullOrEmpty(dep.Nombre) && dep.Nombre.Length > 100)
                    sbErrores.AppendLine("El nombre del departamento excede la longitud máxima permitida. Por favor, verifique e intente de nuevo.");

                if (departamentosExistentes.Any(d =>
                    d.Nombre.Equals(dep.Nombre, StringComparison.OrdinalIgnoreCase) && d.Id != dep.Id))
                {
                    sbErrores.AppendLine("Ya existe un departamento registrado con el mismo nombre. Por favor, utilice un nombre diferente.");
                }

                if (dep.IdJefe <= 0)
                    sbErrores.AppendLine("Debe asignar un jefe válido al departamento.Por favor, verifique e intente de nuevo.");

                if (dep.IdDeptSuperior.HasValue && dep.IdDeptSuperior <= 0)
                    sbErrores.AppendLine("Debe especificar un departamento superior válido.Por favor, verifique e intente de nuevo. ");

                if (dep.IdDeptSuperior == dep.Id)
                    sbErrores.AppendLine("Un departamento no puede establecerse como su propio superior. Por favor, seleccione otro departamento superior.");
            }
            else
            {
                int empleadosACargo = dep.Usuarios?.Count ?? 0;

                if (empleadosACargo > 0)
                {
                    sbErrores.AppendLine($"No es posible eliminar el departamento '{dep.Nombre}' porque tiene {empleadosACargo} empleado{(empleadosACargo > 1 ? "s" : "")} a su cargo.");
                }
            }

            errores = sbErrores.ToString();
            avisos = sbAvisos.ToString();
            return errores.Length == 0;
        }
    }
}
