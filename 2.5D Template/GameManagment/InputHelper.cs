using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public enum MouseButton
{
    Left,
    Right,
    Middel,
    None
}

public class InputHelper
{
    protected MouseState currentMouseState, previousMouseState;
    protected KeyboardState currentKeyboardState, previousKeyboardState;
    protected Vector2 scale, offset;

    public InputHelper()
    {
        scale = Vector2.One;
        offset = Vector2.Zero;
    }

    public void Update()
    {
        previousMouseState = currentMouseState;
        previousKeyboardState = currentKeyboardState;
        currentMouseState = Mouse.GetState();
        currentKeyboardState = Keyboard.GetState();
    }

    public Vector2 Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    public Vector2 Offset
    {
        get { return offset; }
        set { offset = value; }
    }

    public Vector2 MousePosition
    {
        get { return (new Vector2(currentMouseState.X, currentMouseState.Y) - offset ) / scale; }
    }

    public bool MouseButtonPressed(MouseButton m)
    {
        if (m == MouseButton.Left)
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }
        else if (m == MouseButton.Right)
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }
        else if (m == MouseButton.Middel)
        {
            return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
        }
        else return false;
    }

    public bool MouseButtonDown(MouseButton m)
    {
        if (m == MouseButton.Left)
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }
        else if (m == MouseButton.Right)
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }
        else if (m == MouseButton.Middel)
        {
            return currentMouseState.MiddleButton == ButtonState.Pressed;
        }
        else return false;
    }

    public bool ScrolUp()
    {
        return currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue;
    }

    public bool ScrolDown()
    {
        return currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue;
    }

    public bool KeyPressed(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyUp(k);
    }

    public bool IsKeyDown(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k);
    }

    public bool AnyKeyPressed
    {
        get { return currentKeyboardState.GetPressedKeys().Length > 0 && previousKeyboardState.GetPressedKeys().Length == 0; }
    }
}