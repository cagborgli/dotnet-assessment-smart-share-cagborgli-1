using Core;
using System;
using System.IO;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace Client
{
    public class Api
    {
        private const string HOST = "127.0.0.1";
        private const int PORT = 3000;

        private Api()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Send download request
        /// </summary>
        /// <param name="">TODO</param>
        /// <returns>true if request was successful and false if unsuccessful</returns>
        public static bool Download(FileRequest request)
        {
            try
            {
                var client = new TcpClient(HOST, PORT);
                var requestSerializer = new XmlSerializer(typeof(FileRequest));
                var responseSerializer = new XmlSerializer(typeof(StatusResponse));

                using (var stream = client.GetStream())
                {
                    // Serialize request to server
                    requestSerializer.Serialize(stream, request);

                    //Hacky Read Blocking 
                    client.Client.Shutdown(SocketShutdown.Send);

                    var statusResponse = (StatusResponse)responseSerializer.Deserialize(stream);
                    Console.WriteLine(statusResponse.Success ? "Download Was Successful" : "Download Failed");
                }

                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static void View(FileRequest request)
        {
            try
            {
                var client = new TcpClient(HOST, PORT);
                var requestSerializer = new XmlSerializer(typeof(FileRequest));
                var viewresponseSerializer = new XmlSerializer(typeof(ViewResponse));

                using (var stream = client.GetStream())
                {
                    // Serialize request to server
                    requestSerializer.Serialize(stream, request);

                    //Hacky Read Blocking 
                    client.Client.Shutdown(SocketShutdown.Send);

                    //Receiving Response From Server
                    var viewResponse = (ViewResponse)viewresponseSerializer.Deserialize(stream);
                    Console.WriteLine(viewResponse.View);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Send upload request
        /// </summary>
        /// <param name="">TODO</param>
        /// <returns>true if request was successful and false if unsuccessful</returns>
        public static bool Upload(FileRequest request)
        {
            try
            {
                var client = new TcpClient(HOST, PORT);
                var requestSerializer = new XmlSerializer(typeof(FileRequest));
                var responseSerializer = new XmlSerializer(typeof(StatusResponse));

                using (var stream = client.GetStream())
                {
                    // Serialize request to server
                    requestSerializer.Serialize(stream, request);
                    //Hacky Read Blocking 
                    client.Client.Shutdown(SocketShutdown.Send);

                    Console.WriteLine(((StatusResponse)responseSerializer.Deserialize(stream)).Success ? "Upload Successful" : "Upload Failed");
                }

                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}