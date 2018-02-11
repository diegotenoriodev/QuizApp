using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using QuizAPI.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuizAPI.Repository
{
    public interface IQuizDbContext : IDisposable
    {
        DbSet<UserQuiz> Users { get; set; }
        DbSet<Quiz> Quizes { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<TrueFalseOption> TrueFalseOptions { get; set; }
        DbSet<MultipleChoice> MultipleChoices { get; set; }
        DbSet<ImageChoice> ImageChoices { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
        DbSet<Choice> MultipleChoice_ChoiceAnswers { get; set; }
        DbSet<TrueFalseAnswer> TrueFalseAnswers { get; set; }
        DbSet<YesNoAnswer> YesNoAnswers { get; set; }
        DbSet<OpenEnded> OpenEndedAnswers { get; set; }
        int SaveChanges();
        EntityEntry Attach(object entity);
    }

    public class QuizDbContext : IdentityDbContext<UserQuiz>, IQuizDbContext, IDesignTimeDbContextFactory<QuizDbContext>
    {
        private string connectionString;
        public QuizDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        
        public QuizDbContext()
        {
            //this.connectionString = "";
            this.connectionString = "Data Source=LAPTOP-1DME2FQQ\\SQLDIEGO;Database=QuizAPIDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public QuizDbContext CreateDbContext(string[] args)
        {
            return new QuizDbContext();
        }

        public DbSet<Choice> MultipleChoice_ChoiceAnswers { get; set; }
        public new DbSet<UserQuiz> Users { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TrueFalseOption> TrueFalseOptions { get; set; }
        public DbSet<MultipleChoice> MultipleChoices { get; set; }
        public DbSet<ImageChoice> ImageChoices { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
        public DbSet<TrueFalseAnswer> TrueFalseAnswers { get; set; }
        public DbSet<YesNoAnswer> YesNoAnswers { get; set; }
        public DbSet<OpenEnded> OpenEndedAnswers { get; set; }
    }

    public class MyDBSet<T> : DbSet<T> where T : class
    {
        public override LocalView<T> Local => base.Local;

        public override EntityEntry<T> Add(T entity)
        {
            return base.Add(entity);
        }

        public override Task<EntityEntry<T>> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.AddAsync(entity, cancellationToken);
        }

        public override void AddRange(params T[] entities)
        {
            base.AddRange(entities);
        }

        public override void AddRange(IEnumerable<T> entities)
        {
            base.AddRange(entities);
        }

        public override Task AddRangeAsync(params T[] entities)
        {
            return base.AddRangeAsync(entities);
        }

        public override Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.AddRangeAsync(entities, cancellationToken);
        }

        public override EntityEntry<T> Attach(T entity)
        {
            return base.Attach(entity);
        }

        public override void AttachRange(params T[] entities)
        {
            base.AttachRange(entities);
        }

        public override void AttachRange(IEnumerable<T> entities)
        {
            base.AttachRange(entities);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override T Find(params object[] keyValues)
        {
            return base.Find(keyValues);
        }

        public override Task<T> FindAsync(params object[] keyValues)
        {
            return base.FindAsync(keyValues);
        }

        public override Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            return base.FindAsync(keyValues, cancellationToken);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override EntityEntry<T> Remove(T entity)
        {
            return base.Remove(entity);
        }

        public override void RemoveRange(params T[] entities)
        {
            base.RemoveRange(entities);
        }

        public override void RemoveRange(IEnumerable<T> entities)
        {
            base.RemoveRange(entities);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override EntityEntry<T> Update(T entity)
        {
            return base.Update(entity);
        }

        public override void UpdateRange(params T[] entities)
        {
            base.UpdateRange(entities);
        }

        public override void UpdateRange(IEnumerable<T> entities)
        {
            base.UpdateRange(entities);
        }
    }
}
