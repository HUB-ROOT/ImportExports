using ImportExport.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ImportExport.Models
{
    public class City   
    {
        [Key]
        public decimal Id { get; set; }
        public string CityName { get; set; } 
    }
}
 