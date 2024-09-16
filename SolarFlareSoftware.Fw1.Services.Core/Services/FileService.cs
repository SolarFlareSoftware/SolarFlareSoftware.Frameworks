/*
 * Copyright (C) 2023 Solar Flare Software, Inc.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.[8]
 * 
 */
using Microsoft.Win32.SafeHandles;
using SolarFlareSoftware.Fw1.Core.ServiceInterfaces;
using System;
using System.IO;
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
