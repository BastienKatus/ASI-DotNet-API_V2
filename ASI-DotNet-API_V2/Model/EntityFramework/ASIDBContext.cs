using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;

namespace ASI_Dotnet_API_V2.Model.EntityFramework
{
    public class ASIDBContext :DbContext {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public virtual DbSet<Notation> Notations { get; set; }
        public virtual DbSet<Serie> Series { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }
        public ASIDBContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=ASIDB; uid=postgres; password = postgres; ");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Serie>(entity =>
            {
                entity.ToTable("Serie");
                entity.HasKey(e => new { e.SerieId}).HasName("PK_Serie_ser_id");
                entity.Property(e => e.SerieId)
                    .HasColumnName("ser_id");
                entity.Property(e => e.Titre).IsRequired()
                    .HasColumnName("ser_titre");
                entity.Property(e => e.Resume)
                    .HasColumnName("ser_resume")
                    .HasColumnType("TEXT");
                entity.Property(e => e.NbSaisons)
                    .HasColumnName("ser_nbsaisons");
                entity.Property(e => e.NbEpisodes)
                    .HasColumnName("ser_nbepisodes");
                entity.Property(e => e.AnneeCreation)
                    .HasColumnName("ser_anneecreation");
                entity.Property(e => e.Network)
                    .HasColumnName("ser_network");
                
                entity.HasMany(d => d.NotesSeries)
                    .WithOne(p => p.SerieNotee)
                    .HasForeignKey(d => d.SerieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notation_Serie_ser_id");
            });

            modelBuilder.Entity<Notation>(entity =>
            {
                entity.ToTable("Notation");
                entity.HasKey(e => new { e.UtilisateurId, e.SerieId }).HasName("PK_Notation_utl_id_ser_id");
                entity.Property(e => e.UtilisateurId)
                    .HasColumnName("utl_id");
                entity.Property(e => e.SerieId)
                    .HasColumnName("ser_id");
                entity.Property(e => e.Note)
                    .HasColumnName("not_note")
                    .IsRequired()
                    .HasColumnType("int");
                entity.HasOne(d => d.UtilisateurNotant)
                    .WithMany(p => p.NotesUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notation_Utilisateur_utl_id");
                entity.HasOne(d => d.SerieNotee)
                    .WithMany(p => p.NotesSeries)
                    .HasForeignKey(d => d.SerieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notation_Serie_ser_id");
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.ToTable("Utilisateur");
                entity.HasKey(e => e.UtilisateurId).HasName("PK_Utilisateur_utl_id");
                entity.Property(e => e.UtilisateurId)
                    .HasColumnName("utl_id")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Nom)
                    .HasColumnName("utl_nom")
                    .HasColumnType("varchar(50)");
                entity.Property(e => e.Prenom)
                    .HasColumnName("utl_prenom")
                    .HasColumnType("varchar(50)");
                entity.Property(e => e.Mobile)
                    .HasColumnName("utl_mobile")
                    .HasColumnType("char(10)");
                entity.Property(e => e.Mail)
                    .HasColumnName("utl_mail")
                    .IsRequired()
                    .HasColumnType("varchar(100)");
                entity.Property(e => e.Pwd)
                    .HasColumnName("utl_pwd")
                    .IsRequired()
                    .HasColumnType("varchar(64)");
                entity.Property(e => e.Rue)
                    .HasColumnName("utl_rue")
                    .HasColumnType("varchar(200)");
                entity.Property(e => e.CodePostal)
                    .HasColumnName("utl_cp")
                    .HasColumnType("char(5)");
                entity.Property(e => e.Ville)
                    .HasColumnName("utl_ville")
                    .HasColumnType("varchar(50)");
                entity.Property(e => e.Pays)
                    .HasColumnName("utl_pays")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValue("France");
                entity.Property(e => e.Latitude)
                    .HasColumnName("utl_latitude")
                    .HasColumnType("float");
                entity.Property(e => e.Longitude)
                    .HasColumnName("utl_longitude")
                    .HasColumnType("float");
                entity.Property(e => e.DateCreation)
                    .HasColumnName("utl_datecreation")
                    .HasColumnType("date")
                    .IsRequired();
                entity.HasMany(d => d.NotesUtilisateurs)
                    .WithOne(p => p.UtilisateurNotant)
                    .HasForeignKey(e => e.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notation_Utilisateur_utl_id");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
