using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using QuizAPI.Repository;
using System.Linq;

namespace QuizAPI.Model
{
    /// <summary>
    /// Used for identity
    /// </summary>
    public class UserDbContext : QuizDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) 
            : base(((SqlServerOptionsExtension)options.Extensions.First(r => r.GetType() == typeof(SqlServerOptionsExtension))).ConnectionString) { }
    }
}
