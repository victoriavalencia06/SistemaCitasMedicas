using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using SistemaCitasMedicas.Infrastructure.Repositories;

// Librer�as para JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Conexi�n a la base de datos
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36)),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure()));

// Inyeccion de dependencias
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<CitaService>();

builder.Services.AddScoped<IDoctorEspecializacionRepository, DoctorEspecializacionRepository>();
builder.Services.AddScoped<DoctorEspecializacionService>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<DoctorService>();

builder.Services.AddScoped<IEspecializacionRepository, EspecializacionRepository>();
builder.Services.AddScoped<EspecializacionService>();

builder.Services.AddScoped<IHistorialMedicoRepository, HistorialMedicoRepository>();
builder.Services.AddScoped<HistorialMedicoServices>();

builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<PacienteService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AuthService>();

// Configuramos CORS
builder.Services.AddCors();

builder.Services.AddControllers();

// Configuramos Swagger para JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese: Bearer {token}",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var key = builder.Configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key no configurada");
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false; // true si usas HTTPS en producci�n
        opt.SaveToken = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,   // desactiva validaci�n de emisor
            ValidateAudience = false, // desactiva validaci�n de audiencia
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Agregamos autorizaci�n
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Activamos CORS globalmente
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
