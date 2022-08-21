using Ember.Easing;

namespace Ember
{
    public class JumpTrajectory
    {
        public float HeightOfPeak;
        public float HorizontalDistanceToPeak;
        public float MaxHorizontalVelocity;

        public float TimeToPeak => HorizontalDistanceToPeak / MaxHorizontalVelocity;
        public float InitialVelocity => 2 * HeightOfPeak * MaxHorizontalVelocity / HorizontalDistanceToPeak;
        public float Gravity => -2 * HeightOfPeak * MaxHorizontalVelocity * MaxHorizontalVelocity / HorizontalDistanceToPeak * HorizontalDistanceToPeak;

        public static float GetTimeToPeak(float horizontalDistanceToPeak, float maxHorizontalVelocity)
        {
            return horizontalDistanceToPeak / maxHorizontalVelocity;
        }
        public static float GetInitialVelocity(float heightOfPeak, float maxHorizontalVelocity, float horizontalDistanceToPeak)
        {
            return 2 * heightOfPeak * maxHorizontalVelocity / horizontalDistanceToPeak;
        }
        public static float GetGravity(float heightOfPeak, float maxHorizontalVelocity, float horizontalDistanceToPeak)
        {
            return -2 * heightOfPeak * maxHorizontalVelocity * maxHorizontalVelocity / horizontalDistanceToPeak * horizontalDistanceToPeak;
        }
    }
}
