namespace NicePartUsageSocialPlatform.Services;

public class FileService : IFileService
{
    public async Task<string> SaveImageFileAsync(Stream imageFile, CancellationToken cancellationToken)
    {
        var imageFileName = $"{Guid.NewGuid()}.png";
        await using var fileStream = new FileStream(Path.Combine("wwwroot", imageFileName), FileMode.Create);
        await imageFile.CopyToAsync(fileStream, cancellationToken);
        return imageFileName;
    }

    public Uri ConvertFileNameToUri(string imageFileName)
    {
        return new Uri($"http://localhost:5193/{imageFileName}");
    }
}