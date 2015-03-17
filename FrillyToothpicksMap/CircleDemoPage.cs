using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FrillyToothpicksMap
{
    public class CirclePage : ContentPage
    {
        BetterMap map = new BetterMap();

        public CirclePage()
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

            map.AddCircle(new MapCircle(new Position(myLat, myLon), Color.Red, 20));

            map.AddCircle(new MapCircle(new Position(myLat + 0.001, myLon), Color.Blue, 20));

            map.AddCircle(new MapCircle(new Position(myLat - 0.001, myLon), Color.Green, 20));
        }
    }
}

