namespace WebApi.Models
{
    public class UserConstants
    {
        public static List<Users> Users = new()
            {
                    new Users(){ Username="root",Password="Aa@",Role="Admin"}
            };
    }
}
