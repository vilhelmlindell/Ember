namespace Ember.ECS.Components
{
    public class PlayerMovement
    {
        public float MaxHorizontalVelocity;
        public float MaxHorizontalVelocityTime;
        public float TerminalVelocity;
        public float JumpWidth;
        public float JumpHeight;
        public int MinJumpTime;
        public int MidairJumps;
        public int MidairJumpsLeft;
        public int CurrentJumpTime;
        public int HorizontalInput;
        public bool JumpHeld;
        public bool JumpReleased;
        public bool JumpedMidAir;

        public PlayerMovement(float maxHorizontalVelocity, float maxHorizontalVelocityTime, float terminalvelocity, float jumpWidth, float jumpHeight, int minJumpTime, int midairJumps)
        {
            MaxHorizontalVelocity = maxHorizontalVelocity;
            MaxHorizontalVelocityTime = maxHorizontalVelocityTime;
            TerminalVelocity = terminalvelocity;
            JumpWidth = jumpWidth;
            JumpHeight = jumpHeight;
            MinJumpTime = minJumpTime;
            MidairJumps = midairJumps;
            MidairJumpsLeft = midairJumps;
        }
    }
}
