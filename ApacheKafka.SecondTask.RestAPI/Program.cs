using ApacheKafka.Common.Factories;
using ApacheKafka.SecondTask.RestAPI.Models;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(provider => ValidatorFactory.CreateSignalValidator());
builder.Services.AddScoped(provider => new MapperConfiguration(config => config.AddProfile<MappingProfile>()).CreateMapper());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
