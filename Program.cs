using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SimpleChatApi.Data;
using SimpleChatApi.Hubs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ChatContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();
// configure swagger
builder.Services.AddSwaggerGen(swaggerBuilder =>
{
  swaggerBuilder.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "SimpleChatAPI",
    Description = "Simple Char API for managing chats",
  });  
  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  swaggerBuilder.EnableAnnotations();
  swaggerBuilder.IncludeXmlComments(xmlPath);

  // configure swagger for SignalR
  swaggerBuilder.AddSignalRSwaggerGen(signalRSwaggerBuilder =>
  {
    signalRSwaggerBuilder.UseHubXmlCommentsSummaryAsTagDescription = true;
    signalRSwaggerBuilder.UseHubXmlCommentsSummaryAsTag = true;
    signalRSwaggerBuilder.UseXmlComments(xmlPath);
  });

});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseWebSockets();
app.MapHub<ChatHub>("/chat");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }