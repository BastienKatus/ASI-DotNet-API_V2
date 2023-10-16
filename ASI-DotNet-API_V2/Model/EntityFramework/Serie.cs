using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASI_Dotnet_API_V2.Model.EntityFramework
{
    [Table("t_e_serie_ser")]
    public partial class Serie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ser_id")]
        public int SerieId { get; set; }

        [MaxLength(100)]
        [Required]
        [Column("ser_titre")]
        public string Titre { get; set; }

        [Column("ser_resume", TypeName = "TEXT")]
        public string? Resume { get; set; }


        [Column("ser_nbsaisons")]
        public int? NbSaisons { get; set; }


        [Column("ser_nbepisodes")]
        public int? NbEpisodes { get; set; }


        [Column("ser_anneecreation")]
        public int? AnneeCreation { get; set; }

        [MaxLength(50)]
        [Column("ser_network")]
        public string? Network { get; set; }

        [InverseProperty(nameof(Notation.SerieNotee))]
        public  virtual ICollection<Notation> NotesSeries { get; set; }

    }
}
