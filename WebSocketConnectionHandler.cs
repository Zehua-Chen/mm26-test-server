using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MM26TestServer
{
    public class WebSocketConnectionHandler
    {
        private ILogger<WebSocketConnectionHandler> _logger;
        private ProtoConfiguration _protoConfiguration;
        private byte[] data = Encoding.UTF8.GetBytes("ass we can");

        public WebSocketConnectionHandler(
            IConfiguration configuration,
            ILogger<WebSocketConnectionHandler> logger)
        {
            _logger = logger;

            _protoConfiguration = configuration
                .GetSection(ProtoConfiguration.Proto)
                .Get<ProtoConfiguration>();
        }

        public async Task Handle(WebSocket ws)
        {
            _logger.LogInformation("bytes = {0}", data.Length);
            _logger.LogInformation("state = {0}", ws.State);

            await ws.SendAsync(
                new ArraySegment<byte>(data),
                WebSocketMessageType.Binary,
                true,
                CancellationToken.None);

            byte[] buffer = new byte[100];

            await ws.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None);

            _logger.LogInformation(Encoding.UTF8.GetString(buffer));
            _logger.LogInformation("connection closed");
        }
    }
}
