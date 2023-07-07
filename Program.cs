using Quantc.StoriesWebAPI.Common;
using Quantc.StoriesWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddHttpClient<StoryService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(UriSpace.HackerNewsBase);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();
app.UseAuthorization();

app.MapControllers();

app.Run();
