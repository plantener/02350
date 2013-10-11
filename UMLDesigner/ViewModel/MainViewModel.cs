using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLDesigner.Command;
using UMLDesigner.Model;

namespace UMLDesigner.ViewModel
{
 

    public class MainViewModel : ViewModelBase
    {
        private Point moveNodePoint;
        public ObservableCollection<Node> Classes { get; set; }

        // Kommandoer som UI bindes til.
        public ICommand AddClassCommand { get; private set; }
        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }


        public MainViewModel()
        {
            // Her fyldes listen af classes med to classes. Her benyttes et alternativ til konstruktorer med syntaksen 'new Type(){ Attribut = Værdi }'
            // Det der sker er at der først laves et nyt object og så sættes objektets attributer til de givne værdier.
            Classes = new ObservableCollection<Node>()
            { 
                new Node() { ClassName = "TestClass", X = 30, Y = 40, Attributes = {"Hej","Test"}, Methods = {"Vi","tester","mere"}, Properties={"properties"}},
                new Node() { ClassName = "TestClass", X = 140, Y = 230, Methods = {"Endnu", "En", "test"}, Attributes = {"Attribut"}, Properties= {"properties", "her"}},
                new Node() { ClassName = "NewClass", Attributes = {"Attributtest", "Attributtest2"}, Methods = { "MethodTest", "MethodTest2"}, Properties = {"PropertiesTest", "ProperTiesTest2"}}
            };

            AddClassCommand = new AddClassCommand(Classes);
            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
     
        }

        // Captures the mouse, to move nodes
        public void MouseDownNode(MouseButtonEventArgs e)
        {
           e.MouseDevice.Target.CaptureMouse();
          
           
        }

        //Used to move nodes around
        public void MouseMoveNode(MouseEventArgs e)
        {
            //Is the mouse captured?
            if (Mouse.Captured != null)
            {

                
                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
                
                // Fra ellipsen skaffes punktet som den er bundet til.
                Node movingNode = (Node)movingClass.DataContext;

                
                // Canvaset findes her udfra ellipsen.
                Canvas canvas = FindParent<Canvas>(movingClass);
                // Musens position i forhold til canvas skaffes her.
                Point mousePosition = Mouse.GetPosition(canvas);
                // Når man flytter noget med musen vil denne metode blive kaldt mange gange for hvert lille ryk, 
                // derfor gemmes her positionen før det første ryk så den sammen med den sidste position kan benyttes til at flytte punktet med en kommando.
                if (moveNodePoint == default(Point)) moveNodePoint = mousePosition;
                // Punktets position ændres og beskeden bliver så sendt til UI med INotifyPropertyChanged mønsteret.
                
                movingNode.X = (int)mousePosition.X;
                movingNode.Y = (int)mousePosition.Y;
            }
        }

        public void MouseUpNode(MouseEventArgs e)
        {
            //Used to move node
            // noden skaffes.
            FrameworkElement movingClass =(FrameworkElement) e.MouseDevice.Target;
            

            // Ellipsens node skaffes.
            Node movingNode = (Node) movingClass.DataContext;
            // Canvaset skaffes.
            Canvas canvas = FindParent<Canvas>(movingClass);
            // Musens position på canvas skaffes.
            Point mousePosition = Mouse.GetPosition(canvas);
            // Punktet flyttes med kommando. Den flyttes egentlig bare det sidste stykke i en række af mange men da de originale punkt gemmes er der ikke noget problem med undo/redo.
           new MoveNodeCommand(movingNode, (int)mousePosition.X, (int)mousePosition.Y, (int)moveNodePoint.X, (int)moveNodePoint.Y);
            // Nulstil værdier.
            moveNodePoint = new Point();
            // Musen frigøres.
            e.MouseDevice.Target.ReleaseMouseCapture(); 
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }
        //Currently not used
        private Node GetNodeUnderMouse()
        {
            var item = Mouse.DirectlyOver as DockPanel;
            if (item == null)
            {
                return null;
            }
            return item.DataContext as Node;
        }
    }
}