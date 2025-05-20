using System.ComponentModel.DataAnnotations;

namespace miniprojet.Models
{
    public class Appartement
    {
        [Key]
        public int NumApp { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Localite { get; set; }

        [Required(ErrorMessage = "Number of rooms is required")]
        public int NbrPièces { get; set; } // Made non-nullable to match database

        [Required(ErrorMessage = "Value is required")]
        public decimal Valeur { get; set; }

        public int IdProp { get; set; }

        public Propriétaire Propriétaire { get; set; }
    }
}
