using DDDDomain.Shared.EntityProperty;
using DDDEF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDDEF
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext()
        {
        }

        public StorageDbContext(DbContextOptions<StorageDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DocumentItem> DocumentItems { get; set; } = null!;
        public virtual DbSet<DocumentStore> DocumentStores { get; set; } = null!;

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<RoleBase> RoleBase { get; set; } = null!;
        public virtual DbSet<PageBase> PageBases { get; set; } = null!;
        /* 详细权限，细化到每个api */
        public virtual DbSet<PermissionBase> PermissionBases { get; set; } = null!;
        public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;
        /* 详细权限，细化到每个api */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<DocumentItem>(entity =>
            {
                entity.ToTable("documentitem");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreateTime).HasMaxLength(6);

                entity.Property(e => e.Description)
                    .HasMaxLength(200);

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(50);

                entity.Property(e => e.DocStoreId).HasColumnName("DocStoreId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50);

                entity.Property(e => e.Path)
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<DocumentStore>(entity =>
            {
                entity.ToTable("documentstore");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.TypeId).HasColumnName("TypeId");

                entity.Property(e => e.CreateTime).HasMaxLength(6);

                entity.Property(e => e.Description)
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Email)
                    .HasMaxLength(200);

                entity.Property(e => e.LoginId)
                    .HasMaxLength(50)
                    .HasColumnName("LoginId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(200);

                entity.Property(e => e.Status)
                    .HasColumnName("Status");

                entity.Property(e => e.Type)
                    .HasColumnName("Type");

                entity.Property(e => e.Avatar).HasMaxLength(500);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50);

                entity.Property(e => e.RegisterTime).HasMaxLength(6);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("userrole");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.RoleId).HasColumnName("RoleId");

                entity.Property(e => e.UserId).HasColumnName("UserId");
            });

            modelBuilder.Entity<RoleBase>(entity =>
            {
                entity.ToTable("rolebase");

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<PermissionBase>(entity =>
            {
                entity.ToTable("permissionbase");

                entity.Property(e => e.Id);
                entity.Property(e => e.Key).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(200);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("rolepermission");

                entity.Property(e => e.Id);
            });

            modelBuilder.Entity<PageBase>(entity =>
            {
                entity.ToTable("pagebase");

                entity.Property(e => e.Id);

                entity.Property(e => e.Url).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Icon).HasMaxLength(20);

                entity.Property(e => e.Key).HasMaxLength(20);
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void BeforeSaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        EmitAddEvent(entry);
                        break;
                    case EntityState.Modified:
                        EmitEditEvent(entry);
                        break;
                    case EntityState.Deleted:
                        EmitDeleteEvent(entry);
                        break;
                }
            }
        }


        protected virtual void EmitAddEvent(EntityEntry entry)
        {
            EmitEditEvent(entry);
        }

        protected virtual void EmitEditEvent(EntityEntry entry)
        {
            {
                if (entry.Entity is IAuditEditTime model)
                {
                    model.LastUpdateTime = DateTime.Now;
                }
            }
        }

        protected virtual void EmitDeleteEvent(EntityEntry entry)
        {
            {
                if (entry.Entity is ISoftDelete model)
                {
                    model.IsDeleted = true;
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}
