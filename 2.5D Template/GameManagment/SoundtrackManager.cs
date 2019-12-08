//##OBSOLETE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public class SoundtrackManager
{
    //protected AssetManager assetManager;
    protected string songInQueue;
    protected float acceleration = 0.5f;
    protected float speed;
    public SoundtrackManager()
    {
        //assetManager = new AssetManager(content);
    }

    public void PlaySong(string songName)
    {
        if(MediaPlayer.State != MediaState.Playing)
        {
            Song song = GameEnvironment.AssetManager.Content.Load<Song>("Soundtracks/" + songName);
            MediaPlayer.Play(song);
        }
        else
        {
            songInQueue = songName;
            speed = 10f;
            FadeOverTo();
        }
    }

    public void FadeOverTo()
    {
        if(MediaPlayer.Volume > 0)
        {
            MediaPlayer.Volume -= speed;
            speed -= acceleration;
            FadeOverTo();
            return;
        }
        else
        {
            //Song song = GameEnvironment.AssetManager.Content.Load<Song>("Soundtracks/" + songInQueue);
            //MediaPlayer.Play(song);
        }
    }


}
