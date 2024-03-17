using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using LimiteTransacaoPix.DataBase;
using LimiteTransacaoPix.Models.ConvertToString;
using LimiteTransacaoPix.Repository;
using LimiteTransacaoPix.Repository.Interface;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IGestaoLimiteRepository, GestaoLimiteRepository>();
builder.Services.AddScoped<ITransacoesRepository, TransacoesRepository>();
builder.Services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();

builder.Services.AddSingleton<AmazonDynamoDBClient>(sp =>
{
    var localCredentials = new BasicAWSCredentials("local", "local");
    var config = new AmazonDynamoDBConfig
    {
        ServiceURL = "http://host.docker.internal:4566",
    };
    return new AmazonDynamoDBClient(localCredentials, config);
});

builder.Services.AddSingleton<DataBase>();

var app = builder.Build();

var dataBase = app.Services.GetService<DataBase>();
await dataBase.CreateDynamoTableAsync();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{cpf?}");

app.Run();
