using ApiVersioning.DbContexts;
using ApiVersioning.Interfaces;
using ApiVersioning.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
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

            // -------------------- DB --------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Constr")));

            var conn = builder.Configuration.GetConnectionString("Constr");
            Console.WriteLine("CONNECTION STRING = " + conn);

            // -------------------- Services --------------------
            builder.Services.AddScoped<IProductService, ProductService>();

            // -------------------- API Versioning (Controllers only) --------------------
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // -------------------- Swagger for Controllers (V1) --------------------
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

            // -------------------- Swagger for FastEndpoints (V2) --------------------
            builder.Services.SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.Title = "My API v2 (FastEndpoints)";
                    s.Version = "v2";
                    s.DocumentName = "v2";
                };
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

            // -------------------- Swagger UI --------------------
          
                // Swashbuckle Swagger for Controllers (v1)
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
                });
            

            // -------------------- Controllers --------------------
            app.MapControllers();

            app.Run();
        }
    }
}
