//using Microsoft.EntityFrameworkCore;
//using onl.Models;
//using System.Collections.Generic;

//namespace onl.Data
//{

//        public class ApplicationDbContext : DbContext
//        {
//            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//                : base(options)
//            {
//            }

//            public DbSet<Course> Courses { get; set; }
//            public DbSet<Upload> Uploads { get; set; }
//    }
//    }

// Previous code without mcqs and assignment (changes made in uploadvideos controller , upload and uploadvideo model
// uploadvideo.cshtml and applicationdbcontext


using Microsoft.EntityFrameworkCore;
using onl.Models;

namespace onl.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<MCQ> MCQs { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<User> UserInfo { get; set; }

        public DbSet<Cart> Carts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Upload and MCQs
            modelBuilder.Entity<Upload>()
                .HasMany(u => u.MCQs)
                .WithOne(mcq => mcq.Upload)
                .HasForeignKey(mcq => mcq.UploadId);

            // Configure the relationship between Upload and Assignment
            modelBuilder.Entity<Upload>()
                .HasOne(u => u.Assignment)
                .WithOne(a => a.Upload)
                .HasForeignKey<Assignment>(a => a.UploadId);
        }
    }
}
