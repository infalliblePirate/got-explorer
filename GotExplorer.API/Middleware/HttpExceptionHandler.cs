using GotExplorer.BLL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections;

namespace GotExplorer.API.Middleware
{
    public class HttpExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<HttpExceptionHandler> _logger;

        public HttpExceptionHandler(ILogger<HttpExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not HttpException httpException)
            {
                return false;
            }

            var reasonPhrase = ReasonPhrases.GetReasonPhrase(httpException.StatusCode);

            _logger.LogError(
                httpException,
                "Exception occurred: {Message}",
                httpException.Message);

            var problemDetails = new ProblemDetails
            {
                Status = httpException.StatusCode,
                Title = reasonPhrase,
                Detail = httpException.Message,
            };

            foreach (DictionaryEntry entry in httpException.Data)
            {
                problemDetails.Extensions.Add(entry.Key.ToString(), entry.Value);
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
