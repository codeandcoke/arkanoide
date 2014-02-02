using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Arkanoide
{
    /// <summary>
    /// Esta clase representa a la pelota del jugador
    /// </summary>
    public class Ball
    {
        private GraphicsDeviceManager graphics;
        private ContentManager content;

        private int anchura;
        private int altura;
        private Vector2 velocidad;
        private Vector2 posicion;
        private float angulo;
        private Texture2D sprite;

        private Board tabla;
        private Level nivel;

        public int Anchura { get { return this.anchura; } set { this.anchura = value; } }
        public int Altura { get { return this.altura; } set { this.altura = value; } }
        public Vector2 Velocidad { get { return this.velocidad; } set { this.velocidad = value; } }
        public Vector2 Posicion { get { return this.posicion; } set { this.posicion = value; } }
        public float Angulo { get { return this.angulo; } set { this.angulo = value; } }

        public Ball(Vector2 velocidad, Board tabla, Level nivel, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.velocidad = velocidad;
            this.angulo = 45.0f;

            // Posición inicial de la bola
            this.posicion = new Vector2(Arkanoide.ANCHURA_PANTALLA / 2, 400);

            // Debe conocer siempre donde está la tabla y el nivel en el que juega
            this.tabla = tabla;
            this.nivel = nivel;

            this.graphics = graphics;
            this.content = content;
        }

        public void Load()
        {
            this.sprite = this.content.Load<Texture2D>("sprites/bola");

            this.anchura = sprite.Width;
            this.altura = sprite.Height;
        }

        public void Update()
        {
            // Actualiza la posición de la bola
            this.ActualizarDesplazamiento();

            // Comprobar colisiones con los ladrillos
            this.ComprobarColisionLadrillos();

            // Comprueba la colisión de la bola con las paredes que limitan la pantalla
            this.ComprobarColisionParedes();

            // Comprueba la colisión de la bola con la tabla del jugador
            this.ComprobarColisionTabla();
        }

        /// <summary>
        /// Comprueba si la bola colisiona con alguno de los ladrillos del nivel
        /// </summary>
        private void ComprobarColisionLadrillos()
        {
            List<Brick> ladrillos = null;
            Brick ladrillo = null;

            ladrillos = this.nivel.Ladrillos;

            for (int i = 0; i < ladrillos.Count; i++)
            {
                ladrillo = ladrillos[i];
                // Si el ladrillo no está visible, no se tiene en cuenta
                if (!ladrillo.Visible)
                    continue;

                // Comprueba si la bola colisiona con algún ladrillo
                if (this.HayColisionConLadrillo(ladrillo.Posicion))
                {
                    ladrillo.Vida -= 1;

                    // Cuando el ladrillo se rompe, desaparece y el jugador gana los puntos correspondientes
                    if (ladrillo.Vida == 0)
                    {
                        ladrillo.Visible = false;
                        this.nivel.SumarPuntos(ladrillo.Puntos);
                    }

                    this.velocidad.Y = -this.velocidad.Y;

                    return;
                }
            }
        }

        /// <summary>
        /// Actualiza la posición de la bola, en función de su velocidad y dirección
        /// </summary>
        private void ActualizarDesplazamiento()
        {
            this.posicion.X += this.velocidad.X;
            this.posicion.Y += this.velocidad.Y;
        }

        /// <summary>
        /// Comprueba si la bola colisiona con las paredes del nivel, y como debe rebotar en éstas
        /// </summary>
        private void ComprobarColisionParedes()
        {
            // Rebota con la pared izquierda o derecha
            if ((this.posicion.X <= 0)
                || ((this.posicion.X + this.anchura) > Arkanoide.ANCHURA_PANTALLA))
                this.velocidad.X = -this.velocidad.X;

            // Rebota en el techo
            if ((this.posicion.Y + this.altura) <= 0)
                this.velocidad.Y = -this.velocidad.Y;

            // La pelota rebota en el suelo
            if (this.posicion.Y >= Arkanoide.ALTURA_PANTALLA)
            {
                this.nivel.RestarVida();
                this.nivel.PausarJuego();
            }
        }

        /// <summary>
        /// Comprueba si la bola colisiona con la tabla, y como debe rebotar en ésta
        /// </summary>
        private void ComprobarColisionTabla()
        {
            // Si golpea por debajo de la parte alta de la tabla, no se considera colisión
            if ((this.posicion.Y + this.altura) > this.tabla.Posicion.Y)
                return;

            if (((this.posicion.Y + this.altura) >= this.tabla.Posicion.Y)
                && ((this.posicion.X + this.anchura) >= this.tabla.Posicion.X)
                && (this.posicion.X <= (this.tabla.Posicion.X + this.tabla.Anchura)))
            {
                double angulo = 0.0;

                this.velocidad.Y = -this.velocidad.Y;

                // Divido la tabla en 4 partes. Según golpea irá en una dirección u otra
                if (this.posicion.X < (this.tabla.Posicion.X + this.tabla.Anchura / 4))
                {
                    angulo = -50.0;
                }
                else if (this.posicion.X < (this.tabla.Posicion.X + this.tabla.Anchura / 2))
                {
                    angulo = -80.0;
                }
                else if (this.posicion.X < (this.tabla.Posicion.X + this.tabla.Anchura * 3 / 4))
                {
                    angulo = 80.0;
                }
                else if (this.posicion.X < (this.tabla.Posicion.X + this.tabla.Anchura))
                {
                    angulo = 50.0;
                }

                this.CalcularTrayectoria(angulo);
            }
        }
        
        /// <summary>
        /// Comprueba si la bola colisiona con un ladrillo determinado
        /// </summary>
        /// <param name="posicionLadrillo">Posición del ladrillo</param>
        /// <returns></returns>
        private bool HayColisionConLadrillo(Vector2 posicionLadrillo)
        {
            if (((this.posicion.X >= posicionLadrillo.X) && (this.posicion.X <= (posicionLadrillo.X + Brick.Anchura)))
                && ((this.posicion.Y >= posicionLadrillo.Y) && (this.posicion.Y <= (posicionLadrillo.Y + Brick.Altura))))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// A partir del ángulo de salida, calcula la trayectoria de la bola en X e Y
        /// </summary>
        /// <param name="angulo">Ángulo de salida de la bola</param>
        private void CalcularTrayectoria(double angulo)
        {
            Vector2 nuevaPosicion = Vector2.Zero;
            float h = 6.7f;

            if (angulo > 0)
            {
                this.velocidad.X = (float)Math.Floor(Math.Cos(Util.GradosARadianes(Math.Abs(angulo))) * h);
                this.velocidad.Y = -(float)Math.Floor(Math.Sin(Util.GradosARadianes(Math.Abs(angulo))) * h);
            }
            else
            {
                this.velocidad.X = -(float)Math.Floor(Math.Cos(Util.GradosARadianes(Math.Abs(angulo))) * h);
                this.velocidad.Y = -(float)Math.Floor(Math.Sin(Util.GradosARadianes(Math.Abs(angulo))) * h);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.posicion, Color.White);
        }
    }
}
