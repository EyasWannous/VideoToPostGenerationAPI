using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using VideoToPostGenerationAPI;
using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IServices;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Domain.Settings;
using VideoToPostGenerationAPI.Presistence.Data;
using VideoToPostGenerationAPI.Presistence.Hubs;
using VideoToPostGenerationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(configureOptions =>
{
    configureOptions.LowercaseUrls = true;
});


// Add automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();

// External Http APIS
builder.Services.AddHttpClient();

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxRequestBodySize = FileSettings.MaxFileSizeInBytes; // Increase max request body size (500 MB)
//    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30); // Increase Keep-Alive timeout
//    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30); // Increase request headers timeout
//});


builder.Services.AddScoped<IGenerationService, GenerationService>();
builder.Services.AddScoped<IWhisperService, WhisperService>();
builder.Services.AddScoped<IYouTubeService, YouTubeService>();


// WebSocket
builder.Services.AddSignalR();
builder.Services.AddTransient<ClientWebSocket>();

builder.Services.AddWebSockets(configure =>
{
    configure.KeepAliveInterval = TimeSpan.FromMinutes(5);
    configure.AllowedOrigins.Add("ws://192.168.1.9:8000/api/");
});
builder.Services.AddScoped<IWebSocketClientService, WebSocketClientService>(provider =>
{
    var client = provider.GetRequiredService<ClientWebSocket>();
    return new WebSocketClientService(client);
});

//// Enable CORS for all origins, all headers, and all methods
builder.Services.AddCorsDevelopmentPolicy();

// Task Scheduling
//builder.Services.AddQuartz(options => { });

//builder.Services.AddQuartzHostedService(options =>
//{
//    options.WaitForJobsToComplete = true;
//});

//builder.Services.ConfigureOptions<LoggingBackgroundJobSetup>();
//builder.Services.ConfigureOptions<DeletingFilesBackgroundJobSetup>();

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

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //option.IncludeXmlComments(xmlPath);
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

// Request Body Limit
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = FileSettings.MaxFileSizeInBytes;
});

var app = builder.Build();

app.UseCors("Ahmad");


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())  
//{
app.UseSwagger();
app.UseSwaggerUI();
//}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();

app.MapHub<PostHub>("post-hub");


app.Run();