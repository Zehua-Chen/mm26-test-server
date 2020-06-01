using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Linq;
using MM26TestServer.Services;
using MM26TestServer.Models;

namespace MM26TestServer
{
    public class WebSocketConnectionHandler
    {
        private IConfigurationService _configurationService;

        public WebSocketConnectionHandler(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task Handle(WebSocket ws)
        {
            byte[] stateData = _configurationService.State
                .Select((value, index) => (byte)value)
                .ToArray();

            await ws.SendAsync(stateData, WebSocketMessageType.Binary, true, CancellationToken.None);

            foreach (Change change in _configurationService.Changes)
            {
                byte[] changeData = change.Data
                    .Select((value, index) => (byte)value)
                    .ToArray();

                await ws.SendAsync(
                    changeData,
                    WebSocketMessageType.Binary,
                    true,
                    CancellationToken.None);
            }
        }
    }
}
