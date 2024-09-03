//using Microsoft.EntityFrameworkCore;
//using Visa.Checkout;
//using Visa.Checkout.BLL;
//using Visa.Checkout.DAL;
//using Visa.Checkout.DAL.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddScoped<IRuleService, RuleService>();
//builder.Services.AddScoped<IRuleRepository, RuleRepository>();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();



//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Remove or comment out the HTTPS redirection middleware
////app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using Visa.Checkout;
using Visa.Checkout.BLL;
using Visa.Checkout.DAL;
using Visa.Checkout.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000")  // Allow requests from this origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up the Simple Injector container
var container = new Container();
builder.Services.AddSimpleInjector(container, options =>
{
    options.AddAspNetCore()
           .AddControllerActivation();
});



// Register your application's dependencies
builder.Services.AddScoped<IRuleService, RuleService>();
builder.Services.AddScoped<ICheckOutService, CheckOutService>(); 
builder.Services.AddScoped<IRuleRepository, RuleRepository>();

//Check the connectin string is loaded
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Configure Entity Framework and the database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
// Enable CORS
app.UseCors("AllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Explicitly call the correct UseSimpleInjector method
SimpleInjectorUseOptionsAspNetCoreExtensions.UseSimpleInjector(app, container);
container.Verify();

app.Run();
