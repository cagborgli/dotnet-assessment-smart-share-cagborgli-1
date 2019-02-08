using Client.Options;
using CommandLine;
using System;
using static Client.Options.DownloadOptions;
using static Client.Options.UploadOptions;
using static Client.Options.ViewOptions;

namespace Client
{
    public class Program
    {
        private static readonly bool developmentMode = true;

        public static void Main(string[] args)
        {
            if (args.Length <= 0 && developmentMode)
            {
                Console.WriteLine("Program Cannot Start");
            }
            RunCommandArgs(args);
        }

        public static void RunCommandArgs(string[] args)
        {
            Parser.Default.ParseArguments<DownloadOptions, UploadOptions, ViewOptions>(args)
                .MapResult((DownloadOptions opts) => ExecuteDownloadAndReturnExitCode(opts),
                (UploadOptions opts) => ExecuteUploadAndReturnExitCode(opts), 
                (ViewOptions opts) => ExecuteViewAndReturnExitCode(opts), errs => 1);
        }
    }
}