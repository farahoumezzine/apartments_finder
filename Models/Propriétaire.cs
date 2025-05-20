using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace miniprojet.Models
{
    public class Propriétaire
    {
        [Key]
        public int IdProp { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string Prénom { get; set; }

        public ICollection<Appartement> Appartements { get; set; }
    }
}

