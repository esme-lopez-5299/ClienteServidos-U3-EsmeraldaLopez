using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClienteWebSocket
{
   public class Cliente: INotifyPropertyChanged
    {
        ClientWebSocket client = new ClientWebSocket();
        public string Texto { get; set; }
        public ICommand  EnviarCommand { get; set; }
        Dispatcher dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        public Cliente()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            EnviarCommand = new RelayCommand(EnviarTexto);
        }

        private void EnviarTexto()
        {
            Enviar(Texto);
            Texto = "";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Texto"));
        }

        public async Task<bool> Conectar()
        {
           await client.ConnectAsync(new Uri("ws://localhost:1000/unidad3"), CancellationToken.None);
            if(client.State==WebSocketState.Open)
            {
                Recibir();
                return true;
            }
            return false;
        }

        public async void Enviar(string mensaje)
        {
            if(client.State==WebSocketState.Open)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(mensaje);
                await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }           
        
        }

        public async void Recibir()
        {
            if(client.State==WebSocketState.Open)
            {
                byte[] buffer = new byte[1024];
                var result= await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string mensaje = Encoding.UTF8.GetString(buffer, 0, result.Count);
                MessageBox.Show(mensaje);
                Recibir();
            }
        }
    }
}
