using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    // name of this class is a simplification - we do not verify in any way that it represents a closed figure
    public class SolidFigure
        : ObservableCollection<Polygon>,
            ISerializableTo<SolidFigure, SerializedSolidFigure>
    {
        public ISet<Point> Points => new HashSet<Point>(this.SelectMany(polygon => polygon));

        public SolidFigure() { }

        public SolidFigure(IEnumerable<Polygon> collection)
            : base(collection) { }

        public SolidFigure(List<Polygon> list)
            : base(list) { }

        public SerializedSolidFigure ToSerializable()
        {
            return new SerializedSolidFigure
            {
                polygons = this.Select(polygon => polygon.ToSerializable()).ToList()
            };
        }
    }

    [Serializable]
    public class SerializedSolidFigure : ISerializableFrom<SerializedSolidFigure, SolidFigure>
    {
        public List<SerializedPolygon> polygons;

        public SolidFigure ToActualType()
        {
            return new SolidFigure(polygons.Select(polygon => polygon.ToActualType()));
        }
    }
}
