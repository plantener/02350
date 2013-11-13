using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UMLDesigner.Model;

namespace UMLDesigner.ViewModel
{
    public class NodeViewModel : ViewModelBase
    {
        private Node node;

        public int X
        {
            get { return node.X; }
            set { node.X = value; RaisePropertyChanged(() => X); RaisePropertyChanged(() => CanvasCenterX); RaisePropertyChanged(() => North); RaisePropertyChanged(() => South); RaisePropertyChanged(() => West); RaisePropertyChanged(() => East); }
        }

        public int Y
        {
            get { return node.Y; }
            set { node.Y = value; RaisePropertyChanged(() => Y); RaisePropertyChanged(() => CanvasCenterY); RaisePropertyChanged(() => North); RaisePropertyChanged(() => South); RaisePropertyChanged(() => West); RaisePropertyChanged(() => East); }
        }

        public double CanvasCenterX { get { return node.X + Width / 2; } }
        public double CanvasCenterY { get { return node.Y + Height / 2; } }


        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; RaisePropertyChanged(() => Width); RaisePropertyChanged(() => North); RaisePropertyChanged(() => South); RaisePropertyChanged(() => West); RaisePropertyChanged(() => East); }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; RaisePropertyChanged(() => Height); RaisePropertyChanged(() => North); RaisePropertyChanged(() => South); RaisePropertyChanged(() => West); RaisePropertyChanged(() => East); }
        }

        private Point north;
        private Point south;
        private Point east;
        private Point west;

        public Point North      { get { north.X = node.X + Width / 2; north.Y = node.Y; return north; } set { north.X = node.X + Width / 2; north.Y = node.Y; RaisePropertyChanged(() => North); } }
        public Point South      { get { south.X = node.X + Width / 2; south.Y = node.Y + Height; return south; } set { south.X = node.X + Width / 2; south.Y = node.Y + Height; RaisePropertyChanged(() => South); } }
        public Point East       { get { east.X = node.X + Width; east.Y = node.Y + Height / 2; return east; } set { east.X = node.X + Width; east.Y = node.Y + Height / 2; RaisePropertyChanged(() => East); } }
        public Point West       { get { west.X = node.X; west.Y = node.Y + Height / 2; return west; } set { west.X = node.X; west.Y = node.Y + Height / 2; RaisePropertyChanged(() => West); } }

        public String ClassName
        {
            get { return node.ClassName; }
            set { node.ClassName = value; }
        }

        public ObservableCollection<UMLDesigner.Model.Attribute> Attributes { get { return node.Attributes; } set { node.Attributes = value; RaisePropertyChanged(() => Attributes); } }
        public ObservableCollection<string> Properties { get { return node.Properties; } set { node.Properties = value; RaisePropertyChanged(() => Properties); } }
        public ObservableCollection<UMLDesigner.Model.Attribute> Methods { get { return node.Methods; } set { node.Methods = value; RaisePropertyChanged(() => Methods); } }

        public NodeViewModel()
        {
            node = new Node();
        }

        public EdgeViewModel newEdge(NodeViewModel endA, string type)
        {
            EdgeViewModel newEdge = new EdgeViewModel(endA, this, endA.node, this.node, type);
            return newEdge;
        }

        public PointCollection getAnchor(NodeViewModel end, EdgeViewModel edge)
        {
            int d = 3;
            PointCollection temp = new PointCollection();
                //break
            if (X + Width / d > end.X + end.Width && Y + Height / d > end.Y + end.Height)
            {
                temp.Add(North);
                temp.Add(new Point(North.X, end.East.Y));
                temp.Add(end.East);
                edge.setAnchor("east");
            }
            else if (X + Width - Width / d < end.X && Y + Height / d > end.Y + end.Height)
            {
                temp.Add(East);
                temp.Add(new Point(end.South.X, East.Y));
                temp.Add(end.South);
                edge.setAnchor("south");
            }
            else if (X + Width - Width / d < end.X && Y + Height - Height / d < end.Y)
            {
                temp.Add(South);
                temp.Add(new Point(South.X, end.West.Y));
                temp.Add(end.West);
                edge.setAnchor("west");
            }
            else if (X + Width / d > end.X + end.Width && Y + Height - Height / d < end.Y)
            {
                temp.Add(West);
                temp.Add(new Point(end.North.X, West.Y));
                temp.Add(end.North);
                edge.setAnchor("north");
            }
                //Straight above and below
            else if (end.X + end.Width >= X + Width / d && end.X < X && Y > end.Y + end.Height)
            {
                double dTemp = (end.X + end.Width - X) / 2;
                temp.Add(new Point(end.X + end.Width - dTemp, Y));
                temp.Add(new Point(end.X + end.Width - dTemp, end.Y + end.Height));
                edge.setAnchor("south");
            }
            else if (end.X <= X + Width - Width / d && end.X > X && Y > end.Y + end.Height)
            {
                double dTemp = (X + Width - end.X) / 2;
                temp.Add(new Point(end.X + dTemp, Y));
                temp.Add(new Point(end.X + dTemp, end.Y + end.Height));
                edge.setAnchor("south");
            }
            else if (end.X + end.Width >= X + Width / d && end.X < X && Y + Height < end.Y)
            {
                double dTemp = (end.X + end.Width - X) / 2;
                temp.Add(new Point(end.X + end.Width - dTemp, Y + Height));
                temp.Add(new Point(end.X + end.Width - dTemp, end.Y));
                edge.setAnchor("north");
            }
            else if (end.X <= X + Width - Width / d && end.X > X && Y + Height < end.Y)
            {
                double dTemp = (X + Width - end.X) / 2;
                temp.Add(new Point(end.X + dTemp, Y + Height));
                temp.Add(new Point(end.X + dTemp, end.Y));
                edge.setAnchor("north");
            }
            //straight left and right
            else if (X > end.X + end.Width && Y + Height / d <= end.Y + end.Height && Y > end.Y)
            {
                double dTemp = (end.Y + end.Height - Y) / 2;
                temp.Add(new Point(X, Y + dTemp));
                temp.Add(new Point(end.X + end.Width, Y + dTemp));
                edge.setAnchor("east");
            }
            else if (X + Width < end.X && Y + Height / d <= end.Y + end.Height && Y > end.Y)
            {
                double dTemp = (end.Y + end.Height - Y) / 2;
                temp.Add(new Point(X + Width, Y + dTemp));
                temp.Add(new Point(end.X, Y + dTemp));
                edge.setAnchor("west");
            }
            else if (X > end.X + end.Width && Y + Height - Height / d >= end.Y && Y < end.Y)
            {
                double dTemp = (end.Y + end.Height - Y) / 2;
                temp.Add(new Point(X, Y + dTemp));
                temp.Add(new Point(end.X + end.Width, Y + dTemp));
                edge.setAnchor("east");
            }
            else if (X + Width < end.X && Y + Height - Height / d >= end.Y && Y < end.Y)
            {
                double dTemp = (end.Y + end.Height - Y) / 2;
                temp.Add(new Point(X + Width, Y + dTemp));
                temp.Add(new Point(end.X, Y + dTemp));
                edge.setAnchor("west");
            }
            return temp;
        }
    }
}
