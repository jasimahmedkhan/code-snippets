namespace design_patterns.AdaptorPattern;

public interface IFileStorage
{
    Task<byte[]> GetAsync(string path);
    Task SaveAsync(string path, byte[]  data);
    Task DeleteAsync(string path);
}

public class AzureBlobAdapter : IFileStorage
{
    private readonly BlobContainerClient _client;
    public AzureBlobAdapter(BlobContainerClient client) => _client = client;

    public async Task<byte[]> GetAsync(string path)
    {
        var blob = _client.GetBlobClient(path);
        var response = await blob.DownloadAsync();
        using var ms = new MemoryStream();
        await response.Value.Content.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task SaveAsync(string path, byte[] data)
        => await _client.GetBlobClient(path).UploadAsync(new BinaryData(data), overwrite: true);

    public async Task DeleteAsync(string path)
        => await _client.GetBlobClient(path).DeleteIfExistsAsync();
}

public class AwsS3Adapter : IFileStorage
{
    private readonly AmazonS3Client _s3;
    private readonly string _bucket;

    public async Task<byte[]> GetAsync(string path)
    {
        var response = await _s3.GetObjectAsync(_bucket, path);
        using var ms = new MemoryStream();
        await response.ResponseStream.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task SaveAsync(string path, byte[] data)
        => await _s3.PutObjectAsync(new PutObjectRequest { BucketName = _bucket, Key = path, InputStream = new MemoryStream(data) });

    public async Task DeleteAsync(string path)
        => await _s3.DeleteObjectAsync(_bucket, path);
}

