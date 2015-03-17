using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FrillyToothpicksMap
{
    public class PolygonPage : ContentPage
    {
        BetterMap map = new BetterMap();

        public PolygonPage()
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
                    new Position(myLat, myLon + 0.001),
                    new Position(myLat - 0.001, myLon),
                };

            var polygon = new MapPolygon(points, Color.Purple);
                    
            map.AddPolygon(polygon);
        }
    }
}

