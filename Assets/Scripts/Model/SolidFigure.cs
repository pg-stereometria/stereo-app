using System.Collections.ObjectModel;

namespace StereoApp.Model
{
    // name of this class is a simplification - we do not verify in any way that it represents a closed figure
    public class SolidFigure : ObservableCollection<Polygon> { }
}
