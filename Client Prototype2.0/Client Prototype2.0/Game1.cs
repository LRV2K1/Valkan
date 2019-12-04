using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client_Prototype2._0
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerTex;


        Vector2 playerPosition = new Vector2();
        string sendData = "";
        byte[] buffer = new byte[7];

        Socket master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //create client socket
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("192.168.2.6"), 9999);                     //create server socket

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTex = Content.Load<Texture2D>("player");
            playerPosition = new Vector2(100, 100);
            SetupClient();
            //master.Close();
        }
       
        private void SetupClient()
        {
            master.Connect(ipEnd);
        }

        protected override void Update(GameTime gameTime)
        {
            //master.Receive(pbd); //put the received data in the bytearray
            //Console.WriteLine("Our piggyback data " + Encoding.UTF8.GetString(pbd));//write the acknowledge

            //input
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerPosition.X += 5;
                Send();
                Receive();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerPosition.X -= 5;
                Send();
                Receive();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerPosition.Y += 5;
                Send();
                Receive();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                playerPosition.Y -= 5;
                Send();
                Receive();
            }
        }

        private void Send()
        {
            sendData = playerPosition.X.ToString() + " " + playerPosition.Y.ToString(); //string = x,yR
            master.Send(Encoding.UTF8.GetBytes(sendData)); //send byte array of string above
        }

        private void Receive()
        {
            int readByte;
            readByte = master.Receive(buffer);

            byte[] rData = new byte[readByte]; //put received data in byte array
            Array.Copy(buffer, rData, readByte); //get rid of unused bytes at the end
            string[] stringarray = Encoding.UTF8.GetString(rData).Split(' '); //store parts in string array
            playerPosition = new Vector2(int.Parse(stringarray[0]), int.Parse(stringarray[1]));
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
