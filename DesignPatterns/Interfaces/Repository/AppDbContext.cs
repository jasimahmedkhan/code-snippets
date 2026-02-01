using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace design_patterns.Interfaces.Repository;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    
    public DbSet<Order> Orders => Set<Order>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUser(modelBuilder);
        ConfigureOrder(modelBuilder);
        ApplySoftDeleteFilter(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });
    }

    private static void ConfigureOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(64);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });
    }

    private static void ApplySoftDeleteFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(IPersistedEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(IPersistedEntity.IsDeleted));
            var comparison = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(comparison, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}

public static class AppDbContextFactory
{
    public static AppDbContext Create(string? connectionString = null, ILoggerFactory? loggerFactory = null)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        var resolved = string.IsNullOrWhiteSpace(connectionString)
            ? $"Data Source={Path.Combine(AppContext.BaseDirectory, "design-patterns.db")}"
            : connectionString;

        builder.UseSqlite(resolved);

        if (loggerFactory != null)
        {
            builder.UseLoggerFactory(loggerFactory);
        }

        return new AppDbContext(builder.Options);
    }
}
