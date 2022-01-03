using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CreateCustomer.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string BadRequestErrorTypeUrl = "https://httpstatuses.com/400";
        private const string Title = "One or more validation errors occurred.";

        public static async Task<HttpResponseData> BadRequestAsync(
            this HttpRequestData request,
            string error)
        {
            var response = request.CreateResponse(HttpStatusCode.BadRequest);
            var errorDetails = new
            {
                Type = BadRequestErrorTypeUrl,
                Status = HttpStatusCode.BadRequest,
                Title = Title,
                Detail = error,
                Instance = request.Url.AbsoluteUri
            };
            await response.WriteAsJsonAsync(errorDetails, HttpStatusCode.BadRequest);
            return response;
        }

        public static async Task<HttpResponseData> BadRequestAsync(
            this HttpRequestData request, List<ValidationFailure> errors)
        {
            var response = request.CreateResponse(HttpStatusCode.BadRequest);
            var fieldErrors = errors.GroupBy(e => e.PropertyName).ToDictionary(x =>
                x.Key,
                x => x.Select(m => m.ErrorMessage ?? m.ErrorCode).ToArray());

            var validationProblemDetails = new
            {
                Type = BadRequestErrorTypeUrl,
                Status = HttpStatusCode.BadRequest,
                Title = Title,
                Detail = "Please refer to the errors property for additional details.",
                Instance = request.Url.AbsoluteUri,
                Errors = fieldErrors
            };
            await response.WriteAsJsonAsync(validationProblemDetails, HttpStatusCode.BadRequest);
            return response;
        }

        public static async Task<HttpResponseData> Ok<TResult>(
          this HttpRequestData request,
          TResult result)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
