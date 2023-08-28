using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using TesteConhecimentos.Data;
using TesteConhecimentos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


var mysqlConnection = builder.Configuration.GetConnectionString("MysqlConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection)));


builder.Services.AddHttpClient("ViaCep", client =>
{
    client.BaseAddress = new Uri("https://viacep.com.br/ws/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MigrationRun();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.MigrationRun();


app.MapRazorPages();

app.Run();