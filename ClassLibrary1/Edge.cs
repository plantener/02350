using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UMLDesigner.Model
{
    public class Edge : ViewModelBase
    {
        private Node start;
        public Node Start
        {
            get { return start; }
            set { start = value; }
        }

        private Node end;
        public Node End
        {
            get { return end; }
            set { end = value; }
        }
    }
}
