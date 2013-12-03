using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using UMLDesigner.Model;

namespace UMLDesigner.ViewModel
{
    public class EdgeViewModel : ViewModelBase
    {
        private Edge edge;

        private NodeViewModel nVMEndA;
        public NodeViewModel NVMEndA { get { return nVMEndA; }
            set { nVMEndA = value; RaisePropertyChanged(() => NVMEndA); RaisePropertyChanged(() => Path); }
        }

        private NodeViewModel nVMEndB;
        public NodeViewModel NVMEndB { get { return nVMEndB; }
            set { nVMEndB = value; RaisePropertyChanged(() => NVMEndB); RaisePropertyChanged(() => Path); }
        }
        
        public string MultA
        {
            get { return edge.MultA; }
            set { edge.MultA = value; RaisePropertyChanged(() => MultA); }
        }

        public string MultB
        {
            get { return edge.MultB; }
            set { edge.MultB = value; RaisePropertyChanged(() => MultB); }
        }

        private Point xMultA;
        private Point xMultB;

        public Point XMultA
        {
            get { return xMultA; }
            set { xMultA = value; RaisePropertyChanged(() => XMultA); }
        }

        public Point XMultB
        {
            get { return xMultB; }
            set { xMultB = value; RaisePropertyChanged(() => XMultB); }
        }

        public string Name
        {
            get { return edge.Name; }
            set { edge.Name = value; RaisePropertyChanged(() => Name); }
        }
        
        private Point posName;

        public Point PosName
        {
            get { return posName; }
            set { posName = value; RaisePropertyChanged(() => PosName); }
        }
        private bool multAllowed;
        public bool MultAllowed
        {
            get { return multAllowed; }
            set { multAllowed = value; RaisePropertyChanged(() => MultAllowed); }
        }

        private int multBorder;
        public int MultBorder
        {
            get { return multBorder; }
            set { multBorder = value; RaisePropertyChanged(() => MultBorder); }
        }

        private EdgeType Type { get { return edge.Type; } set { edge.Type = value; RaisePropertyChanged(() => Type); } }
        public EdgeType type
        {
            get { return Type; }
            set { Type = value; RaisePropertyChanged(() => type); }
        }
        private string colorFill;
        public string ColorFill { get { return colorFill; } set { colorFill = value; RaisePropertyChanged(() => ColorFill); } }

        private string dashed;
        public string Dashed { get { return dashed; } set { dashed = value; RaisePropertyChanged(() => Dashed); } }

        private string path;
        public string Path { get { return path; } set { path = value; RaisePropertyChanged(() => Path); } }

        private string arrow;
        public string Arrow { get { return arrow; } set { arrow = value; RaisePropertyChanged(() => Arrow); } }

        private string newAnchor;
        private string oldAnchor = "";
        private int angle;

        private PointCollection pathObjects = new PointCollection();

        private PointCollection normArrow = new PointCollection();
        private PointCollection genArrow = new PointCollection();
        private PointCollection rombArrow = new PointCollection();
        private PointCollection thisArrow;
        private PointCollection rArrow;

        public EdgeViewModel(){}

        public EdgeViewModel(NodeViewModel nVMEndA, NodeViewModel nVMEndB, EdgeType type)
        {
            edge = new Edge();
            NVMEndA = nVMEndA;
            NVMEndB = nVMEndB;
            MultA = "";
            MultB = "";
            Type = edgeTypeConverter(type);
            initArrow();
            newPath();
        }

        public void newPath()
        {
            pathObjects = getAnchor();
            rotateArrow();
            setPath();
            setArrow();
            //if (pathObjects.Count != 0)
            //{
            //    XMultA = pathObjects.ElementAt(0);
            //    XMultB = pathObjects.ElementAt(pathObjects.Count - 1);
            //}
        }

        private void initArrow()
        {
            //Normal arrow
            normArrow.Add(new Point(-5, 10));
            normArrow.Add(new Point(0, 0));
            normArrow.Add(new Point(5, 10));
            //Generalization
            genArrow.Add(new Point(-5, 10));
            genArrow.Add(new Point(0, 0));
            genArrow.Add(new Point(5, 10));
            genArrow.Add(new Point(-5, 10));
            //Rhombus
            rombArrow.Add(new Point(-5, 10));
            rombArrow.Add(new Point(0, 0));
            rombArrow.Add(new Point(5, 10));
            rombArrow.Add(new Point(0, 20));
            rombArrow.Add(new Point(-5, 10));

        }

