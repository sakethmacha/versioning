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

           
            // In services configuration
            builder.Services.AddFastEndpoints()
                .AddSwaggerDocument(o =>
                {
                    o.Title = "My API v2 (FastEndpoints)";
                    o.Version = "v2";
                    o.DocumentName = "v2";
                });

            // In middleware
           
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
            app.UseOpenApi(); // Generates the OpenAPI JSON
            app.UseSwaggerUi(c =>
            {
                c.ConfigureDefaults(); // For FastEndpoints v2
            });
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
