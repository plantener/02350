using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using UMLDesigner.Utilities;

namespace UMLDesigner.View
{
    /// <summary>
    /// Interaction logic for GridWithRulerxaml.xaml
    /// </summary>
    public partial class GridWithRulerxaml : UserControl
    {
        public GridWithRulerxaml()
        {
            InitializeComponent();

            //Loaded event is necessary as Adorner is null until control is shown.
            Loaded += GridWithRulerxaml_Loaded;

        }

        void GridWithRulerxaml_Loaded(object sender, RoutedEventArgs e)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            var rulerAdorner = new GridAdorner(this);
            adornerLayer.Add(rulerAdorner);
        }
    }
}