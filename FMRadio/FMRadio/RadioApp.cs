using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Audio.Radio;
using Meadow.Foundation.Sensors.Buttons;

namespace BasicFMRadio
{
    public class RadioApp : App<F7Micro, RadioApp>
    {
        IDigitalOutputPort stereoLed;

        GraphicsLibrary display;

        TEA5767 radio;

        PushButton buttonMute, buttonNext;

        public RadioApp()
        {
            InitializeHardware();
        }

        void UpdateDisplay ()
        {
            display.Clear();

            display.DrawText(0, 0, $"{radio.GetFrequency()}");

            display.DrawText(0, 10, (radio.IsStereo() ? "stereo" : "mono"));
            stereoLed.State = radio.IsStereo();

            display.DrawText(0, 20, radio.GetSignalLevel() + "db");

            if(radio.IsStereo())
            {
                display.DrawText(0, 30, "muted");
            }

            display.Show();
        }

        public void InitializeHardware()
        {
            Console.WriteLine("Configuring hardware");
            stereoLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);

            Console.WriteLine("Create buttons");
            buttonMute = new PushButton(Device, Device.Pins.D14);
            buttonNext = new PushButton(Device, Device.Pins.D15);

            buttonMute.Clicked += ButtonMute_Clicked;
            buttonNext.Clicked += ButtonNext_Clicked;

            Console.WriteLine("Create Spi bus");

            var spiBus = Device.CreateSpiBus();

            Console.WriteLine("Create display");
            var st7565 = new ST7565(device: Device, spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D00,
                resetPin: Device.Pins.D01,
                width: 128, height: 64);

            st7565.SetContrast(10);

            Console.WriteLine("Create graphics lib");

            display = new GraphicsLibrary(st7565);
            display.CurrentFont = new Font8x8();

            Console.WriteLine("Create I2C bus");
            var i2cBus = Device.CreateI2cBus();

            Console.WriteLine("Create TEA5767 instance");
            radio = new TEA5767(i2cBus);
        }

        private void ButtonMute_Clicked(object sender, EventArgs e)
        {
            radio.IsMuted = !radio.IsMuted;
            UpdateDisplay();
        }

        private void ButtonNext_Clicked(object sender, EventArgs e)
        {
            radio.SearchNextSilent();
            UpdateDisplay();
        }
    }
}