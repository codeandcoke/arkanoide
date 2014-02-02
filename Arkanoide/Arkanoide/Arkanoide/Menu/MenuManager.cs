using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkanoide.Menu
{
   public class MenuManager
   {
       public static Color ColorSeleccionado;
       public static Color ColorNoSeleccionado;
       private int itemSeleccionado;

       private KeyboardState teclado;
       private KeyboardState tecladoAnterior;

       private List<MenuItem> listaItems;

       public int ItemSeleccionado { get { return this.itemSeleccionado; } set { this.itemSeleccionado = value; } }

       public MenuManager()
           : base()
       {
           ColorSeleccionado = Color.Red;
           ColorNoSeleccionado = Color.White;

           this.listaItems = new List<MenuItem>();
       }

       public void Load()
       {
           this.listaItems.Add(new MenuItem("Jugar", new Vector2(100, 100)));
           this.listaItems.Add(new MenuItem("Creditos", new Vector2(100, 150)));
           this.listaItems.Add(new MenuItem("Salir", new Vector2(100, 200)));
       }

       public void Update()
       {
           this.teclado = Keyboard.GetState();

           // Resetea el estado de las opciones del menú
           for (int i = 0; i < this.listaItems.Count; i++)
           {
               this.listaItems[i].Seleccionado = false;
           }

           this.listaItems[this.itemSeleccionado].Seleccionado = true;

           if ((this.teclado.IsKeyDown(Keys.Down)) && (this.tecladoAnterior.IsKeyUp(Keys.Down)))
           {
               this.itemSeleccionado++;
               if (this.itemSeleccionado > this.listaItems.Count - 1)
                   this.itemSeleccionado = 0;
           }

           if ((this.teclado.IsKeyDown(Keys.Up)) && (this.tecladoAnterior.IsKeyUp(Keys.Up)))
           {
               this.itemSeleccionado--;
               if (this.itemSeleccionado < 0)
                   this.itemSeleccionado = this.listaItems.Count - 1;
           }

           this.tecladoAnterior = teclado;
       }

       public void Draw(SpriteBatch spriteBatch)
       {
           foreach (MenuItem menuItem in this.listaItems)
           {
               menuItem.Draw(spriteBatch);
           }
       }

   }
}
