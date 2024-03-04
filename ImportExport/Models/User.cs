using ImportExport.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ImportExport.Models
{
    public class User : IImportable  
    {
        [Key]
        public int Id { get; set; } 
        public string username { get; set; } 
        public string lastName { get; set; } 
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public decimal Phone { get; set; } 
        public Address UserAddress { get; set; }
        public bool Status { get; set; }
   
    }
}
