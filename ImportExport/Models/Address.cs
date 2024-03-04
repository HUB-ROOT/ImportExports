using ImportExport.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ImportExport.Models
{
    public class Address : IImportable
    {
        [Key]
        public int id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
