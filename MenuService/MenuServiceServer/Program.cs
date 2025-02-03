using MenuServiceServer.Extensions;

namespace MenuServiceServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:63049")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //������������ DI
            builder.Services.ConfigureServices(builder.Configuration);

            var app = builder.Build();
            app.UseCors("AllowFrontend");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
                app.UseSwagger();
                app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            Task.WaitAll(app.FillMockDataAsync());

            app.Run();
        }
    }
}
