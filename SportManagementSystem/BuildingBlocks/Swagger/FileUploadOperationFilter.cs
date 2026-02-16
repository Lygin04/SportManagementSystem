using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportManagementSystem.BuildingBlocks.Swagger;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.ApiDescription.ParameterDescriptions
            .Where(p => p.Type == typeof(IFormFile) || p.ModelMetadata?.ModelType == typeof(IFormFile))
            .ToList();

        if (fileParams.Count == 0)
        {
            return;
        }

        // Keep path parameters (e.g., {id}) so they show up in Swagger UI.
        operation.Parameters = operation.Parameters
            .Where(p => p.In == ParameterLocation.Path)
            .ToList();

        var schema = new OpenApiSchema
        {
            Type = "object",
            Properties = fileParams.ToDictionary(
                p => p.Name!,
                _ => new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                }),
            Required = fileParams.Select(p => p.Name!).ToHashSet()
        };

        operation.RequestBody = new OpenApiRequestBody
        {
            Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = schema
                }
            }
        };
    }
}
