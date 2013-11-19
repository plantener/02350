using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UMLDesigner.Model;
using UMLDesigner.View;
using UMLDesigner.ViewModel;
using Attribute = UMLDesigner.Model.Attribute;

namespace UMLDesigner.Command
{
    class AddItemToNodeCommand : IUndoRedoCommand
    {
        private object _parameter;
        private NodeViewModel _focusedClass;
        private ObservableCollection<NodeViewModel> _classes;
        public ObservableCollection<NewItems> itemsList { get; set; }
        public ObservableCollection<NewItems> methodList { get; set; }

        public ObservableCollection<String> AvailableTypes;
        public ObservableCollection<String> AvailableTypesForMethod;

        public List<UMLDesigner.Model.Attribute> attributesToAdd;
        public List<UMLDesigner.Model.Attribute> methodsToAdd;

        public PopupWindow PopupWindow = null;

        public AddItemToNodeCommand(NodeViewModel focusedClass,ObservableCollection<NodeViewModel> classes, object parameter)
        {
            _focusedClass = focusedClass;
            _parameter = parameter;
            _classes = classes;
            itemsList = new ObservableCollection<NewItems>();
            methodList = new ObservableCollection<NewItems>();

            AvailableTypes = new ObservableCollection<String>();
            AvailableTypesForMethod = new ObservableCollection<String>();

            attributesToAdd = new List<Model.Attribute>();
            methodsToAdd = new List<Model.Attribute>();


        }

        public void Execute()
        {
          

            //Used to detect if its a redo that has been done, if it is a redo, we dont want to create the popup window
            if (PopupWindow == null)
            {

                foreach (var i in _focusedClass.Attributes)
                {
                    itemsList.Add(

                        new NewItems { ClassName = i.Name, Type = i.Type, Visibility = i.Modifier }

                );
                }

                foreach (var i in _focusedClass.Methods)
                {
                    methodList.Add(
                    
                        new NewItems {ClassName = i.Name, Type = i.Type, Visibility = i.Modifier}
                    
                );
                }
            
            PopupWindow = new PopupWindow
            {
                dgList = {ItemsSource = itemsList},
                methodList = {ItemsSource = methodList},
                TestType = {ItemsSource = AvailableTypes},
                methodType = {ItemsSource = AvailableTypesForMethod}
            };


                //Make sure you can choose type void when adding method
           
            AvailableTypesForMethod.Add("void");
            

            AvailableTypes.Add("Int");
            AvailableTypes.Add("String");
            AvailableTypes.Add("Float");
            AvailableTypes.Add("Double");
            AvailableTypesForMethod.Add("Int");
            AvailableTypesForMethod.Add("String");
            AvailableTypesForMethod.Add("Float");
            AvailableTypesForMethod.Add("Double");


            //Add Classnames as available types
            foreach (NodeViewModel node in _classes)
            {
                AvailableTypes.Add(node.ClassName);
                AvailableTypesForMethod.Add(node.ClassName);
            }
            //Used if a redo is made
           
                PopupWindow.ShowDialog();



                if (PopupWindow.DialogResult.HasValue && PopupWindow.DialogResult.Value)
                {
                    _focusedClass.Attributes.Clear();
                    _focusedClass.Methods.Clear();

                    foreach (NewItems n in itemsList)
                    {
                        //Dont add empty selections
                        if (string.IsNullOrEmpty(n.ClassName))
                        {
                            continue;
                        }
                        //Convert the inputted data into attribute type, that we can do undo on aswell.
                        attributesToAdd.Add(new UMLDesigner.Model.Attribute
                        {
                            Name = n.ClassName,
                            Modifier = n.Visibility,
                            Type = n.Type
                        });
                    }

                    foreach (NewItems n in methodList)
                    {
                        //dont add empty selections
                        if (string.IsNullOrEmpty(n.ClassName))
                        {
                            continue;
                        }
                        methodsToAdd.Add(new UMLDesigner.Model.Attribute
                        {
                            Name = n.ClassName,
                            Modifier = n.Visibility,
                            Type = n.Type
                        });
                    }

                }
                else
                    //Cancel was pressed
                {
                    return;
                }
            }
            foreach (UMLDesigner.Model.Attribute a in attributesToAdd)
                    {
                      
                            _focusedClass.Attributes.Add(a);
                    }
            foreach (UMLDesigner.Model.Attribute m in methodsToAdd)
            {
                _focusedClass.Methods.Add(m);
            }

        }

        public void UnExecute()
        {
            foreach (UMLDesigner.Model.Attribute a in attributesToAdd)
            {

                _focusedClass.Attributes.Remove(a);
                
            }
            foreach (UMLDesigner.Model.Attribute a in methodsToAdd)
            {
                _focusedClass.Methods.Remove(a);
            }
        }

       
       
    }
    //NewItems is used to compose an element in the xaml. The popup window binds to each of these.
     public class NewItems
        {
         public String ClassName  { get;  set; }
         public String Type { get; set; }
         public bool Visibility { get; set; }

         public NewItems()
         {
             //Set this in the constructor, such that it is the first choice when choosing type in the popup
             Type = "Int";
         }
        
        }


    
}
