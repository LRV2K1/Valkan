using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace Server_or_Client_Prototype_3._0
{
    //this class is the connection mananger. Here you can choose to setup a connect as a client or host
    //this is also the class where all the data from the client or server will be send to or received.
    //currently all players are stored here with all having a playerID and a poisition.
    //all this program does visually, is simply drawing all the players on the screen with the data stored in the list of players.
    //if you like this class, make sure to leave a thumbs up and if you want to see more of this make sure to subscribe!
    public class Game1 : Game 
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerTex;
        InputHelper inputHelper;
               
        Client client;
        Server server;

        static Random random;
        static SpriteFont font;
        static int iD;
        static float seconds;
        public static Random Random { get { return random; } }
        public static SpriteFont Font { get { return font; } }
        public static int ID { get { return iD; } }
        public static List<Player> players;     

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            random = new Random();
            inputHelper = new InputHelper();
            players = new List<Player>();
            iD = random.Next(100000);
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTex = Content.Load<Texture2D>("player");
            font = Content.Load<SpriteFont>("Fonts/Arial");
        }





        protected override void Update(GameTime gameTime)
        {
            seconds += gameTime.TotalGameTime.Seconds;
            inputHelper.Update(gameTime);

            if (client != null)
            {
                if ( seconds >= 0)
                {
                    client.Send();
                    client.Receive();
                    seconds = 0;
                }
                if (inputHelper.KeyTapped(Keys.Escape))
                {
                    client.Disconnect();
                    client = null;
                }
            }
            if (server != null)
            {
                if (inputHelper.KeyTapped(Keys.Escape))
                {
                    server.Disconnect();
                }
            }


            if (inputHelper.KeyTapped(Keys.H) && server == null && client == null) //create server
            {
                server = new Server(); 
                players.Add(new Player(ID, random.Next(50, graphics.PreferredBackBufferWidth - 50), random.Next(50, graphics.PreferredBackBufferHeight - 50)));
            }
            if (inputHelper.KeyTapped(Keys.C) && server == null && client == null) //create client
            {
                client = new Client();
                Player(ID).PositionX = Player(ID).PositionX;
                Player(ID).PositionY = Player(ID).PositionY;
            }

            if (!(client == null && server == null))
            {
                if (inputHelper.KeyPressed(Keys.W))
                {
                    Player(ID).PositionY -= 5;
                }
                if (inputHelper.KeyPressed(Keys.S))
                {
                    Player(ID).PositionY += 5;
                }
                if (inputHelper.KeyPressed(Keys.A))
                {
                    Player(ID).PositionX -= 5;
                }
                if (inputHelper.KeyPressed(Keys.D))
                {
                    Player(ID).PositionX += 5;
                }
            }
        }



        public static Player Player(int ID)
        {
            for (int index = 0; index < players.Count; index++)
            {
                if (players[index].ID == ID)
                {
                    return players[index];
                }
            }
            players.Add(new Player(ID));
            return players[players.Count - 1];
        }

        public static int IDToIndex(int ID)
        {
            for (int index = 0; index < players.Count; index++)
            {
                if (players[index].ID == ID)
                {
                    return index;
                }
            }
            return 0;
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            foreach (Player player in players)
            {
                if (!(client == null && server == null))
                {
                    spriteBatch.Draw(playerTex, new Vector2(player.PositionX, player.PositionY), Color.White); //draw all players
                }
            }
            if (client != null)
            {
                client.Draw(gameTime, spriteBatch); //draw client text
            }
            if (server != null)
            {
                server.Draw(gameTime, spriteBatch); //draw server text
            }
            spriteBatch.End();
        }
    }
}
