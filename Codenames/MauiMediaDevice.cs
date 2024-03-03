namespace Codenames;

public class MediaFileResult
{
    public static MediaFileResult Ok(string pic) => new() { Success = true, PicUrl = pic };
    
    public bool Success { get; set; }
    public string PicUrl { get; set; }
}

public class MauiMediaDevice
{
    public async Task<MediaFileResult> CapturePhotoAsync()
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions { Title = "take a picture" });

            string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using Stream sourceStream = await photo.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(localFilePath);
            await sourceStream.CopyToAsync(localFileStream);
			
            return new MediaFileResult { Success = true, PicUrl = localFilePath };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new MediaFileResult { Success = false };
        }
    }

    public async Task<MediaFileResult> ChooseFromGalleryAsync()
    {
        try
        {
            var fileResult = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "pick a picture" });
            return new MediaFileResult { Success = true, PicUrl = fileResult.FullPath };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new MediaFileResult { Success = false };
        }
    }
}