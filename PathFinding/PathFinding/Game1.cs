using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

/// <summary>
/// This is the main type for your game.
/// </summary>
public class Game1 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Grid grid;
    Player player;
    AI ai;
    InputHelper inputHelper;
    Vector2 currentPlayerpos;
    Vector2 oldPlayerpos;
    public static ContentManager ContentManager { get; private set; }
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        ContentManager = Content;
        Content.RootDirectory = "Content";
        inputHelper = new InputHelper();
        grid = new Grid();
        player = new Player(grid);
        ai = new AI(player, grid);
        graphics.PreferredBackBufferWidth = 1000;
        graphics.PreferredBackBufferHeight = 800;
    }

    protected override void Initialize()
    {
        ai.Initialize();
        base.Initialize();
    }
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        grid.LoadContent();
        player.LoadContent();
        ai.LoadContent();
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        inputHelper.Update(gameTime);
        player.Update(inputHelper);
        currentPlayerpos = player.playerpos;

        // update ai alleen als de speler stil staat
        if (currentPlayerpos == oldPlayerpos)
        {
            //update ai 1x per keer dat de speler stilstaat
            ai.Update(currentPlayerpos);
        }
        oldPlayerpos = currentPlayerpos;

        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        grid.Draw(spriteBatch);
        player.Draw(spriteBatch);
        ai.Draw(spriteBatch);
        base.Draw(gameTime);
        spriteBatch.End();
    }
}
