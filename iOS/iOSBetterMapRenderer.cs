using System;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms;
using FrillyToothpicksMap;
using FrillyToothpicksMap.iOS;
using MapKit;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using UIKit;
using CoreLocation;
using System.Linq;

[assembly: ExportRenderer(typeof(BetterMap), typeof(BetterMapRenderer))]
namespace FrillyToothpicksMap.iOS
{
    public class BetterMapRenderer : MapRenderer
    {
        BetterMap betterMap;
        MKMapView mapView;

        Dictionary<long, MKPolylineRenderer> lineRenderers = new Dictionary<long, MKPolylineRenderer>();
        List<MKPolyline> lineOverlays = new List<MKPolyline>();
        Dictionary<long, MKCircleRenderer> circleRenderers = new Dictionary<long, MKCircleRenderer>();
        List<MKCircle> circles = new List<MKCircle>();

        Dictionary<long, MKPolygonRenderer> polygonRenderers = new Dictionary<long, MKPolygonRenderer>();
        List<MKPolygon> polygons = new List<MKPolygon>();


        public BetterMapRenderer()
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                betterMap = e.NewElement as BetterMap;
                if (betterMap != null)
                {
                    mapView = Control as MKMapView;
                    betterMap.MapDataAdedEvent += BetterMap_MapDataAdedEvent;
                    betterMap.MapDataRemovedEvent += BetterMap_MapDataRemovedEvent;

                    mapView.OverlayRenderer = (m, o) =>
                    {
                        if (circleRenderers.ContainsKey((long)o.Handle))
                        {
                            var handle = (long)o.Handle;
                            return circleRenderers[handle];
                        }
                        if (lineRenderers.ContainsKey((long)o.Handle))
                        {
                            var handle = (long)o.Handle;
                            return lineRenderers[handle];
                        }
                        if (polygonRenderers.ContainsKey((long)o.Handle))
                        {
                            var handle = (long)o.Handle;
                            return polygonRenderers[handle];
                        }
                        return null;
                    };

                    if (betterMap.ReSend)
                    {
                        // We must reprocess these lines because they were hooked up before
                        // the events were wired in.
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
                }
            }
            else
            {
                // lets remove events here
                if (betterMap != null)
                {
                    betterMap.MapDataAdedEvent -= BetterMap_MapDataAdedEvent;
                    betterMap.MapDataRemovedEvent -= BetterMap_MapDataRemovedEvent;

                    foreach (var lineRenderer in lineRenderers.Values)
                    {
                        lineRenderer.Dispose();
                    }
                    foreach (var line in lineOverlays)
                    {
                        line.Dispose();
                    }
                    foreach (var circleRenderer in circleRenderers.Values)
                    {
                        circleRenderer.Dispose();
                    }
                    foreach (var circle in circles)
                    {
                        circle.Dispose();
                    }
                    foreach (var polygonRenderer in polygonRenderers.Values)
                    {
                        polygonRenderer.Dispose();
                    }
                    foreach (var polygon in polygons)
                    {
                        polygon.Dispose();
                    }
                }
            }
        }

        void BetterMap_MapDataRemovedEvent(BetterMapDrawing drawing)
        {
            var mapCircle = drawing as MapCircle;
            if (mapCircle != null)
            {                    
                var c = circles.Where(p => (long)p.Handle == mapCircle.Handle).FirstOrDefault();
                if (c != null)
                {
                    circles.Remove(c);
                }
                if (circleRenderers.ContainsKey(mapCircle.Handle))
                    circleRenderers.Remove(mapCircle.Handle);                
            }
            var mapLine = drawing as MapLine;
            if (mapLine != null)
            {
                var l = lineOverlays.Where(p => (long)p.Handle == mapLine.Handle).FirstOrDefault();
                if (l != null)
                {
                    lineOverlays.Remove(l);
                }
                if (lineRenderers.ContainsKey(mapLine.Handle))
                    lineRenderers.Remove(mapLine.Handle);
            }
            var mapPolygon = drawing as MapPolygon;
            if (mapPolygon != null)
            {
                var p = polygons.Where(x => (long)x.Handle == mapLine.Handle).FirstOrDefault();
                if (p != null)
                {
                    polygons.Remove(p);
                }
                if (polygonRenderers.ContainsKey(mapLine.Handle))
                    polygonRenderers.Remove(mapLine.Handle);
            }
        }

        void BetterMap_MapDataAdedEvent(BetterMapDrawing drawing)
        {
            if ((drawing as MapCircle) != null)
            {
                var mapCircle = drawing as MapCircle;
                var coord = ToCoord(mapCircle.Position);
                var circle = MKCircle.Circle(coord, mapCircle.Radius);
                circles.Add(circle);

                var circleRenderer = new MKCircleRenderer(circle);
                circleRenderer.FillColor = UIColor.FromRGB((nfloat)mapCircle.Color.R, (nfloat)mapCircle.Color.G, (nfloat)mapCircle.Color.B);
                //circleRenderer.FillColor = UIColor.Yellow;
                circleRenderer.Alpha = 1.0f;
                var handle = (long)circle.Handle;
                circleRenderers.Add(handle, circleRenderer);

                mapView.AddOverlay(circle);

                mapCircle.Handle = (long)circle.Handle;
            }
            else if ((drawing as MapLine) != null)
            {
                var mapLine = drawing as MapLine;
                var coords = ToCoords(mapLine.Positions);
                var line = MKPolyline.FromCoordinates(coords);
                lineOverlays.Add(line);

                var lineRenderer = new MKPolylineRenderer(line);
                lineRenderer.FillColor = UIColor.FromRGB((nfloat)mapLine.Color.R, (nfloat)mapLine.Color.G, (nfloat)mapLine.Color.B);
                lineRenderer.StrokeColor = UIColor.FromRGB((nfloat)mapLine.Color.R, (nfloat)mapLine.Color.G, (nfloat)mapLine.Color.B);
                //circleRenderer.FillColor = UIColor.Yellow;
                lineRenderer.Alpha = 1.0f;
                var handle = (long)line.Handle;
                lineRenderers.Add(handle, lineRenderer);

                mapView.AddOverlay(line);

                mapLine.Handle = (long)line.Handle;
            }
            else if ((drawing as MapPolygon) != null)
            {
                var mapPolygon = drawing as MapPolygon;
                var coords = ToCoords(mapPolygon.Positions);
                var polygon = MKPolygon.FromCoordinates(coords);
                polygons.Add(polygon);

                var polygonRenderer = new MKPolygonRenderer(polygon);
                polygonRenderer.FillColor = ToUIColor(mapPolygon.Color);
                polygonRenderer.StrokeColor = ToUIColor(mapPolygon.Color);
                var handle = (long)polygon.Handle;
                polygonRenderers.Add(handle, polygonRenderer);
                mapView.AddOverlay(polygon);

                mapPolygon.Handle = handle;
            }
        }

        CLLocationCoordinate2D ToCoord(Position position)
        {
            return new CLLocationCoordinate2D(position.Latitude, position.Longitude);
        }

        CLLocationCoordinate2D[] ToCoords(Position[] positions)
        {
            var result = positions.Select(x => new CLLocationCoordinate2D(x.Latitude, x.Longitude)).ToArray();
            return result;
        }

        UIColor ToUIColor(Color c)
        {
            return UIColor.FromRGB((nfloat)c.R, (nfloat)c.G, (nfloat)c.B);
        }

    }
}

