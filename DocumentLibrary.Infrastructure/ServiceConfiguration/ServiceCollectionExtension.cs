using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Infrastructure.Api;
using DocumentLibrary.Infrastructure.EF;
using DocumentLibrary.Infrastructure.EF.Repositories;
using DocumentLibrary.Infrastructure.Minio;
using DocumentLibrary.Infrastructure.ServiceConfiguration;
using DocumentLibrary.Infrastructure.TempLink;
using DocumentLibrary.Infrastructure.Thumbnail;
using DocumentLibrary.Infrastructure.Token;
using DocumentsLibrary.Application.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Minio;
using System.Text;

namespace DocumentLibrary.Infrastructure.ServiceConfiguration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDocumentsRepository, DocumentsRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddSingleton<ITempCodeGenerator, TempCodeGenerator>();
            services.AddSingleton<IDateProvider, DateProvider>();
            services.AddSingleton<IThumbnailGenerator, ThumbnailGenerator>();
            services.AddEndpointUri();

            var encyptionService = new EncyptionService(configuration["Encryption:Key"]!, configuration["Encryption:IV_Base64"]!);
            services.AddSingleton<IEncyptionService>(encyptionService);

            services.AddEntityFramework(configuration);

            services.AddUserIdentity();

            services.AddJwtAuth(configuration);

            services.AddMinio(configuration);
           
            return services;
        }

        private static IServiceCollection AddEndpointUri(this IServiceCollection services)
        {
            services.AddScoped<IEndpointUris, EndpointUris>(provider =>
            {
                var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                var req = httpContextAccessor!.HttpContext!.Request;
                var baseUrl = $"{req.Scheme}://{req.Host}";

                return new EndpointUris(baseUrl);
            });

            return services;
        }

        private static IServiceCollection AddUserIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddRoles<AppRole>();

            return services;
        }

        private static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = new JwtConfiguration();
            configuration.GetSection("Jwt").Bind(jwtConfiguration);
            services.AddSingleton(jwtConfiguration);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key!))

                };
            });

            return services;
        }

        private static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AppDbContext, AppDbContext>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("SqlServer");
                options.UseSqlServer(connectionString, options =>
                {
                    options.EnableRetryOnFailure();
                    options.MigrationsAssembly("DocumentLibrary.Infrastructure");
                });
            });

            return services;
        }

        private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
        {
            var minioConfiguration = new MinioConfiguration();
            configuration.GetSection("Minio").Bind(minioConfiguration);
            services.AddSingleton(minioConfiguration);

            services.AddMinio(configureClient => configureClient
                    .WithEndpoint(minioConfiguration.ServiceUrl)
                    .WithCredentials(minioConfiguration.AccessKey, minioConfiguration.SecretKey)
                    .WithSSL(minioConfiguration.SslSupport)
                    );

            return services;
        }
    }
}
