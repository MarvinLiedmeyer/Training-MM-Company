using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CompanyAPI
{
    public class Program
    {
        // Einrichten des Hosts. Einstiegspunkt der App ------------------------------
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        // ---------------------------------------------------------------------------
    }
}
