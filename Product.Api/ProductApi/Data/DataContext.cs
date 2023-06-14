using Microsoft.EntityFrameworkCore;
using ProductApi.Entities;

namespace ProductApi.Data
{
	public class DataContext : DbContext
    {
		public DataContext(DbContextOptions options) : base(options)
		{

		}
		public DbSet<Item>? Items { get; set; }
	}
}