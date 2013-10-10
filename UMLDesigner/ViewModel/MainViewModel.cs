using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
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

        private T FindParent<T>(DependencyObject child)

  where T : DependencyObject
        {

            T parent = VisualTreeHelper.GetParent(child) as T;

            if (parent != null)
                return parent;
            else
                return FindParent<T>(parent);

        }
    }
}