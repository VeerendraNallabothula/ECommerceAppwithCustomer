namespace ECommerceApp.DTOs
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }

        public bool Sucess { get; set; }

        public T Data { get; set; }

        public List<string> Errors { get; set; }

        public ApiResponse()
        {
            Sucess = true;
            Errors = new List<string>();
        }
        public ApiResponse(int statusCode, List<string> errors)
        {
            StatusCode = statusCode;
            Sucess = false;
            Errors = errors;
        }
        public ApiResponse(int statusCode, string error)
        {
            StatusCode = statusCode;
            Sucess = false;
            Errors = new List<string> { error };
        }
        public ApiResponse(int statusCode, T data)
        {
            StatusCode = statusCode;
            Sucess = true;
            Data = data;
            Errors = new List<string>();
        }
    }
}
