using System;
using System.Collections.Generic;
using iRoster.Shared;
using iRosterApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace iRosterApi.Data
{
    public partial class DevContext : DbContext
    {
        public DevContext()
        {
        }

        public DevContext(DbContextOptions<DevContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AltContactInfo> AltContactInfos { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<ContactType> ContactTypes { get; set; } = null!;
        public virtual DbSet<DeviceCode> DeviceCodes { get; set; } = null!;
        public virtual DbSet<Key> Keys { get; set; } = null!;
        public virtual DbSet<OrgAdmin> OrgAdmins { get; set; } = null!;
        public virtual DbSet<OrgGroup> OrgGroups { get; set; } = null!;
        public virtual DbSet<OrgGroupAdmin> OrgGroupAdmins { get; set; } = null!;
        public virtual DbSet<OrgGroupUser> OrgGroupUsers { get; set; } = null!;
        public virtual DbSet<Organisation> Organisations { get; set; } = null!;
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; } = null!;
        public virtual DbSet<Revent> Revents { get; set; } = null!;
        public virtual DbSet<ReventTime> ReventTimes { get; set; } = null!;
        public virtual DbSet<ReventTimeRole> ReventTimeRoles { get; set; } = null!;
        public virtual DbSet<RosterRole> RosterRoles { get; set; } = null!;
        public virtual DbSet<TemplateRevent> TemplateRevents { get; set; } = null!;
        public virtual DbSet<TemplateReventRole> TemplateReventRoles { get; set; } = null!;
        public virtual DbSet<TemplateUserReventTime> TemplateUserReventTimes { get; set; } = null!;
        public virtual DbSet<TimeZoneName> TimeZoneNames { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserReventTime> UserReventTimes { get; set; } = null!;
        public virtual DbSet<VwIncompleteReventTime> VwIncompleteReventTimes { get; set; } = null!;
        public virtual DbSet<VwOrgEventsFuture> VwOrgEventsFutures { get; set; } = null!;
        public virtual DbSet<VwUserRosteredEvent> VwUserRosteredEvents { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=THINKPADX1;Initial Catalog=Dev;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AltContactInfo>(entity =>
            {
                entity.ToTable("AltContactInfo");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.KeepPrivate)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Value)
                    .HasMaxLength(200)
                    .IsFixedLength();

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.AltContactInfos)
                    .HasForeignKey(d => d.ContactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AltContactInfo_ContactType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AltContactInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AltContactInfo_User");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<ContactType>(entity =>
            {
                entity.ToTable("ContactType");

                entity.Property(e => e.ContactType1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ContactType");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<DeviceCode>(entity =>
            {
                entity.HasKey(e => e.UserCode);

                entity.HasIndex(e => e.DeviceCode1, "IX_DeviceCodes_DeviceCode")
                    .IsUnique();

                entity.HasIndex(e => e.Expiration, "IX_DeviceCodes_Expiration");

                entity.Property(e => e.UserCode).HasMaxLength(200);

                entity.Property(e => e.ClientId).HasMaxLength(200);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DeviceCode1)
                    .HasMaxLength(200)
                    .HasColumnName("DeviceCode");

                entity.Property(e => e.SessionId).HasMaxLength(100);

                entity.Property(e => e.SubjectId).HasMaxLength(200);
            });

            modelBuilder.Entity<Key>(entity =>
            {
                entity.HasIndex(e => e.Use, "IX_Keys_Use");

                entity.Property(e => e.Algorithm).HasMaxLength(100);

                entity.Property(e => e.IsX509certificate).HasColumnName("IsX509Certificate");
            });

            modelBuilder.Entity<OrgAdmin>(entity =>
            {
                entity.HasKey(e => new { e.OrganisationId, e.UserId });

                entity.ToTable("OrgAdmin");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<OrgGroup>(entity =>
            {
                entity.ToTable("OrgGroup");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.GroupName).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeZoneId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.OrgGroups)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrgGroup_Organisation");
            });

            modelBuilder.Entity<OrgGroupAdmin>(entity =>
            {
                entity.HasKey(e => new { e.OrgGroupId, e.UserId });

                entity.ToTable("OrgGroupAdmin");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<OrgGroupUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OrgGroupUser");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Organisation>(entity =>
            {
                entity.ToTable("Organisation");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeZone).HasMaxLength(50);
            });

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.HasIndex(e => e.ConsumedTime, "IX_PersistedGrants_ConsumedTime");

                entity.HasIndex(e => e.Expiration, "IX_PersistedGrants_Expiration");

                entity.HasIndex(e => new { e.SubjectId, e.ClientId, e.Type }, "IX_PersistedGrants_SubjectId_ClientId_Type");

                entity.HasIndex(e => new { e.SubjectId, e.SessionId, e.Type }, "IX_PersistedGrants_SubjectId_SessionId_Type");

                entity.Property(e => e.Key).HasMaxLength(200);

                entity.Property(e => e.ClientId).HasMaxLength(200);

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.SessionId).HasMaxLength(100);

                entity.Property(e => e.SubjectId).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<Revent>(entity =>
            {
                entity.ToTable("REvent");

                entity.Property(e => e.ReventId).HasColumnName("REventId");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TemplateReventId).HasColumnName("TemplateREventId");

                entity.Property(e => e.TimeZoneId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.OrgGroup)
                    .WithMany(p => p.Revents)
                    .HasForeignKey(d => d.OrgGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REvent_OrgGroup");
            });

            modelBuilder.Entity<ReventTime>(entity =>
            {
                entity.ToTable("REventTime");

                entity.Property(e => e.ReventTimeId).HasColumnName("REventTimeId");

                entity.Property(e => e.ReventId).HasColumnName("REventId");

                entity.HasOne(d => d.Revent)
                    .WithMany(p => p.ReventTimes)
                    .HasForeignKey(d => d.ReventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REventTime_REvent");
            });

            modelBuilder.Entity<ReventTimeRole>(entity =>
            {
                entity.HasKey(e => new { e.RosterRoleId, e.ReventTimeId });

                entity.ToTable("REventTimeRole");

                entity.Property(e => e.ReventTimeId).HasColumnName("REventTimeId");

                entity.Property(e => e.MinRequired).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ReventTime)
                    .WithMany(p => p.ReventTimeRoles)
                    .HasForeignKey(d => d.ReventTimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REventTimeRole_REventTime");

                entity.HasOne(d => d.RosterRole)
                    .WithMany(p => p.ReventTimeRoles)
                    .HasForeignKey(d => d.RosterRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REventTimeRole_RosterRole");
            });

            modelBuilder.Entity<RosterRole>(entity =>
            {
                entity.HasKey(e => e.RosteredRoleId);

                entity.ToTable("RosterRole");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TemplateRevent>(entity =>
            {
                entity.ToTable("TemplateREvent");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DelimDays)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DelimMonths)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DelimWeeks)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DurationMinutes).HasDefaultValueSql("((1))");

                entity.Property(e => e.EveryN).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeZoneId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.OrgGroup)
                    .WithMany(p => p.TemplateRevents)
                    .HasForeignKey(d => d.OrgGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateREvent_OrgGroup");
            });

            modelBuilder.Entity<TemplateReventRole>(entity =>
            {
                entity.ToTable("TemplateREventRoles");

                entity.Property(e => e.TemplateReventId).HasColumnName("TemplateREventId");

                entity.HasOne(d => d.TemplateRevent)
                    .WithMany(p => p.TemplateReventRoles)
                    .HasForeignKey(d => d.TemplateReventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateREventRoles_TemplateREvent");
            });

            modelBuilder.Entity<TemplateUserReventTime>(entity =>
            {
                entity.ToTable("TemplateUserREventTime");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DayNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TemplateReventId).HasColumnName("TemplateREventId");

                entity.HasOne(d => d.TemplateRevent)
                    .WithMany(p => p.TemplateUserReventTimes)
                    .HasForeignKey(d => d.TemplateReventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TemplateUserREventTime_TemplateREvent");
            });

            modelBuilder.Entity<TimeZoneName>(entity =>
            {
                entity.ToTable("TimeZone");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.SysName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(256);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Surname).HasMaxLength(256);
            });

            modelBuilder.Entity<UserReventTime>(entity =>
            {
                entity.ToTable("UserREventTime");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ReventTimeId).HasColumnName("REventTimeId");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ReventTime)
                    .WithMany(p => p.UserReventTimes)
                    .HasForeignKey(d => d.ReventTimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserREventTime_REventTime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserReventTimes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserREventTime_User");

                entity.HasOne(d => d.R)
                    .WithMany(p => p.UserReventTimes)
                    .HasForeignKey(d => new { d.RoleId, d.ReventTimeId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserREventTime_REventTimeRole");
            });

            modelBuilder.Entity<VwIncompleteReventTime>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwIncompleteREventTimes");

                entity.Property(e => e.Event).HasMaxLength(200);

                entity.Property(e => e.GroupName).HasMaxLength(50);

                entity.Property(e => e.Organisation).HasMaxLength(50);

                entity.Property(e => e.RosterAs).HasMaxLength(50);

                entity.Property(e => e.Timezone).HasMaxLength(500);
            });

            modelBuilder.Entity<VwOrgEventsFuture>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwOrgEventsFuture");

                entity.Property(e => e.CurrentUtcOffset).HasMaxLength(6);

                entity.Property(e => e.Event).HasMaxLength(200);

                entity.Property(e => e.GroupName).HasMaxLength(50);

                entity.Property(e => e.Organisation).HasMaxLength(50);

                entity.Property(e => e.Timezone).HasMaxLength(500);
            });

            modelBuilder.Entity<VwUserRosteredEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwUserRosteredEvents");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Event).HasMaxLength(200);

                entity.Property(e => e.FirstName).HasMaxLength(256);

                entity.Property(e => e.GroupName).HasMaxLength(50);

                entity.Property(e => e.Organisation).HasMaxLength(50);

                entity.Property(e => e.RosterAs).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(256);

                entity.Property(e => e.Timezone).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
