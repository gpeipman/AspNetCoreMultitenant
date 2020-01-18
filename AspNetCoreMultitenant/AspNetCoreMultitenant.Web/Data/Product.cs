using System;

namespace AspNetCoreMultitenant.Web.Data
{
    public class Product : BaseEntity
    {
        private string _name;
        private string _description;
        private ProductCategory _category;

        public Product()
        {
        }

        public Product(int tenantId, int id, string name, string description)
        {
            TenantId = tenantId;
            Id = id;
            Name = name;
            Description = description;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new Exception(nameof(Name) + " cannot be null or empty string");
                }

                if(value.Length > 50)
                {
                    throw new Exception(nameof(Name) + " cannot be longer than 50 characters");
                }

                _name = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception(nameof(Description) + " cannot be null or empty string");
                }

                if (value.Length > 512)
                {
                    throw new Exception(nameof(Description) + " cannot be longer than 512 characters");
                }

                _description = value;
            }
        }

        public ProductCategory Category
        {
            get { return _category; }
            set
            {
                if(value == null)
                {
                    throw new Exception(nameof(Category) + " cannot be null");
                }

                _category = value;
            }
        }
    }
}
