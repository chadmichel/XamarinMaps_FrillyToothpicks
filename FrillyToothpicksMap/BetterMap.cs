using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Collections.Generic;

namespace FrillyToothpicksMap
{
    public class BetterMap : Map
    {
        List<MapCircle> circles = new List<MapCircle>();
        List<MapLine> lines = new List<MapLine>();
        List<MapPolygon> polygons = new List<MapPolygon>();

        public BetterMap()
        {
            ReSend = false;
        }

        public void AddCircle(MapCircle circle)
        {
            if (circle != null)
            {
                circles.Add(circle);
                if (MapDataAdedEvent == null)
                    ReSend = true;
                else
                    MapDataAdedEvent(circle);
            }
        }

        public void ClearCircles()
        {
            if (MapDataRemovedEvent != null)
            {
                foreach (var circle in circles)
                {
                    MapDataRemovedEvent(circle);
                }
                circles.Clear();
            }
        }

        public void AddLine(MapLine line)
        {
            if (line != null)
            {
                lines.Add(line);
                if (MapDataAdedEvent == null)
                    ReSend = true;
                else
                    MapDataAdedEvent(line);
            }
        }

        public void ClearLines()
        {
            if (MapDataRemovedEvent != null)
            {
                foreach (var line in lines)
                {
                    MapDataRemovedEvent(line);
                }
                lines.Clear();
            }
        }

        public void AddPolygon(MapPolygon polygon)
        {
            if (polygon != null)
            {
                polygons.Add(polygon);
                if (MapDataAdedEvent == null)
                    ReSend = true;
                else
                    MapDataAdedEvent(polygon);
            }
        }

        public void ClearPolygons()
        {
            if (MapDataRemovedEvent != null)
            {
                foreach (var poly in polygons)
                {
                    MapDataRemovedEvent(poly);
                }
                polygons.Clear();
            }
        }

        public delegate void MapDataAdded(BetterMapDrawing drawing);

        public delegate void MapDataRemoved(BetterMapDrawing drawing);

        public event MapDataAdded MapDataAdedEvent;
        public event MapDataRemoved MapDataRemovedEvent;

        public bool ReSend { get; set; }

        public MapCircle[] Circles { get { return circles.ToArray(); } }

        public MapLine[] Lines { get { return lines.ToArray(); } }

        public MapPolygon[] Polygons { get { return polygons.ToArray(); } }
    }

    public abstract class BetterMapDrawing
    {
        public long Handle { get; set; }
    }

    public class MapCircle : BetterMapDrawing
    {
        public Position Position { get; private set; }

        public Color Color { get; private set; }

        public double Radius { get; private set; }

        public MapCircle(Position position, Color color, double radius)
        {
            Position = position;
            Color = color;
            Radius = radius;
        }
    }

    public class MapLine : BetterMapDrawing
    {
        public Position[] Positions { get; private set; }

        public Color Color { get; private set; }

        public double Radius { get; private set; }

        public MapLine(Position[] positions, Color color, double radius)
        {
            Positions = positions;
            Color = color;
            Radius = radius;
        }
    }

    public class MapPolygon : BetterMapDrawing
    {
        public Position[] Positions { get; private set; }

        public Color Color { get; private set; }

        public MapPolygon(Position[] positions, Color color)
        {
            Positions = positions;
            Color = color;
        }
    }
}

