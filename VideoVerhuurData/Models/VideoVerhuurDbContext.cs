using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoVerhuurData.Models;

public class VideoVerhuurDbContext : DbContext
{
	public DbSet<Klanten> Klanten { get; set; }
	public DbSet<Genres> Genres { get; set; }	
	public DbSet<Films> Films { get; set; }	
	public DbSet<Verhuringen> Verhuringen { get; set;}

	public VideoVerhuurDbContext() { }

	public VideoVerhuurDbContext(DbContextOptions options) : base(options) { }


}
