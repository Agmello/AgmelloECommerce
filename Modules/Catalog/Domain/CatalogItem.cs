using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Catalog.Domain
{
    public class CatalogItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Availability { get; set; } = "Expired";
        public decimal Price { get; set; } = -1.0m;
        public string ImageUrl { get; set; } = null;
        public List<Category> Categories { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();


        public void Update(CatalogItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            Name = item.Name;
            Description = item.Description;
            Availability = item.Availability;
            Price = item.Price;
            ImageUrl = item.ImageUrl;
            Categories.Clear();
            Categories.AddRange(item.Categories);
            Tags.Clear();
            Tags.AddRange(item.Tags);
        }
        public void UpdateField(string fieldName, object value)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new ArgumentException("Field name cannot be empty.", nameof(fieldName));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            switch (fieldName.ToLowerInvariant())
            {
                case nameof(Name):
                    Name = value.ToString() ?? string.Empty;
                    break;
                case nameof(Description):
                    Description = value.ToString() ?? string.Empty;
                    break;
                case nameof(Availability):
                    Availability = value.ToString() ?? "Expired";
                    break;
                case nameof(Price):
                    Price = Convert.ToDecimal(value);
                    break;
                case nameof(ImageUrl):
                    ImageUrl = value.ToString() ?? null;
                    break;
                default:
                    throw new ArgumentException($"Unknown field: {fieldName}", nameof(fieldName));
            }
        }

        [Table("Category")]
        public class Category
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = string.Empty;

            public List<CatalogItem> Items { get; set; } = new();
        }

        [Table("Tag")]
        public class Tag
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = string.Empty;

            public List<CatalogItem> Items { get; set; } = new();
        }
    }
}
