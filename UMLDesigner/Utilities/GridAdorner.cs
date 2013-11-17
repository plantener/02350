using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace UMLDesigner.Utilities
{
    public class GridAdorner : Adorner
    {
        private const int LINEFACTOR = 20;
        private FrameworkElement element;
        public GridAdorner(UIElement el)
            : base(el)
        {
            element = el as FrameworkElement;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            double height = element.ActualHeight;
            double width = element.ActualWidth;

            double linesHorizontal = height / LINEFACTOR;
            double linesVertical = width / LINEFACTOR;

            var pen = new Pen(Brushes.Blue, 0.1) { StartLineCap = PenLineCap.Triangle, EndLineCap = PenLineCap.Triangle };

            int offset = 0;

            for (int i = 0; i <= linesVertical; ++i)
            {
                offset = offset + LINEFACTOR;
                drawingContext.DrawLine(pen, new Point(offset, 0), new Point(offset, height));
            }

            offset = 0;

            for (int i = 0; i <= linesHorizontal; ++i)
            {
                offset = offset + LINEFACTOR;
                drawingContext.DrawLine(pen, new Point(0, offset), new Point(width, offset));
            }
        }
    }
}