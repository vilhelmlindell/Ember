using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ember.ECS.Components;

namespace Ember.ECS.Systems;

public class PlayerMovementSystem : System
{
    public PlayerMovementSystem(World world) : base(world, new Filter(world)
        .Include(typeof(Physics),
                 typeof(BoxCollider),
                 typeof(PlayerMovement)))
    {
    }

    protected override void UpdateEntity(Entity entity, GameTime gameTime)
    {
        var physics = entity.GetComponent<Physics>();
        var collider = entity.GetComponent<BoxCollider>();
        var movement = entity.GetComponent<PlayerMovement>();

        float horizontalFriction = 5 / movement.MaxHorizontalVelocityTime;
        float horizontalAcceleration = movement.MaxHorizontalVelocity * horizontalFriction;
        float maxJumpTime = movement.JumpWidth / movement.MaxHorizontalVelocity;

        if (Input.KeyDown(Keys.D))
            movement.HorizontalInput = 1;
        else if (Input.KeyDown(Keys.A))
            movement.HorizontalInput = -1;
        else
            movement.HorizontalInput = 0;

        if (collider.TouchingDown)
        {
            movement.JumpHeld = false;
            movement.JumpedMidAir = false;
            movement.JumpReleased = false;
            movement.CurrentJumpTime = 0;
            movement.MidairJumpsLeft = movement.MidairJumps;
        }

        if (Input.KeyPressed(Keys.Space) && collider.TouchingDown)
        {
            physics.Velocity.Y -= (2 * movement.JumpHeight * movement.MaxHorizontalVelocity) / movement.JumpWidth;
            movement.JumpHeld = true;
            movement.CurrentJumpTime = 0;
        }

        if (movement.MidairJumpsLeft > 0)
        {
            if (Input.KeyPressed(Keys.Space) && !collider.TouchingDown)
            {
                physics.Velocity.Y = 0;
                physics.Velocity.Y -= (2 * movement.JumpHeight * movement.MaxHorizontalVelocity) / movement.JumpWidth;
                movement.MidairJumpsLeft--;
                movement.JumpedMidAir = true;
            }
        }

        if (Input.KeyDown(Keys.Space) && movement.JumpHeld)
            movement.CurrentJumpTime++;

        if (Input.KeyReleased(Keys.Space) && movement.JumpHeld)
            movement.JumpReleased = true;

        physics.Acceleration.X = movement.HorizontalInput * horizontalAcceleration;
        physics.Velocity.X -= physics.Velocity.X * horizontalFriction;

        if (movement.JumpReleased && physics.Velocity.Y < 0 && !movement.JumpedMidAir)
        {
            physics.Velocity.Y += (2 * movement.JumpHeight * movement.MaxHorizontalVelocity * movement.MaxHorizontalVelocity) / (movement.JumpWidth * movement.JumpWidth)
                                * (maxJumpTime / Math.Clamp(movement.CurrentJumpTime, movement.MinJumpTime, maxJumpTime));
        }
        else
            physics.Velocity.Y += (2 * movement.JumpHeight * movement.MaxHorizontalVelocity * movement.MaxHorizontalVelocity)
                                / (movement.JumpWidth * movement.JumpWidth);

        if (physics.Velocity.Y > movement.TerminalVelocity)
            physics.Velocity.Y = movement.TerminalVelocity;
    }
}
