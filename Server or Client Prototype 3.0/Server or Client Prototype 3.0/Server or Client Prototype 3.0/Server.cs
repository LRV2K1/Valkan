using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server_or_Client_Prototype_3._0
{
    class Server
    {
        static Socket serverSocket;
        static IPEndPoint ipEnd;
        static byte[] _buffer;

        int count = 0;

        public Server()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipEnd = new IPEndPoint(IPAddress.Any, 9999);
            _buffer = new byte[64];

            serverSocket.Bind(ipEnd);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }
        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
        private static void AcceptCallBack(IAsyncResult AR)
        {
            Socket socket = serverSocket.EndAccept(AR);
            Console.WriteLine("client connected");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }



        private static void ReceiveCallBack(IAsyncResult AR)
        {
            //receive: players data
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                int received = socket.EndReceive(AR);
                byte[] rData = new byte[received];
                Array.Copy(_buffer, rData, received);
                string[] playerd = Encoding.UTF8.GetString(rData).Split(' '); //store parts in string array
                if (playerd[0] == "Disconnect")
                {
                    Console.WriteLine("Client " + playerd[1] + " disconnected");
                }
                else
                {
                    Game1.Player(int.Parse(playerd[0])).PositionX = int.Parse(playerd[1]);
                    Game1.Player(int.Parse(playerd[0])).PositionY = int.Parse(playerd[2]);
                }



                //send: list of player data as string
                StringBuilder playerData = new StringBuilder();
                foreach (Player player in Game1.players)
                {
                    Console.WriteLine("send " + player.ID.ToString() + " " + player.PositionX.ToString() + " " + player.PositionY.ToString());
                    playerData.Append(player.ID + " " + player.PositionX + " " + player.PositionY).Append(",");
                }
                playerData.Length--; //remove ',' at the end

                byte[] data = Encoding.UTF8.GetBytes(playerData.ToString()); //send entire array of all player information
                
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
            catch
            {

            }
        }

        
        public void Disconnect()
        {
        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.Font, "hosting", new Vector2(300, 5), Color.Black);
        }
    }
}
