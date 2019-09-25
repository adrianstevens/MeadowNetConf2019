using System;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.ePaper;
using Meadow.Foundation.Graphics;

namespace BasicFMRadio
{
    public class ePaper : App<F7Micro, ePaper>
    {
        EPD2i9b display;
        GraphicsLibrary graphics;

        public ePaper()
        {
            InitializeHardware();

            UpdateDisplay();
        }


        void InitializeHardware()
        {
            Console.WriteLine("Configuring hardware");

            Console.WriteLine("Create display driver instance");
            var spiBus = Device.CreateSpiBus();

            display = new EPD2i9b(device: Device, spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                busyPin: Device.Pins.D03);

            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font8x12();
        }

        void UpdateDisplay()
        { 
            //any color but black will show the ePaper alternate color 
            graphics.DrawRectangle(0, 0, 128, 34, Meadow.Foundation.Color.Red, false);

            graphics.DrawText(2, 2, ".NET Conf 2019");
            graphics.DrawText(2, 20, "Meadow F7");

            int ySpacing = 6;

            for (int i = 0; i < 3; i++)
            {
                graphics.DrawLine(2, 70 + ySpacing * i, 22, 50 + ySpacing * i);
                graphics.DrawLine(22, 50 + ySpacing * i, 42, 70 + ySpacing * i);
                graphics.DrawLine(44, 70 + ySpacing * i, 64, 50 + ySpacing * i);
                graphics.DrawLine(64, 50 + ySpacing * i, 84, 70 + ySpacing * i);
                graphics.DrawLine(86, 70 + ySpacing * i, 106, 50 + ySpacing * i);
                graphics.DrawLine(106, 50 + ySpacing * i, 126, 70 + ySpacing * i);
            }

            Console.WriteLine("Show");

            graphics.Show();
        }
    }
}