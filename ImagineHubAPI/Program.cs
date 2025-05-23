using ImagineHubAPI.Config;
using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Middlewares;
using ImagineHubAPI.Repositories;
using ImagineHubAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<ServiceUrlsConfig>(builder.Configuration.GetSection("ServiceUrls"));
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add Swagger services
builder.Services.AddSwaggerServices();

// EF Core DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<PasswordHasherService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Get service URLs from configuration
var serviceUrls = builder.Configuration.GetSection("ServiceUrls").Get<ServiceUrlsConfig>();

// Log configuration values
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
logger.LogInformation("ServiceUrls configuration:");
logger.LogInformation($"WebUi: {serviceUrls?.WebUi}");
logger.LogInformation($"WebUiNetwork: {serviceUrls?.WebUiNetwork}");
logger.LogInformation($"AiAgent: {serviceUrls?.AiAgent}");

var allowedOrigins = new List<string>();

if (!string.IsNullOrEmpty(serviceUrls?.WebUi))
    allowedOrigins.Add(serviceUrls.WebUi);
if (!string.IsNullOrEmpty(serviceUrls?.WebUiNetwork))
    allowedOrigins.Add(serviceUrls.WebUiNetwork);
allowedOrigins.Add("http://ui:3000"); // Always add Docker container URL

// Configure HttpClient for ImageService
builder.Services.AddHttpClient<IImageService, ImageService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigins.ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(origin => true); // For development only
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

// Enable CORS - must be before other middleware
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (!string.IsNullOrEmpty(serviceUrls?.WebUi))
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", serviceUrls.WebUi);
        if (!string.IsNullOrEmpty(serviceUrls?.WebUiNetwork))
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", serviceUrls.WebUiNetwork);
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
    }
});

app.UseAuthentication();
app.UseMiddleware<UserIdMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();