using ED.GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineVendingMachineServer.WebAPI.Models;

namespace OnlineVendingMachineServer.WebAPI.Context;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<GoogleToken> GoogleTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserRole<Guid>>();

        builder.Entity<IdentityUserLogin<Guid>>()
        .HasKey(l => new { l.LoginProvider, l.ProviderKey });


        // 👇 Bunun yerine şu yapı eklenmeli:
        builder.Entity<Product>().UseTptMappingStrategy();

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        //base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();
        string? userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(p => p.Type == "UserName")?.Value;
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt)
                    .CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeletedAt)
                        .CurrentValue = DateTime.UtcNow;
                }
                else
                {
                    entry.Property(p => p.UpdatedAt)
                        .CurrentValue = DateTime.UtcNow;
                }
            }

            //if (entry.State == EntityState.Deleted)
            //{
            //    throw new ArgumentException("Db'den silme işlemi yapamazsınız!");
            //}

            //switch (entry.State)
            //{
            //    case EntityState.Added:
            //        entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            //        break;
            //    case EntityState.Modified:
            //        entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            //        break;
            //}

        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}