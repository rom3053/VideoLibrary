using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.API.Models;

namespace VideoLibrary.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<VideoQuality> VideoQualities { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Submission>().OwnsMany(p => p.VideoQualities, q =>
        //     {
        //         q.WithOwner().HasForeignKey("SubmissionId");
        //         q.Property<int>("Id");
        //         q.HasKey("Id");
        //     });
        //}
    }
}
