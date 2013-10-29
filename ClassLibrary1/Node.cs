using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLDesigner.Model
{
    public class Node : ViewModelBase
    {
        private int x;
        public int X
        {
            get { return x; }
            set { x = value;  RaisePropertyChanged(() => X);}
        }
        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; RaisePropertyChanged(() => Y); }
        }

        //Height and Width are not binded to currently, in order for the control to autoexpand. We might need it in the future.
        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; RaisePropertyChanged(() => Width); }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; RaisePropertyChanged(() => Height); }
        }

        private String className;
        public String ClassName
        {
            get { return className; }
            set { className = value; }
        }

        public ObservableCollection<Attribute> Attributes { get; set; }
        public ObservableCollection<String> Properties { get; set; }
        public ObservableCollection<String> Methods { get; set; }



        // Konstruktoren bruges i dette tilfælde til at sætte standard værdi for attributerne.
        public Node()
        {
            Attributes = new ObservableCollection<Attribute>();
            Properties = new ObservableCollection<String>();
            Methods = new ObservableCollection<String>();

            X = Y = 0;
           // Width = Height = 100;
        }  
    }

    public class Attribute : ViewModelBase
    {
        //private or public
       private bool modifier;

        public bool Modifier
        {
            get { return modifier; }
            set { modifier = value; RaisePropertyChanged(() => Modifier); }
        }
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(() => Name); }
        }
       private String type;

        public String Type
        {
            get { return type; }
            set { type = value; RaisePropertyChanged(() => Type); }
        }
    }
}