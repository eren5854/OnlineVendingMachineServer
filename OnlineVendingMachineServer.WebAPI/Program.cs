using DotNetEnv;
using ED.GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineVendingMachineServer.WebAPI.Context;
using OnlineVendingMachineServer.WebAPI.Middlewares;
using OnlineVendingMachineServer.WebAPI.Models;
using OnlineVendingMachineServer.WebAPI.Options;
using OnlineVendingMachineServer.WebAPI.Repositories;
using OnlineVendingMachineServer.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.SetIsOriginAllowed(origin => true) // AllowAnyOrigin() YERİNE BUNU YAZIYORUZ
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials(); // SIGNALR İÇİN BU SATIR ŞART
        });
});

Env.Load();
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration["DEFAULT_CONNECTION"];
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

DotNetEnv.Env.Load();

builder.Services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());
builder.Services.AddSignalR();

// SignalR'a kendi UserID okuyucumuzu tanıtıyoruz:
//builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
    options.Lockout.MaxFailedAccessAttempts = 50;
    options.Lockout.AllowedForNewUsers = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<JwtOption>(builder.Configuration.GetSection("Jwt"));
builder.Services.ConfigureOptions<JwtTokenSetupConfiguration>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer();
builder.Services.AddAuthorizationBuilder();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
//builder.Services.AddScoped<IAppUserService, AppUserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
    setup.CustomOperationIds(apiDesc =>
    {
        return $"{apiDesc.ActionDescriptor.RouteValues["action"]}";
    });
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    const int maxRetryCount = 20;
    const int delaySeconds = 10;
    var attempt = 0;

    while (attempt < maxRetryCount)
    {
        try
        {
            Console.WriteLine($"[Database Check] PostgreSQL bağlantısı deneniyor... (Deneme {attempt + 1}/{maxRetryCount})");

            if (dbContext.Database.CanConnect())
            {
                Console.WriteLine("[Database Check] PostgreSQL bağlantısı başarılı!");
                break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Database Check] Bağlantı hatası: {ex.Message}");
        }

        attempt++;
        if (attempt < maxRetryCount)
        {
            Console.WriteLine($"[Database Check] {delaySeconds} saniye bekleniyor, yeniden denenecek...");
            Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
        }
        else
        {
            Console.WriteLine("[Database Check] Maksimum deneme sayısına ulaşıldı. Uygulama kapatılıyor.");
            return; // uygulamayı sonlandırır
        }
    }
}



app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

//ExtensionMiddleware.CreateAdmin(app);

app.UseStaticFiles();

app.UseHttpsRedirection();

// 1. ÖNCE KİMLİK DOĞRULAMAYI (AUTHENTICATION) EKLİYORUZ:
// Bu satır gelen JWT Token'ı okuyup kullanıcının kim olduğunu anlar.
app.UseAuthentication();

// 2. SONRA YETKİLENDİRMEYİ (AUTHORIZATION) EKLİYORUZ:
// Bu satır kimliği belli olan kullanıcının o adrese girmeye yetkisi var mı diye bakar.
app.UseAuthorization();

app.MapControllers();

app.Run();