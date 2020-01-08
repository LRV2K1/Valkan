using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Server_Prototype2._0
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerTex;


        Vector2 playerPosition = new Vector2();
        Vector2 receivedPlayerPosition;
        string action;
        Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //create server socket
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("192.168.2.6"), 9999); //ip of joeri's desktop, port is some random number 192.168.1.13 OR 192.168.2.6

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTex = Content.Load<Texture2D>("player");
            playerPosition = new Vector2(200, 200);
            listenerSocket.Bind(ipEnd);
            listenerSocket.Listen(0);
            int clientNo = 0; //identify different clients                                                   //listen for socket that want to connect
            Socket clientSocket = listenerSocket.Accept();                                                    //accept that socket

            //threads
            Thread clientThread;
            clientThread = new Thread(() => ClientConnection(clientSocket, clientNo));                        //give each client a number and thread
            clientThread.Start();                                                                             //and start that shit
            clientNo++;
        }

        protected override void Update(GameTime gameTime)
        {
        }

        private void ClientConnection(Socket clientSocket, int clNr)
        {
            //message
            byte[] buffer = new byte[clientSocket.SendBufferSize];//client specifies buffer size
            int pos;
            int readByte; //a small number, number of bytes, constant value
            do
            {
                //receive data
                readByte = clientSocket.Receive(buffer); //store received data

                //transfer received data over to member variables
                byte[] rData = new byte[readByte]; //put received data in byte array
                Array.Copy(buffer, rData, readByte); //get rid of unused bytes at the end
                string[] stringarray = Encoding.UTF8.GetString(rData).Split(' '); //store parts in string array
                receivedPlayerPosition = new Vector2(int.Parse(stringarray[0]), int.Parse(stringarray[1])); //put the first 2 parts in pos

                playerPosition = receivedPlayerPosition;
                //return player position
                string sendData = playerPosition.X.ToString() + " " + playerPosition.Y.ToString(); //string = x,y
                clientSocket.Send(Encoding.UTF8.GetBytes(sendData)); //send byte array of string above


                Console.WriteLine("we got " + Encoding.UTF8.GetString(rData) + " from client" + clNr + "pos " + playerPosition);             //write data received from client nr
            } while (readByte > 0);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.Draw(playerTex, playerPosition, Color.White);
            spriteBatch.End();
        }
    }
}
