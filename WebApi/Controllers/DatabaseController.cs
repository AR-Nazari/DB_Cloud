using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebApi.Models;
using Dapper;
using WebApi.Services;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IDapper _dapper;

        public DatabaseController(IDapper dapper)
        {
            _dapper = dapper;
        }

        [Authorize]
        [Route("SearchTable")]
        [HttpPost]
        public async Task<IActionResult> Serach_Table([FromBody] Search_Model search)
        {

            // Condition_Model condition = new Condition_Model();
            List<Condition_AND_Model> condition_AND = search.Condition_AND;
            List<Condition_OR_Model> condition_OR = search.Condition_OR;
            List<object> result = new List<object>();

            int Count_Of_Condition_AND = condition_AND.Count;
            string Condition_AND_str = "";

            int Count_Of_Condition_OR = condition_OR.Count;
            string Condition_OR_str = "";

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


        [Authorize]
        [Route("SP")]
        [HttpPost]
        public async Task<IActionResult> SP(StoreProcedure_Model sp_Model)
        {

            List<object> result = new List<object>();
            List<Parameter_Model> param = sp_Model.parameter_Models;

            
            int Count_Of_Parameter = param != null ? param.Count : 0;
            string Parameters_str = "";
            string Parameter_dataType_prefix = "";
            string Parameter_dataType_postfix = "";
            string Between_Parameters = ",";
            //int count_Param = para
            for (int i = 0; i < Count_Of_Parameter; i++)
            {
                if (i == Count_Of_Parameter - 1)
                    Between_Parameters = "";
                switch (param[i].Parameter_Datatype)
                {
                    case "int":
                        Parameter_dataType_prefix = "";
                        Parameter_dataType_postfix = "";
                        break;
                    case "string":
                        Parameter_dataType_prefix = "N'";
                        Parameter_dataType_postfix = "'";
                        break;
                }


                Parameters_str += "  @" + param[i].Parameter_Name + " = " + Parameter_dataType_prefix + param[i].Parameter_Value + Parameter_dataType_postfix + Between_Parameters;
                // Parameters_str = "@State_Name__Text = N'ه' ";
            }

            object[] args = new object[] { sp_Model.Database, sp_Model.StoreProcedure_Name, Parameters_str };
            string Query = String.Format("EXEC {0}.[dbo].{1} {2} ", args);



            try
            {
                result = await Task.FromResult(_dapper.GetAll<object>(sp_Model.Database, Query, new DynamicParameters { }, CommandType.Text));
                // result.Add(Query);
            }
            catch (Exception e)
            {

                result.Add(e.Message);
            }

            return await Task.FromResult(Ok(result));

        }
    }
}
