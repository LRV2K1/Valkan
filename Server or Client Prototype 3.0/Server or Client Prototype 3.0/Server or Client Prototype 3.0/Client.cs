using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server_or_Client_Prototype_3._0
{
    class Client
    {
        public static Socket clientSocket;
        static IPEndPoint ipEnd;
        static string status;
        static IPAddress IP;
        static int port;

        public Client()
        {
            IP = IPAddress.Parse("192.168.2.6");
            port = 9999;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //create client socket
            ipEnd = new IPEndPoint(IP, port); //ip of joeri's desktop, port is some random number home - > 192.168.1.13 OR 192.168.2.6  <- utrecht huis
            LoopConnect();
        }

        private static void LoopConnect()
        {
            int attempts = 0;
            while (!clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    clientSocket.Connect(ipEnd); //try to connect to server
                }
                catch (SocketException)
                {
                    Console.WriteLine("Could not connect to " + IP + ":" + port + ", attempts: " + attempts.ToString());
                }
            }
            Console.WriteLine("Connected to " + IP + ":" + port);
        }

        public void Send() //send playerdata
        {
            byte[] data = Encoding.UTF8.GetBytes(Game1.Player(Game1.ID).ID.ToString() + " " + Game1.Player(Game1.ID).PositionX.ToString() + " " + Game1.Player(Game1.ID).PositionY.ToString());
            clientSocket.Send(data);
        }



        public void Receive()
        {
            byte[] buffer = new byte[64];
            int readByte = clientSocket.Receive(buffer);
            byte[] rData = new byte[readByte]; //initialize new byte array
            Array.Copy(buffer, rData, readByte); //copy buffer array to rData array

            //put received data in Players list
            string[] players = Encoding.UTF8.GetString(rData).Split(',');
            foreach (string player in players)
            {
                string[] variables = player.Split(' ');

                Game1.Player(int.Parse(variables[0])).PositionX = int.Parse(variables[1]);
                Game1.Player(int.Parse(variables[0])).PositionY = int.Parse(variables[2]);
            }
        }



        public void Disconnect()
        {
            byte[] data = Encoding.UTF8.GetBytes("Disconnect " + Game1.ID.ToString());
            clientSocket.Send(data);
            Receive();

            clientSocket.Shutdown(SocketShutdown.Both);
            Console.WriteLine("Disconnected from " + IP + ":" + port);
            clientSocket.Close();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.Font, "Connected to IP: " + IP, new Vector2(300, 5), Color.Black);
        }
    }
}