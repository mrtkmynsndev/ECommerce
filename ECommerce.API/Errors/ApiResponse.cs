using System;

namespace ECommerce.API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            this.StatusCode = statusCode;
            this.Message = message ?? GetDefaultMessageFromStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageFromStatusCode(int statusCode)
        {
            // return statusCode switch
            // {
            //     400 => "A bad request be carefull",
            //     401 => "Authorized, you are not",
            //     404 => "Resource found, it was not",
            //     500 => "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change",
            //     _ => null
            // };

            switch (statusCode)
            {
                case 400: return "A bad request be carefull";
                case 401: return "Authorized, you are not";
                case 404: return "Resource found, it was not";
                case 500: return "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change";
                default: return string.Empty;
            }
        }
    }
}