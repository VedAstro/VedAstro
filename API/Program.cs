using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .Build();
            //var channel = GrpcChannel.ForAddress("http://localhost:7071");
            host.Run();
        }
    }
}


