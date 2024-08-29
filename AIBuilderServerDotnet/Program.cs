    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using AIBuilderServerDotnet.Data;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using AIBuilderServerDotnet.Interfaces;
    using AIBuilderServerDotnet.Repository;
    using AIBuilderServerDotnet.Services;


    var builder = WebApplication.CreateBuilder(args);

    // Environmental variables
    var host = Environment.GetEnvironmentVariable("DB_HOST");
    var database = Environment.GetEnvironmentVariable("DB_NAME");
    var username = Environment.GetEnvironmentVariable("DB_USER");
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var port = Environment.GetEnvironmentVariable("DB_PORT");
    var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                    ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
    var kestrelPort = Environment.GetEnvironmentVariable("PORT") ?? "5000"; // Default to 5000 if PORT is not set

    // Updated connection string to allow SSL with self-signed certificates
    var connectionString = $"Host={host};Database={database};Username={username};Password={password};Port={port};SSL Mode=Require;Trust Server Certificate=true";

    // Comment out for localhost
    // Configure Kestrel to listen on the specified port
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(int.Parse(kestrelPort));
    });

    builder.WebHost.UseUrls($"http://localhost:{kestrelPort}");



    // Add services to the container.
    builder.Services.AddControllers();

    // Register ApplicationDbContext with dependency injection
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Register the UserRepository and IUserRepository (SCOPED creates an instance of the service for each request)
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    // Register the ProjectRepository and IProjectRepository (SCOPED creates an instance of the service for each request)
    builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
    
    // Register the WidgetRepository and IWidgetRepository (SCOPED creates an instance of the service for each request)
    builder.Services.AddScoped<IWidgetRepository, WidgetRepository>();

    // Register the PageRepository and IPageRepository (SCOPED creates an instance of the service for each request)
    builder.Services.AddScoped<IPageRepository, PageRepository>();

    // Register ModalRepository and IModalRepository (SCOPED creates an instance of the service for each request)
    builder.Services.AddScoped<IModalRepository, ModalRepository>();

    // Register the AutoMapper to register more profiles automatically <SRP>
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    // Register the JWT secret as a singleton
    builder.Services.AddSingleton(jwtSecret);

    builder.Services.AddSingleton<IJwtService, JwtService>();


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            // Set the clock skew to zero to prevent tokens from being valid for an extra 5 minutes
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
