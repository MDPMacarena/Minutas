using Minutas.Models;
using System.Text;

namespace Minutas.Repositories
{
    public class DepartamentoRepository:Repository<Departamento>
    {
        public DepartamentoRepository(DbminutasContext context) : base(context)
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
    }
}
