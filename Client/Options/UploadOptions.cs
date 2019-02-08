using System;
using System.IO;
using Client.Utils;
using CommandLine;
using Core;

namespace Client.Options
{
    [Verb("upload", HelpText = "Uploads a file")]
    public class UploadOptions
    {
        [Option('f',"filename", HelpText = "The file name", Required = true)]
        public string FileName { get; set; }

        [Option('p',"password", HelpText = "Password for the file", Required = false)]
        public string Password { get; set; } = PasswordGenerator.Generate();

        [Option('e',"expiration", HelpText = "When file expires in hrs", Required = false)]
        public DateTime Expiration { get; set; } = DateTime.Now.AddSeconds(86400);

        [Option('m', "maxdownloads", HelpText = "Maximun number of downloads", Required = false)]
        public int MaxDownloads { get; set; } = 1000;
      
        public static int ExecuteUploadAndReturnExitCode(UploadOptions options)
        {
            
            try
            {
                // Fetch data from FileStream and put into byte array
                using (FileStream fileSource = new FileStream(options.FileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fileSource.Length];
                    int n = fileSource.Read(bytes);

                    // Building client uploadrequest
                    var request = new FileRequest
                    {
                        Action = Core.Action.upload,
                        Filename = options.FileName,
                        TheFile = bytes,
                        Password = options.Password,
                        TimeCreated = DateTime.Now,
                        MaxDownload = options.MaxDownloads,
                        Expiration = options.Expiration,
                        TotalDownload = 0,
                    };

                    var file = new FileInfo(options.FileName);
                    Console.WriteLine($"Uploading {file.FullName}");
                    Console.WriteLine($"Password: {options.Password}");
                    Console.WriteLine($"Maxinmum Download: {options.MaxDownloads}");

                    Api.Upload(request);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }
    }
}