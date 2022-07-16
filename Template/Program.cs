using Microsoft.EntityFrameworkCore;
using Template.Infrastructure;
using Users.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var Configuration = builder.Configuration;

#region service

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddUsersAndAuthentication(Configuration);

services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));


services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});



#endregion

var app = builder.Build();

#region pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion