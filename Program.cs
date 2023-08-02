using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Quantc.StoriesWebAPI.Common;
using Quantc.StoriesWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<StoryService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UriSpace.HackerNewsBase);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(setup =>
{
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
    setup.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(2, 0);
    setup.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "First version", Version = "1" });
    setup.SwaggerDoc("v2", new OpenApiInfo { Title = "Improved version", Version = "2" });
});

builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.DocumentTitle = "News WebAPI Swagger UI";
        s.SwaggerEndpoint("v1/swagger.json", "V1 - first version");
        s.SwaggerEndpoint("v2/swagger.json", "V2 - improved version");
    });
}

app.UseHttpsRedirection();

app.UseResponseCaching();
app.UseAuthorization();

app.MapControllers();

app.Run();
