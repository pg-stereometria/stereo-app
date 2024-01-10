using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StereoApp.Model.Interfaces;
using UnityEngine;

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

        public override float TotalArea()
        {
            float sum = 0.0f;
            foreach(var polygon in Faces)
            {
                sum += polygon.CalculateArea();
            }
            return sum;
        }

        public override float Volume()
        {
            float sum = 0.0f;
            foreach (var polygon in Faces)
            {
                var triangles = polygon.GetTriangles();
                foreach (var triangle in triangles)
                {
                    sum += SignedVolumeOfTriangle(triangle[0].ToVector3(), triangle[1].ToVector3(), triangle[2].ToVector3());
                }
            }
            return sum;
        }

        private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var v321 = p3.x * p2.y * p1.z;
            var v231 = p2.x * p3.y * p1.z;
            var v312 = p3.x * p1.y * p2.z;
            var v132 = p1.x * p3.y * p2.z;
            var v213 = p2.x * p1.y * p3.z;
            var v123 = p1.x * p2.y * p3.z;

            var normal = Vector3.Cross(p2 - p1, p3 - p1);
            var origin = new Vector3(0, 0, 0);
            var multiplyer = -1;
            if(Vector3.Dot(normal,-1*p1) < 0)
                multiplyer = 1;
            return multiplyer * (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
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
