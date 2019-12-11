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

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        ai.Initialize();
        base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        grid.LoadContent();
        player.LoadContent();
        ai.LoadContent();
        // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
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

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        grid.Draw(spriteBatch);
        // TODO: Add your drawing code here
        player.Draw(spriteBatch);
        ai.Draw(spriteBatch);
        base.Draw(gameTime);
        spriteBatch.End();
    }
}
