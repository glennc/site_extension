using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace DotNetCoreUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var thing = RuntimeInfo.Parse("{\"runtimeOptions\": {\"framework\": {\"name\": \"Microsoft.NETCore.App\",\"version\": \"1.1.1\"}}}");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
