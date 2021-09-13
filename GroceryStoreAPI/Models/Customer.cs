using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Models
{
    public class Customer
    {
        public Customer() { }

        public Customer(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        [JsonProperty("id")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        [Required]
        public int Id { get; set; }

        [JsonProperty("name")]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }
    }
}
