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

        public string Name
        {
            get { return edge.Name; }
            set { edge.Name = value; RaisePropertyChanged(() => Name); }
        }

        private EdgeType Type { get { return edge.Type; } set { edge.Type = value; RaisePropertyChanged(() => Type); } }

        private string colorFill;
        public String ColorFill { get { return colorFill; } set { colorFill = value; RaisePropertyChanged(() => ColorFill); } }

        private string dashed;
        public String Dashed { get { return dashed; } set { dashed = value; RaisePropertyChanged(() => Dashed); } }

        private String path;
        public String Path { get { return path; } set { path = value; RaisePropertyChanged(() => Path); } }

        private String arrow;
        public String Arrow { get { return arrow; } set { arrow = value; RaisePropertyChanged(() => Arrow); } }

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
            pathObjects = NVMEndA.getAnchor(NVMEndB, this);
            rotateArrow();
            setPath();
            setArrow();
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
                    return EdgeType.AGG;
                case "ASS":
                    thisArrow = normArrow;
                    dashed = "1 0";
                    colorFill = "Transperant";
                    return EdgeType.ASS;
                case "COM":
                    thisArrow = rombArrow;
                    dashed = "1 0";
                    colorFill = "#2E8DEF";
                    return EdgeType.COM;
                case "DEP":
                    thisArrow = normArrow;
                    dashed = "5 5";
                    colorFill = "Transperant";
                    return EdgeType.DEP;
                case "GEN":
                    thisArrow = genArrow;
                    dashed = "1 0";
                    colorFill = "White";
                    return EdgeType.GEN;
                case "NOR":
                    thisArrow = new PointCollection();
                    dashed = "1 0";
                    colorFill = "Transperant";
                    return EdgeType.NOR;
                default:
                    thisArrow = normArrow;
                    dashed = "1 0";
                    colorFill = "Transperant";
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
    }
}
