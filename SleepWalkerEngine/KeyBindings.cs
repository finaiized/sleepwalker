using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace SleepwalkerEngine
{
    /// <summary>
    /// Binds keys or mouse buttons to named 'actions'.
    /// 
    /// An action is a container for keys. For example, the action "Shoot" may be bound to two different keys,
    /// and checking for either of them can trigger the action.
    /// </summary>
    public class KeyBindings
    {
        /// <summary>
        /// The list of keys associated with this binding
        /// </summary>
        private List<Keys> keyList = new List<Keys>();

        /// <summary>
        /// The mouse buttons associated with this binding
        /// </summary>
        private List<MouseButton> mouseButtonList = new List<MouseButton>();

        /// <summary>
        /// The state of this action.
        /// </summary>
        private bool currentState = false;

        /// <summary>
        /// The previous state of this action.
        /// </summary>
        private bool previousState = false;

        /// <summary>
        /// The name of this key binding.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Checks to see whether or not this key binding is currently pressed/clicked.
        /// </summary>
        public bool IsDown
        {
            get { return currentState; }
        }

        /// <summary>
        /// Checks to see whether or not this key binding was pressed.
        /// 
        /// This is used for button presses meant to activate only once, even if pressed longer than one frame.
        /// </summary>
        public bool WasPressed
        {
            get { return currentState && !previousState; }
        }

        /// <summary>
        /// Create a key binding.
        /// </summary>
        /// <param name="name">The name of the key binding</param>
        public KeyBindings(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Add additional keys to the key binding.
        /// </summary>
        /// <param name="key">The key to bind the action to.</param>
        public void Add(Keys key)
        {
            if (!keyList.Contains(key))
            {
                keyList.Add(key);
            }
        }

        /// <summary>
        /// Add additional mouse buttons to the key binding.
        /// </summary>
        /// <param name="mouseState">The mouse button to bind the action to.</param>
        public void Add(MouseButton mouseState)
        {
            if (!mouseButtonList.Contains(mouseState))
            {
                mouseButtonList.Add(mouseState);
            }
        }

        /// <summary>
        /// Update the state of the action.
        /// </summary>
        /// <param name="keyState"></param>
        internal void Update(KeyboardState keyState, MouseState mState)
        {
            previousState = currentState;
            currentState = false;

            foreach (var key in keyList)
            {
                if (keyState.IsKeyDown(key))
                {
                    currentState = true;
                }
            }

            foreach (var button in mouseButtonList)
            {
                switch (button)
                {
                    case MouseButton.Left:
                        if (mState.LeftButton == ButtonState.Pressed)
                            currentState = true;
                        break;
                    case MouseButton.Middle:
                        if (mState.MiddleButton == ButtonState.Pressed)
                            currentState = true;
                        break;
                    case MouseButton.Right:
                        if (mState.RightButton == ButtonState.Pressed)
                            currentState = true;
                        break;
                }
            }
        }
    }
}
