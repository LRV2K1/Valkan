using System;
using Microsoft.Xna.Framework;

public class Animation : SpriteSheet
{
    protected float frameTime;
    protected bool isLooping;
    protected float time;
    protected bool isBackAndForth;
    protected bool isBackWards;
    protected bool goBack = false;
    //Okay
    public Animation(string assetname, bool isLooping, bool isBackAndForth, float frameTime = 0.1f) : base(assetname)
    {
        this.frameTime = frameTime;
        this.isLooping = isLooping;
        this.isBackAndForth = isBackAndForth;
    }

    public void Play(bool backWards)
    {
        isBackWards = backWards;
        if (isBackWards)
        {
            sheetIndex = NumberSheetElements - 1;
        }
        else
        {
            sheetIndex = 0;
        }
        time = 0.0f;
    }

    public void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        while (time > frameTime)
        {
            time -= frameTime;
            if (isLooping)
            {
                if (isBackAndForth)
                {
                    if (sheetIndex == NumberSheetElements - 1)
                    {
                        goBack = true;
                    }
                    else if (sheetIndex == 0)
                    {
                        goBack = false;
                    }
                    if (goBack)
                    {
                        sheetIndex = (sheetIndex - 1) % NumberSheetElements;
                    }
                    else
                    {
                        sheetIndex = (sheetIndex + 1) % NumberSheetElements;
                    }
                }
                else
                {
                    sheetIndex = (sheetIndex + 1) % NumberSheetElements;
                }
            }
            else
            {
                if (isBackWards)
                {
                    sheetIndex = (sheetIndex - 1) % NumberSheetElements;
                }
                else
                {
                    sheetIndex = Math.Min(sheetIndex + 1, NumberSheetElements - 1);
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

    public bool IsBackAndForth
    {
        get { return isBackAndForth; }
    }

    public bool IsBackWards
    {
        get { return isBackWards; }
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

