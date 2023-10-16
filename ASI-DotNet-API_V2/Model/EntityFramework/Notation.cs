using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASI_Dotnet_API_V2.Model.EntityFramework
{
    [Table("Notation")]
    public partial class Notation
    {
        [Key]
        [ForeignKey(nameof(Utilisateur))]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Key]
        [ForeignKey(nameof(Serie))]
        [Column("ser_id")]
        public int SerieId { get; set; }

        [Column("not_note")]
        [Range(0, 5, ErrorMessage = "La note doit être comprise entre 0 et 5.")]
        public int Note { get; set; }


       [InverseProperty(nameof(Utilisateur.NotesUtilisateurs))]
        public  Utilisateur UtilisateurNotant { get; set; }

        [InverseProperty(nameof(Serie.NotesSeries))]
        public  Serie SerieNotee { get; set; }
    }
}
