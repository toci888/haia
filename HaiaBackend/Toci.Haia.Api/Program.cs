using Toci.Haia.Api;
using Toci.Haia.ChatGPT;
using Toci.Haia.Database.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
MappingProfile mappingProfile = new MappingProfile();

builder.Services.AddDbContext<ComedyDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IChatGptService, ChatGptService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSignalR();

builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60); // Client wait time before server considers it disconnected
    options.KeepAliveInterval = TimeSpan.FromSeconds(15); // Ping interval to keep the connection alive
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});



var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();
