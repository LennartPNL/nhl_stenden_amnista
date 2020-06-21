using System.Net.Sockets;

namespace Amnista.Events
{
    public class ClientMessageReceivedEventArgs
    {
        public Socket Client { get; set; }
        public string Message { get; set; }

        public ClientMessageReceivedEventArgs(Socket client, string message)
        {
            Client = client;
            Message = message;
        }
    }
}
