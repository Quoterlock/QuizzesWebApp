using Microsoft.EntityFrameworkCore;
using QuizApp_API.DataAccess.Entities;

namespace QuizApp_API.DataAccess.Data
{
    public class QuizAppDbContext : DbContext
    {
        public DbSet<UserQuizRate> Rates { get; set; }
        public DbSet<QuizResult> Results { get; set; }
        
        public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) 
            : base (options) 
        { 
            
        }
    }
}
