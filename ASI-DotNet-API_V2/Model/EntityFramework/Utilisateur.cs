using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ASI_Dotnet_API_V2.Model.EntityFramework
{
    [Table("t_e_utilisateur_utl")]
    public class Utilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Column("utl_nom", TypeName = "varchar(50)")]
        public string? Nom { get; set; }

        [Column("utl_prenom", TypeName = "varchar(50)")]
        public string? Prenom { get; set; }

        [Column("utl_mobile", TypeName = "char(10)")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "…")]
        public string? Mobile { get; set; }

        [Required]
        [Column("utl_mail", TypeName = "varchar(100)")]
        [EmailAddress(ErrorMessage = "Le mail saisie ne semble pas correspondondre aux attentes")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La longueur d’un email doit être comprise entre 6 et 100 caractères.")]
        public string? Mail { get; set; }

        [Required]
        [Column("utl_pwd", TypeName = "varchar(64)")]
        public string? Pwd { get; set; }

        [Column("utl_rue", TypeName = "varchar(200)")]
        public string? Rue { get; set; }

        [Column("utl_cp", TypeName = "char(5)")]
        public string? CodePostal { get; set; }

        [Column("utl_ville", TypeName = "varchar(50)")]
        public string? Ville { get; set; }

        [DefaultValue("France")]
        [Column("utl_pays", TypeName = "varchar(50)")]
        public string? Pays { get; set; }


        [Column("utl_latitude", TypeName = "float")]
        public double? Latitude { get; set; }

        [Column("utl_longitude", TypeName = "float")]
        public double? Longitude { get; set; }

        [Required]
        [Column("utl_datecreation", TypeName = "datetime")]
        public DateTime? DateCreation { get; set; }



       [InverseProperty(nameof(Notation.UtilisateurNotant))]
        public virtual ICollection<Notation> NotesUtilisateurs { get; set; }
    }
}
