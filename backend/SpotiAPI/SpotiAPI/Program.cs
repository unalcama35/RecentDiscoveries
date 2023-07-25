global using SpotiAPI.Data;
global using Microsoft.EntityFrameworkCore;
using SpotiAPI.Services;
using Microsoft.Extensions.Caching.Memory;

namespace SpotiAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<SongService>();
            builder.Services.AddScoped <SpotifyService>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddMemoryCache();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // Swaggerin ne gibi bir faydasi var.
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); //CORSun diger hallerini arastir, ne tipte kisitlamalar yapabiliyorum.



            app.MapControllers();

            app.Run();
        }
    }
}