using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.ECS;
using Ember.ECS.Systems;
using Ember.ECS.Components;
using Ember.Graphics;
using Ember.Animations;
using Ember.Tiles;
using Ember.Items;
using Ember.UI;

namespace Ember
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera _camera;
        private World _world;
        private PhysicsSystem _physicsSystem;
        private TilemapCollisionSystem _tileCollisionSystem;
        private PlayerMovementSystem _playerMovementSystem;
        private SpriteAnimatorSystem _spriteAnimatorSystem;
        private CameraFollowSystem _cameraFollowSystem;
        private SpriteRendererSystem _spriteRendererSystem;
        private TilemapRendererSystem _tilemapRendererSystem;
        private PlayerControllerSystem _playerControllerSystem;
        private Entity _player;
        private Entity _tilemap;

        private UiManager _uiManager;
        private Inventory _inventory;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Directory.SetCurrentDirectory("/home/vilhelm/dev/csharp/Ember/Ember");
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _camera = new Camera(GraphicsDevice.Viewport);
            _world = new World(64);
            _player = _world.EntityManager.CreateEntity();
            _tilemap = _world.EntityManager.CreateEntity();
            _uiManager = new UiManager(GraphicsDevice.Viewport);
            //_inventory = new Inventory(_uiManager, 8, 4, 32, 32, 8, 8, new Sprite(Content.Load<Texture2D>("Assets/Sprites/ItemFrame")));
            //_uiManager.AddChild(_inventory);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Shaders.LoadShaders(Content, GraphicsDevice);
            Item.LoadItems(Content);
            Tile.LoadTiles(Content);

            _inventory.AddItemStack(new ItemStack(Item.Items[ItemId.Grass]), 0);

            Tilemap tilemap = new Tilemap(200, 200, 32);
            for (int x = 0; x < 200; x++)
            {
                for (int y = 6; y < 200; y++)
                {
                    tilemap.SetTile(x, y, TileId.Grass);
                }
            }

            _tilemap.AddComponent(tilemap);
            _player.AddComponent(new Position());
            _player.AddComponent(new Physics());
            var sprite = new SpriteRenderer()
            {
                Texture = Content.Load<Texture2D>("Assets/Sprites/Slime"),
                LayerDepth = DrawLayer.Default
            };
            _player.AddComponent(sprite);
            var spriteAnimator = new SpriteAnimator()
            {
                Animation = AnimationParser.LoadJson("Content/Assets/Animations/Slime.json")
            };
            spriteAnimator.Play("Idle");
            _player.AddComponent(spriteAnimator);
            _player.AddComponent(new BoxCollider()
            {
                Width = 32,
                Height = 23,
                Tilemap = tilemap
            });
            _player.AddComponent(new PlayerMovement(3, 60, 5, 120, 100, 5, 3));
            _player.AddComponent(new CameraFollow());
            _player.AddComponent(new Player()
            {
                Tilemap = tilemap
            });

            _physicsSystem = new PhysicsSystem(_world);
            _tileCollisionSystem = new TilemapCollisionSystem(_world);
            _playerMovementSystem = new PlayerMovementSystem(_world);
            _spriteAnimatorSystem = new SpriteAnimatorSystem(_world);
            _cameraFollowSystem = new CameraFollowSystem(_world, _camera);
            _spriteRendererSystem = new SpriteRendererSystem(_world, _spriteBatch, _camera);
            _tilemapRendererSystem = new TilemapRendererSystem(_world, _spriteBatch, _camera);
            _playerControllerSystem = new PlayerControllerSystem(_world, _camera);

            _world.SystemManager.UpdateSystems.Add(_physicsSystem);
            _world.SystemManager.UpdateSystems.Add(_tileCollisionSystem);
            _world.SystemManager.UpdateSystems.Add(_physicsSystem);
            _world.SystemManager.UpdateSystems.Add(_tileCollisionSystem);
            _world.SystemManager.UpdateSystems.Add(_playerMovementSystem);
            _world.SystemManager.UpdateSystems.Add(_playerControllerSystem);
            _world.SystemManager.UpdateSystems.Add(_spriteAnimatorSystem);
            _world.SystemManager.UpdateSystems.Add(_cameraFollowSystem);
            _world.SystemManager.DrawSystems.Add(_spriteRendererSystem);
            _world.SystemManager.DrawSystems.Add(_tilemapRendererSystem);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            //_world.Update(gameTime);
            _uiManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //_world.Draw(gameTime);
            _uiManager.Draw(_spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
