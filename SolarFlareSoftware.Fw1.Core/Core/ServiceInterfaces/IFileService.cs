namespace SolarFlareSoftware.Fw1.Core.ServiceInterfaces
{
    public interface IFileService
    {
        byte[] ReadFile(string fullPath, string fileName);
        bool DeleteFile(string fullPath, string fileName);
        bool SaveFile(string fullPath, string fileName, byte[] contents);
    }
}