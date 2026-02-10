using ApiVersioning.Interfaces;
using ApiVersioning.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ApiVersioning.DbContexts;
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

            //// Controllers (V1)
            builder.Services.AddControllers();

            // FastEndpoints (V2)
            builder.Services.AddFastEndpoints();



            // FastEndpoints Swagger
            builder.Services.AddSwaggerDocument(o =>
            {
                o.Title = "My API v2";
                o.Version = "v2";
                o.DocumentName = "v2";
            });


            // DB
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Constr")));

            // Services
            builder.Services.AddScoped<IProductService, ProductService>();

            // MVC API Versioning (ONLY for controllers)
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            var app = builder.Build();

            // FastEndpoints routing & versioning
            app.UseFastEndpoints(c =>
            {
                c.Versioning.Prefix = "api/v";
                c.Versioning.PrependToRoute = true;
            });
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;

            //    try
            //    {
            //        var dbContext = services.GetRequiredService<AppDbContext>();
            //        dbContext.Database.Migrate();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred while migrating the database.");
            //        throw; // Fail fast if migration fails
            //    }
            //}

            // FastEndpoints Swagger (NOT SwaggerGen)
            app.UseOpenApi();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1 (Controllers)");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2 (FastEndpoints)");
            });

            app.MapControllers();
            app.Run();
        }
    }
}
