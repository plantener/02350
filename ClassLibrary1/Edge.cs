using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLDesigner.Model
{
    class Edge : ViewModelBase
    {
        private Node start;
        public Node Start
        {
            get { return start; }
            set { start = value; RaisePropertyChanged(() => Start); }
        }

        private Node end;
        public Node End
        {
            get { return end; }
            set { end = value; RaisePropertyChanged(() => End); }
        }

        private int startX;
        public int StartX
        {
            get { return startX; }
            set { startX = value; RaisePropertyChanged(() => StartX); }
        }

        private int startY;
        public int StartY
        {
            get { return startY; }
            set { startY = value; RaisePropertyChanged(() => StartY); }
        }

        private int endX;
        public int EndX
        {
            get { return endX; }
            set { endX = value; RaisePropertyChanged(() => EndX); }
        }

        private int endY;
        public int EndY
        {
            get { return endY; }
            set { endY = value; RaisePropertyChanged(() => EndY); }
        }
        
        private string multiplicityStart = "";
        public string MultiplicityStart
        {
            get { return multiplicityStart; }
            set { multiplicityStart = value; RaisePropertyChanged(() => MultiplicityStart); }
        }

        private string multiplicityEnd = "";
        public string MultiplicityEnd
        {
            get { return multiplicityEnd; }
            set { multiplicityEnd = value; RaisePropertyChanged(() => MultiplicityEnd); }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(() => Name); }
        }

        private string type = "Assosiation";

        public Edge(Node start, Node end)
        {
            this.start = start;
            this.end = end;

            startX = start.X;
            startY = start.Y;
            endX = end.X;
            endY = end.Y;
        }
    }
}
