namespace EduPlatform.Presentation.Helpers
{
    public class ProblemDetail
    {
        public ProblemDetail(int statusCode, string message = null, string detail = null)
        {
            StatusCode = statusCode;
            Message = message;
            Detail = detail; 
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Detail { get; set; }
    }
}
