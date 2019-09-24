using System.Threading;
using Meadow;

namespace BasicFMRadio
{
    class Program
    {
        static IApp app;
        public static void Main(string[] args)
        {
            // instantiate and run new meadow app
            app = new ePaper();

            Thread.Sleep(-1);
        }
    }
}