using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MM26TestServer
{
    public static class Foo
    {
        public static void UseVisualizerHandler(this IApplicationBuilder builder)
        {
            builder.Use(Foo.Middleware);
        }

        private static async Task Middleware(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path.ToUriComponent() == "/visualizer")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                    var configuration = ActivatorUtilities.GetServiceOrCreateInstance<IConfiguration>(context.RequestServices);

                    await Handle(ws, configuration);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }
        }

        private static async Task Handle(WebSocket ws, IConfiguration configuration)
        {
            var protoConfig = configuration.GetSection(ProtoConfiguration.Proto)
                .Get<ProtoConfiguration>();

            IEnumerable<string> files = Directory.EnumerateFiles(protoConfig.Source)
                .OrderBy(s => s)
                .Where(path => Path.GetExtension(path) == ".pb");

            foreach (var file in files)
            {
                byte[] content = await File.ReadAllBytesAsync(file);
                await ws.SendAsync(
                    new ArraySegment<byte>(content),
                    WebSocketMessageType.Binary,
                    true,
                    CancellationToken.None);
            }
        }
    }
}
