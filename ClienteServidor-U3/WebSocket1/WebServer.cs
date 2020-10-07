using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WebSocket1
{
   public class WebServer
    {
        HttpListener listener = new HttpListener();
        Dispatcher current;

        public ObservableCollection<string> Mensajes { get; set; } = new ObservableCollection<string>();


        public WebServer()
        {
            listener.Prefixes.Add("http://localhost:8080/unidad3/");
            listener.Start();
            listener.BeginGetContext(GetContext, null);
        }

        private async void GetContext(IAsyncResult ar)
        {
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(GetContext, null);

            if(context.Request.IsWebSocketRequest)
            {
                var socket = await context.AcceptWebSocketAsync(null);
                if(socket.WebSocket.State==System.Net.WebSockets.WebSocketState.Open)
                {
                    string mensaje = "Conexion aceptada";
                    var buffer = Encoding.UTF8.GetBytes(mensaje);
                    await socket.WebSocket.SendAsync(new ArraySegment<byte>(buffer), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }


    }
}
