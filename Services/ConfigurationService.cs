using MM26TestServer.Models;

namespace MM26TestServer.Services
{
    public interface IConfigurationService
    {
        int[] State { get; set; }
        Change[] Changes { get; set; }
    }

    public class ConfigurationService : IConfigurationService
    {
        public int[] State { get; set; }
        public Change[] Changes { get; set; }
    }
}