        private void setPath()
        {
            string temp = "M";
            for (int i = 0; i < pathObjects.Count; i++)
            {
                temp += " " + pathObjects.ElementAt(i).X + "," + pathObjects.ElementAt(i).Y;
            }
            Path = temp;
        }

        private void setArrow()
        {
            string temp = "";
            if (!Type.Equals(EdgeType.NOR) && pathObjects.Count != 0)
            {
                for (int i = 0; i < thisArrow.Count; i++)
                {
                    temp += " " + (pathObjects.ElementAt(pathObjects.Count - 1).X + rArrow.ElementAt(i).X) +
                        "," + (pathObjects.ElementAt(pathObjects.Count - 1).Y + rArrow.ElementAt(i).Y);
                    
                }
                Arrow = temp;
            }
        }

        private EdgeType edgeTypeConverter(EdgeType type)
        {
            switch (type)
            {
                case EdgeType.AGG:
                    thisArrow = rombArrow;
                    dashed = "1 0";
                    colorFill = "White";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.AGG;
                case EdgeType.ASS:
                    thisArrow = normArrow;
                    dashed = "1 0";
                    colorFill = "Transperant";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.ASS;
                case EdgeType.COM:
                    thisArrow = rombArrow;
                    dashed = "1 0";
                    colorFill = "#2E8DEF";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.COM;
                case EdgeType.DEP:
                    thisArrow = normArrow;
                    dashed = "5 5";
                    colorFill = "Transperant";
                    multAllowed = false;
                    multBorder = 0;
                    return EdgeType.DEP;
                case EdgeType.GEN:
                    thisArrow = genArrow;
                    dashed = "1 0";
                    colorFill = "White";
                    multAllowed = false;
                    multBorder = 0;
                    return EdgeType.GEN;
                case EdgeType.NOR:
                    thisArrow = new PointCollection();
                    dashed = "1 0";
                    colorFill = "Transperant";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.NOR;
                default:
                    thisArrow = normArrow;
                    dashed = "1 0";
                    colorFill = "Transperant";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.ASS;
            }
        }

        public void rotateArrow()
        {
            if(oldAnchor != newAnchor){
                oldAnchor = newAnchor;
                double cTheta = Math.Cos(angle*(Math.PI/180));
                double sTheta = Math.Sin(angle*(Math.PI/180));
                PointCollection temp = new PointCollection();
                for (int i = 0; i < thisArrow.Count; i++ )
                {
                    double x = (int) ((thisArrow.ElementAt(i).X - 0) * cTheta - (thisArrow.ElementAt(i).Y - 0) * sTheta);
                    double y = (int) ((thisArrow.ElementAt(i).X - 0) * sTheta + (thisArrow.ElementAt(i).Y - 0) * cTheta);
                    temp.Add(new Point(x,y));
                }
                rArrow = temp;
            }
        }

        public void setAnchor(string anchor)
        {
            if (anchor == "north")
                angle = 180;
            else if (anchor == "south")
                angle = 0;
            else if (anchor == "west")
                angle = 90;
            else if (anchor == "east")
                angle = 270;
            this.newAnchor = anchor;
        }

