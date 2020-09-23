using System;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
                    var handler = ActivatorUtilities.CreateInstance<WebSocketConnectionHandler>(context.RequestServices);
                    await handler.Handle(ws);
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
    }
}
