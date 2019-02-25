namespace SmartKitchen.Models
{
    public class Response
    {
        public bool Successfull { get; set; }
        public int Error { get; set; }

        public Response() { }

        public Response(int error)
        {
            Successfull = false;
            Error = error;
        }

        public static Response Success()
        {
            return new Response
            {
                Successfull = true,
                Error = 0
            };
        }
    }
}