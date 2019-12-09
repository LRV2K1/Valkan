using System;
using Microsoft.Xna.Framework;

public class Animation : SpriteSheet
{
    protected float frameTime;
    protected bool isLooping;
    protected float time;
    protected bool looped;

    public Animation(string assetname, bool isLooping, float frameTime = 0.1f) : base(assetname)
    {
        this.frameTime = frameTime;
        this.isLooping = isLooping;
        looped = false;
    }

    public void Play()
    {
        sheetIndex = 0;
        time = 0.0f;
    }

    public void Update(GameTime gameTime)
    {
        if (looped)
        {
            return;
        }
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        while (time > frameTime)
        {
            time -= frameTime;
            if (isLooping)
            {
                sheetIndex = (sheetIndex + 1) % NumberSheetElements;
            }
            else
            {
                sheetIndex = Math.Min(sheetIndex + 1, NumberSheetElements - 1);
                if (SheetIndex == NumberSheetElements - 1)
                {
                    looped = true;
                }
            }
        }
    }

    public float FrameTime
    {
        get { return frameTime; }
    }

    public bool IsLooping
    {
        get { return isLooping; }
    }

    public int CountFrames
    {
        get { return NumberSheetElements; }
    }

    public bool AnimationEnded
    {
        get { return !isLooping && sheetIndex >= NumberSheetElements - 1; }
    }
}

