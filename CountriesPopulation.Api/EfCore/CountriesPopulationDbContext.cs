using System;
using CountriesPopulation.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CountriesPopulation.Api.EfCore
{
    public class CountriesPopulationDbContext : DbContext
    {
        public CountriesPopulationDbContext(DbContextOptions<CountriesPopulationDbContext> options)
        : base(options)
        { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Population> Populations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Country>(c =>
            {
                c.ToTable("Countries");
                c.Property(x => x.Code).IsRequired().HasMaxLength(10);
                c.Property(x => x.Name).IsRequired().HasMaxLength(128);
                c.Property(x => x.Iso3).IsRequired().HasMaxLength(10);
                c.HasKey(x => x.Id);
                c.HasMany(x => x.PopulationCount).WithOne(p => p.Country).HasForeignKey(x => x.CountryId);

            }
            );
            builder.Entity<Population>(p =>
            {
                p.Property(x => x.Year).IsRequired();
                p.HasKey(x => x.Id);
                p.Property(x => x.Count).IsRequired();
            });
        }
    }
}

