using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLDesigner.Command;
using UMLDesigner.Model;
using UMLDesigner.Utilities;

namespace UMLDesigner.ViewModel {


   public class MainViewModel : ViewModelBase {

      // Holder styr på undo/redo.
      private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

      private int relativeMousePositionX = -1;
      private int relativeMousePositionY = -1;
      private bool isFocused = false;
      public bool IsFocused { get { return isFocused; } set { isFocused = value; RaisePropertyChanged(() => IsFocused); } }
      private NodeViewModel focusedClass = null;
      public NodeViewModel FocusedClass { get { return focusedClass; } private set { focusedClass = value; if (focusedClass == null) { IsFocused = false; } else { IsFocused = true; };} }
      private bool canPaste = false;
      public bool CanPaste { get { return canPaste; } set { canPaste = value; RaisePropertyChanged(() => CanPaste); } }
      private NodeViewModel copyClass = null;
      public NodeViewModel CopyClass { get { return copyClass; } private set { copyClass = value; if (copyClass == null) { CanPaste = false; } else { CanPaste = true; };} }
      private Point _oldMousePos;
      private bool _pressed = false;
      FrameworkElement movingClass;

      private Point moveNodePoint;
      public ObservableCollection<NodeViewModel> Classes { get; set; }
      public ObservableCollection<EdgeViewModel> Edges { get; set; }

      private bool isAddingEdge = false;
      private NodeViewModel startEdge;
      private string type = "";

      // Kommandoer som UI bindes til.
       private bool _gridVisibility;
      public bool GridVisibility { get { return _gridVisibility; } set { _gridVisibility = value; RaisePropertyChanged(() => GridVisibility); }}

      public ICommand UndoCommand { get; private set; }
      public ICommand RedoCommand { get; private set; }
      public ICommand KeyDownUndoCommand { get; private set; }

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
      public ICommand KeyUpCommand { get; private set; }
      public ICommand AddItemToNodeCommand { get; private set; }
      public ICommand MouseDownCanvasCommand { get; private set; }
      public ICommand CopyCommand { get; private set; }
      public ICommand PasteCommand { get; private set; }
      public ICommand DeleteCommand { get; private set; }
      //Used to collapse nodes from GUI
      public ICommand CollapseExpandCommand { get; set; }
       //Export canvas to image
      public ICommand ExportCommand { get; private set; }
      //GUI binds to see if nodes should be collapsed
      public string NodesAreCollapsed { get; set; }

      private Canvas canvas;

      public MainViewModel() {
         // Her fyldes listen af classes med to classes. Her benyttes et alternativ til konstruktorer med syntaksen 'new Type(){ Attribut = Værdi }'
         // Det der sker er at der først laves et nyt object og så sættes objektets attributer til de givne værdier.
         Classes = new ObservableCollection<NodeViewModel>()
            { 
                new NodeViewModel() { ClassName = "TestClass", X = 30, Y = 40,  Methods = { new Attribute { Name = "metode", Modifier = true, Type = "int" } }},
                new NodeViewModel() { ClassName = "TestClass", X = 140, Y = 230, },
                new NodeViewModel() { ClassName = "NewClass", Attributes = { new Attribute { Name = "Testattribut", Modifier = true, Type = "int" } } }
            };

         Edges = new ObservableCollection<EdgeViewModel>() {
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
         AddNORCommand = new RelayCommand(AddNOR);
         MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
         MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
         MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
         KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDownNode);
         KeyUpCommand = new RelayCommand<KeyEventArgs>(UndoRedo_KeyUp);
         KeyDownUndoCommand = new RelayCommand<object>(param => KeyDownUndo(param));

         //   AddItemToNodeCommand = new RelayCommand(AddItemToNode);
         AddItemToNodeCommand = new RelayCommand<object>(param => AddItemToNode(FocusedClass, Classes, param));
         MouseDownCanvasCommand = new RelayCommand<MouseEventArgs>(MouseDownCanvas);
         CopyCommand = new RelayCommand(Copy);
         PasteCommand = new RelayCommand(Paste);
         DeleteCommand = new RelayCommand(delete);

         CollapseExpandCommand = new RelayCommand(CollapseViewChanged);

         ExportCommand = new RelayCommand(ExportImage);

         Debug.WriteLine("Højde" + Classes[0].Height);

         //Application.Current.MainWindow.InputBindings.Add(new KeyBinding(UndoCommand, new KeyGesture(Key.A, ModifierKeys.Control)));
      }


      private void delete()
      {
          //deletes the edge connected to the delited class
          foreach (EdgeViewModel edge in Edges)
          {
              if (FocusedClass == edge.NVMEndA || FocusedClass == edge.NVMEndB)
              {
                  Edges.Remove(edge);
              }
          }
          Classes.Remove(FocusedClass);
      }


      private void Copy()
      {
          CopyClass = new NodeViewModel();
          CopyClass.ClassName = FocusedClass.ClassName;
          CopyClass.X = 0;
          CopyClass.Y = 0;
          foreach (Attribute attribute in FocusedClass.Attributes)
          {
              CopyClass.Attributes.Add(attribute);
          }
          foreach (Attribute method in FocusedClass.Methods)
          {
              CopyClass.Methods.Add(method);
          }
      }

      private void Paste()
      {
          Classes.Add(CopyClass);
          FocusedClass = CopyClass;
      }


      //Switch status on collapsed/expanded. Could probably be done prettier
      private void CollapseViewChanged() {
         if (NodesAreCollapsed == "Collapsed") {
            NodesAreCollapsed = "Visible";
         } else {
            NodesAreCollapsed = "Collapsed";
         }
         RaisePropertyChanged(() => NodesAreCollapsed);
         for (int i = 0; i < Edges.Count; i++)
         {
             Edges[i].newPath();
         }
      }

