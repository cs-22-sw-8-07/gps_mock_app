using gps_mock_app.Controllers;
using gps_mock_app.Models;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace gps_mock_app.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Variables

        private double? latitude;
        private double? longitude;
        private Thread mockThread;

        #endregion

        #region Constructor

        public MainViewModel()
        {
            //Latitude = 50.423423432;
            //Longitude = 10.534543543;

            StartMock();
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            RunMock = false;
        }

        public void SetupMapControl()
        {
            CurrentWindow.mapControl.MouseDoubleClick += (sender, args) =>
            {
                var map = sender as Map;

                // Disables the default mouse double-click action.
                args.Handled = true;

                // Determin the location to place the pushpin at on the map.

                //Get the mouse click coordinates
                Point mousePosition = args.GetPosition(map);
                //Convert the mouse coordinates to a locatoin on the map
                Location pinLocation = map.ViewportPointToLocation(mousePosition);

                Latitude = pinLocation.Latitude;
                Longitude = pinLocation.Longitude;

                // The pushpin to add to the map.
                Pushpin pin = new Pushpin();
                pin.Location = pinLocation;

                // Adds the pushpin to the map.
                map.Children.Clear();
                map.Children.Add(pin);
            };
        }

        private void StartMock()
        {
            mockThread = new Thread(() =>
            {
                while (RunMock)
                {
                    if (Latitude != null && Longitude != null)
                    {
                        string message = JsonConvert.SerializeObject(new LocationDTO(Latitude.Value, Longitude.Value));
                        UdpController.SendMessage(message);
                    }
                    Thread.Sleep(1000);
                }
            });
            mockThread.Start();
        }

        #endregion

        #region Properties

        public bool RunMock { get; set; } = true;
        public MainWindow CurrentWindow { get; set; }

        public double? Latitude
        {
            get => latitude;
            set
            {
                SetValue(ref latitude, value);
                OnPropertyChanged(nameof(CurrentLocationString));
            }
        }
        public double? Longitude
        {
            get => longitude;
            set
            {
                SetValue(ref longitude, value);
                OnPropertyChanged(nameof(CurrentLocationString));
            }
        }
        public string CurrentLocationString
        {
            get
            {
                return Latitude == null || Longitude == null ? "No location picked" : $"{Latitude.ToString().Replace(",", ".")}, {Longitude.ToString().Replace(",", ".")}";
            }
        }

        #endregion
    }
}
