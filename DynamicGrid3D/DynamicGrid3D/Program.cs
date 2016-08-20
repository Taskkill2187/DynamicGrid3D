using System;

namespace DynamicGrid3D
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (XNA_Interface game = new XNA_Interface())
            {
                game.Run();
            }
        }
    }
#endif
}

