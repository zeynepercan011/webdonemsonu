namespace webdonemsonu.Services
{
	public interface IImageService
	{
		Task<byte[]> ConvertImageToByteArrayAsync(IFormFile file);
		string GetImageDisplay(byte[] imageData);
	}
}