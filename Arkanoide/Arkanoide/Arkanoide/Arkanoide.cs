using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Arkanoide.Menu;

namespace Arkanoide
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Arkanoide : Microsoft.Xna.Framework.Game
    {
        enum EstadoJuego : int
        {
            Menu = 0,
            Jugar = 1,
            Creditos = 2,
            Salir = 3
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static int ANCHURA_PANTALLA = 540;
        public static int ALTURA_PANTALLA = 700;

        private MenuManager menuManager;
        private EstadoJuego estadoJuego;
        private Level nivel;
        private int puntos;
        private int vidas;

        public static SpriteFont Fuente;

        public int Puntos { get { return this.puntos; } set { this.puntos = value; } }
        public int Vidas { get { return this.vidas; } set { this.vidas = value; } }

        public Arkanoide()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Inicializa el juego e instacia el Menú
            this.estadoJuego = EstadoJuego.Menu;
            this.menuManager = new MenuManager();

            // Inicializa el nivel por defecto
            this.nivel = new Level(1, 5, new Vector2(4, 4), this.graphics, Content, this);

            // Inicializa variables globales del juego
            this.puntos = 0;
            this.vidas = 3;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Carga el Menú
            Arkanoide.Fuente = Content.Load<SpriteFont>("fonts/Hud");
            this.menuManager.Load();

            // Carga el nivel por defecto
            this.nivel.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState teclado ;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            teclado = Keyboard.GetState();

            if (this.estadoJuego == EstadoJuego.Menu)
            {
                this.menuManager.Update();

                if (teclado.IsKeyDown(Keys.Enter))
                {
                    if (this.menuManager.ItemSeleccionado == 0)
                        this.estadoJuego = EstadoJuego.Jugar;
                    else if (this.menuManager.ItemSeleccionado == 1)
                        this.estadoJuego = EstadoJuego.Creditos;
                    else if (this.menuManager.ItemSeleccionado == 2)
                        this.estadoJuego = EstadoJuego.Salir;

                }
            }
            else if (this.estadoJuego == EstadoJuego.Jugar)
            {
                // Modo juego
                this.nivel.Update();

                // Si se pulsa la tecla Escape se vuelve al menú
                if (teclado.IsKeyDown(Keys.Escape))
                    this.estadoJuego = EstadoJuego.Menu;
            }
            else if (this.estadoJuego == EstadoJuego.Creditos)
            {
                // Muestra los créditos del juego

                // Si se pulsa la tecla Escape se vuelve al menú
                if (teclado.IsKeyDown(Keys.Escape))
                    this.estadoJuego = EstadoJuego.Menu;
            }
            else if (this.estadoJuego == EstadoJuego.Salir)
            {
                // Sale directamente del juego
                this.Exit();
            }
            else
            {
                // Nunca debería llegar aqui. No hacer nada
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Fija el tamaño de la pantalla
            this.graphics.PreferredBackBufferWidth = Arkanoide.ANCHURA_PANTALLA;
            this.graphics.PreferredBackBufferHeight = Arkanoide.ALTURA_PANTALLA;
            this.graphics.ApplyChanges();

            this.spriteBatch.Begin();

            if (this.estadoJuego == EstadoJuego.Menu)
                this.menuManager.Draw(this.spriteBatch);
            else if (this.estadoJuego == EstadoJuego.Jugar)
            {
                this.spriteBatch.DrawString(Arkanoide.Fuente, "Puntos: " + this.puntos, new Vector2(25, 10), Color.Blue);
                this.spriteBatch.DrawString(Arkanoide.Fuente, "Vidas: " + this.vidas, 
                    new Vector2(Arkanoide.ANCHURA_PANTALLA - 100, 10), Color.Blue);
                this.nivel.Draw(spriteBatch);
            }
            else if (this.estadoJuego == EstadoJuego.Creditos)
                this.spriteBatch.DrawString(Arkanoide.Fuente, "Estos son los creditos", new Vector2(300, 300), Color.White);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
