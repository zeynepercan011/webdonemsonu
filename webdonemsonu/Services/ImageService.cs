using Microsoft.AspNetCore.Hosting;
using webdonemsonu.Services;

public class ImageService : IImageService
{
	private readonly IWebHostEnvironment _env;

	public ImageService(IWebHostEnvironment env)
	{
		_env = env; //sunucu ortamını temsil eder.
	}

	public async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile file)
	{
		//Yüklenen resmi (IFormFile) byte[] dizisine dönüştürür.
		using var memoryStream = new MemoryStream(); //Geçici bir memorystream oluştu.
		await file.CopyToAsync(memoryStream);
		return memoryStream.ToArray();
	}

	public string GetImageDisplay(byte[] imageData)
	{
		if (imageData == null || imageData.Length == 0)
		{
			return "/images/default-product.png"; // Varsayılan resim yolunu döndür
		}
		return $"data:image/jpeg;base64,{Convert.ToBase64String(imageData)}";
	}
}