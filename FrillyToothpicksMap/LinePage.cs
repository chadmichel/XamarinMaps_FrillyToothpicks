using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FrillyToothpicksMap
{
    public class LinePage : ContentPage
    {
        BetterMap map = new BetterMap();

        public LinePage()
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

            var points = new Position[]
            {
                new Position(myLat + 0.001, myLon),
                new Position(myLat, myLon),
                new Position(myLat - 0.001, myLon),
            };

            var mapLine = new MapLine(points, Color.Red);
            map.AddLine(mapLine);

            var points2 = new Position[]
                {
                    new Position(myLat + 0.001, myLon + 0.001),
                    new Position(myLat, myLon + 0.001),
                    new Position(myLat - 0.001, myLon + 0.001),
                };

            var mapLine2 = new MapLine(points2, Color.Blue);
            map.AddLine(mapLine2);

            var points3 = new Position[]
                {
                    new Position(myLat + 0.001, myLon - 0.001),
                    new Position(myLat, myLon - 0.001),
                    new Position(myLat - 0.001, myLon - 0.001),
                };

            var mapLine3 = new MapLine(points3, Color.Green);
            map.AddLine(mapLine3);
        }
    }
}

