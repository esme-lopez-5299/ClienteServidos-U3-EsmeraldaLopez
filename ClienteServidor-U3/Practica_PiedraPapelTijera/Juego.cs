using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Practica_PiedraPapelTijera
{
    public enum Opcion { Piedra, Papel, Tijeras}
    public class Juego: INotifyPropertyChanged

    {
        public string Jugador1 { get; set; }
        public string Jugador2 { get; set; }

        public byte PuntosJugador1 { get; set; }
        public byte PuntosJugador2 { get; set; }

        public Opcion? SeleccionJugador1 { get; set; }
        public Opcion? SeleccionJugador2 { get; set; }
        public string Mensaje { get; set; }
        public ICommand SeleccionarOpcionCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        HttpListener servidor;
        ClientWebSocket cliente;
        Dispatcher currentDispatcher;

        public Juego()
        {
            currentDispatcher = Dispatcher.CurrentDispatcher;
        }

        public void CrearPartida(string nombre)
        {
            servidor = new HttpListener();
            servidor.Prefixes.Add("http://*:1000/ppt/");
            servidor.Start();
            servidor.BeginGetContext(OnContext, null);
            Jugador1 = nombre;
            Mensaje = "Esperando que se conecte un adversario";
        }

        private void OnContext(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        void Actualizar(string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
