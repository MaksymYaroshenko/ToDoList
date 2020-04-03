using Microsoft.Extensions.Configuration;
using System.IO;

namespace ToDoList
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSetting { get; set; }

        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
