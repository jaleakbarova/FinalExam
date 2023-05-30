

namespace Bilet14.Utilities.Extensions;

public static class FileExtension
{
    public static bool CheckFileSize(this IFormFile file, int size)
    {
        return file.Length / 1024 > size;
    }

    public static bool CheckFileType(this IFormFile file, string type)
    {
        return file.ContentType.Contains(type + '/');
    }

    public static async Task<string> SaveFileAsync(this IFormFile file, string root, string main)
    {
        string uniqueFile = Guid.NewGuid().ToString() + "_" + file.FileName;

        string path = Path.Combine(root, "assets", main, uniqueFile);

        FileStream stream = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream);

        return uniqueFile;

    }



}