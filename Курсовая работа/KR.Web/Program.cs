using BlazorDownloadFile;
using Blazored.LocalStorage;
using KR.API.Data;
using KR.Web.Security;
using KR.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddOptions();
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddBlazorDownloadFile();


builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreBaseConncetion"))
);
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<StorageHistoryService>();
builder.Services.AddScoped<PurchaseAgreementService>();
builder.Services.AddScoped<UserPostService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderProductService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<SalaryHistoryService>();
builder.Services.AddScoped<StatsService>();
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<BackupService>();

builder.Services.AddMudServices();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.MapBlazorHub();

app.MapFallbackToPage("/_Host");


app.Run();