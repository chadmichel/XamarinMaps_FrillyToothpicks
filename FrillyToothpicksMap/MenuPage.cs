using System;
using Xamarin.Forms;

namespace FrillyToothpicksMap
{
    public class MenuPage : ContentPage
    {
        public MenuPage()
        {

            var markers = new Button() { Text = "Markers" };
            markers.Clicked += async (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new MarkersPage());
            };

            var circles = new Button() { Text = "Circle" };
            circles.Clicked += async (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new CirclePage());
            };

            var lines = new Button() { Text = "Lines" };
            lines.Clicked += async (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new LinePage());
            };

            var polygons = new Button() { Text = "Polygons" };
            polygons.Clicked += async (object sender, EventArgs e) =>
            {
                Navigation.PushAsync(new PolygonPage());
            };
            
            Content = new StackLayout
            {
                Children =
                {
                    markers,
                    circles,
                    lines,
                    polygons
                }
            };
        }
    }
}

