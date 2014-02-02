using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Arkanoide
{
    /// <summary>
    /// Esta clase representa cada uno de los ladrillos de un nivel determinado
    /// </summary>
    public class Brick
    {
        private GraphicsDeviceManager graphics;
        private ContentManager content;

        /// <summary>
        /// Códigos que marcan el tipo de ladrillo
        /// </summary>
        public const int Amarillo = 1;
        public const int Azul = 2;
        public const int Blanco = 3;
        public const int Gris = 4;
        public const int Morado = 5;
        public const int Negro = 6;
        public const int Rojo = 7;
        public const int Verde = 8;

        /// <summary>
        /// Dimensiones de los ladrillos
        /// </summary>
        public const int Anchura = 45;
        public const int Altura = 23;

        /// <summary>
        /// Espacio que se considera como borde en un ladrillo
        /// </summary>
        public const int Borde = 5;

        private int tipo;
        private int puntos;
        private int vida;
        private bool visible;
        private Vector2 posicion;
        private Texture2D sprite;

        public int Tipo { get { return this.tipo; } }
        public int Puntos { get { return this.puntos; } }
        public int Vida { get { return this.vida; } set { this.vida = value; } }
        public bool Visible { get { return this.visible; } set { this.visible = value; } }
        public Vector2 Posicion { get { return this.posicion; } set { this.posicion = value; } } 

        public Brick(int tipo, Vector2 posicion, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.tipo = tipo;
            if (tipo == Brick.Rojo)
            {
                this.puntos = 3;
                this.vida = 2;
            }
            else
            {
                this.puntos = 1;
                this.vida = 1;
            }
            this.visible = true;
            this.posicion = posicion;

            this.graphics = graphics;
            this.content = content;
        }

        public void Load()
        {
            string tipoLadrillo = null;

            if (this.tipo == Brick.Amarillo)
            {
                tipoLadrillo = "ladrillo_amarillo";
            }
            else if (this.tipo == Brick.Azul)
            {
                tipoLadrillo = "ladrillo_azul";
            }
            else if (this.tipo == Brick.Blanco)
            {
                tipoLadrillo = "ladrillo_blanco";
            }
            else if (this.tipo == Brick.Gris)
            {
                tipoLadrillo = "ladrillo_gris";
            }
            else if (this.tipo == Brick.Morado)
            {
                tipoLadrillo = "ladrillo_morado";
            }
            else if (this.tipo == Brick.Negro)
            {
                tipoLadrillo = "ladrillo_negro";
            }
            else if (this.tipo == Brick.Verde)
            {
                tipoLadrillo = "ladrillo_verde";
            }
            else if (this.tipo == Brick.Rojo)
            {
                tipoLadrillo = "ladrillo_rojo";
            }

            this.sprite = this.content.Load<Texture2D>("bricks/" + tipoLadrillo);
            if (this.sprite == null)
                throw new Exception("Textura nula");
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.posicion, Color.White);
        }
    }
}
