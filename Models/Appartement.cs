using System.ComponentModel.DataAnnotations;

namespace miniprojet.Models
{
    public class Appartement
    {
        [Key]
        public int NumApp { get; set; }

        [Required]
        public string Localite { get; set; }

        [Required]
        public int NbrPièces { get; set; }

        [Required]
        public decimal Valeur { get; set; }

        public int? IdProp { get; set; }

        public virtual Propriétaire Propriétaire { get; set; }

        public string ImagePath { get; set; }
    }
}