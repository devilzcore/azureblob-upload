using image_upload.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the connection string for Azure Blob Storage Emulator from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("AzureBlobStorageConnectionString");

// Add the AzureBlobStorageService to the service collection
var blobStorageService = new AzureBlobStorageService(connectionString, "images");
builder.Services.AddSingleton(blobStorageService);

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

app.UseCors(options => options
  .AllowAnyOrigin()
  .AllowAnyHeader()
  .AllowAnyMethod()
);

app.Run();
