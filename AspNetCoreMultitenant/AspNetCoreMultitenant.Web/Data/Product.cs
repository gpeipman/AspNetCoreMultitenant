using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMultitenant.Web.Data
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        [Required]
        public ProductCategory Category { get; set; }
    }
}
