using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly IDapper _dapper;

        public TableController(IDapper dapper)
        {
            _dapper = dapper;
        }

        [Authorize]
        [Route("SearchTable")]
        [HttpPost]
        public async Task<IActionResult> Serach_Table([FromBody] Search_Model search)
        {

            
            List<Condition_AND_Model>? condition_AND = search.Condition_AND;
            List<Condition_OR_Model>? condition_OR = search.Condition_OR;
            List<object> result = new List<object>();

            int Count_Of_Condition_AND = 0;
            string Condition_AND_str = "";
            int Count_Of_Condition_OR = 0;
            string Condition_OR_str = "";
            if (condition_AND is not null)
                Count_Of_Condition_AND = condition_AND.Count;

            if (condition_OR is not null)
                Count_Of_Condition_OR = condition_OR.Count;

            for (int i = 0; i < Count_Of_Condition_AND; i++)
            {

                Condition_AND_str += " AND " + condition_AND[i].Field_Name + condition_AND[i].OP + condition_AND[i].Field_value + " ";
            }
            object[] args = new object[] { search.Top, search.Database, search.Table, Condition_AND_str };//, condition.Field , condition.OP, condition.Field_value};
            string Query = String.Format("Select Top {0} * From {1}.dbo.{2}  where 1=1  {3} ", args);



            try
            {
                result = await Task.FromResult(_dapper.GetAll<object>(search.Database, Query, new DynamicParameters { }, CommandType.Text));

            }
            catch (Exception e)
            {
                result.Add(e.Message);
            }

            return await Task.FromResult(Ok(result));
        }
    }
}
