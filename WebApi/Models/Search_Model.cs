using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    [Serializable]
    public class Search_Model
    {
        // public string Token { get; set; }

        [Required]
        public string Database { get; set; } = "";
        [Required]
        public string Table { get; set; } = "";
        [Range(1, 1000)]
        [Required]
        public int Top { get; set; } = 1000;
        public List<Condition_AND_Model>? Condition_AND { get; set; }
        public List<Condition_OR_Model>? Condition_OR { get; set; }
      
    }
}
