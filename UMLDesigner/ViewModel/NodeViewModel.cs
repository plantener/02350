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
    [System.Xml.Serialization.XmlInclude(typeof(NodeViewModel))]
    public class NodeViewModel : ViewModelBase
    {
        public Node node;

        public int Id
        {
            get { return node.Id; }
            set { node.Id = value; RaisePropertyChanged(() => Id); }
        }

        public int X
        {
            get { return node.X; }
            set { node.X = value; RaisePropertyChanged(() => X); RaisePropertyChanged(() => North); RaisePropertyChanged(() => South); RaisePropertyChanged(() => West); RaisePropertyChanged(() => East); }
        }

        public int Y
        {
            get { return node.Y; }
            set { node.Y = value; RaisePropertyChanged(() => Y); RaisePropertyChanged(() => North); RaisePropertyChanged(() => South); RaisePropertyChanged(() => West); RaisePropertyChanged(() => East); }
        }

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

        private bool selected = false;
        public bool Selected { get { return selected; } set { selected = value; RaisePropertyChanged(() => Selected); RaisePropertyChanged(() => SelectedColor); } }

        public String SelectedColor { get { return Selected ? "Gray" : "#2E8DEF"; } }

        public ObservableCollection<UMLDesigner.Model.Attribute> Attributes { get { return node.Attributes; } set { node.Attributes = value; RaisePropertyChanged(() => Attributes); } }
        public ObservableCollection<UMLDesigner.Model.Attribute> Methods { get { return node.Methods; } set { node.Methods = value; RaisePropertyChanged(() => Methods); } }

        public NodeViewModel()
        {
            node = new Node();
        }

        public NodeViewModel(Node node)
        {
            this.node = node;
        }
    }
}
