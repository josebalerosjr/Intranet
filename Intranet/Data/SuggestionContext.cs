using Microsoft.EntityFrameworkCore;

namespace Intranet.Models
{
    public class SuggestionContext : DbContext
    {
        public SuggestionContext(DbContextOptions<SuggestionContext> options) : base(options)
        {
        }

        public DbSet<Suggestion> Suggestions { get; set; }
    }
}