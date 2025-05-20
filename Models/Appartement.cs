using System.ComponentModel.DataAnnotations;

namespace miniprojet.Models
{
    public class Appartement
    {
        [Key]
        public int NumApp { get; set; }

        [Required]
        public int IdProp { get; set; }

        [Required(ErrorMessage = "Locality is required")]
        [StringLength(100)]
        public string Localite { get; set; }

        [Range(1, 10, ErrorMessage = "Number of rooms must be between 1 and 10")]
        public int NbrPièces { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Value must be positive")]
        public decimal Valeur { get; set; }

        public Propriétaire Propriétaire { get; set; }
    }
}
