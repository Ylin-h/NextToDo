using Microsoft.EntityFrameworkCore;

namespace NextWebApi.Models
{
    public class NextToDoDbContext: DbContext
    {
        public NextToDoDbContext(DbContextOptions<NextToDoDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<ToDo> ToDo { get; set; }
        public virtual DbSet<Memo> Memo { get; set; }
    }
}
