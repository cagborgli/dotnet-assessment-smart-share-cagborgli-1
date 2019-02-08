using System;
using System.IO;
using CommandLine;
using Core;

namespace Client.Options
{
    [Verb("view", HelpText = "views a file")]
    public class ViewOptions
    {
        [Option('f',"filename", HelpText = "Unique name of file to be downloaded", Required = true)]
        public string FileName { get; set; }

        [Option('p',"password", HelpText = "Password used to access file", Required = true)]
        public string Password { get; set; }

        public static int ExecuteViewAndReturnExitCode(ViewOptions options)
        {
            try
            {
                // Building client request
                var request = new FileRequest
                {
                    Action = Core.Action.view,
                    Filename = options.FileName,
                    Password = options.Password,
                };

                Console.WriteLine($"Attempting to view file: {options.FileName}");
                Api.View(request);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }
    }
}
