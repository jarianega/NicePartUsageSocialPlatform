using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NicePartUsageSocialPlatform.Models;

namespace NicePartUsageSocialPlatform.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<NpuCreation> NpuCreations { get; set; }
    public DbSet<Score> Scores { get; set; }
    public DbSet<Element> Elements { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            var elements = context.Set<Element>();
            if (!elements.Any())
            {
                elements.Add(new Element() { Name = "Frog" });
                elements.Add(new Element() { Name = "Sea shell" });
                elements.Add(new Element() { Name = "Hat" });
                elements.Add(new Element() { Name = "Brush" });
                elements.Add(new Element() { Name = "Sausage" });
                context.SaveChanges();
            }
        });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasMany(x => x.NicePartUsageCreations)
            .WithOne(x => x.User);

        builder.Entity<NpuCreation>()
            .HasMany(x => x.Elements)
            .WithMany(x => x.NicePartUsageCreations);
        builder.Entity<NpuCreation>()
            .HasKey(x => x.Id);

        builder.Entity<Score>()
            .HasOne(x => x.NpuCreation)
            .WithMany(x => x.Scores)
            .HasForeignKey(x => x.NpuCreationId);
        builder.Entity<Score>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
        builder.Entity<Score>()
            .HasKey(x => new { x.UserId, x.NpuCreationId });

        base.OnModelCreating(builder);
    }
}