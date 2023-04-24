using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace azureblob_upload.Services
{
  public class AzureBlobStorageService
  {
    private readonly CloudBlobContainer _blobContainer;

    public AzureBlobStorageService(string connectionString, string containerName)
    {
      // Parse the connection string and create a reference to the storage account
      CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

      // Create a reference to the blob container
      CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
      _blobContainer = blobClient.GetContainerReference(containerName);
      _blobContainer.CreateIfNotExistsAsync();
    }

    public async Task<string> UploadFileAsync(string fileName, Stream stream)
    {
      // Create a new blob with the specified name
      CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(fileName);

      // Upload the stream to the blob
      await blob.UploadFromStreamAsync(stream);

      // Return the URL of the blob
      return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadFileAsync(string fileName)
    {
      CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(fileName);
      if (!await blob.ExistsAsync())
      {
        return null;
      }

      MemoryStream stream = new MemoryStream();
      await blob.DownloadToStreamAsync(stream);
      stream.Seek(0, SeekOrigin.Begin);

      return stream;
    }
  }
}