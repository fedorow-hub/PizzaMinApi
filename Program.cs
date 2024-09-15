using Resend;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = "re_cwb8f7nu_KkiY31WKtYvjNepxRevPJQjp";// TODO перенести в конфиг
});
builder.Services.AddTransient<IResend, ResendClient>();

RegisterServices(builder.Services);

var app = builder.Build();

Configure(app);

var apis = app.Services.GetServices<IApi>();

foreach (var api in apis)
{
    if (api is null) throw new InvalidProgramException("Api not found");
    api.Register(app);
}

SeedData.EnsurePopulated(app);

app.Run();


void RegisterServices(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddDbContext<PizzaDb>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    });

    services.AddScoped<IPizzaRepository, PizzaRepository>();
    services.AddSingleton<ITokenService>(new TokenService());
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddAuthorization();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    services.AddTransient<IApi, ProductApi>();
    services.AddTransient<IApi, IngredientApi>();
    services.AddTransient<IApi, AuthApi>();
    services.AddTransient<IApi, CategoryApi>();
    services.AddTransient<IApi, CartApi>();
    services.AddTransient<IApi, OrderApi>();
    services.AddTransient<IApi, ProfileApi>();
    services.AddTransient<IApi, StoryApi>();

    services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy =>
                        {
                            policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        });
    });
}


void Configure(WebApplication app)
{
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseCors(MyAllowSpecificOrigins);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PizzaDb>();
        db.Database.EnsureCreated();
    }
}

