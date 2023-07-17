using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebApi.Models;
using Dapper;
using WebApi.Services;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.Enum;

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
        [Route("SP")]
        [HttpPost]
        public async Task<IActionResult> SP(StoreProcedure_Model sp_Model)
        {
            
            object result = new();
            //int Res_EXEC = 0;
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
           
            //{ sp_Model.Response_Type= Response_Type.Row}
            switch (sp_Model.Response_Type)
            {
                case Response_Type.Exec:
                    //Exec
                    result = true;
                    try
                    {
                       // result = await Task.FromResult(_dapper.EXEC(sp_Model.Database, Query, new DynamicParameters { }, CommandType.Text));
                          await Task.FromResult(_dapper.EXEC(sp_Model.Database, Query, new DynamicParameters { }, CommandType.Text));
                       
                        // result.Add(Query);
                    }
                    catch (Exception e)
                    {

                        result = false;
                    }
                    break;
                case Response_Type.Scalar:
                    //Scalar
                    try
                    {
                        result = await Task.FromResult(_dapper.EXECSCALAR(sp_Model.Database, Query, new DynamicParameters { }, CommandType.Text));
                        // result.Add(Query);
                    }
                    catch (Exception e)
                    {

                        result = e.Message;
                    }
                    break;
                case Response_Type.Row:
                    //Row
                    try
                    {
                        result = await Task.FromResult(_dapper.GetAll<object>(sp_Model.Database, Query, new DynamicParameters { }, CommandType.Text));
                        // result.Add(Query);
                    }
                    catch (Exception e)
                    {

                        result=e.Message;
                    }
                    break;
            }

            return Ok(result);

        }
    }
}
