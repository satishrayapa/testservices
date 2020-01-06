using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;


namespace TAGov.Common.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly ISwaggerOptions _swaggerOptions;
        private const string _securitySchemeName = "JWT bearer token";
        private readonly Dictionary<string, IEnumerable<string>> _securityRequirements =
            new Dictionary<string, IEnumerable<string>> { { _securitySchemeName, new string[] { } } };

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        /// <param name="swaggerOptions">swaggerOptions.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, ISwaggerOptions swaggerOptions)
        {
            _provider = provider;
            _swaggerOptions = swaggerOptions;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            options.DescribeAllEnumsAsStrings();
            options.AddSecurityDefinition(_securitySchemeName, new ApiKeyScheme
            {
                Name = "Authorization",
                In = "header",
                Type = "apiKey",
                Description = "Format:  \"bearer \\<your_token\\>\""
            });
            options.AddSecurityRequirement(_securityRequirements);

            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = _swaggerOptions.Name,
                Version = description.ApiVersion.ToString(),
                Description = _swaggerOptions.Description
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
