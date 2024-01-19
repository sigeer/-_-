using DDDDomain.Users;
using DDDEF.Controllers;
using DDDEF.EntityLog;
using DDDEF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDDEF
{
    public class StorageDbContext : DbContext
    {
        int UserId { get; set; }
        public StorageDbContext(IIdentityUserContainer identityUserContainer)
        {
            UserId = identityUserContainer.UserId;
        }

        public StorageDbContext(IIdentityUserContainer identityUserContainer, DbContextOptions<StorageDbContext> options)
            : base(options)
        {
            UserId = identityUserContainer.UserId;
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
            modelBuilder.Entity<DocumentItem>(entity =>
            {
                entity.ToTable("documentitem");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CreationTime).HasMaxLength(6);

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

                entity.Property(e => e.CreationTime).HasMaxLength(6);

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
            var saved = base.SaveChanges();
            AfterSaveChanges();
            return saved;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            var saved = base.SaveChangesAsync(cancellationToken);
            AfterSaveChanges();
            return saved;
        }

        public event EventHandler<IEnumerable<EntityEntry>>? OnBeforeChanges;
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

                if (entry.Entity is IEntity)
                {
                    var tableName = entry.CurrentValues.EntityType.GetTableName();
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        Logs.Enqueue(new EntityLogContext()
                        {
                            OldEntity = entry.State == EntityState.Added ? null : entry.OriginalValues.ToObject(),
                            NewEntity = null,
                            TableName = tableName,
                            Type = entry.State.GetTypeFromEntityState(),
                            Ref = entry
                        });
                    }
                }
            }
        }

        public event EventHandler<IEnumerable<EntityEntry>>? OnAfterChanges;
        protected virtual void AfterSaveChanges()
        {
            while (Logs.TryDequeue(out var log))
            {
                log.NewEntity = log.Ref.Entity;
                if (log.Type == EntityLogContext.TYPE_DELETE)
                    log.NewEntity = null;

                log.TablePrimaryId = log.Ref.CurrentValues.GetValue<int>(nameof(IEntityLog.Id));
                this.GenerateRecord(log.TableName, log.TablePrimaryId, log.Type, log.OldEntity, log.NewEntity, 0);
            }

            OnAfterChanges?.Invoke(this, ChangeTracker.Entries());
        }


        protected virtual void EmitAddEvent(EntityEntry entry)
        {
            EmitEditEvent(entry);
            {
                if (entry.Entity is ICreationTime model)
                {
                    model.CreationTime = DateTime.Now;
                }
            }
        }

        protected virtual void EmitEditEvent(EntityEntry entry)
        {
            {
                if (entry.Entity is IUpdateTime model)
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

        Queue<EntityLogContext> Logs = new Queue<EntityLogContext>();
    }
}
