using System;
using System.ComponentModel.DataAnnotations;
namespace miniprojet.Models
{
    public class Location
    {
        [Key]
        public int IdLoc { get; set; }

        [Required]
        public int NumApp { get; set; }

        [Required(ErrorMessage = "Rental date is required")]
        [DataType(DataType.Date)]
        public DateTime DatLoc { get; set; }

        [Range(1, 120, ErrorMessage = "Number of months must be between 1 and 120")]
        public int NbrMois { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal Montant { get; set; }

        public Locataire Locataire { get; set; }
        public Appartement Appartement { get; set; }
    }
}
