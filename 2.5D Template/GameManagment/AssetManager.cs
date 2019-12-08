using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;

public class AssetManager
{
    protected ContentManager contentManager;
    protected string songInQueue;
    protected float acceleration = 0.001f;
    protected float speed;
    protected bool fadeDown;
    protected bool fadeUp;
    public AssetManager(ContentManager content)
    {
        contentManager = content;
    }

    public Texture2D GetSprite(string assetName)
    {
        if (assetName == "")
        { 
            return null;
        }
        return contentManager.Load<Texture2D>(assetName);
    }

    public void Update(GameTime gameTime)
    {
        if (fadeDown)
        {
            if (MediaPlayer.Volume > 0.01f)
            {
                MediaPlayer.Volume -= speed;
                speed += acceleration;
            }
            else
            {
                MediaPlayer.Volume = 0f;
                MediaPlayer.Play(contentManager.Load<Song>("Soundtracks/" + songInQueue));
                fadeDown = false;
                fadeUp = true;
                speed = 0.001f;
            }
        }
        else if(fadeUp)
        {
            if(MediaPlayer.Volume < 1f)
            {
                MediaPlayer.Volume += speed;
                speed += acceleration;
            }
            else
            {
                MediaPlayer.Volume = 1f;
                fadeUp = false;
            }
        }
    }

    public void PlaySound(string assetName)
    {
        if (assetName == "")
        {
            return;
        }
        SoundEffect snd = contentManager.Load<SoundEffect>("Sounds/" + assetName);
        snd.Play();
    }

    public void PlaySong(string assetName, bool repeat = true)
    {
        if (MediaPlayer.State != MediaState.Playing)
        {
            MediaPlayer.IsRepeating = repeat;
            MediaPlayer.Play(contentManager.Load<Song>("Soundtracks/" + assetName));
        }
        else
        {
            songInQueue = assetName;
            speed = 0.001f;
            fadeDown = true;
        }
    }

    public ContentManager Content
    {
        get { return contentManager; }
    }
}