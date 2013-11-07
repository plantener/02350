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

        private NodeViewModel nVMEndA;
        public NodeViewModel NVMEndA { get { return nVMEndA; }
            set { nVMEndA = value; RaisePropertyChanged(() => NVMEndA); RaisePropertyChanged(() => EndA); } }

        private NodeViewModel nVMEndB;
        public NodeViewModel NVMEndB { get { return nVMEndB; }
            set { nVMEndB = value; RaisePropertyChanged(() => NVMEndB); RaisePropertyChanged(() => EndB); } }

        public Node EndA { get { return edge.EndA; } 
            set { edge.EndA = value; RaisePropertyChanged(() => EndA); RaisePropertyChanged(() => Path); } }

        public Node EndB { get { return edge.EndB; } 
            set { edge.EndB = value; RaisePropertyChanged(() => EndB); RaisePropertyChanged(() => Path); } }

        private EdgeType Type { get { return edge.Type; } set { edge.Type = value; RaisePropertyChanged(() => Type); } }

        private string colorFill;
        private string dashed;

        private String path;
        public String Path { get { return path; } set { path = value; RaisePropertyChanged(() => Path); } }

        private String arrow;
        public String Arrow { get { setArrow(); return arrow; } set { arrow = value; RaisePropertyChanged(() => Arrow); } }

        private PointCollection pathObjects = new PointCollection();

        private PointCollection normArrow = new PointCollection();
        private PointCollection genArrow = new PointCollection();
        private PointCollection rombArrow = new PointCollection();
        private PointCollection thisArrow;

        public EdgeViewModel(NodeViewModel nVMEndA, NodeViewModel nVMEndB, Node endA, Node endB, string type)
        {
            edge = new Edge();
            EndA = endA;
            EndB = endB;
            this.nVMEndA = nVMEndA;
            this.nVMEndB = nVMEndB;
            Type = edgeTypeConverter(type);
            initArrow();
            newPath();
            setArrow();
        }

        public void newPath()
        {
            if (pathObjects != null)
            {
                pathObjects.Clear();

                pathObjects.Add(new Point(NVMEndA.X, NVMEndA.Y));
                pathObjects.Add(new Point(NVMEndA.X, NVMEndB.Y));
                pathObjects.Add(new Point(NVMEndB.X, NVMEndB.Y));

                setPath();
            }
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
            genArrow.Add(new Point(-5, 10));
            genArrow.Add(new Point(0, 0));
            genArrow.Add(new Point(5, 10));
            genArrow.Add(new Point(0, 20));
            genArrow.Add(new Point(-5, 10));

        }

        private void setPath()
        {
            path = "M";
            for (int i = 0; i < pathObjects.Count; i++)
            {
                path += " " + pathObjects.ElementAt(i).X + "," + pathObjects.ElementAt(i).Y;
            }
            setArrow();
        }

        private void setArrow()
        {
            arrow = "";
            if (!Type.Equals(EdgeType.NOR))
            {
                for (int i = 0; i < pathObjects.Count; i++)
                {
                    arrow += " " + (EndB.X + thisArrow.ElementAt(i).X) +
                        "," + (EndB.Y + thisArrow.ElementAt(i).Y);
                    Console.WriteLine(arrow);
                }
                Console.WriteLine(arrow);
            }
        }

        private EdgeType edgeTypeConverter(string type)
        {
            switch (type)
            {
                case "AGG":
                    thisArrow = rombArrow;
                    return EdgeType.AGG;
                case "ASS":
                    thisArrow = normArrow;
                    return EdgeType.ASS;
                case "COM":
                    thisArrow = rombArrow;
                    return EdgeType.COM;
                case "DEP":
                    thisArrow = normArrow;
                    return EdgeType.DEP;
                case "GEN":
                    thisArrow = genArrow;
                    return EdgeType.GEN;
                case "NOR":
                    return EdgeType.NOR;
                default:
                    thisArrow = normArrow;
                    return EdgeType.ASS;
            }
        }

        private void setAnchor()
        {
            if (NVMEndA.X+NVMEndA.Width <= NVMEndB.X)
            {

            } else if (NVMEndA.X >= NVMEndB.X + NVMEndB.Width)
            {

            }
        }
    }
}
