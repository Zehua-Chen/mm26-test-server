using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Linq;
using Microsoft.Extensions.Logging;
using MM26TestServer.Services;
using MM26TestServer.Models;

namespace MM26TestServer
{
    public class WebSocketConnectionHandler
    {
        private IConfigurationService _configurationService;
        private ILogger<WebSocketConnectionHandler> _logger;

        public WebSocketConnectionHandler(
            IConfigurationService configurationService,
            ILogger<WebSocketConnectionHandler> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        public async Task Handle(WebSocket ws)
        {
            if (_configurationService.State == null
                || _configurationService.Changes == null)
            {
                _logger.LogWarning("Configuration incomplete, no data sent");
                return;
            }

            byte[] stateData = _configurationService.State
                .Select((value, index) => (byte)value)
                .ToArray();

            _logger.LogInformation("Sending State (Length = {0})", stateData.Length);

            await ws.SendAsync(stateData, WebSocketMessageType.Binary, true, CancellationToken.None);

            _logger.LogInformation("State Sent");

            foreach (Change change in _configurationService.Changes)
            {
                byte[] changeData = change.Data
                    .Select((value, index) => (byte)value)
                    .ToArray();

                _logger.LogInformation("Sending Change (Length = {0})", changeData.Length);

                await ws.SendAsync(
                    changeData,
                    WebSocketMessageType.Binary,
                    true,
                    CancellationToken.None);

                _logger.LogInformation("Change Sent");

                await Task.Delay((int)(change.Delay * 1000));

                _logger.LogInformation("Wait Done");
            }

            _logger.LogInformation("Done");
        }
    }
}
