using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Arkanoide
{
    /// <summary>
    /// Clase que representa a la tabla que mueve el jugador
    /// </summary>
    public class Board
    {
        private GraphicsDeviceManager graphics;
        private ContentManager content;

        public enum Status : int
        {
            Parada = 0,
            Moviendose = 1,
            Disparando = 2
        }

        // Normalmente 60x15
        private int anchura;
        private int altura;
        private int velocidad;
        private Status estado;
        private int vidas;
        private Vector2 posicion;
        private Texture2D sprite;

        public int Anchura { get { return this.anchura; } set { this.anchura = value; } }
        public int Altura { get { return this.altura; } set { this.altura = value; } }
        public Status Estado { get { return this.estado; } set { this.estado = value; } }
        public int Vidas { get { return this.vidas; } set { this.vidas = value; } }
        public Vector2 Posicion { get { return this.posicion; } set { this.posicion = value; } }

        public Board(int velocidad, GraphicsDeviceManager graphics, ContentManager content)
            :base()
        {
            this.velocidad = velocidad;
            this.estado = Status.Parada;
            this.vidas = 3;
            this.posicion = new Vector2(Arkanoide.ANCHURA_PANTALLA / 2, Arkanoide.ALTURA_PANTALLA - 100);

            this.graphics = graphics;
            this.content = content;
        }

        public void Load()
        {
            this.sprite = this.content.Load<Texture2D>("sprites/tabla");

            this.anchura = this.sprite.Width;
            this.altura = this.sprite.Height;
        }

        public void Update()
        {
            KeyboardState teclado;

            teclado = Keyboard.GetState();

            if (teclado.IsKeyDown(Keys.Left))
            {
                this.posicion.X -= this.velocidad;
                if (this.posicion.X < 0)
                    this.posicion.X = 0;

                this.estado = Status.Moviendose;
            }
            else if (teclado.IsKeyDown(Keys.Right))
            {
                this.posicion.X += this.velocidad;
                if ((this.posicion.X + this.anchura) >= Arkanoide.ANCHURA_PANTALLA)
                    this.posicion.X = Arkanoide.ANCHURA_PANTALLA - this.anchura;

                this.estado = Status.Moviendose;
            }
            else
                this.estado = Status.Parada;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.posicion, Color.White);
        }
    }
}
