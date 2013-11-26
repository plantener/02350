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
    public class Node : ViewModelBase
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        private String className;
        public String ClassName
        {
            get { return className; }
            set {
                className = value;
                RaisePropertyChanged(() => ClassName); }
        }

        public ObservableCollection<Attribute> Attributes { get; set; }
        public ObservableCollection<String> Properties { get; set; }
        public ObservableCollection<Attribute> Methods { get; set; }



        // Konstruktoren bruges i dette tilfælde til at sætte standard værdi for attributerne.
        public Node()
        {
            Attributes = new ObservableCollection<Attribute>();
            Properties = new ObservableCollection<String>();
            Methods = new ObservableCollection<Attribute>();

            X = Y = 0;
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