﻿namespace GotExplorer.BLL.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; }

        public HttpException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpException(int statusCode, string? message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpException(int statusCode, string? message, Exception? exception) : base(message, exception)
        {
            StatusCode = statusCode;
        }
    }
}
