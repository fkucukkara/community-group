using CG.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace CG.Data.DBContext;

public class CGDBContext : DbContext
{
    public CGDBContext(DbContextOptions<CGDBContext> options) : base(options)
    { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<CommunityGroup> CommunityGroups { get; set; }
}
