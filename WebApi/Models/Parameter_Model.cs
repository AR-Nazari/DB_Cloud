using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Parameter_Model
    {
        [Required]
        public string Parameter_Name { get; set; } = "";

        [Required]
        public string Parameter_Datatype { get; set; } = "";
        [Required]
        public string Parameter_Value { get; set; } = "";
    }
}
