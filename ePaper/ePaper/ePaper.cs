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
            graphics.DrawText(0, 0, "ePaper for Meadow");
            graphics.DrawText(0, 16, ".NET Conf 2019");
            graphics.DrawText(0, 32, "Wilderness Labs", Meadow.Foundation.Color.Yellow);

            graphics.Show();
        }
    }
}