using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Data
{
    public class SoproplDbContext : DbContext
    {
        public SoproplDbContext(DbContextOptions<SoproplDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Arrow> Arrows { get; set; }
        // public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Access> AccessList { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //initial identity with custom aims and make it compatible with other entities
            builder.Entity<User>(entity =>
            {

                // Indexes for "normalized" username and email, to allow efficient lookups
                entity.HasAlternateKey(u => u.NormalizedUserName);
                entity.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

                // Limit the size of columns to use efficient database types
                entity.Property(u => u.UserName).HasMaxLength(128);
                entity.Property(u => u.NormalizedUserName).HasMaxLength(128);
                entity.Property(u => u.Email).HasMaxLength(256);
                entity.Property(u => u.NormalizedEmail).HasMaxLength(256);

                // A concurrency token for use with the optimistic concurrency checking
                entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                entity.HasMany(u => u.Notifications)
                    .WithOne(n => n.User)
                    .HasPrincipalKey(u => u.NormalizedUserName)
                    .HasForeignKey(n => n.NormalizedUserName)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Members)
                    .WithOne(m => m.User)
                    .HasPrincipalKey(u => u.NormalizedUserName)
                    .HasForeignKey(m => m.NormalizedUserName)
                    .OnDelete(DeleteBehavior.Cascade);



                entity.HasMany(u => u.Invitations)
                    .WithOne(i => i.User)
                    .HasPrincipalKey(u => u.NormalizedUserName)
                    .HasForeignKey(i => i.NormalizedUserName)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Photo>(entity =>
            {
                entity.HasOne(p => p.User)
                    .WithOne(u => u.Photo)
                    .HasForeignKey<User>(u => u.PhotoId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.Project)
                    .WithOne(p => p.Logo)
                    .HasForeignKey<Project>(u => u.LogoId);

                entity.HasOne(p => p.Organization)
                    .WithOne(o => o.Logo)
                    .HasForeignKey<Organization>(u => u.LogoId);

            });


            //----------------------------------------------------------------
            //initial entities of project to make it more efficient
            builder.Entity<Resource>(entity =>
            {
                entity.HasAlternateKey(r => new { r.ActivityName, r.NormalizedTeamName, r.NormalizedOrganizationName, r.NormalizedProjectName });

                entity.Property(r => r.NormalizedOrganizationName).HasMaxLength(128);
                entity.Property(r => r.NormalizedProjectName).HasMaxLength(128);
                entity.Property(r => r.NormalizedTeamName).HasMaxLength(128);
                entity.Property(r => r.ActivityName).HasMaxLength(128);

            });
            builder.Entity<Organization>(entity =>
            {
                entity.HasIndex(o => o.NormalizedName).IsUnique();
                entity.Property(o => o.NormalizedName).HasMaxLength(128);
                entity.Property(o => o.Name).IsRequired().HasMaxLength(128);

                entity.HasMany(o => o.Members)
                    .WithOne(m => m.Organiztion)
                    .HasPrincipalKey(o => o.NormalizedName)
                    .HasForeignKey(m => m.NormalizedOrganizationName)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Teams)
                    .WithOne(t => t.Organization)
                    .HasPrincipalKey(o => o.NormalizedName)
                    .HasForeignKey(t => t.NormalizedOrganizationName)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Projects)
                    .WithOne(p => p.Organization)
                    .HasPrincipalKey(o => o.NormalizedName)
                    .HasForeignKey(p => p.NormalizedOrganizationName)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Invitations)
                    .WithOne(i => i.Organization)
                    .HasPrincipalKey(o => o.NormalizedName)
                    .HasForeignKey(i => i.NormalizedOrganizationName)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(o => o.ConcurrencyStamp).IsConcurrencyToken();
            });

            builder.Entity<Team>(entity =>
            {
                entity.HasAlternateKey(t => new { t.NormalizedOrganizationName, t.NormalizedName });
                entity.Property(t => t.NormalizedName).HasMaxLength(128);
                entity.Property(t => t.Name).HasMaxLength(128);
                entity.Property(t => t.NormalizedOrganizationName).HasMaxLength(128);

                entity.HasMany(t => t.Members)
                    .WithOne(m => m.Team)
                    .HasPrincipalKey(t => new { t.NormalizedOrganizationName, t.NormalizedName })
                    .HasForeignKey(m => new { m.NormalizedOrganizationName, m.NormalizedTeamName })
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.Tasks)
                    .WithOne(r => r.Team)
                    .HasPrincipalKey(t => new { t.NormalizedName, t.NormalizedOrganizationName })
                    .HasForeignKey(r => new { r.NormalizedTeamName, r.NormalizedOrganizationName })
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.AccessList)
                    .WithOne(a => a.Team)
                    .HasPrincipalKey(t => new { t.NormalizedName, t.NormalizedOrganizationName })
                    .HasForeignKey(a => new { a.NormalizedTeamName, a.NormalizedOrganizationName })
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Member>(entity =>
            {
                entity.HasIndex(m => new { m.NormalizedOrganizationName, m.NormalizedUserName, m.NormalizedTeamName }).IsUnique();
                entity.Property(m => m.NormalizedUserName).IsRequired();
                entity.Property(m => m.NormalizedOrganizationName).IsRequired();
                entity.Property(m => m.NormalizedTeamName).IsRequired(false);

                entity.Property(m => m.NormalizedOrganizationName).HasMaxLength(128);
                entity.Property(m => m.NormalizedTeamName).HasMaxLength(128);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(128);

                entity.Property(e => e.Type).IsRequired();

                entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            });

            builder.Entity<Project>(entity =>
            {
                entity.HasAlternateKey(p => new { p.NormalizedName, p.NormalizedOrganizationName });

                entity.Property(p => p.Description).HasColumnType("ntext");
                entity.Property(p => p.NormalizedOrganizationName).HasMaxLength(128);
                entity.Property(p => p.NormalizedName).HasMaxLength(128);
                entity.Property(p => p.Name).HasMaxLength(128);

                entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                entity.HasMany(p => p.Activities)
                    .WithOne(a => a.Project)
                    .HasPrincipalKey(p => new { p.NormalizedName, p.NormalizedOrganizationName })
                    .HasForeignKey(a => new { a.NormalizedProjectName, a.NormalizedOrganizationName })
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.AccessList)
                    .WithOne(a => a.Project)
                    .HasPrincipalKey(p => new { p.NormalizedName, p.NormalizedOrganizationName })
                    .HasForeignKey(a => new { a.NormalizedProjectName, a.NormalizedOrganizationName })
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Access>(entity =>
            {
                entity.HasAlternateKey(a => new { a.NormalizedOrganizationName, a.NormalizedProjectName, a.NormalizedTeamName });

                entity.Property(a => a.NormalizedOrganizationName).HasMaxLength(128);
                entity.Property(a => a.NormalizedProjectName).HasMaxLength(128);
                entity.Property(a => a.NormalizedTeamName).HasMaxLength(128);
            });

            builder.Entity<Activity>(entity =>
            {

                entity.HasAlternateKey(a => new
                {
                    a.Name,
                    a.NormalizedProjectName,
                    a.NormalizedOrganizationName
                });

                entity.Property(a => a.Description).HasColumnType("ntext");

                entity.Property(a => a.Name).IsRequired().HasMaxLength(128);
                entity.Property(a => a.NormalizedOrganizationName).HasMaxLength(128);
                entity.Property(a => a.NormalizedProjectName).HasMaxLength(128);

                entity.Property(a => a.ConcurrencyStamp).IsConcurrencyToken();

                entity.HasMany(a => a.OutArrows)
                                .WithOne(oa => oa.FromActivity)
                                .HasPrincipalKey(a => new
                                {
                                    a.Name,
                                    a.NormalizedProjectName,
                                    a.NormalizedOrganizationName
                                })
                                .HasForeignKey(oa => new { oa.ToActivityName, oa.NormalizedProjectName, oa.NormalizedOrganizationName })
                                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.InArrows)
                                    .WithOne(ia => ia.ToActivity)
                                    .HasPrincipalKey(a => new { a.Name, a.NormalizedProjectName, a.NormalizedOrganizationName })
                                    .HasForeignKey(ia => new { ia.FromActivityName, ia.NormalizedProjectName, ia.NormalizedOrganizationName })
                                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.Resources)
                                    .WithOne(r => r.Activity)
                                    .HasPrincipalKey(a => new { a.Name, a.NormalizedProjectName, a.NormalizedOrganizationName })
                                    .HasForeignKey(r => new { r.ActivityName, r.NormalizedProjectName, r.NormalizedOrganizationName })
                                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Arrow>(entity =>
            {
                entity.HasKey(e => new { e.NormalizedProjectName, e.NormalizedOrganizationName, e.FromActivityName, e.ToActivityName });
                entity.Property(aa => aa.NormalizedOrganizationName).HasMaxLength(128);
                entity.Property(aa => aa.NormalizedProjectName).HasMaxLength(128);
            });

            builder.Entity<Notification>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName);
                entity.Property(n => n.NormalizedUserName).HasMaxLength(128);
            });

            builder.Entity<Invitation>(entity =>
            {
                entity.Property(i => i.NormalizedOrganizationName).HasMaxLength(128);
                entity.Property(i => i.NormalizedUserName).HasMaxLength(128);
            });
        }
    }
}