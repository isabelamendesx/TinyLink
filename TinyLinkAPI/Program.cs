
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TinyLink.Domain.Entities;
using TinyLink.Domain.Interfaces;
using TinyLink.Infra.Data.Context;
using TinyLink.Infra.Data.Repository;
using TinyLink.Service.Services;
using TinyLink.Service.Validators;

namespace TinyLinkAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IRandomizer, Randomizer>();
            builder.Services.AddScoped<IRepository<Url>, Repository<Url>>();
            builder.Services.AddScoped<IUrlRepository, UrlRepository>();
            builder.Services.AddScoped<IUrlValidator, UrlValidator>();
            builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();
            builder.Services.AddScoped<ILogger, Logger<UrlRepository>>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TinyUrlContext>(options => options.UseSqlite(connStr));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapFallback(async (IUrlShortenerService service, HttpContext ctx) =>
            {
                var shortUrl = ctx.Request.Path.ToUriComponent().Trim('/');
                var urlMatch = service.MatchShortUrl(shortUrl);

                if (urlMatch == null)
                {
                    return Results.BadRequest("Invalid short url");
                }

                return Results.Redirect(urlMatch.Destination);
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
