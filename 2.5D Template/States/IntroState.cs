using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

class IntroState : GameObjectList
 {
    Texture2D videoTexture;
    Video video;
    VideoPlayer videoPlayer;
    protected bool resume;

    public IntroState(int layer = 101, string id = "intro") 
        : base(layer,id)
    {
        video = GameEnvironment.AssetManager.Content.Load<Video>("Videos/Fragment 1(intro)");
        videoPlayer = new VideoPlayer();
        videoPlayer.Play(video);
        Add(GameEnvironment.ScreenFade);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (inputHelper.AnyKeyPressed)
        {
            resume = true;
            GameEnvironment.ScreenFade.FadeBlack();
        }
    }

    public override void Update(GameTime gameTime)
    {
        if(resume)
        {
            videoPlayer.Volume = (float)((255 - GameEnvironment.ScreenFade.A) / 255);
            if(videoPlayer.Volume == 0f)
            {
                GameEnvironment.GameStateManager.SwitchTo("titleScreen");
            }
        }
        if(videoPlayer.State == MediaState.Stopped)
        {
            GameEnvironment.GameStateManager.SwitchTo("titleScreen");
        }
        videoTexture = videoPlayer.GetTexture();
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(videoTexture, this.GlobalPosition, Color.White);
        base.Draw(gameTime, spriteBatch);
    }

}