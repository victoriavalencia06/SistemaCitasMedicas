using Microsoft.EntityFrameworkCore;
using SistemaCitasMedicas.Application.Services;
using SistemaCitasMedicas.Domain.Repositories;
using SistemaCitasMedicas.Infrastructure.Data;
using SistemaCitasMedicas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 36)),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure()));
// Inyectar otros servicios, repositorios, etc.
builder.Services.AddScoped<ICitaRepository, CitaRepository>(); // tu implementación real
builder.Services.AddScoped<CitaService>();
builder.Services.AddScoped<IDoctorEspecializacionRepository, DoctorEspecializacionRepository>(); // tu implementación real
builder.Services.AddScoped<DoctorEspecializacionService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>(); // tu implementación real
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<IEspecializacionRepository, EspecializacionRepository>(); // tu implementación real
builder.Services.AddScoped<EspecializacionService>();
builder.Services.AddScoped<IHistorialMedicoRepository, HistorialMedicoRepository>(); // tu implementación real
builder.Services.AddScoped<HistorialMedicoServices>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>(); // tu implementación real
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<IRolRepository, RolRepository>(); // tu implementación real
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); // tu implementación real
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
