using MinutasManage.Models;

namespace MinutasManage.Repositories
{
    public class MinutasRepository
    {
        private readonly DbminutasContext Context;
        public MinutasRepository(DbminutasContext context)
        {
            Context = context;
        }

        public void Insert(Minutas entity)
        {
            Context.Minutas.Add(entity);
            Context.SaveChanges();
        }


    }
}
