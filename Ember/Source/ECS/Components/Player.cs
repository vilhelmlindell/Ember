namespace Ember.ECS.Components
{
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping
    }

    public class Player
    {
        public PlayerState State;
        public Tilemap Tilemap;
    }
}
