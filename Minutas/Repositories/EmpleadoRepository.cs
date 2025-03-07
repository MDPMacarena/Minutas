using Minutas.Models;
using System.Text;

namespace Minutas.Repositories
{
    public class EmpleadoRepository : Repository<Usuarios>
    {
        public EmpleadoRepository(DbminutasContext context) : base(context)
        {
        }
        public bool ValidarEmpleado(Usuarios empleado, out string errores)
        {
            var sb = new StringBuilder();

            // Si el rol es null, no lo validamos aún
            if (empleado.IdRol == 0) // Verifica si es null
            {
                sb.AppendLine("El rol será asignado más tarde.");


            }

            if (string.IsNullOrWhiteSpace(empleado.NumEmpleado))
                sb.AppendLine("El número de empleado está vacío.");
            if (string.IsNullOrWhiteSpace(empleado.Nombre))
                sb.AppendLine("El nombre está vacío.");
            if (string.IsNullOrWhiteSpace(empleado.Correo))
                sb.AppendLine("El correo está vacío.");
            if (empleado.IdDepartamento <= 0)
                sb.AppendLine("El ID del departamento no es válido.");



            errores = sb.ToString();
            return errores.Length == 0;
        }


        public void Eliminar(Usuarios empleado)
        {
            if (empleado.Minutas.Any() || empleado.MinutaUsuario.Any())
            {
                empleado.Activo = false;
                Update(empleado);
            }
            else
            {
                Delete(empleado);
            }
        }

        public void EditarEmpleado(Usuarios empleado)
        {
            var emp = Get(empleado.Id);
            if (emp != null)
            {
                emp.NumEmpleado = empleado.NumEmpleado;
                emp.Nombre = empleado.Nombre;
                emp.Correo = empleado.Correo;
                emp.IdDepartamento = empleado.IdDepartamento;
                emp.IdRol = empleado.IdRol;
                emp.FechaNacimiento = empleado.FechaNacimiento;

                Update(emp);
            }
        }

        public IEnumerable<Usuarios> GetEmpleadosActivos()
        {
            return GetAll().Where(e => e.Activo == true).OrderBy(e => e.Nombre);
        }


    }

}
