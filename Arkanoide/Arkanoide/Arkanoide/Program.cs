using System;

namespace Arkanoide
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Arkanoide game = new Arkanoide())
            {
                game.Run();
            }
        }
    }
#endif
}

