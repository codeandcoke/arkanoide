using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Arkanoide
{
    /// <summary>
    /// Clase que representa cada uno de los niveles del juego
    /// </summary>
    public class Level
    {
        private GraphicsDeviceManager graphics;
        private ContentManager content;
        private Arkanoide juego;

        private int numero;
        private Board tabla;
        private Ball bola;
        private List<Brick> ladrillos;
        private bool pausa;

        public int Numero { get { return this.numero; } set { this.numero = value; } }
        public List<Brick> Ladrillos { get { return this.ladrillos; } }
        public bool Pausa { get { return this.pausa; } set { this.pausa = value; } }

        public Level(int numero, int velocidadTabla, Vector2 velocidadBola, GraphicsDeviceManager graphics, ContentManager content,
            Arkanoide juego)
            : base()
        {
            this.numero = numero;
            this.tabla = new Board(velocidadTabla, graphics, content);
            this.bola = new Ball(velocidadBola, this.tabla, this, graphics, content);

            this.graphics = graphics;
            this.content = content;
            this.juego = juego;
        }

        public void Load()
        {
            // TODO: Carga del nivel
            string rutaNivel = null;
            StreamReader sr = null;
            string linea = null;
            string[] codigos;
            int i = 0, j = 0;
            Brick ladrillo = null;

            rutaNivel = string.Format("Content/levels/level{0}.txt", this.numero);
            sr = File.OpenText(rutaNivel);

            j = 170;
            this.ladrillos = new List<Brick>();
            while ((linea = sr.ReadLine()) != null)
            {
                codigos = linea.Split(',');
                foreach(String codigo in codigos)
                {
                    ladrillo = new Brick(int.Parse(codigo), new Vector2(i, j), this.graphics, this.content);
                    ladrillo.Load();
                    this.ladrillos.Add(ladrillo);

                    i += Brick.Anchura;
                }

                i = 0;
                j += Brick.Altura - 1;
            }

            // Carga la tabla
            this.tabla.Load();
            this.bola.Load();
        }

        public void Update()
        {
            KeyboardState teclado = Keyboard.GetState();

            if (this.pausa)
            {
                // El juego continua cuando el jugador pulsa la tecla Espacio
                if (teclado.IsKeyDown(Keys.Space))
                    this.ReanudarJuego();
            }

            // Si el juego está detenido no se actualizan la posición de ningún elemento de la pantalla
            if (this.pausa)
                return;

            // Actualiza la tabla
            this.tabla.Update();
            this.bola.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Pinta el nivel
            foreach(Brick ladrillo in this.ladrillos)
            {
                if (ladrillo.Visible)
                    ladrillo.Draw(spriteBatch);
            }

            // Pinta la tabla
            this.tabla.Draw(spriteBatch);
            this.bola.Draw(spriteBatch);
        }

        /// <summary>
        /// Detiene el juego
        /// </summary>

        public void PausarJuego()
        {
            this.pausa = true;

            if (this.juego.Vidas == 0)
            {
                this.TerminarPartida();
            }
        }

        /// <summary>
        /// Reanuda el juego
        /// </summary>
        public void ReanudarJuego()
        {
            this.pausa = false;
            this.bola.Posicion = new Vector2(this.tabla.Posicion.X + this.tabla.Anchura / 2, this.tabla.Posicion.Y);
            this.bola.Velocidad = new Vector2(this.bola.Velocidad.X, -this.bola.Velocidad.Y);
        }

        public void SumarPuntos(int puntos)
        {
            this.juego.Puntos += puntos;
        }

        public void RestarVida()
        {
            this.juego.Vidas--;
        }

        private void TerminarPartida()
        {
            // El jugador termina la partida porque ha agotado todas las vidas
            if (this.juego.Vidas == 0)
            {
            }
        }
    }
}
