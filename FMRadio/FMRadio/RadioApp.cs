using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Audio.Radio;
using System.Threading.Tasks;

namespace BasicFMRadio
{
    public class RadioApp : App<F7Micro, RadioApp>
    {
        IDigitalOutputPort stereoLed;
        IDigitalInputPort muteButton;

        GraphicsLibrary display;

        TEA5767 radio;

        public RadioApp()
        {
            InitializeHardware();

            UpdateDisplay();

            MonitorButtons();
        }

        void UpdateDisplay ()
        {
            display.Clear();

            display.DrawText(0, 0, $"{radio.GetFrequency()}");

            display.DrawText(0, 10, (radio.IsStereo() ? "stereo" : "mono"));
            stereoLed.State = radio.IsStereo();

            display.DrawText(0, 20, radio.GetSignalLevel() + "db");

            display.DrawText(0, 30, radio.IsMuted ? "mute" : "on");

            display.Show();
        }

        void MonitorButtons()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (muteButton.State == true)
                    {
                        radio.IsMuted = !radio.IsMuted;

                    }

                    UpdateDisplay();

                    Thread.Sleep(500);
                }
            });
        }

        void InitializeHardware()
        {
            Console.WriteLine("Configuring hardware");
            stereoLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);

            Console.WriteLine("Create radio button");
            muteButton = Device.CreateDigitalInputPort(Device.Pins.D12);

            Console.WriteLine("Create Spi bus");

            var spiBus = Device.CreateSpiBus();

            Console.WriteLine("Create display");
            var st7565 = new ST7565(device: Device, spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D00,
                resetPin: Device.Pins.D01,
                width: 128, height: 64);

            st7565.SetContrast(5);

            Console.WriteLine("Create graphics lib");

            display = new GraphicsLibrary(st7565);
            display.CurrentFont = new Font8x8();

            Console.WriteLine("Create I2C bus");
            var i2cBus = Device.CreateI2cBus();

            Console.WriteLine("Create TEA5767 instance");
            radio = new TEA5767(i2cBus);

            Thread.Sleep(500); //quick test

            radio.SetFrequency(94.9f);
        }

        private void Button_Changed(object sender, DigitalInputPortEventArgs e)
        {
            Console.WriteLine("Button pressed");
            radio.SearchNextSilent();
            UpdateDisplay();
        }
    }
}