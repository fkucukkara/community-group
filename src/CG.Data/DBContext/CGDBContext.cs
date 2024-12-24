using CG.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace CG.Data.DBContext;

public class CGDBContext(DbContextOptions<CGDBContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<CommunityGroup> CommunityGroups { get; set; }
}
