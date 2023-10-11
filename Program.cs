using System.Text;
using ApiRezolveHotel.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReviseDotnet;
using ReviseDotnet.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
            Title = "Dotnet Api",
            Description = @"
    Api always send a custom response with 200 status code. Actual response can get from response.status.
    Interface of custom response in Typescript

    export interface ApiResponse<T> {
      /**
       * status code of response
       */
      status: number;
      /**
       * response data of server
       */
      data: T;
      /**
       * Message in case of success and error
       * Error message can be multiple in case of validation of form
       */
      message: string[];
      /**
       * Error code helps in logging and also helps in multi language to show message
       * based on error code
       * @default -1
       */
      errorCode: number | string;
    }
    
    Schema of Response Body is T"
        });
    });

builder.Services.AddDbContext<SqlLiteDbContext>();

// [TODO] Encrypt appsettings json with sops
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        // Get Issuer, Audience and security key from appsettings.json
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DbCreate.CreateDatabase(app);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCustomResponse();


app.Run();
