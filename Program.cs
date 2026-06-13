using fanaticEdit.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ログの設定
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddEnvironmentVariables(prefix: "SQLCONNSTR_SSSWare_");
var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_SSSWare_ConnectionStrings__DefaultConnection");

builder.Services.AddDbContext<FanaticServeContext>(options =>
    options.UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure()));

// データベース例外フィルター
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
