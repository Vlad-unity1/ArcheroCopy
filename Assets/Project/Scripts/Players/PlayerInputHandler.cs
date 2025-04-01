using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerInputHandler
    {
        public readonly Joystick Joystick;

        public PlayerInputHandler(Joystick joystick)
        {
            Joystick = joystick;
        }

        public Vector3 GetInputDirection()
        {
            return new Vector3(Joystick.Horizontal, 0f, Joystick.Vertical).normalized;
        }
    }
}