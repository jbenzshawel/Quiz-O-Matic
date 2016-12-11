// using System;
// using ApiQuizGenerator.Models;
// using Microsoft.EntityFrameworkCore;

// namespace ApiQuizGenerator.DAL
// {
//     public class QuizGeneratorContext : DbContext  
//     {
//         public QuizGeneratorContext(DbContextOptions<QuizGeneratorContext> options)
//             : base(options)
//         { }

//         public DbSet<Quiz> Quizes { get; set; }

//         public DbSet<QuizType> QuizTypes { get; set; }

//         public DbSet<Question> Questions { get; set; }

//         public DbSet<Answer> Answers { get; set; }     

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             optionsBuilder
//             .UseNpgsql("Host=localhost;Database=QuizGenerator;Username=QuizGeneratorAdmin;Password=localdbpw");
//         }   
//     }

  
// }