        private PointCollection getAnchor()
        {
            int lengthHalf;
            PointCollection temp = new PointCollection();
            int a = 5, h = 20, l = 25;
            if (NVMEndA.West.X - 30 <= NVMEndB.East.X && NVMEndA.West.X + 30 >= NVMEndB.East.X && (NVMEndA.North.Y >= NVMEndB.South.Y || NVMEndA.South.Y <= NVMEndB.North.Y))
            {
                if (NVMEndA.North.Y >= NVMEndB.South.Y)
                {
                    temp.Add(NVMEndA.North);
                    XMultA = new Point(NVMEndA.North.X - l - a, NVMEndA.North.Y - h - a);
                }
                else
                {
                    temp.Add(NVMEndA.South);
                    XMultA = new Point(NVMEndA.South.X + a, NVMEndA.South.Y + a);
                }
                temp.Add(new Point(NVMEndA.North.X, NVMEndB.East.Y));
                temp.Add(NVMEndB.East);
                setAnchor("east");
                XMultB = new Point(NVMEndB.East.X + a, NVMEndB.East.Y - h - a);

            }
            else if (NVMEndB.West.X - 30 <= NVMEndA.East.X && NVMEndB.West.X + 30 >= NVMEndA.East.X && (NVMEndA.North.Y > NVMEndB.South.Y || NVMEndA.South.Y < NVMEndB.North.Y))
            {
                temp.Add(NVMEndA.East);
                if (NVMEndA.North.Y >= NVMEndB.South.Y)
                {
                    temp.Add(new Point(NVMEndB.South.X, NVMEndA.East.Y));
                    temp.Add(NVMEndB.South);
                    setAnchor("south");
                    XMultB = new Point(NVMEndB.South.X + a, NVMEndB.South.Y + a);
                }
                else
                {
                    temp.Add(new Point(NVMEndB.North.X, NVMEndA.East.Y));
                    temp.Add(NVMEndB.North);
                    setAnchor("north");
                    XMultB = new Point(NVMEndB.North.X - l - a, NVMEndB.North.Y - h - a);
                }
                XMultA = new Point(NVMEndA.East.X + a, NVMEndA.East.Y - h - a);

            }
            else if (NVMEndA.West.X >= NVMEndB.East.X)
            {
                lengthHalf = (int)((NVMEndA.West.X - NVMEndB.East.X) / 2);
                temp.Add(NVMEndA.West);
                temp.Add(new Point(NVMEndB.East.X + lengthHalf, NVMEndA.West.Y));
                temp.Add(new Point(NVMEndB.East.X + lengthHalf, NVMEndB.East.Y));
                temp.Add(NVMEndB.East);
                setAnchor("east");
                XMultA = new Point(NVMEndA.West.X - l - a, NVMEndA.West.Y + a);
                XMultB = new Point(NVMEndB.East.X + a, NVMEndB.East.Y - h - a);
            }
            else if (NVMEndA.East.X <= NVMEndB.West.X)
            {
                lengthHalf = (int)((NVMEndB.West.X - NVMEndA.East.X) / 2);
                temp.Add(NVMEndA.East);
                temp.Add(new Point(NVMEndA.East.X + lengthHalf, NVMEndA.East.Y));
                temp.Add(new Point(NVMEndA.East.X + lengthHalf, NVMEndB.West.Y));
                temp.Add(NVMEndB.West);
                setAnchor("west");
                XMultA = new Point(NVMEndA.East.X + a, NVMEndA.East.Y - h - a);
                XMultB = new Point(NVMEndB.West.X - l - a, NVMEndB.West.Y + a);
            }
            else if (NVMEndA.North.Y >= NVMEndB.South.Y)
            {
                lengthHalf = (int)((NVMEndA.North.Y - NVMEndB.South.Y) / 2);
                temp.Add(NVMEndA.North);
                temp.Add(new Point(NVMEndA.North.X, NVMEndB.South.Y + lengthHalf));
                temp.Add(new Point(NVMEndB.South.X, NVMEndB.South.Y + lengthHalf));
                temp.Add(NVMEndB.South);
                setAnchor("south");
                XMultA = new Point(NVMEndA.North.X - l - a, NVMEndA.North.Y - h - a);
                XMultB = new Point(NVMEndB.South.X + a, NVMEndB.South.Y + a);
            }
            else if (NVMEndA.South.Y <= NVMEndB.North.Y)
            {
                lengthHalf = (int)((NVMEndB.North.Y - NVMEndA.South.Y) / 2);
                temp.Add(NVMEndA.South);
                temp.Add(new Point(NVMEndA.South.X, NVMEndA.South.Y + lengthHalf));
                temp.Add(new Point(NVMEndB.North.X, NVMEndA.South.Y + lengthHalf));
                temp.Add(NVMEndB.North);
                setAnchor("north");
                XMultA = new Point(NVMEndA.South.X + a, NVMEndA.South.Y + a);
                XMultB = new Point(NVMEndB.North.X - l - a, NVMEndB.North.Y - h - a);
            }
            return temp;
        }
    }
}
