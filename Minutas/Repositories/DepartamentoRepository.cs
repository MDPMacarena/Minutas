using Microsoft.EntityFrameworkCore;
using Minutas.Models;
using System.Text;

namespace Minutas.Repositories
{
    public class DepartamentoRepository:Repository<Departamento>
    {
        public DepartamentoRepository(DbContext context) : base(context)
        {
        }

        public bool ValidarDepartamento(Departamento dep, out string errores)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(dep.Nombre))
                sb.AppendLine("El Nombre está vacío.");
            if (dep.IdJefe <= 0)
                sb.AppendLine("El Id del Jefe no es válido.");
            if (dep.IdDeptSuperior <= 0)
                sb.AppendLine("El Id del Departamento Superior no es válido.");

            errores = sb.ToString();
            return errores.Length == 0;
        }

        public void Eliminar(Departamento dep)
        {
            if (dep.Minutas.Any() || dep.Usuarios.Any() || dep.InverseIdDeptSuperiorNavigation.Any())
            {
                dep.Activo = false;
                Update(dep);
            }
            else
            {
                Delete(dep);
            }
        }

        public void EditarDepartamento(Departamento dep)
        {
            var departamento = Get(dep.Id);
            if (departamento != null)
            {
                departamento.Nombre = dep.Nombre;
                departamento.IdJefe = dep.IdJefe;
                departamento.IdDeptSuperior = dep.IdDeptSuperior;

                Update(departamento);
            }
        }

        public IEnumerable<Departamento> GetDepartamentosActivos()
        {
            return GetAll().Where(d => d.Activo == true).OrderBy(d => d.Nombre);
        }
    }
}
