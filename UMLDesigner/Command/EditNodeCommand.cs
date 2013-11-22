using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLDesigner.View;
using UMLDesigner.ViewModel;
using Attribute = UMLDesigner.Model.Attribute;

namespace UMLDesigner.Command
{
    class EditNodeCommand: IUndoRedoCommand
    {
        private NodeViewModel _focusedClass;
        private ObservableCollection<NodeViewModel> _classes;
        private ObservableCollection<Attribute> _methodList;
        private ObservableCollection<Attribute> _attributeList;
        private PopupWindow _popupWindow;
        private ObservableCollection<string> _availableTypesForAttributes;
        private ObservableCollection<string> _availableTypesForMethod;
        private List<Attribute> _methodsToAdd;
        private List<Attribute> _attributesToAdd;
        private List<Attribute> _undoAbleAtrList;
        private List<Attribute> _undoAbleMethodList;


        //The constructor does a lot of things, to ensure that when redo is executing, only as little as possible is done there.
        public EditNodeCommand(NodeViewModel focusedClass, ObservableCollection<NodeViewModel> classes)
        {
            _focusedClass = focusedClass;
            _classes = classes;
            _attributeList = new ObservableCollection<Attribute>();
            _methodList = new ObservableCollection<Attribute>();
            _undoAbleAtrList = new List<Attribute>();
            _undoAbleMethodList = new List<Attribute>();

            _availableTypesForAttributes = new ObservableCollection<String>();
            _availableTypesForMethod = new ObservableCollection<String>();

            _attributesToAdd = new List<Model.Attribute>();
            _methodsToAdd = new List<Model.Attribute>();

            //Populate the popupbox with attributes already on the node.
            foreach (var i in _focusedClass.Attributes)
            {
                Attribute a = new Attribute() { Modifier = i.Modifier, Name = i.Name, Type = i.Type };
                _attributeList.Add(i);
                _undoAbleAtrList.Add(a);

            }

            //Populate the popupbox with methods already on the node.
            foreach (var i in _focusedClass.Methods)
            {
                Attribute a = new Attribute(){Modifier = i.Modifier, Name = i.Name, Type = i.Type};
                _methodList.Add(i);
                _undoAbleMethodList.Add(a);
            }


            _popupWindow = new PopupWindow
            {
                dgList = { ItemsSource = _attributeList },
                methodList = { ItemsSource = _methodList },
                TestType = { ItemsSource = _availableTypesForAttributes },
                methodType = { ItemsSource = _availableTypesForMethod }
            };

            PopulateTypesInPopupBox();
            
            _popupWindow.ShowDialog();

            if (_popupWindow.DialogResult.HasValue && _popupWindow.DialogResult.Value)
            {

              

                foreach (Attribute n in _attributeList)
                {

                    //Dont add empty selections
                    if (string.IsNullOrEmpty(n.Name))
                    {
                        continue;
                    }

                    //Add the item to the node
                    _attributesToAdd.Add(n);
                }

                foreach (Attribute n in _methodList)
                {
                    //dont add empty selections
                    if (string.IsNullOrEmpty(n.Name))
                    {
                        continue;
                    }
                    _methodsToAdd.Add(n);
                }

            }
            //Cancel was pressed
            else{ return;}
        }

        public void Execute()
        {
            _focusedClass.Attributes.Clear();
            _focusedClass.Methods.Clear();


            foreach (UMLDesigner.Model.Attribute a in _attributesToAdd)
            {

                _focusedClass.Attributes.Add(a);
            }
            foreach (UMLDesigner.Model.Attribute m in _methodsToAdd)
            {
                _focusedClass.Methods.Add(m);
            }

            if(_undoAbleAtrList.Count >0){
            System.Console.WriteLine("Hvad er undo nu " + _undoAbleAtrList[0].Name);
                System.Console.WriteLine("Det her er attributlisten" + _attributeList[0].Name); 
            }
        }

        public void UnExecute()
        {
            //Clear the current items in the list:
            _focusedClass.Attributes.Clear();
            _focusedClass.Methods.Clear();
            //_attributeList.Clear();
            //_methodList.Clear();

            //Reload the old data:
            foreach (Attribute a in _undoAbleAtrList)
            {
                System.Console.WriteLine(a.Name);
                _focusedClass.Attributes.Add(a);
            }
            foreach (Attribute a in _undoAbleMethodList)
            {
                _focusedClass.Methods.Add(a);
            }
        }


          public void PopulateTypesInPopupBox()
        {
            //Make sure you can choose type void when adding method
            _availableTypesForMethod.Add("void");

            _availableTypesForAttributes.Add("Int");
            _availableTypesForAttributes.Add("String");
            _availableTypesForAttributes.Add("Float");
            _availableTypesForAttributes.Add("Double");
            _availableTypesForMethod.Add("Int");
            _availableTypesForMethod.Add("String");
            _availableTypesForMethod.Add("Float");
            _availableTypesForMethod.Add("Double");

            //Add Classnames as available types
            foreach (NodeViewModel node in _classes)
            {
                _availableTypesForAttributes.Add(node.ClassName);
                _availableTypesForMethod.Add(node.ClassName);
            }
        }
    }

  
}
