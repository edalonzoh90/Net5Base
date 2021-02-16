namespace BOT.DATA.Helper
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
    }

    public class ApiRequest
    {
        public string Uri { get; set; }
        public string Method { get; set; }
        public string ContentType { get; set; }
        public int? TimeOut { get; set; }
        public string SData { get; set; }
    }

    public class Token
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
    }
}
