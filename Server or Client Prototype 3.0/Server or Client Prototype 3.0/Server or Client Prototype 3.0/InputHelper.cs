using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Server_or_Client_Prototype_3._0
{
    class InputHelper //handles users input
    {
        private KeyboardState currentKeyboardState, previousKeyboardState;
        public void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }
        public bool KeyPressed(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k);
        }
        public bool KeyTapped(Keys k)
        {
            if (!previousKeyboardState.IsKeyDown(k) && currentKeyboardState.IsKeyDown(k))
            {
                return currentKeyboardState.IsKeyDown(k);
            }
            else
            {
                return false;
            }
        }
    }
}
