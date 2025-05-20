using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace miniprojet.Models
{
    public class Propriétaire
    {
        [Key]
        public int IdProp { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string Prénom { get; set; }

        public List<Appartement> Appartements { get; set; }
    }
}

