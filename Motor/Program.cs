using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Motor.Models;
using Motor.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<R4rContext>(options =>
    options.UseNpgsql("Server=containers-us-west-15.railway.app;Database=railway;Port=6613;User Id=postgres;Password=P1uIYcTfSal2qMZqwZzX"));
builder.Services.AddScoped<MotorService, MotorService>();
builder.Services.AddScoped<CategoryService, CategoryService>();
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<CartService, CartService>();
builder.Services.AddScoped<OrderService, OrderService>();
builder.Services.AddScoped<BlogService, BlogService>();
//services cors

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("https://motor.up.railway.app").AllowAnyHeader()
                                                .AllowAnyMethod();
                      });
});


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   /*ValidateLifetime = true,*/
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = "JWTAuthenticationServer",
                   ValidAudience = "JWTServicePostmanClient",
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx")),
                   /* ClockSkew = TimeSpan.Zero*/
               };
           });
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();
app.UseStatusCodePages();

// global cors policy
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin 
    .AllowCredentials());

// Configure
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.Run();
