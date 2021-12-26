using LogiwaApi.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using LogiwaApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LgwDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LogiwaConnection")), ServiceLifetime.Scoped);
//builder.Services.AddMediatR(typeof(StartupBase));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=> options.CustomSchemaIds(type => type.FullName?.Split('.').Last().Replace("+", string.Empty)));
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();
app.UseMiddleware<LgwExceptionHandlerMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
