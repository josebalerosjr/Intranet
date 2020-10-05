using Intranet.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Data.Admin
{
    public class SuggestionContext : DbContext
    {
        public SuggestionContext(DbContextOptions<SuggestionContext> options) : base(options)
        {
        }

        public DbSet<Suggestion> Suggestions { get; set; }
    }
}