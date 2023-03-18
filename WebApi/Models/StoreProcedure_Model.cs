using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class StoreProcedure_Model
    {
        [Required]
        public string Database { get; set; } = "";
        [Required]
        public string StoreProcedure_Name { get; set; } = "";
        
        public List<Parameter_Model>? parameter_Models { get; set; }
    }
}
