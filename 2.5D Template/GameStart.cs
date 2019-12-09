using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameStart : GameEnvironment
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    static void Main()
    {
        GameStart game = new GameStart();
        game.Run();
    }

    public GameStart()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        screen = new Point(1920, 1080);
        windowSize = new Point(1280, 720);
        FullScreen = false;
        gameStateManager.AddGameState("introState", new IntroState());
        gameStateManager.AddGameState("titleScreen", new TitleScreenState());
        gameStateManager.AddGameState("playingState", new PlayingState(Content));
        gameStateManager.AddGameState("settingsState", new SettingsState());
        gameStateManager.SwitchTo("introState");
    }
    protected override void UnloadContent()
    {
    }
}

