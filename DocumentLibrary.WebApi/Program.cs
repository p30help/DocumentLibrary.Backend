using DocumentLibrary.Infrastructure.ServiceConfiguration;
using DocumentLibrary.WebApi.Middlewares.ErrorHandling;
using DocumentsLibrary.Application;
using DocumentsLibrary.Application.Common;
using DocumentsLibrary.Application.ServiceConfiguration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IEndpointUrls, EndpointUrls>(provider =>
{
    var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
    var req = httpContextAccessor!.HttpContext!.Request;
    var baseUrl = $"{req.Scheme}://{req.Host}";

    return new EndpointUrls(baseUrl);
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

//builder.Services.AddImageSharp(options =>
//{
//    options.
//});

#region Swagger

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

/*
// https://stackoverflow.com/questions/56234504/migrating-to-swashbuckle-aspnetcore-version-5
// https://pradeeploganathan.com/rest/add-security-requirements-oas3-swagger-netcore3-1-using-swashbuckle/
// https://stackoverflow.com/questions/43447688/setting-up-swagger-asp-net-core-using-the-authorization-headers-bearer
builder.Services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "V1" });

                //show description of enums
                //options.DocumentFilter<EnumDocumentFilter>();

                //for show example value on models
                //options.ExampleFilters();

                //// JWT-token authentication by password
                //c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    Flow = "password",
                //    TokenUrl = SpSettings.Authentication.Authority + "/connect/token",
                //});

                var securityName = "Bearer";
                options.AddSecurityDefinition(securityName, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    //Description = "Please insert User & Password",
                    //In = ParameterLocation.Header,
                    //Name = "Authorization",
                    Flows = new OpenApiOAuthFlows()
                    {
                        Password = new OpenApiOAuthFlow()
                        {
                            TokenUrl = new Uri("https://localhost:7090/api/login"),
                        }
                    },
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = securityName,
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            });

*/

#endregion

#region Cors

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
    //.AllowCredentials();
    //.WithOrigins("http://localhost:4200"); //url of angular localhost for testing
}));

#endregion

var app = builder.Build();

app.EnsureDatabaseMigration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseImageSharp();

app.UseCors("CorsPolicy");

app.UseApiErrorHandlingMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();