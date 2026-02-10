using ApiVersioning.DbContexts;
using ApiVersioning.Interfaces;
using ApiVersioning.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
namespace ApiVersioning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddControllers();
            //// Add FastEndpoints
            //builder.Services.AddFastEndpoints();
            //builder.Services.AddSwaggerDocument(o =>
            //{
            //    o.Title = "My API v2";
            //    o.Version = "v2";
            //    o.DocumentName = "v2";
            //});

            //builder.Services.AddDbContext<AppDbContext>(options =>
            //options.UseSqlServer(
            //    builder.Configuration.GetConnectionString("Constr")));


            //// Add your services
            //builder.Services.AddScoped<IProductService, ProductService>();
            //// API Versioning
            //builder.Services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.ReportApiVersions = true;
            //});

            //// Versioned API Explorer
            //builder.Services.AddVersionedApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "'v'VVV";   // v1, v2
            //    options.SubstituteApiVersionInUrl = true;
            //});

            //// Swagger 
            //builder.Services.AddSwaggerGen(options =>
            //{
            //    // Build provider temporarily (safe at startup)
            //    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            //    foreach (var description in provider.ApiVersionDescriptions)
            //    {
            //        options.SwaggerDoc(
            //            description.GroupName,
            //            new OpenApiInfo
            //            {
            //                Title = "API Versioning Demo",
            //                Version = description.ApiVersion.ToString(),
            //                Description = description.IsDeprecated
            //                    ? "This API version has been deprecated."
            //                    : null
            //            });
            //    }
            //});

            //var app = builder.Build();

            //var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            //app.UseFastEndpoints(c =>
            //{
            //    c.Versioning.Prefix = "api/v";
            //    c.Versioning.PrependToRoute = true;
            //});
            //app.UseOpenApi();


            //app.UseSwagger();

            //app.UseSwaggerUI(options =>
            //{
            //    foreach (var description in provider.ApiVersionDescriptions)
            //    {
            //        options.SwaggerEndpoint(
            //            $"/swagger/{description.GroupName}/swagger.json",
            //            description.GroupName.ToUpperInvariant()
            //        );
            //        //options.SwaggerEndpoint("/ swagger / v2 / swagger.json", "API V2(FastEndpoints)");
            //    }
            //});

            //app.MapControllers();
            //app.Run();
           
            var builder = WebApplication.CreateBuilder(args);

            // -------------------- Controllers (V1) --------------------
            builder.Services.AddControllers();

            // Swagger for Controllers (v1)
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API v1 (Controllers)",
                    Version = "v1"
                });
            });

            // -------------------- FastEndpoints (V2) --------------------
            builder.Services.AddFastEndpoints();

            // Swagger for FastEndpoints (v2) - USE SwaggerDocument() not AddSwaggerDocument()
            builder.Services.SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.Title = "My API v2 (FastEndpoints)";
                    s.Version = "v2";
                    s.DocumentName = "v2";
                };
            });

            // -------------------- DB --------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Constr")));

            // -------------------- Services --------------------
            builder.Services.AddScoped<IProductService, ProductService>();

            // -------------------- API Versioning (Controllers only) --------------------
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            var app = builder.Build();

            // -------------------- Middleware Order (IMPORTANT) --------------------
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // -------------------- FastEndpoints --------------------
            app.UseFastEndpoints(c =>
            {
                c.Versioning.Prefix = "api/v";
                c.Versioning.PrependToRoute = true;
            });

            // -------------------- Swagger --------------------
            app.UseSwagger();      // For both Controllers and FastEndpoints

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1 (Controllers)");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2 (FastEndpoints)");
            });

            // -------------------- Controllers --------------------
            app.MapControllers();

            app.Run();

        }
    }
}
