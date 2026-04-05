namespace SportsLeague.DataAccess.Repositories
{
    public class GenericRepository<T>
    {
        private LeagueDbContext context;

        public GenericRepository(LeagueDbContext context)
        {
            this.context = context;
        }
    }
}