using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using WebAPIWithAuth.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//add db
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    options.UseNpgsql(
        DataUtil.GetConnectionString(builder.Configuration)
    ));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    //added authentication to swagger
    opts =>
    {
        opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = HeaderNames.Authorization,
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
        });
    });
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(opts => opts.SignIn.RequireConfirmedAccount = false)
                                                                                .AddRoles<IdentityRole>()
                                                                                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddCors(opts => opts.AddDefaultPolicy(policy =>
policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

app.UseCors();
app.MapIdentityApi<IdentityUser>();

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
