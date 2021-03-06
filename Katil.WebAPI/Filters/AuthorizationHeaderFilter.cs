﻿using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Katil.WebAPI.Filters
{
    public class AuthorizationHeaderFilter : IOperationFilter
    {
        public AuthorizationHeaderFilter()
        {
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Token",
                    In = "header",
                    Description = "access token",
                    Required = true,
                    Type = "string"
                });
            }
        }
    }
}
