namespace WebApi.Models
{
    public class Tokens
    {
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
    }

    public class TokenRes
    {
        public string Token { get; set; }
    
    }

}
