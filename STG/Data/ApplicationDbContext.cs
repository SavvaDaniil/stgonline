using Microsoft.EntityFrameworkCore;
using STG.Entities;
using STG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonType> LessonTypes { get; set; }
        public DbSet<ConnectionLessonToLevel> ConnectionsLessonToLevel { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<ObserverLessonUser> ObserversLessonUser { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageDay> PackageDays { get; set; }
        public DbSet<PackageLesson> PackageLessons { get; set; }
        public DbSet<ConnectionUserToPrivatePackage> ConnectionsUserToPrivatePackage { get; set; }
        public DbSet<PurchasePackage> PurchasePackages { get; set; }

        public DbSet<PurchaseLesson> PurchaseLessons { get; set; }
        public DbSet<PurchaseSubscription> PurchaseSubscriptions { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PreUserWithAppointment> PreUserWithAppointments { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoSection> VideoSections { get; set; }
        public DbSet<VideoSubsection> VideoSubsections { get; set; }

        public DbSet<Extend> Extends { get; set; }
        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<AmoCRMData> AmoCRMDatas { get; set; }
        public DbSet<TechData> TechDatas { get; set; }
        public DbSet<PackageChat> PackageChats { get; set; }
    }
}
