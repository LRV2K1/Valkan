using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

public class AssetManager
{
    protected ContentManager contentManager;
    protected Dictionary<string, Texture2D> textures;

    public AssetManager(ContentManager content)
    {
        contentManager = content;
        textures = new Dictionary<string, Texture2D>();
    }

    public Texture2D GetSprite(string assetName)
    {
        if (assetName == "")
        {
            return null;
        }
        else if (textures.ContainsKey(assetName))
        {
            return textures[assetName];
        }
        Texture2D tex = contentManager.Load<Texture2D>(assetName);
        textures.Add(assetName, tex);
        return tex;
    }

    public void PlaySound(string assetName)
    {
        if (assetName == "")
        {
            return;
        }
        SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
        snd.Play();
    }

    public void PlayMusic(string assetName, bool repeat = true)
    {
        MediaPlayer.IsRepeating = repeat;
        MediaPlayer.Play(contentManager.Load<Song>(assetName));
    }

    public ContentManager Content
    {
        get { return contentManager; }
    }
}