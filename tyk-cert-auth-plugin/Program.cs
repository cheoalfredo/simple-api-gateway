using Grpc.Core;
using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace tyk_cert_auth_plugin
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Port to attach the gRPC server to:
            const int Port = 5555;

            // We initialize a  Grpc.Core.Server and attach our dispatcher implementation to it:
            Server server = new Server
            {
                Services = { Coprocess.Dispatcher.BindService(new DispatcherImpl()) },
                Ports = { new ServerPort("192.168.0.18", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("gRPC server listening on " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
            
            /*var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            handler.ClientCertificates.Add(new X509Certificate2("d:\\cert.pem"));
            handler.ServerCertificateCustomValidationCallback =  (a,b,c,d) => true;

            using (var c = new HttpClient(handler))
            {               
                c.BaseAddress = new Uri("https://localhost:5001");
                var res = c.GetAsync("/jsonp").Result;
                Console.WriteLine(res.StatusCode);
            }*/

        }
    }
}
