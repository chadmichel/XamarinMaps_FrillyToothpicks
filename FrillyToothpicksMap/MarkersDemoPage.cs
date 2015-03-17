using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FrillyToothpicksMap
{
    public class MarkersPage : ContentPage
    {
        BetterMap map = new BetterMap();

        public MarkersPage()
        {
            Content = new StackLayout()
            {
                Children =
                {
                    map
                }
            };

            var myLat = 40.8106;
            var myLon = -96.6803;

            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(myLat, myLon), Distance.FromMiles(0.3)));

            map.Pins.Add(new Pin()
                {
                    Position = new Position(myLat, myLon),
                    Type = PinType.SearchResult,
                    Label = "something",
                });
        }
    }
}

