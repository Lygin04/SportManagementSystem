using Minio;
using Minio.DataModel.Args;
using SportManagementSystem.Modules.Images.Infrastructure;

namespace SportManagementSystem.Extensions;

public static class MinioInitializationExtensions
{
    private const int MaxAttempts = 10;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(2);

    public static async Task InitializeMinioAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var minioClient = scope.ServiceProvider.GetRequiredService<IMinioClient>();
        var options = scope.ServiceProvider.GetRequiredService<MinioOptions>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(MinioInitializationExtensions));

        if (string.IsNullOrWhiteSpace(options.Bucket))
        {
            throw new InvalidOperationException("Minio bucket name is not configured.");
        }

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                var existsArgs = new BucketExistsArgs().WithBucket(options.Bucket);
                var bucketExists = await minioClient.BucketExistsAsync(existsArgs, cancellationToken);
                if (!bucketExists)
                {
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(options.Bucket);
                    await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                    logger.LogInformation("Created MinIO bucket '{Bucket}'.", options.Bucket);
                }

                var policyArgs = new SetPolicyArgs()
                    .WithBucket(options.Bucket)
                    .WithPolicy(BuildAnonymousDownloadPolicy(options.Bucket));
                await minioClient.SetPolicyAsync(policyArgs, cancellationToken);
                logger.LogInformation("Applied anonymous download policy for MinIO bucket '{Bucket}'.", options.Bucket);

                return;
            }
            catch (Exception ex) when (attempt < MaxAttempts)
            {
                logger.LogWarning(
                    ex,
                    "MinIO initialization failed on attempt {Attempt}/{MaxAttempts}. Retrying in {DelaySeconds}s.",
                    attempt,
                    MaxAttempts,
                    RetryDelay.TotalSeconds);
                await Task.Delay(RetryDelay, cancellationToken);
            }
        }

        throw new InvalidOperationException(
            $"Failed to initialize MinIO bucket '{options.Bucket}' after {MaxAttempts} attempts.");
    }

    private static string BuildAnonymousDownloadPolicy(string bucket)
        => $$"""
           {
             "Version":"2012-10-17",
             "Statement":[
               {
                 "Effect":"Allow",
                 "Principal":{"AWS":["*"]},
                 "Action":["s3:GetObject"],
                 "Resource":["arn:aws:s3:::{{bucket}}/*"]
               }
             ]
           }
           """;
}
