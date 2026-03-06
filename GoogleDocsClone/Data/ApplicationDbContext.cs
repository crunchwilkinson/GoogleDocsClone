using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GoogleDocsClone.Models;

namespace GoogleDocsClone.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Document> Docs { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
