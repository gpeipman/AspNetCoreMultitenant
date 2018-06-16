using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMultitenant.Web.Data
{
    public class ProductCategory : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
