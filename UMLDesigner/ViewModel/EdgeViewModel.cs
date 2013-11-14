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
    public class EdgeViewModel : ViewModelBase
    {
        private Edge edge;

        private NodeViewModel nVMEndA;
        public NodeViewModel NVMEndA { get { return nVMEndA; }
            set { nVMEndA = value; RaisePropertyChanged(() => NVMEndA); RaisePropertyChanged(() => EndA); RaisePropertyChanged(() => Path); }
        }

        private NodeViewModel nVMEndB;
        public NodeViewModel NVMEndB { get { return nVMEndB; }
            set { nVMEndB = value; RaisePropertyChanged(() => NVMEndB); RaisePropertyChanged(() => EndB); RaisePropertyChanged(() => Path); }
        }

        public Node EndA
        {
            get { return edge.EndA; }
            set { edge.EndA = value; RaisePropertyChanged(() => EndA); RaisePropertyChanged(() => NVMEndA); RaisePropertyChanged(() => Path); }
        }

        public Node EndB
        {
            get { return edge.EndB; }
            set { edge.EndB = value; RaisePropertyChanged(() => EndB); RaisePropertyChanged(() => NVMEndB); RaisePropertyChanged(() => Path); }
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

        public EdgeViewModel(NodeViewModel nVMEndA, NodeViewModel nVMEndB, Node endA, Node endB, string type)
        {
            edge = new Edge();
            EndA = endA;
            EndB = endB;
            NVMEndA = nVMEndA;
            NVMEndB = nVMEndB;
            Type = edgeTypeConverter(type);
            initArrow();
            newPath();
            Console.WriteLine(type);
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
            Console.WriteLine(pathObjects.Count);
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
                Console.WriteLine(arrow);
            }
        }

        private EdgeType edgeTypeConverter(string type)
        {
            switch (type)
            {
                case "AGG":
                    thisArrow = rombArrow;
                    dashed = "1 0";
                    colorFill = "White";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.AGG;
                case "ASS":
                    thisArrow = normArrow;
                    dashed = "1 0";
                    colorFill = "Transperant";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.ASS;
                case "COM":
                    thisArrow = rombArrow;
                    dashed = "1 0";
                    colorFill = "#2E8DEF";
                    multAllowed = true;
                    multBorder = 1;
                    return EdgeType.COM;
                case "DEP":
                    thisArrow = normArrow;
                    dashed = "5 5";
                    colorFill = "Transperant";
                    multAllowed = false;
                    multBorder = 0;
                    return EdgeType.DEP;
                case "GEN":
                    thisArrow = genArrow;
                    dashed = "1 0";
                    colorFill = "White";
                    multAllowed = false;
                    multBorder = 0;
                    return EdgeType.GEN;
                case "NOR":
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
            Console.WriteLine("new: " + newAnchor + " old: " + oldAnchor);
            if(oldAnchor != newAnchor){
                Console.WriteLine(newAnchor + " " + angle);
                oldAnchor = newAnchor;
                double cTheta = Math.Cos(angle*(Math.PI/180));
                double sTheta = Math.Sin(angle*(Math.PI/180));
                PointCollection temp = new PointCollection();
                for (int i = 0; i < thisArrow.Count; i++ )
                {
                    double x = (int) ((thisArrow.ElementAt(i).X - 0) * cTheta - (thisArrow.ElementAt(i).Y - 0) * sTheta);
                    double y = (int) ((thisArrow.ElementAt(i).X - 0) * sTheta + (thisArrow.ElementAt(i).Y - 0) * cTheta);
                    Console.WriteLine("x: " + x + " y: " + y);
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

        public PointCollection getAnchor()
        {
            int d = 3, a = 5, h = 20, l = 25, tW = 75;
            PointCollection temp = new PointCollection();
            //break
            if (NVMEndA.X + NVMEndA.Width / d > NVMEndB.X + NVMEndB.Width && NVMEndA.Y + NVMEndA.Height / d > NVMEndB.Y + NVMEndB.Height)
            {
                temp.Add(NVMEndA.North);
                temp.Add(new Point(NVMEndA.North.X, NVMEndB.East.Y));
                temp.Add(NVMEndB.East);
                XMultA = new Point(NVMEndA.North.X + a, NVMEndA.North.Y - h - a);
                XMultB = new Point(NVMEndB.East.X + a, NVMEndB.East.Y + a);
                PosName = new Point(NVMEndA.North.X - tW, NVMEndB.East.Y - h - a);
                setAnchor("east");
            }
            else if (NVMEndA.X + NVMEndA.Width - NVMEndA.Width / d < NVMEndB.X && NVMEndA.Y + NVMEndA.Height / d > NVMEndB.Y + NVMEndB.Height)
            {
                temp.Add(NVMEndA.East);
                temp.Add(new Point(NVMEndB.South.X, NVMEndA.East.Y));
                temp.Add(NVMEndB.South);
                XMultA = new Point(NVMEndA.East.X + a, NVMEndA.East.Y + a);
                XMultB = new Point(NVMEndB.South.X - l - a, NVMEndB.South.Y + a);
                //PosName = new Point(NVMEndB.South.X - tW - a, NVMEndA.East.Y - h - a);
                setAnchor("south");
            }
            else if (NVMEndA.X + NVMEndA.Width - NVMEndA.Width / d < NVMEndB.X && NVMEndA.Y + NVMEndA.Height - NVMEndA.Height / d < NVMEndB.Y)
            {
                temp.Add(NVMEndA.South);
                temp.Add(new Point(NVMEndA.South.X, NVMEndB.West.Y));
                temp.Add(NVMEndB.West);
                XMultA = new Point(NVMEndA.South.X - l - a, NVMEndA.South.Y + a);
                XMultB = new Point(NVMEndB.West.X - l - a, NVMEndB.West.Y - h - a);
                //PosName = new Point(NVMEndA.South.X, NVMEndB.West.Y+a);
                setAnchor("west");
            }
            else if (NVMEndA.X + NVMEndA.Width / d > NVMEndB.X + NVMEndB.Width && NVMEndA.Y + NVMEndA.Height - NVMEndA.Height / d < NVMEndB.Y)
            {
                temp.Add(NVMEndA.West);
                temp.Add(new Point(NVMEndB.North.X, NVMEndA.West.Y));
                temp.Add(NVMEndB.North);
                XMultA = new Point(NVMEndA.West.X - l - a, NVMEndA.West.Y - h - a);
                XMultB = new Point(NVMEndB.North.X + a, NVMEndB.North.Y - h - a);
                //PosName = new Point(NVMEndB.North.X, NVMEndA.West.Y-h-a);
                setAnchor("north");
            }
            //Straight above and below
            else if (NVMEndB.X + NVMEndB.Width >= NVMEndA.X + NVMEndA.Width / d && NVMEndB.X < NVMEndA.X && NVMEndA.Y > NVMEndB.Y + NVMEndB.Height)
            {
                double dTemp = (NVMEndB.X + NVMEndB.Width - NVMEndA.X) / 2;
                temp.Add(new Point(NVMEndB.X + NVMEndB.Width - dTemp, NVMEndA.Y));
                temp.Add(new Point(NVMEndB.X + NVMEndB.Width - dTemp, NVMEndB.Y + NVMEndB.Height));
                XMultA = new Point(NVMEndB.X + NVMEndB.Width - dTemp + a, NVMEndA.Y - h - a);
                XMultB = new Point(NVMEndB.X + NVMEndB.Width - dTemp - l - a, NVMEndB.Y + NVMEndB.Height + a);
                //PosName = new Point(NVMEndB.X + NVMEndB.Width - dTemp - tW - a, NVMEndB.South.Y + (NVMEndA.North.Y - NVMEndB.South.Y) / 2 - h / 2);
                setAnchor("south");
            }
            else if (NVMEndB.X <= NVMEndA.X + NVMEndA.Width - NVMEndA.Width / d && NVMEndB.X > NVMEndA.X && NVMEndA.Y > NVMEndB.Y + NVMEndB.Height)
            {
                double dTemp = (NVMEndA.X + NVMEndA.Width - NVMEndB.X) / 2;
                temp.Add(new Point(NVMEndB.X + dTemp, NVMEndA.Y));
                temp.Add(new Point(NVMEndB.X + dTemp, NVMEndB.Y + NVMEndB.Height));
                XMultA = new Point(NVMEndB.X + dTemp + a, NVMEndA.Y - h - a);
                XMultB = new Point(NVMEndB.X + dTemp - l - a, NVMEndB.Y + NVMEndB.Height + a);
                //PosName = new Point(NVMEndB.X + dTemp - tW - a, NVMEndB.South.Y + (NVMEndA.North.Y - NVMEndB.South.Y) / 2 - h / 2);
                setAnchor("south");
            }
            else if (NVMEndB.X + NVMEndB.Width >= NVMEndA.X + NVMEndA.Width / d && NVMEndB.X < NVMEndA.X && NVMEndA.Y + NVMEndA.Height < NVMEndB.Y)
            {
                double dTemp = (NVMEndB.X + NVMEndB.Width - NVMEndA.X) / 2;
                temp.Add(new Point(NVMEndB.X + NVMEndB.Width - dTemp, NVMEndA.Y + NVMEndA.Height));
                temp.Add(new Point(NVMEndB.X + NVMEndB.Width - dTemp, NVMEndB.Y));
                XMultA = new Point(NVMEndB.X + NVMEndB.Width - dTemp - l - a, NVMEndA.Y + NVMEndA.Height + a);
                XMultB = new Point(NVMEndB.X + NVMEndB.Width - dTemp + a, NVMEndB.Y - h - a);
                //PosName = new Point(NVMEndB.X + NVMEndB.Width - dTemp - tW - a, NVMEndA.South.Y + (NVMEndB.North.Y - NVMEndA.South.Y) / 2 - h / 2);
                setAnchor("north");
            }
            else if (NVMEndB.X <= NVMEndA.X + NVMEndA.Width - NVMEndA.Width / d && NVMEndB.X > NVMEndA.X && NVMEndA.Y + NVMEndA.Height < NVMEndB.Y)
            {
                double dTemp = (NVMEndA.X + NVMEndA.Width - NVMEndB.X) / 2;
                temp.Add(new Point(NVMEndB.X + dTemp, NVMEndA.Y + NVMEndA.Height));
                temp.Add(new Point(NVMEndB.X + dTemp, NVMEndB.Y));
                XMultA = new Point(NVMEndB.X + dTemp - l - a, NVMEndA.Y + NVMEndA.Height + a);
                XMultB = new Point(NVMEndB.X + dTemp + a, NVMEndB.Y - h - a);
                //PosName = new Point(NVMEndB.X + dTemp - tW - a, NVMEndA.South.Y + (NVMEndB.North.Y - NVMEndA.South.Y) / 2 - h / 2);
                setAnchor("north");
            }
            //straight left and right
            else if (NVMEndA.X > NVMEndB.X + NVMEndB.Width && NVMEndA.Y + NVMEndA.Height / d <= NVMEndB.Y + NVMEndB.Height && NVMEndA.Y > NVMEndB.Y)
            {
                double dTemp = (NVMEndB.Y + NVMEndB.Height - NVMEndA.Y) / 2;
                temp.Add(new Point(NVMEndA.X, NVMEndA.Y + dTemp));
                temp.Add(new Point(NVMEndB.X + NVMEndB.Width, NVMEndA.Y + dTemp));
                XMultA = new Point(NVMEndA.X - l - a, NVMEndA.Y + dTemp - h - a);
                XMultB = new Point(NVMEndB.X + NVMEndB.Width + a, NVMEndA.Y + dTemp + a);
                setAnchor("east");
            }
            else if (NVMEndA.X + NVMEndA.Width < NVMEndB.X && NVMEndA.Y + NVMEndA.Height / d <= NVMEndB.Y + NVMEndB.Height && NVMEndA.Y > NVMEndB.Y)
            {
                double dTemp = (NVMEndB.Y + NVMEndB.Height - NVMEndA.Y) / 2;
                temp.Add(new Point(NVMEndA.X + NVMEndA.Width, NVMEndA.Y + dTemp));
                temp.Add(new Point(NVMEndB.X, NVMEndA.Y + dTemp));
                XMultA = new Point(NVMEndA.X + NVMEndA.Width + a, NVMEndA.Y + dTemp + a);
                XMultB = new Point(NVMEndB.X - l - a, NVMEndA.Y + dTemp - h - a);
                setAnchor("west");
            }
            else if (NVMEndA.X > NVMEndB.X + NVMEndB.Width && NVMEndA.Y + NVMEndA.Height - NVMEndA.Height / d >= NVMEndB.Y && NVMEndA.Y < NVMEndB.Y)
            {
                double dTemp = (NVMEndB.Y + NVMEndB.Height - NVMEndA.Y) / 2;
                temp.Add(new Point(NVMEndA.X, NVMEndA.Y + dTemp));
                temp.Add(new Point(NVMEndB.X + NVMEndB.Width, NVMEndA.Y + dTemp));
                XMultA = new Point(NVMEndA.X - l - a, NVMEndA.Y + dTemp - h - a);
                XMultB = new Point(NVMEndB.X + NVMEndB.Width + a, NVMEndA.Y + dTemp + a);
                setAnchor("east");
            }
            else if (NVMEndA.X + NVMEndA.Width < NVMEndB.X && NVMEndA.Y + NVMEndA.Height - NVMEndA.Height / d >= NVMEndB.Y && NVMEndA.Y < NVMEndB.Y)
            {
                double dTemp = (NVMEndB.Y + NVMEndB.Height - NVMEndA.Y) / 2;
                temp.Add(new Point(NVMEndA.X + NVMEndA.Width, NVMEndA.Y + dTemp));
                temp.Add(new Point(NVMEndB.X, NVMEndA.Y + dTemp));
                XMultA = new Point(NVMEndA.X + NVMEndA.Width + a, NVMEndA.Y + dTemp + a);
                XMultB = new Point(NVMEndB.X - l - a, NVMEndA.Y + dTemp - h - a);
                setAnchor("west");
            }
            return temp;
        }
    }
}
