using ConcertStats.Application;
using ConcertStats.Application.Configuration;
using ConcertStats.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        // todo configure JWT authentication
        
        // options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        // {
        //     ValidateIssuer = true,
        //     ValidateAudience = true,
        //     ValidateLifetime = true,
        //     ValidateIssuerSigningKey = true,
        //     ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //     ValidAudience = builder.Configuration["Jwt:Audience"],
        //     IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
        //         System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        // };
    });

builder.Services.Configure<EncryptionConfig>(
    builder.Configuration.GetSection("EncryptionConfig:Password"));

// database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection not found");

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("https://localhost:4200"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();