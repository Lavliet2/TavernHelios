using System.Net.Http;
using MenuServiceServer.Extensions;

namespace TavernHelios.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ��������� CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:63049", "http://localhost:3000") // ��������� ��������� ����������
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureServices(builder.Configuration);

            // ����������� HttpClient � ����������� ��������� SSL (�� ��� ����������!)
            //var menuServiceUrl = builder.Configuration.GetSection("ApiSettings:MenuServiceUrl").Value;
            //var menuServiceUrl = Environment.GetEnvironmentVariable("MENU_SERVICE_URL")
            //         ?? builder.Configuration.GetSection("ApiSettings:MenuServiceUrl").Value;

            //if (string.IsNullOrEmpty(menuServiceUrl))
            //{
            //    throw new Exception("MenuServiceUrl �� ����� � ������������!");
            //}

            //builder.Services.AddHttpClient("MenuServiceClient", client =>
            //{
            //    client.BaseAddress = new Uri(menuServiceUrl); // �����: ���� � �����!
            //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //    client.Timeout = TimeSpan.FromSeconds(10);
            //})
            //.ConfigurePrimaryHttpMessageHandler(() =>
            //{
            //    var handler = new HttpClientHandler();
            //    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //    return handler;
            //});



            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
