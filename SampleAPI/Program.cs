using SampleAPI.Infrastructure.Extensions;
using SampleAPI.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => { options.Filters.Add<ApiValidationFilter>(); });
builder.Services.AddDistributedMemoryCache();
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddResponseCaching();
builder.Services.AddAuthentication();
builder.Services.AddLogging(b => b
    .AddFilter("LoggingMiddleware", LogLevel.Debug)
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("Microsoft.Hosting", LogLevel.Warning)
    .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning)
    .AddFilter("System.Net.Http", LogLevel.Warning)
    .AddConsole());



var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(cpb => {
    cpb.AllowAnyOrigin();
    cpb.AllowAnyHeader();
});
app.UseAuthorization();
app.MapControllers();
app.UseSession();
app.Run();
