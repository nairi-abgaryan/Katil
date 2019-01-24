using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Katil.WebAPI.Filters
{
    public class FileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // controller and action name
            if (operation.OperationId == "ApiFileByDisputeGuidPost")
            {
                operation.Consumes.Add("multipart/form-data");

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "OriginalFileName",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "FileType",
                    In = "formData",
                    Description = "File Type",
                    Required = true,
                    Type = "integer"
                });

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "FileName",
                    In = "formData",
                    Description = "File Name",
                    Required = true,
                    Type = "string"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "isInline",
                    In = "formData",
                    Description = "Content Disposition",
                    Required = false,
                    Type = "bool"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "AddedBy",
                    In = "formData",
                    Description = "Added By",
                    Required = false,
                    Type = "int"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "FilePackageId",
                    In = "formData",
                    Description = "FilePackageId",
                    Required = false,
                    Type = "byte"
                });
            }

            if (operation.OperationId == "ApiCommonfilesPost")
            {
                operation.Consumes.Add("multipart/form-data");

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "OriginalFileName",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "FileType",
                    In = "formData",
                    Description = "File Type",
                    Required = true,
                    Type = "integer"
                });

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "FileName",
                    In = "formData",
                    Description = "File Name",
                    Required = true,
                    Type = "string"
                });
            }
        }
    }
}
