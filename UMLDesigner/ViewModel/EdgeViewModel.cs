using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UMLDesigner.Model;

namespace UMLDesigner.ViewModel
{
    public class EdgeViewModel : ViewModelBase
    {
        private Edge edge;

        public Node Start { get { return edge.Start; } set { edge.Start = value; newPath(); RaisePropertyChanged(() => Start); RaisePropertyChanged(() => Path); } }
        public Node End { get { return edge.End; } set { edge.End = value; newPath(); RaisePropertyChanged(() => End); RaisePropertyChanged(() => Path); } }

        private String path;
        public String Path { get { return path; } set { path = value; RaisePropertyChanged(() => Path); } }

        private PointCollection pathObjects = new PointCollection();

        public EdgeViewModel(Node start, Node end)
        {
            edge = new Edge();
            edge.Start = start;
            edge.End = end;
            Console.WriteLine(End.Y);
            newPath();
        }

        private void newPath()
        {
            if (pathObjects != null)
            {
                pathObjects.Clear();

                pathObjects.Add(new Point(Start.X + Start.Width / 2, Start.Y + Start.Height / 2));
                pathObjects.Add(new Point(Start.X + Start.Width / 2, End.Y + End.Height / 2));
                pathObjects.Add(new Point(End.X + End.Width / 2, End.Y + End.Height / 2));

                setPath();
            }
        }

        private void setPath()
        {
            path = "M";
            for (int i = 0; i < pathObjects.Count; i++)
            {
                path += " " + pathObjects.ElementAt(i).X + "," + pathObjects.ElementAt(i).Y;
                Console.WriteLine(Path);
            }
            Console.WriteLine(Path);
        }
    }
}
