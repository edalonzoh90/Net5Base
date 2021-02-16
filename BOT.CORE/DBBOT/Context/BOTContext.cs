using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BOT.CORE.DBBOT.Model;

#nullable disable

namespace BOT.CORE.DBBOT.Context
{
    public partial class BOTContext : DbContext
    {
        //public BOTContext()
        //{
        //}

        public BOTContext(DbContextOptions<BOTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Codigo> Codigos { get; set; }
        public virtual DbSet<Notificacion> Notificacions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=serverafloresc.database.windows.net;Database=Asistente400;Persist Security Info=False;User ID=userazure;Password=Shieldteam34;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Codigo>(entity =>
            {
                entity.Property(e => e.Codigo1).HasColumnName("Codigo");

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            });

            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.ToTable("Notificacion");

                entity.Property(e => e.NotificacionId).ValueGeneratedNever();

                entity.Property(e => e.Asunto)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Cabecera).IsUnicode(false);

                entity.Property(e => e.Cuerpo).IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Pie).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
