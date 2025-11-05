using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PrideLink.Server.TransLinkDataBase;

public partial class MasContext : DbContext
{
    public MasContext()
    {
    }

    public MasContext(DbContextOptions<MasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblGeneralConfiguration> TblGeneralConfigurations { get; set; }

    public virtual DbSet<TblGeneralConfigurationType> TblGeneralConfigurationTypes { get; set; }

    public virtual DbSet<TblHobby> TblHobbies { get; set; }

    public virtual DbSet<TblHobbyUserMappingTable> TblHobbyUserMappingTables { get; set; }

    public virtual DbSet<TblRelationshipStatusType> TblRelationshipStatusTypes { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserRoleMappingTable> TblUserRoleMappingTables { get; set; }

    public virtual DbSet<TblUserRoleType> TblUserRoleTypes { get; set; }

    public virtual DbSet<VWUserFriendFinderProfile> VWUserFriendFinderProfile { get; set; }
    public virtual DbSet<VWUserHobbies> VWUserHobbies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:maddisonbailey.database.windows.net,1433;Database=MAS;User ID=Maddi;Password=Cobilove19;Trusted_Connection=False;Encrypt=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VWUserHobbies>(entity =>
        {
            entity.HasNoKey(); // since views don't have a primary key
            entity.ToView("VWUserHobbies"); // name of the SQL view
        });
        modelBuilder.Entity<VWUserFriendFinderProfile>(entity =>
        {
            entity.HasNoKey(); // since views don't have a primary key
            entity.ToView("VWUserFriendFinderProfile"); // name of the SQL view
        });
        modelBuilder.Entity<TblGeneralConfiguration>(entity =>
        {
            entity.HasKey(e => e.GeneralConfigurationNo).HasName("PK__tblGener__ECC2A6D23C21B5C5");

            entity.ToTable("tblGeneralConfiguration", tb => tb.HasTrigger("tblGeneralConfiguration_SystemTrigger"));

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Date1).HasColumnType("datetime");
            entity.Property(e => e.Date2).HasColumnType("datetime");
            entity.Property(e => e.Date3).HasColumnType("datetime");
            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");

            entity.HasOne(d => d.TypeNoNavigation).WithMany(p => p.TblGeneralConfigurations)
                .HasForeignKey(d => d.TypeNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblGenera__TypeN__6D4D2A16");

            entity.HasOne(d => d.UserNoNavigation).WithMany(p => p.TblGeneralConfigurations)
                .HasForeignKey(d => d.UserNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblGenera__UserN__6C5905DD");
        });

        modelBuilder.Entity<TblGeneralConfigurationType>(entity =>
        {
            entity.HasKey(e => e.GeneralConfigurationTypeNo).HasName("PK__tblGener__0ED47BB0FD96A39C");

            entity.ToTable("tblGeneralConfigurationType", tb => tb.HasTrigger("tblGeneralConfigurationType_SystemTrigger"));

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Date1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Date2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Date3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GeneralConfigurationTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Int1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Int2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Int3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ref1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ref2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ref3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");
        });

        modelBuilder.Entity<TblHobby>(entity =>
        {
            entity.HasKey(e => e.HobbyNo).HasName("PK__tblHobby__0ABF2EB47E5A3887");

            entity.ToTable("tblHobby", tb => tb.HasTrigger("tblHobby_SystemTrigger"));

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("active");
            entity.Property(e => e.HobbyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");
        });

        modelBuilder.Entity<TblHobbyUserMappingTable>(entity =>
        {
            entity.HasKey(e => e.HobbyUserMappingTableNo).HasName("PK__tblHobby__E6161764297C4042");

            entity.ToTable("tblHobbyUserMappingTable", tb => tb.HasTrigger("tblHobbyUserMappingTable_SystemTrigger"));

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("active");
            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");

            entity.HasOne(d => d.HobbyNoNavigation).WithMany(p => p.TblHobbyUserMappingTables)
                .HasForeignKey(d => d.HobbyNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblHobbyU__Hobby__1F4E99FE");

            entity.HasOne(d => d.UserNoNavigation).WithMany(p => p.TblHobbyUserMappingTables)
                .HasForeignKey(d => d.UserNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblHobbyU__UserN__1E5A75C5");
        });

        modelBuilder.Entity<TblRelationshipStatusType>(entity =>
        {
            entity.HasKey(e => e.RelationshipStatusTypeNo).HasName("PK__tblRelat__CE29A63BE2818039");

            entity.ToTable("tblRelationshipStatusType", tb => tb.HasTrigger("tblRelationshipStatusType_SystemTrigger"));

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.RelationshipStatusTypeName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserNo).HasName("PK__tblUser__1788955FAB90F39F");

            entity.ToTable("tblUser", tb => tb.HasTrigger("tblUser_SystemTrigger"));

            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("active");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NickName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.SecondName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");
            entity.Property(e => e.UserId)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<TblUserRoleMappingTable>(entity =>
        {
            entity.HasKey(e => e.UserRoleMappingTableNo).HasName("PK__tblUserR__6F933039DCB41C52");

            entity.ToTable("tblUserRoleMappingTable", tb => tb.HasTrigger("tblUserRoleMappingTable_SystemTrigger"));

            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");

            entity.HasOne(d => d.RoleAddedByUserNoNavigation).WithMany(p => p.TblUserRoleMappingTableRoleAddedByUserNoNavigations)
                .HasForeignKey(d => d.RoleAddedByUserNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblUserRo__RoleA__049AA3C2");

            entity.HasOne(d => d.UserNoNavigation).WithMany(p => p.TblUserRoleMappingTableUserNoNavigations)
                .HasForeignKey(d => d.UserNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblUserRo__UserN__02B25B50");

            entity.HasOne(d => d.UserRoleTypeNoNavigation).WithMany(p => p.TblUserRoleMappingTables)
                .HasForeignKey(d => d.UserRoleTypeNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblUserRo__UserR__03A67F89");
        });

        modelBuilder.Entity<TblUserRoleType>(entity =>
        {
            entity.HasKey(e => e.UserRoleTypeNo).HasName("PK__tblUserR__FBFB32367B32387E");

            entity.ToTable("tblUserRoleType", tb => tb.HasTrigger("tblUserRoleType_SystemTrigger"));

            entity.Property(e => e.TSystemCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemCreateDate");
            entity.Property(e => e.TSystemCreateProcedureId)
                .HasDefaultValueSql("(@@procid)")
                .HasColumnName("t_SystemCreateProcedureId");
            entity.Property(e => e.TSystemLastModifiedUserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("t_SystemLastModifiedUserName");
            entity.Property(e => e.TSystemModifiedCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("t_SystemModifiedCount");
            entity.Property(e => e.TSystemModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("t_SystemModifiedDate");
            entity.Property(e => e.UserRoleDescription)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserRoleName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
