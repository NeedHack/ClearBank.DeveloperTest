using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class Configuration : IConfiguration
    {
        public string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}