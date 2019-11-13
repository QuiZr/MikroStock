using System;
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
            switch (context.Exception)
            {
                case ValidationException e:
                    context.ExceptionHandled = true;
                    context.Result = new BadRequestObjectResult(e.Errors.Select(x => x.ErrorMessage));
                    break;
                case ArgumentException e:
                    context.ExceptionHandled = true;
                    context.Result = new BadRequestObjectResult(e.ParamName + ": " + e.Message);
                    break;
            }
        }
    }
}