using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
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
            current = Dispatcher.CurrentDispatcher;
            listener.Prefixes.Add("http://localhost:1000/unidad3/");
            listener.Start();
            listener.BeginGetContext(GetContext, null);
        }

        private async void GetContext(IAsyncResult ar)
        
        {
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(GetContext, null);

            if (context.Request.IsWebSocketRequest)
            {
                var socketContext = await context.AcceptWebSocketAsync(null);

                if (socketContext.WebSocket.State == WebSocketState.Open)
                {
                    string mensaje = "Conexión aceptada.";
                    var buffer = Encoding.UTF8.GetBytes(mensaje);
                    await socketContext.WebSocket.SendAsync(new ArraySegment<byte>(buffer), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                    Recibir(socketContext.WebSocket);
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }


        public async void Recibir(WebSocket socket)
        {
            try
            {
                var buffer = new byte[1024];
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);


                var mensaje = Encoding.UTF8.GetString(buffer, 0, result.Count);

                current.Invoke(new Action(() =>
                {
                    Mensajes.Add(mensaje);
                }));


                mensaje = "Mensaje recibido.";
                buffer = Encoding.UTF8.GetBytes(mensaje);
                await socket.SendAsync(new ArraySegment<byte>(buffer), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                Recibir(socket);
            }
            catch (WebSocketException)
            {

                
            }
            
        }
    }
}