      public void ExportImage()
      {
          Canvas mainCanvas = FindParent<Canvas>(canvas);
          SaveFileDialog dialog = new SaveFileDialog()
          {
              Title = "Export to image",
              FileName = "Untitled",
              Filter = " PNG (*.png)|*.png| JPEG (*.jpeg)|*.jpeg"
          };


          if (dialog.ShowDialog() != true)
              return;

          string path = dialog.FileName;
          ExportToImage.ExportToPng(path, mainCanvas, getResolution());
      }

      public void AddItemToNode(NodeViewModel FocusedClass, ObservableCollection<NodeViewModel> Classes, object parameter) {
         undoRedoController.AddAndExecute(new AddItemToNodeCommand(FocusedClass, Classes, parameter));
      }

      private void MouseDownCanvas(MouseEventArgs obj)
      {
          FrameworkElement clickedObj = (FrameworkElement)obj.MouseDevice.Target;
          if (obj.Source is MainWindow)
          {
              FocusedClass = null;
              //hotfix, ikke pænt
              if (movingClass != null)
              {
                  DependencyObject scope = FocusManager.GetFocusScope(movingClass);
                  FocusManager.SetFocusedElement(scope, clickedObj as IInputElement);
                  Keyboard.ClearFocus();
                  Application.Current.MainWindow.Focus();
              }
              FocusedClass = null;
          }

      }

      public void AddNode() {
         undoRedoController.AddAndExecute(new AddClassCommand(Classes));
      }


      public void KeyDownUndo(object param) {
        
         //Keyboard shortcuts for undo/redo when key is pressed
         if ((string)param == "undo") {
            UndoRedoController.GetInstance().Undo();
            _pressed = true;
         }
         if ((string)param == "redo") {
            UndoRedoController.GetInstance().Redo();
            _pressed = true;
         }
      }

      //Captures a keyboard press if on a node
      public void KeyDownNode(KeyEventArgs e) {
         //clears focus from current node if 'enter' is pressed
         if (e.Key == Key.Return) {
            FrameworkElement parent = (FrameworkElement)movingClass.Parent;
            while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable) {
               parent = (FrameworkElement)parent.Parent;
            }

            DependencyObject scope = FocusManager.GetFocusScope(movingClass);
            FocusManager.SetFocusedElement(scope, parent as IInputElement);
            Keyboard.ClearFocus();
            Application.Current.MainWindow.Focus();
            FocusedClass = null;
         }
      }

      //Keyboard shortcut for undo/redo when key is released
      public void UndoRedo_KeyUp(KeyEventArgs e) {
         if (_pressed) {
            _pressed = false;
         }
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

        // Captures the mouse, to move nodes
        public void MouseDownNode(MouseButtonEventArgs e)
        {
            if (!isAddingEdge)
            {
                canvas = FindParent<Canvas>((FrameworkElement)e.MouseDevice.Target);
                _oldMousePos = e.GetPosition(canvas);
                e.MouseDevice.Target.CaptureMouse();
            }
        }

        //Used to move nodes around
        public void MouseMoveNode(MouseEventArgs e)
        {
            //Is the mouse captured?
            if (Mouse.Captured != null)
            {

                movingClass = (FrameworkElement)e.MouseDevice.Target;

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
                canvas = FindParent<Canvas>(movingClass);
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

      public void MouseUpNode(MouseEventArgs e) {
         //Used to move node
         // noden skaffes.
         FrameworkElement movingClass = (FrameworkElement)e.MouseDevice.Target;
         //Noden sættes i fokus
         FocusedClass = (NodeViewModel)movingClass.DataContext;

         if (isAddingEdge) {
            if (startEdge == null)
               startEdge = FocusedClass;
            else if (startEdge != FocusedClass)
            {
               undoRedoController.AddAndExecute(new AddEdgeCommand(Edges, new EdgeViewModel(startEdge, FocusedClass, type)));
               isAddingEdge = false;
               startEdge = null;
                    type = "";
            }
         } else {
            if (_oldMousePos == e.GetPosition(FindParent<Canvas>((FrameworkElement)e.MouseDevice.Target))) {
               e.MouseDevice.Target.ReleaseMouseCapture();
               return;
            }

            // Ellipsens node skaffes.
            NodeViewModel movingNode = (NodeViewModel)movingClass.DataContext;
            // Canvaset skaffes.
            canvas = FindParent<Canvas>(movingClass);
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

      public static T FindParent<T>(DependencyObject child) where T : DependencyObject {
         //get parent item
         DependencyObject parentObject = VisualTreeHelper.GetParent(child);

         //we've reached the end of the tree
         if (parentObject == null) return null;

         //check if the parent matches the type we're looking for
         T parent = parentObject as T;
         if (parent != null) {
            return parent;
         } else {
            return FindParent<T>(parentObject);
         }
      }
      //Currently not used
      private NodeViewModel GetNodeUnderMouse() {
         var item = Mouse.DirectlyOver as DockPanel;
         if (item == null) {
            return null;
         }
         return item.DataContext as NodeViewModel;
      }

      private Point getResolution()
      {
          double xMax = 0, yMax = 0;
          foreach (NodeViewModel nodeVM in Classes)
          {
              if (nodeVM.X+nodeVM.Width > xMax)
              {
                  xMax = nodeVM.X + nodeVM.Width;
              }
              if (nodeVM.Y + nodeVM.Height > yMax)
              {
                  yMax = nodeVM.Y + nodeVM.Height;
              }
          }
          return new Point(xMax, yMax);
      }
   }
}