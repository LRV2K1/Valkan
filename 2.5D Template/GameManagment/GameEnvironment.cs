﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameEnvironment : Game
{
    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;
    protected InputHelper inputHelper;
    protected Matrix spriteScale;
    protected Point windowSize;

    protected static Point screen;
    protected static GameStateManager gameStateManager;
    protected static Random random;
    protected static AssetManager assetManager;
    protected static GameSettingsManager gameSettingsManager;
    protected static ScreenFade screenFade;
    protected static MultiplayerManager multiplayerManager;
    protected SpriteGameObject spritemouse;

    protected static bool quitGame;
    protected static bool newChanges;

    protected static string stringid;
    static List<OutputText> output;

    public GameEnvironment()
    {
        graphics = new GraphicsDeviceManager(this);

        multiplayerManager = new MultiplayerManager();
        inputHelper = new InputHelper();
        gameStateManager = new GameStateManager();
        spriteScale = Matrix.CreateScale(1, 1, 1);
        random = new Random();
        assetManager = new AssetManager(Content);
        gameSettingsManager = new GameSettingsManager();
        
        char charid = (char)(48);
        stringid += charid;
        output = new List<OutputText>();
    }

    public static string SpecialID
    {
        get
        {
            string s = stringid;
            stringid = AddChar(stringid);
            return s;
        }
    }

    public static string AddChar(string s)
    {
        char letter = s[s.Length - 1];
        string key = "";
        if (s.Length > 1)
        {
            key = s.Substring(0, s.Length - 1);
        }
        int nummer = (int)letter + 1;
        if (nummer > 125)
        {
            if (key == "")
            {
                key = ((char)(48)).ToString();
            }
            else
            {
                key = AddChar(key);
            }
            nummer = 48;
        }
        letter = (char)nummer;
        string text = key;
        text += letter.ToString();
        return text;
    }

    public static void OutputWindow(string text)
    {
        output.Add(new OutputText(text));
    }

    protected void DrawOutput(GameTime gameTime)
    {
        foreach (OutputText text in output)
        {
            text.Draw(gameTime, spriteBatch);
        }
    }

    public static Point Screen
    {
        get { return screen; }
        set { screen = value; }
    }

    public static Random Random
    {
        get { return random; }
    }

    public static AssetManager AssetManager
    {
        get { return assetManager; }
    }

    public static GameStateManager GameStateManager
    {
        get { return gameStateManager; }
    }

    public static GameSettingsManager GameSettingsManager
    {
        get { return gameSettingsManager; }
    }

    public static ScreenFade ScreenFade
    {
        get { return screenFade; }
    }

    public static bool QuitGame
    {
        get { return quitGame; }
        set { quitGame = value; }
    }

    public static bool NewChanges
    {
        get { return newChanges; }
        set { newChanges = value; }
    }

    public bool FullScreen
    {
        get { return graphics.IsFullScreen; }
        set
        {
            ApplyResolutionSettings(value);
        }
    }

    public void ApplyResolutionSettings(bool fullScreen = false)
    {
        if (!fullScreen)
        {
            graphics.PreferredBackBufferWidth = windowSize.X;
            graphics.PreferredBackBufferHeight = windowSize.Y;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }
        else
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        float targetAspectRatio = (float)screen.X / (float)screen.Y;
        int width = graphics.PreferredBackBufferWidth;
        int height = (int)(width / targetAspectRatio);
        if (height > graphics.PreferredBackBufferHeight)
        {
            height = graphics.PreferredBackBufferHeight;
            width = (int)(height * targetAspectRatio);
        }

        Viewport viewport = new Viewport();
        viewport.X = (graphics.PreferredBackBufferWidth / 2) - (width / 2);
        viewport.Y = (graphics.PreferredBackBufferHeight / 2) - (height / 2);
        viewport.Width = width;
        viewport.Height = height;
        GraphicsDevice.Viewport = viewport;

        inputHelper.Scale = new Vector2((float)GraphicsDevice.Viewport.Width / screen.X,
                                        (float)GraphicsDevice.Viewport.Height / screen.Y);
        inputHelper.Offset = new Vector2(viewport.X, viewport.Y);
        spriteScale = Matrix.CreateScale(inputHelper.Scale.X, inputHelper.Scale.Y, 1);
    }

    protected override void LoadContent()
    {
        DrawingHelper.Initialize(this.GraphicsDevice);
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spritemouse = new SpriteGameObject("Sprites/Menu/spr_mouse", 200);
        if(screenFade == null)
        {
            screenFade = new ScreenFade("Sprites/Menu/spr_button");
            
        }
    }


    protected void HandleInput()
    {
        inputHelper.Update();
        spritemouse.Position = inputHelper.MousePosition;
        if (ScreenFade.FadeToBlack || ScreenFade.FadeToWhite)
        {
            return;
        }
        if (inputHelper.KeyPressed(Keys.F5))
        {
            FullScreen = !FullScreen;
        }

        gameStateManager.HandleInput(inputHelper);
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        multiplayerManager.Update(gameTime);
        gameStateManager.Update(gameTime);
        ScreenFade.Update(gameTime);
        if(quitGame)
        {
            Exit();
            return;
        }
        if(newChanges && !FullScreen)
        {
            FullScreen = true;
            newChanges = false;
        }
        else if(newChanges && FullScreen)
        {
            FullScreen = false;
            newChanges = false;
        }
        int outputx = 0;
        for (int i = output.Count - 1; i >= 0; i--)
        {
            output[i].Update(gameTime);
            if (output[i].Timer <= 0)
            {
                output.RemoveAt(i);
            }
            else
            {
                output[i].Position = new Vector2(60, outputx);
                outputx += 30;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale);
        gameStateManager.Draw(gameTime, spriteBatch);
        spritemouse.Draw(gameTime, spriteBatch);
        screenFade.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }
}