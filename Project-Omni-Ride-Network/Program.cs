using Dna;
using Dna.AspNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Project_Omni_Ride_Network {
    public class Program {
        public static void Main(string[] args) {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder()
            .UseDnaFramework(construct => {
                construct.AddFileLogger();
            })
            .UseStartup<Startup>()
            .Build();
    }
}
