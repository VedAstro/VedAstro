using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace API
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}


