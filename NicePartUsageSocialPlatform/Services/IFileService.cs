namespace NicePartUsageSocialPlatform.Services;

public interface IFileService
{
    Task<string> SaveImageFileAsync(Stream imageFile, CancellationToken cancellationToken);
    public Uri ConvertFileNameToUri(string imageFileName);
}