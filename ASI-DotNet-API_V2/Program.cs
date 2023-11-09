using Microsoft.EntityFrameworkCore;
using ASI_Dotnet_API_V2.Model.EntityFramework;
using ASI_DotNet_API_V2.Model.DataManager;
using ASI_DotNet_API_V2.Model.Repository;

namespace ASI_DotNet_API_V2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ASIDBContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("ASIDBContextSQLite"));
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IDataRepository<Utilisateur>, UtilisateurManager>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}