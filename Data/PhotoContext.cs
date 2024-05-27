using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
	public class PhotoContext : IdentityDbContext<IdentityUser>
	{
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Category>? Categories { get; set; }
        public DbSet<ContactMessage>? Messages { get; set; }
		public PhotoContext()
		{
		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost,1433;Database=photo_db;User Id=sa;Password=dockerStrongPwd123;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Categoria 1" },
                new Category { Id = 2, Name = "Categoria 2" },
                new Category { Id = 3, Name = "Categoria 3" },
                new Category { Id = 4, Name = "Categoria 4" },
                new Category { Id = 5, Name = "Categoria 5" },
                new Category { Id = 6, Name = "Categoria 6" },
                new Category { Id = 7, Name = "Categoria 7" }
                );

            modelBuilder.Entity<Photo>().HasData(
                new Photo { Id = 1, Title = "Titolo 1", Description = "Descrizione 1", IsVisible = true},
                new Photo { Id = 2, Title = "Titolo 2", Description = "Descrizione 2", IsVisible = true },
                new Photo { Id = 3, Title = "Titolo 3", Description = "Descrizione 3", IsVisible = true },
                new Photo { Id = 4, Title = "Titolo 4", Description = "Descrizione 4", IsVisible = true }
                );

            base.OnModelCreating(modelBuilder);

        }
    }
}

