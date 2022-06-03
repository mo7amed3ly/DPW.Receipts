using WebApi.Authorization;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddCors();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddScoped<IUserService, UserService>();


var app = builder.Build();
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

//app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseMiddleware<BasicAuthMiddleware>();
app.MapControllers();

app.Run();
