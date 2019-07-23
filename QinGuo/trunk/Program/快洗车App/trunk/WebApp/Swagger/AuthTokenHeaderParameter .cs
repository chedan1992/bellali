using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace WebApp
{
    public class AuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.parameters = operation.parameters ?? new List<Parameter>();
            //MemberAuthorizeAttribute 自定义的身份验证特性标记
            var isAuthor = operation != null;
            if (isAuthor)
            {
                //in query header 
                operation.parameters.Add(new Parameter()
                {
                    name = "Authorization",
                    @in = "header", //query formData ..
                    description = "身份验证",
                    required = false,
                    type = "string"
                });
            }
        }
    }
}