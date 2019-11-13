using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MikroStok.API
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException e)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(e.Errors.Select(x => x.ErrorMessage));
            }
        }
    }
}