using BlazorDevIta.ERP.BlazorWasm.Server.Configurations;
using BlazorDevIta.ERP.Business.Data;
using BlazorDevIta.ERP.Infrastructure;
using BlazorDevIta.ERP.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//builder.Services.AddDbContext<ERPDbContext>(opt =>
//{
//    opt.UseSqlServer(
//            builder.Configuration.GetConnectionString("DefaultConnection"),
//            //Parte utile per indicare dove si trova il contenxt quando si fa una migration
//            b => b.MigrationsAssembly("BlazorDevIta.ERP.BlazorWasm.Server"));
//});

//Con EF si pu� utilizzare l'"In Memory DB" per esempio per effettuare i test.
builder.Services.AddDbContext<ERPDbContext>(opt =>
{
    opt.UseInMemoryDatabase("BlazorDevItaDB");
});

builder.Services.AddScoped<DbContext, ERPDbContext>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EFRepository<,>));

//AutoMapper DI
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
