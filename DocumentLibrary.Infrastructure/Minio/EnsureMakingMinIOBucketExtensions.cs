using DocumentLibrary.Domain.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentLibrary.Infrastructure.Minio
{
    public static class EnsureMakingMinIOBucketExtensions
    {
        public static void EnsureMakingMinioBuckets(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var fileRepository = serviceScope.ServiceProvider.GetRequiredService<IFileRepository>();
                fileRepository.TryToCreateDirectories();
            }
        }
    }
}
