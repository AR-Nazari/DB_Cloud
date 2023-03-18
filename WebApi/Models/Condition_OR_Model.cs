using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Condition_OR_Model
    {
      //  [Required]
        public string Field_Name { get; set; } = "";
        //[Required]
        public string OP { get; set; } = "";
       // [Required]
        public string Field_value { get; set; } = "";
       // [Required]
        public string Field_type { get; set; } = "";
                

    }
}
