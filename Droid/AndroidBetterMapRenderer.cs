using System;
using Xamarin.Forms;
using FrillyToothpicksMap.Droid;
using FrillyToothpicksMap;
using Xamarin.Forms.Maps.Android;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BetterMap), typeof(BetterMapRenderer))]
namespace FrillyToothpicksMap.Droid
{
    public class BetterMapRenderer : MapRenderer
    {

        /// <summary>
        /// Android has a strange callback object to get an actual copy of the map.
        /// There is another way to do this, but it is deprecated, so this is probably
        /// the better way.
        /// </summary>
        class MapReady : Java.Lang.Object, IOnMapReadyCallback
        {
            Action<GoogleMap> _mapReady;

            public MapReady(Action<GoogleMap> mapReady)
            {
                _mapReady = mapReady;
            }

            public void OnMapReady(GoogleMap googleMap)
            {
                _mapReady(googleMap);
            }
        }


        BetterMap betterMap;
        Android.Gms.Maps.MapView mapView;
        GoogleMap googleMap;

        public BetterMapRenderer()
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                betterMap = e.NewElement as BetterMap;
                if (betterMap != null)
                {
                    mapView = Control as Android.Gms.Maps.MapView;

                    mapView.GetMapAsync(new MapReady((gMap) =>
                            {
                                // get the real google map
                                googleMap = gMap;

                                betterMap.MapDataAdedEvent += BetterMap_MapDataAdedEvent;
                                betterMap.MapDataRemovedEvent += BetterMap_MapDataRemovedEvent;


                                if (betterMap.ReSend)
                                {
                                    // We must reprocess these lines because they were hooked up before
                                    // the events were wired in.
                                    Redraw();
                                }
                            }));
                }
            }
            else
            {
                // lets remove events here
                if (betterMap != null)
                {
                    betterMap.MapDataAdedEvent -= BetterMap_MapDataAdedEvent;
                    betterMap.MapDataRemovedEvent -= BetterMap_MapDataRemovedEvent;                       
                }                    
            }
        }

        void Redraw()
        {            
            foreach (var c in betterMap.Circles)
            {
                BetterMap_MapDataAdedEvent(c);
            }

            foreach (var l in betterMap.Lines)
            {
                BetterMap_MapDataAdedEvent(l);
            }

            foreach (var p in betterMap.Polygons)
            {
                BetterMap_MapDataAdedEvent(p);
            }
        }

        void BetterMap_MapDataRemovedEvent(BetterMapDrawing drawing)
        {
            googleMap.Clear();
            Redraw();
        }

        void BetterMap_MapDataAdedEvent(BetterMapDrawing drawing)
        {
            if (drawing as MapLine != null)
            {
                var mapLine = drawing as MapLine;
                var line = new PolylineOptions();
                line.InvokeColor(mapLine.Color.ToAndroid());
                foreach (var p in mapLine.Positions)
                {
                    line.Add(ToLatLng(p));   
                }
                googleMap.AddPolyline(line);
            }
            else if (drawing as MapCircle != null)
            {
                var mapCircle = drawing as MapCircle;
                var circle = new CircleOptions();
                circle.InvokeCenter(ToLatLng(mapCircle.Position));
                circle.InvokeFillColor(mapCircle.Color.ToAndroid());
                circle.InvokeStrokeColor(mapCircle.Color.ToAndroid());
                circle.InvokeRadius(mapCircle.Radius);
                googleMap.AddCircle(circle);
            }
            else if (drawing as MapPolygon != null)
            {
                var mapPolygon = drawing as MapPolygon;
                var polygon = new PolygonOptions();
                foreach (var p in mapPolygon.Positions)
                {
                    polygon.Add(ToLatLng(p));   
                }
                polygon.InvokeStrokeColor(mapPolygon.Color.ToAndroid());
                polygon.InvokeFillColor(mapPolygon.Color.ToAndroid());
                googleMap.AddPolygon(polygon);
            }
        }

        LatLng ToLatLng(Position p)
        {
            return new LatLng(p.Latitude, p.Longitude);
        }
    }
}

