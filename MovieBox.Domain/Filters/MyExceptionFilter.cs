﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBox.Domain.Filters
{
    public class MyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<MyExceptionFilterAttribute> logger;

        public MyExceptionFilterAttribute(ILogger<MyExceptionFilterAttribute> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);

            base.OnException(context);
        }
    }
}
