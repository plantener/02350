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

        // Holder styr på undo/redo.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        private int relativeMousePositionX = -1;
        private int relativeMousePositionY = -1;
        private bool isFocused = false;
        public bool IsFocused { get { return isFocused; } set { isFocused = value; RaisePropertyChanged(() => IsFocused); } }
        private NodeViewModel focusedClass = null;
        public NodeViewModel FocusedClass { get { return focusedClass; } private set { focusedClass = value; if (focusedClass == null) { IsFocused = false; } else { IsFocused = true; };} }
        private Point _oldMousePos;
        private Point moveNodePoint;
        public ObservableCollection<NodeViewModel> Classes { get; set; }
        public ObservableCollection<EdgeViewModel> Edges { get; set; }

        private bool isAddingEdge = false;
        private NodeViewModel startEdge;
        private string type;

        // Kommandoer som UI bindes til.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand AddClassCommand { get; private set; }
        public ICommand AddAGGCommand { get; private set; }
        public ICommand AddASSCommand { get; private set; }
        public ICommand AddCOMCommand { get; private set; }
        public ICommand AddDEPCommand { get; private set; }
        public ICommand AddGENCommand { get; private set; }
        public ICommand AddNORCommand { get; private set; }
        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }
        public ICommand KeyDownCommand { get; private set; }
        public ICommand AddItemToNodeCommand { get; private set; }
        public ICommand MouseDownCanvasCommand { get; private set; }
        //Used to collapse nodes from GUI
        public ICommand CollapseExpandCommand { get; set ; }
        //GUI binds to see if nodes should be collapsed
        public string NodesAreCollapsed { get; set; }


        public MainViewModel()
        {
            // Her fyldes listen af classes med to classes. Her benyttes et alternativ til konstruktorer med syntaksen 'new Type(){ Attribut = Værdi }'
            // Det der sker er at der først laves et nyt object og så sættes objektets attributer til de givne værdier.
            Classes = new ObservableCollection<NodeViewModel>()
            { 
                new NodeViewModel() { ClassName = "TestClass", X = 30, Y = 40,  Methods = { new Attribute { Name = "metode", Modifier = true, Type = "int" } }, Properties={"properties"}},
                new NodeViewModel() { ClassName = "TestClass", X = 140, Y = 230, Properties= {"properties", "her"},},
                new NodeViewModel() { ClassName = "NewClass", Attributes = { new Attribute { Name = "Testattribut", Modifier = true, Type = "int" } }, Properties = { "PropertiesTest", "ProperTiesTest2" } }
            };

            Edges = new ObservableCollection<EdgeViewModel>()
            {
            };

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes. Her vidersendes metode kaldne til UndoRedoControlleren.
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            AddClassCommand = new RelayCommand(AddNode);
            AddAGGCommand = new RelayCommand(AddAGG);
            AddASSCommand = new RelayCommand(AddASS);
            AddCOMCommand = new RelayCommand(AddCOM);
            AddDEPCommand = new RelayCommand(AddDEP);
            AddGENCommand = new RelayCommand(AddGEN);
            AddGENCommand = new RelayCommand(AddNOR);
            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDownNode);

         //   AddItemToNodeCommand = new RelayCommand(AddItemToNode);
          AddItemToNodeCommand = new RelayCommand<object>(param => AddItemToNode(FocusedClass,Classes,param));
          MouseDownCanvasCommand = new RelayCommand<MouseEventArgs>(MouseDownCanvas);

          CollapseExpandCommand = new RelayCommand(CollapseViewChanged);

            Debug.WriteLine("Højde" + Classes[0].Height);
        }

        //Switch status on collapsed/expanded. Could probably be done prettier
        private void CollapseViewChanged()
        {
            if (NodesAreCollapsed == "Collapsed")
            {
                NodesAreCollapsed = "Visible";
            }
            else
            {
                NodesAreCollapsed = "Collapsed";
            }
            RaisePropertyChanged(() => NodesAreCollapsed);
        }

       
        public void AddItemToNode(NodeViewModel FocusedClass, ObservableCollection<NodeViewModel> Classes, object parameter)
        {
            undoRedoController.AddAndExecute( new AddItemToNodeCommand(FocusedClass, Classes, parameter));
        }

        private void MouseDownCanvas(MouseEventArgs obj)
        {
            FrameworkElement clickedObj = (FrameworkElement)obj.MouseDevice.Target;
            if (clickedObj.DataContext is NodeViewModel)
            {
            }
            else
            {
                FocusedClass = null;
            }
        }

        public void AddNode()
        {
            undoRedoController.AddAndExecute(new AddClassCommand(Classes));
        }

        public void AddEdge()
        {
            isAddingEdge = true;
        }

        public void AddAGG()
        {
            type = "AGG";
            AddEdge();
        }

        public void AddASS()
        {
            type = "ASS";
            AddEdge();
        }

        public void AddCOM()
        {
            type = "COM";
            AddEdge();
        }

        public void AddDEP()
        {
            type = "DEP";
            AddEdge();
        }

        public void AddGEN()
        {
            type = "GEN";
            AddEdge();
        }

        public void AddNOR()
        {
            type = "NOR";
            AddEdge();
        }

        //Captures a keyboard press if on a node
        public void KeyDownNode(KeyEventArgs e)
        {
            //clears focus from current node if 'enter' is pressed
            if (e.Key == Key.Return)
            {
                Keyboard.ClearFocus();
                FocusedClass = null;
            }
        }



        // Captures the mouse, to move nodes
        public void MouseDownNode(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                _oldMousePos = e.GetPosition(FindParent<Canvas>((FrameworkElement)e.MouseDevice.Target));
                e.MouseDevice.Target.CaptureMouse();
            }
        }

        //Used to move nodes around
        public void MouseMoveNode(MouseEventArgs e)
        {
            //Is the mouse captured?
            if (Mouse.Captured != null)
            {

                FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;

                //If the clicked field in the node is the textfield, we dont want to move it around
                if (movingClass is System.Windows.Controls.TextBox)
                {
                    return;
                }

                //sets the relative offset when a node is moved around. Done so the cursor doesnt jump to the corner of the class. This is set back to -1 when mouse is released
                if (relativeMousePositionX == -1 && relativeMousePositionY == -1)
                {
                    relativeMousePositionX = (int)Mouse.GetPosition(movingClass).X;
                    relativeMousePositionY = (int)Mouse.GetPosition(movingClass).Y;
                }


                // Fra ellipsen skaffes punktet som den er bundet til.
                NodeViewModel movingNode = (NodeViewModel)movingClass.DataContext;
                
                // Canvaset findes her udfra ellipsen.
                Canvas canvas = FindParent<Canvas>(movingClass);
                // Musens position i forhold til canvas skaffes her.
                Point mousePosition = Mouse.GetPosition(canvas);
                // Når man flytter noget med musen vil denne metode blive kaldt mange gange for hvert lille ryk, 
                // derfor gemmes her positionen før det første ryk så den sammen med den sidste position kan benyttes til at flytte punktet med en kommando.
                if (moveNodePoint == default(Point)) moveNodePoint = mousePosition;
                // Punktets position ændres og beskeden bliver så sendt til UI med INotifyPropertyChanged mønsteret.

                movingNode.X = (int)mousePosition.X - relativeMousePositionX;
                movingNode.Y = (int)mousePosition.Y - relativeMousePositionY;

                for (int i = 0; i < Edges.Count; i++)
                {
                    if (movingNode == Edges[i].NVMEndA || movingNode == Edges[i].NVMEndB)
                        Edges[i].newPath();
                }
            }
        }

        public void MouseUpNode(MouseEventArgs e)
        {
            //Used to move node
            // noden skaffes.
            FrameworkElement movingClass =(FrameworkElement) e.MouseDevice.Target;
            //Noden sættes i fokus
            FocusedClass = (NodeViewModel)movingClass.DataContext;

            if (isAddingEdge)
            {
                if (startEdge == null)
                    startEdge = FocusedClass;
                else if (startEdge != FocusedClass)
                {
                    undoRedoController.AddAndExecute(new AddEdgeCommand(Edges, FocusedClass.newEdge(startEdge, type)));
                    isAddingEdge = false;
                    startEdge = null;
                    type = "";
                }
            }
            else
            {
                if (_oldMousePos == e.GetPosition(FindParent<Canvas>((FrameworkElement)e.MouseDevice.Target)))
                {
                    e.MouseDevice.Target.ReleaseMouseCapture();
                    return;
                }

                // Ellipsens node skaffes.
                NodeViewModel movingNode = (NodeViewModel)movingClass.DataContext;
                // Canvaset skaffes.
                Canvas canvas = FindParent<Canvas>(movingClass);
                // Musens position på canvas skaffes.
                Point mousePosition = Mouse.GetPosition(canvas);

                // Punktet flyttes med kommando. Den flyttes egentlig bare det sidste stykke i en række af mange men da de originale punkt gemmes er der ikke noget problem med undo/redo.

                undoRedoController.AddAndExecute(new MoveNodeCommand(movingNode, (int)mousePosition.X - relativeMousePositionX, (int)mousePosition.Y - relativeMousePositionY, (int)moveNodePoint.X - relativeMousePositionX, (int)moveNodePoint.Y - relativeMousePositionY, Edges));
                // Nulstil værdier.
                moveNodePoint = new Point();
                //Reset the relative offsets for the moved node
                relativeMousePositionX = -1;
                relativeMousePositionY = -1;
                // Musen frigøres.
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
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
        /*//Currently not used
        private Node GetNodeUnderMouse()
        {
            var item = Mouse.DirectlyOver as DockPanel;
            if (item == null)
            {
                return null;
            }
            return item.DataContext as NodeViewModel;
        }*/
    }
}