using Acerola.Domain;
using Acerola.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;
using ApplicationException = Acerola.Application.ApplicationException;

namespace Acerola.WebApi.Filters;

public sealed class DomainExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException domainException)
        {
            string json = JsonConvert.SerializeObject(domainException.Message);

            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        if (context.Exception is ApplicationException applicationException)
        {
            string json = JsonConvert.SerializeObject(applicationException.Message);

            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        if (context.Exception is InfrastructureException infrastructureException)
        {
            string json = JsonConvert.SerializeObject(infrastructureException.Message);

            context.Result = new BadRequestObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}