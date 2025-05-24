
using ImagineHubAPI.Config;
using ImagineHubAPI.Data;
using ImagineHubAPI.Interfaces;
using ImagineHubAPI.Middlewares;
using ImagineHubAPI.Repositories;
using ImagineHubAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5169");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
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
builder.Services.AddHttpClient<IImageService, ImageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://192.168.1.104:3000",
                "http://78.172.156.103:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Needed if using cookies or authorization headers
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var origin = ctx.Context.Request.Headers["Origin"].ToString();
        
        if (origin == "http://localhost:3000" || 
            origin == "http://192.168.1.104:3000" || 
            origin == "http://78.172.156.103:3000")
        {
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, OPTIONS");
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, Origin");
        }
    }
});

app.UseAuthentication();
app.UseMiddleware<UserIdMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
