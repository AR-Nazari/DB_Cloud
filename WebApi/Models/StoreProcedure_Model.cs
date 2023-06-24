using System.ComponentModel.DataAnnotations;
using WebApi.Models.Enum;

namespace WebApi.Models
{
    public class StoreProcedure_Model
    {
        [Required]
        public string Database { get; set; } = "";
        [Required]
        public string StoreProcedure_Name { get; set; } = "";
        
        public List<Parameter_Model>? parameter_Models { get; set; }

        //Response Type: 0:Just Execute  1:Return Scalar   2:Return Rows
        public Response_Type Response_Type { get; set; } 
    }
}
