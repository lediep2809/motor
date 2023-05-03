using Microsoft.OpenApi.Models;
using R4R_API.Models;
using R4R_API.Services;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<R4rContext>(options =>
    options.UseNpgsql("Server=containers-us-west-12.railway.app;Database=railway;Port=7353;User Id=postgres;Password=nwQ6SIdnBq9a3XcVh7IJ"));
builder.Services.AddScoped<RoomsService, RoomsService>();
//services cors

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("https://r4r.up.railway.app/",
                                              "http://r4r.up.railway.app/");
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

//app cors
// Configure
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.Run();