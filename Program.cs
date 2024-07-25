using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Settings;
using VideoToPostGenerationAPI.Presistence.Data;
using VideoToPostGenerationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(configureOptions =>
{
    configureOptions.LowercaseQueryStrings = true;
});

// Add automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();

// Swagger Authorization
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "VideoToPostGeneration API", Version = "v1" });

    option.AddSecurityDefinition
    (
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );

    option.AddSecurityRequirement
    (
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

// DataBase
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");

builder.Services.Configure<JwtOptions>(jwtOptionsSection);

var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

var signingKey = Encoding.ASCII.GetBytes(jwtOptions!.SigningKey);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(signingKey),
    ValidateIssuer = true,
    ValidIssuer = jwtOptions.ValidIssuer,
    ValidateAudience = true,
    ValidAudience = jwtOptions.ValidAudience,
    RequireExpirationTime = true,
    ValidateLifetime = true,
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameters;

});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
//// Build the different providers you need
//var webRootProvider = new PhysicalFileProvider(builder.Environment.WebRootPath);

//var newPathProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, @"Other"));

//// Create the Composite Provider
//var compositeProvider = new CompositeFileProvider(webRootProvider, newPathProvider);

//// Replace the default provider with the new one
//app.Environment.WebRootFileProvider = compositeProvider;

//app.UseStaticFiles(new StaticFileOptions
//{
//    // Add the other folder, using the Content Root as the base
//    FileProvider = new PhysicalFileProvider(
//    Path.Combine(builder.Environment.ContentRootPath, "OtherFolder"))
//});

app.UseAuthorization();

app.MapControllers();

app.Run();
