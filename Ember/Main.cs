using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ember.ECS;

namespace Ember
{
    public class Main : Game
    {
        private struct Transform
        {
            public Vector3 Position;
        }
        private struct Velocity
        {
            public Vector3 Speed;
        }
        private struct Test
        {
            public int Hello;
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private World _world;
        private Entity _entity;
        private View _view;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _world = new World();
            _entity = _world.CreateEntity();

            _world.AddComponent<Velocity>(_entity);
            _world.AddComponent<Transform>(_entity);

            _view = new View(_world).With(typeof(Transform));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Entity entity in _view)
            {
                Transform transform = _world.GetComponent<Transform>(_entity);
                Console.WriteLine("Test");
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
