using Microsoft.EntityFrameworkCore;
using Backend.Models;
namespace Backend.Contexts;

public class SnippetContext : DbContext
{
    public DbSet<Snippet> Snippets { get; set; } = default!;

    public SnippetContext(DbContextOptions<SnippetContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Snippet>()
            .Property(e => e.Arguments)
            .HasConversion(
                v => string.Join(',', v ?? new string[0]),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
}
