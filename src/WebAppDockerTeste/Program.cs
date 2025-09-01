
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using WebAppDockerTeste.Data;

namespace WebAppDockerTeste
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // EF Core + SQL Server
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {   //Docker Compose
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlServerOptionsAction: sqlOptions =>
                {
                    // Ensina a API a tentar novamente a ligação em caso de falha.
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(40), // Não usa 40s para única tentativa mas sim tentativas que escalam progressivamente até 40s
                        errorNumbersToAdd: null); // Usa a lista padrão de erros de rede transitórios
                });
            });


            //builder.Services.AddDbContext<AppDbContext>(opt =>
            //    opt.UseSqlServer(builder.Configuration.GetConnectionString("ContainerSqlIndependente")));//IIS Express

            // Add services to the container.

            builder.Services.AddHealthChecks();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, token) =>
                {
                    // Limpa quaisquer servidores que possam ter sido adicionados por defeito
                    document.Servers.Clear();
                    // Adiciona o nosso servidor correto, lido do .env
                    document.Servers.Add(new OpenApiServer
                    {
                        Url = builder.Configuration["PUBLIC_API_URL"],
                        Description = "Public API Server"
                    });
                    return Task.CompletedTask;
                });
            });

            //builder.Services.AddOpenApi();// padrão v1.json


            var app = builder.Build();

            app.UseForwardedHeaders();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();            // => /openapi/v1.json
                app.MapScalarApiReference(); // => /scalar/v1
            }

            // Isto garante que a base de dados é criada e as migrações são aplicadas
            // sempre que a aplicação arranca, seja em Development ou Production.
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.MapHealthChecks("/healthz");
            app.Run();
        }
    }
}
