using Microsoft.Win32.SafeHandles;
using SolarFlareSoftware.Fw1.Core.ServiceInterfaces;
using System.Runtime.InteropServices;

namespace SolarFlareSoftware.Fw1.Services.Core
{
    /// <summary>
    /// NOTE: I commented out all of the code that would be needed if we had to impersonate a user. I had ops grant permission to the service 
    /// accounts under which eSER will be running, as well as to each member of the development team (so that we can run directly from Visual Studio)
    /// </summary>
    public class FileService : IDisposable, IFileService
    {
        //private ILogger<FileService> Logger { get; set; }
        SafeAccessTokenHandle safeAccessTokenHandle = null;
        const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token.   
        const int LOGON32_LOGON_INTERACTIVE = 2;

        //[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        //public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        //    int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        ////public FileService(string un, string domain, string pw, ILogger<FileService> logger)
        //public FileService(ILogger<FileService> logger)
        //{
        //    Logger = logger;
        //}

        //private bool Logon(string un, string domain, string pw)
        //{
        //    return LogonUser(un, domain, pw, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out safeAccessTokenHandle);
        //}

        public bool DeleteFile(string fullPath, string fileName)
        {
            bool deleted = false;
            if (File.Exists(Path.Combine(fullPath, fileName)))
            {
                try
                {
                    File.Delete(Path.Combine(fullPath, fileName));
                    deleted = true;
                }
                catch (Exception ex)
                {
                    deleted = false;
                    int ret = Marshal.GetLastWin32Error();
                    Console.WriteLine("SaveFileToServer failed with error code : {0}", ret);
                    throw new System.ComponentModel.Win32Exception(ret);
                }
            }
            return deleted;
        }

        public bool SaveFile(string fullPath, string fileName, byte[] contents)
        {
            bool saved = false;
            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                using (var fileStream = new FileStream(Path.Combine(fullPath, fileName), FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(contents, 0, contents.Length);
                    saved = true;
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, string.Format("Exception in SaveFileToServer({0}, {1})", fullPath, fileName));
                saved = false;
                int ret = Marshal.GetLastWin32Error();
                Console.WriteLine("SaveFileToServer failed with error code : {0}", ret);
                throw new System.ComponentModel.Win32Exception(ret);
            }
            return saved;
        }

        public byte[] ReadFile(string fullPath, string fileName)
        {
            byte[] data = null;            
            data = File.ReadAllBytes(Path.Combine(fullPath, fileName));
            return data;
        }

        public void Dispose()
        {
            
        }
    }

}
