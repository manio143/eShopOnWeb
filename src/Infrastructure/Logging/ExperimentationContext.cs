using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class ExperimentationContext : DbContext
{
    #pragma warning disable CS8618 // Required by Entity Framework
    public ExperimentationContext(DbContextOptions<ExperimentationContext> options) : base(options) {}

    public DbSet<ExperimentationAssignment> Assignments { get; set; }
    public DbSet<BasketSessionMapping> BasketSessionMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

[Table("Assignment", Schema = "exp")]
public class ExperimentationAssignment
{
    [Key]
    public Guid SessionId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(128)")]
    public string ExperimentName { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(64)")]
    public string VariantId { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

[Table("BasketSessionMapping", Schema = "exp")]
[PrimaryKey(nameof(SessionId), nameof(BasketId))]
public class BasketSessionMapping
{
    [ForeignKey("Assignment")]
    public Guid SessionId { get; set; }

    public int BasketId { get; set; }
}

public class ExperimentationService : IExperimentationService
{
    private readonly ExperimentationContext _dbContext;

    public ExperimentationService(ExperimentationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveExperimentAssignmentAsync(Guid sessionId, string experimentName, string variantId)
    {
        var assignment = await _dbContext.Assignments
            .Where(a => a.SessionId == sessionId)
            .FirstOrDefaultAsync();

        if (assignment == null)
        {
            assignment = new ExperimentationAssignment
            {
                SessionId = sessionId,
                ExperimentName = experimentName,
                VariantId = variantId
            };

            _dbContext.Assignments.Add(assignment);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task MapBasketToSessionAsync(int basketId, Guid sessionId)
    {
        var mapping = new BasketSessionMapping
        {
            BasketId = basketId,
            SessionId = sessionId
        };

        _dbContext.BasketSessionMappings.Add(mapping);
        await _dbContext.SaveChangesAsync();
    }
}

