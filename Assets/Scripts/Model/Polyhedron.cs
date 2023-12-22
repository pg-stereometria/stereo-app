using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    // name of this class is a simplification - we do not verify in any way that it represents a closed figure
    public class Polyhedron : SolidFigure, ISerializableTo<Polyhedron, SerializedPolyhedron>
    {
        public ISet<Point> Points => new HashSet<Point>(Faces.SelectMany(polygon => polygon));
        public ObservableCollection<Polygon> Faces { get; }

        public Polyhedron()
        {
            Faces = new ObservableCollection<Polygon>();
        }

        public Polyhedron(IEnumerable<Polygon> collection)
        {
            Faces = new ObservableCollection<Polygon>(collection);
        }

        public override SerializedSolidFigure ToSerializableFigure()
        {
            return ToSerializable();
        }

        public SerializedPolyhedron ToSerializable()
        {
            return new SerializedPolyhedron
            {
                faces = Faces.Select(polygon => polygon.ToSerializable()).ToList()
            };
        }

        public Point CalculateMidpoint()
        {
            var sum = new Point(0, 0, 0);
            foreach (var point in Points)
            {
                sum += point;
            }
            var midpoint = sum / Points.Count;
            return midpoint;
        }
    }

    [Serializable]
    public class SerializedPolyhedron
        : SerializedSolidFigure,
            ISerializableFrom<SerializedPolyhedron, Polyhedron>
    {
        public List<SerializedPolygon> faces;

        public override SolidFigure ToActualFigure()
        {
            return ToActualType();
        }

        public Polyhedron ToActualType()
        {
            return new Polyhedron(faces.Select(polygon => polygon.ToActualType()));
        }
    }
}
