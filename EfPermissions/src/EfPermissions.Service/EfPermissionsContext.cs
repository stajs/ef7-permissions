using Microsoft.Data.Entity;

namespace EfPermissions.Service
{
	public class EfPermissionsContext : DbContext
	{
		public DbSet<Animal> Animals { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=EfPermissions;Trusted_Connection=True;MultipleActiveResultSets=true");
		}
	}
}