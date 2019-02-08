using Core;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var requestSerializer = new XmlSerializer(typeof(FileRequest));
                var responseSerializer = new XmlSerializer(typeof(StatusResponse));
                var viewresponseSerializer = new XmlSerializer(typeof(ViewResponse));

                var port = 3000;
                var address = IPAddress.Parse("127.0.0.1");

                // Start TcpListener
                var listener = new TcpListener(address, port);
                listener.Start();

                // Await connection from client
                Console.WriteLine("Awaiting connection...");

                //keeps server running 
                while (true)
                {
                    var client = listener.AcceptTcpClient();

                    //Run task Concurrently 
                    Task.Run(() =>
                    {
                        Console.WriteLine("Received connection.");

                        using (var stream = client.GetStream())
                        {
                            // Deserialize FileRequest
                            var request = (FileRequest)requestSerializer.Deserialize(stream);
                            bool results;

                            if(request.Action == Core.Action.download)
                            {
                                results = CheckDb(request);
                                responseSerializer.Serialize(stream, new StatusResponse { Success = results });
                            }
                            else if(request.Action == Core.Action.upload) 
                            {
                                results = SaveToDb(request); 
                                responseSerializer.Serialize(stream, new StatusResponse { Success = results });
                            }
                            else
                            {
                                var views = ViewDB(request); // view information from database
                                viewresponseSerializer.Serialize(stream, new ViewResponse { View = views});
                            }
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        private static String ViewDB(FileRequest request)
        {
            File file = MapFileRequestToFileEntity(request);

            using (var context = new SmartShareContext())
            {
                try
                {
                    var Dbfiles = context.Files;

                    var FileView = (from f in Dbfiles
                                    where f.FileName == file.FileName
                                    select f).First();

                    return FileView.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                return "File Does Not Exist";
            }
        }

        private static object ToRequest(File fileView)
        {
            var fRequest = new FileRequest
            {
                Filename = fileView.FileName,
                TimeCreated = fileView.TimeCreated,
                Expiration = fileView.Expiration,
                MaxDownload = fileView.MaxDownload,
                TotalDownload = fileView.TotalDownload
            };

            return fRequest;
        }

        private static bool CheckDb(FileRequest request)
        {
            File file = MapFileRequestToFileEntity(request);

            using (var context = new SmartShareContext())
            {
                try
                {
                    var Dbfiles = context.Files;

                    var findFile = (from f in Dbfiles
                                   where f.FileName == file.FileName
                                   select f).Any();

                    var ifdownloadReached = (from f in Dbfiles
                                           where f.TotalDownload == f.MaxDownload
                                           select f).Any();

                    var isExpired = (from f in Dbfiles
                                           where DateTime.Compare(f.Expiration, DateTime.Now) < 0
                                           select f).Any();

                    var isPassword = (from f in Dbfiles
                                     where f.Password.Equals(file.Password)
                                     select f).Any();

                    if (isExpired || ifdownloadReached )
                    {
                        var todelete =(from f in Dbfiles
                                       where f.FileName.Equals(file.FileName)
                                       select f).First();

                        Dbfiles.Remove(todelete);
                        context.SaveChanges();

                        return false;
                    }

                    if (!isPassword)
                    {
                        return false;
                    }

                    if (findFile)
                    {
                        var increasetotal = (from f in Dbfiles
                                             where f.FileName == file.FileName
                                             select f).FirstOrDefault();

                        increasetotal.TotalDownload++;
                        context.Files.Update(increasetotal);
                        context.SaveChanges();

                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    return false;
                }

                return false;
            }
        }

        public static bool SaveToDb(FileRequest request)
        {
            File file = MapFileRequestToFileEntity(request);

            using (var context = new SmartShareContext())
            {
                try
                {
                    context.Files.Add(file);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine("File Already Exist, Please try a Different Filename to Upload");
                    return false;
                }
            }
        }

        public static File MapFileRequestToFileEntity(FileRequest request)
        {
            File file = new File
            {
                FileName = request.Filename,
                TheFile = request.TheFile,
                Password = request.Password,
                TimeCreated = request.TimeCreated,
                MaxDownload = request.MaxDownload,
                Expiration = request.Expiration,
                TotalDownload = request.TotalDownload
            };

            return file;
        }
    }
}