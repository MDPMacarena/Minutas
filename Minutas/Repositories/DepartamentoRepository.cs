using Microsoft.EntityFrameworkCore;
using MinutasManage.Models;
using MinutasManage.Repositories;
using System.Text;

public class DepartamentoRepository
{
    private readonly DbminutasContext Context;

    public DepartamentoRepository(DbminutasContext context)
    {
        Context = context;
    }

    public void Insert(Departamento dep)
    {
        Context.Departamento.Add(dep);
        Context.SaveChanges();
    }

    public IEnumerable<Departamento> GetAll()
    {
        return Context.Departamento
                      .AsNoTracking()
                      .Include(d => d.IdJefeNavigation)
                      .Include(d => d.IdDeptSuperiorNavigation)
                      .ToList();
    }

    public Departamento? Get(object id)
    {
        return Context.Departamento
                      .Include(d => d.IdJefeNavigation)
                      .Include(d => d.IdDeptSuperiorNavigation)
                      .Include(d => d.Usuarios) // ← Agregado
                      .Include(d => d.Minutas) // ← Si quieres validar también minutas
                      .Include(d => d.InverseIdDeptSuperiorNavigation) // ← Hijos
                      .FirstOrDefault(d => d.Id == (int)id);
    }

    public void Update(Departamento dep)
    {
        var original = Context.Departamento.FirstOrDefault(d => d.Id == dep.Id);
        if (original != null)
        {
            original.Nombre = dep.Nombre;
            original.IdJefe = dep.IdJefe;
            original.IdDeptSuperior = dep.IdDeptSuperior;
            // ... cualquier otro campo

            Context.SaveChanges(); // EF ya está haciendo tracking del original
        }
    }

    public void Delete(Departamento dep)
    {
        Context.Departamento.Remove(dep);
        Context.SaveChanges();
    }

    public void Eliminar(Departamento dep)
    {
        // En este punto ya se asume que la validación se hizo afuera
        if (dep.Minutas.Any() || dep.InverseIdDeptSuperiorNavigation.Any())
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
        var departamento = Context.Departamento.FirstOrDefault(d => d.Id == dep.Id);
        if (departamento != null)
        {
            departamento.Nombre = dep.Nombre;
            departamento.IdJefe = dep.IdJefe;
            departamento.IdDeptSuperior = dep.IdDeptSuperior;
            // ¡No asignes dep.IdJefeNavigation ni dep.IdDeptSuperiorNavigation!
            Context.SaveChanges();
        }
    }
    public IEnumerable<Departamento> GetDepartamentosActivos()
    {
        return Context.Departamento
                      .Include(d => d.IdJefeNavigation)
                      .Include(d => d.IdDeptSuperiorNavigation)
                      .Where(d => d.Activo.HasValue && d.Activo.Value)
                      .OrderBy(d => d.Nombre)
                      .ToList();
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
                sbErrores.AppendLine("El nombre del departamento está vacío.");

            if (departamentosExistentes.Any(d =>
                d.Nombre.Equals(dep.Nombre, StringComparison.OrdinalIgnoreCase) && d.Id != dep.Id))
            {
                sbErrores.AppendLine("Ya existe un departamento con ese nombre.");
            }

            if (dep.IdJefe <= 0)
                sbErrores.AppendLine("El Id del jefe no es válido.");

            if (dep.IdDeptSuperior.HasValue && dep.IdDeptSuperior <= 0)
                sbErrores.AppendLine("El Id del departamento superior no es válido.");

            if (dep.IdDeptSuperior == dep.Id)
                sbErrores.AppendLine("El departamento no puede ser su propio superior.");
        }
        else
        {
            int empleadosACargo = dep.Usuarios?.Count ?? 0;

            if (empleadosACargo > 0)
            {
                sbErrores.AppendLine($"No se puede eliminar el departamento '{dep.Nombre}' porque tiene {empleadosACargo} empleado{(empleadosACargo > 1 ? "s" : "")} a su cargo.");
            }
        }

        errores = sbErrores.ToString();
        avisos = sbAvisos.ToString();
        return errores.Length == 0;
    }


}

