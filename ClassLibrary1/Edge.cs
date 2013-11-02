using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLDesigner.Model
{
    public class Edge : ViewModelBase
    {
        private Node start;
        public Node Start
        {
            get
            {
                //Console.WriteLine(Path);
                return start; }
            set
            {
                start = value;
                newPath(Start, End);
                RaisePropertyChanged(() => Start);
                RaisePropertyChanged(() => Path);
            }
        }

        private Node end;
        public Node End
        {
            get { return end; }
            set 
            {
                end = value;
                RaisePropertyChanged(() => End);
                RaisePropertyChanged(() => Path);
            }
        }

        private String path;
        public String Path
        {
            get { return path; }
            set
            {
                path = value;
                newPath(Start, End); RaisePropertyChanged(() => Path);
            }
        }

        public ObservableCollection<int> PathX { get; set; }
        public ObservableCollection<int> PathY { get; set; }

        public Edge(Node _start, Node _end)
        {
            Start = _start;
            End = _end;
            PathX = new ObservableCollection<int>();
            PathY = new ObservableCollection<int>();

            newPath(Start, End);
        }

        private void newPath(Node _start, Node _end)
        {
            if (PathX != null && PathY != null)
            {
                PathX.Clear();
                PathY.Clear();
               // Console.WriteLine(Start.X + Start.Width / 2);
               // Console.WriteLine(Start.X);
                PathX.Add(Start.X + Start.Width / 2);
                PathY.Add(Start.Y + Start.Height / 2);

                PathX.Add(Start.X + Start.Width / 2);
                PathY.Add(End.Y + End.Height / 2);

                PathX.Add(End.X + End.Width / 2);
                PathY.Add(End.Y + End.Height / 2);


                setPath();
            }
        }

        private void setPath()
        {
            path = "M";
            for (int i = 0; i < PathX.Count; i++)
            {
                path += " " + PathX[i] + "," + PathY[i];
                Console.WriteLine(Path);
            }
            Console.WriteLine(Path);
        }
    }
}
