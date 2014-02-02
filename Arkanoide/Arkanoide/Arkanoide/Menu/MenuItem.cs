using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arkanoide.Menu
{
    /// <summary>
    /// Representa cada elemento del Menú de Opciones
    /// </summary>
    public class MenuItem
    {
        private string item;            // Opción
        private Vector2 posicion;       // Posición en la pantalla
        private bool seleccionado;      // Está/No Está seleccionado

        public string Item { get { return this.item; } set { this.item = value; } }
        public Vector2 Posicion { get { return this.posicion; } set { this.posicion = value; } }
        public bool Seleccionado { get { return this.seleccionado; } set { this.seleccionado = value; } }

        public MenuItem(string item, Vector2 posicion)
        {
            this.item = item;
            this.posicion = posicion;
            this.seleccionado = false;
        }

        /// <summary>
        /// Pinta la opción de menú en la pantalla
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.seleccionado)
                spriteBatch.DrawString(Arkanoide.Fuente, this.item, this.posicion, MenuManager.ColorSeleccionado);
            else
                spriteBatch.DrawString(Arkanoide.Fuente, this.item, this.posicion, MenuManager.ColorNoSeleccionado);
        }
    }
}
