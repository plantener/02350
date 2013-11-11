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

namespace UMLDesigner.Command
{
    class AddItemToNodeCommand : IUndoRedoCommand
    {
        private object _parameter;
        private NodeViewModel _focusedClass;
        private ObservableCollection<NodeViewModel> _classes;
        public ObservableCollection<NewItems> itemsList { get; set; }
        public ObservableCollection<String> AvailableTypes;
        public List<UMLDesigner.Model.Attribute> itemsToAdd;

        public AddItemToNodeCommand(NodeViewModel focusedClass,ObservableCollection<NodeViewModel> Classes, object parameter)
        {
            _focusedClass = focusedClass;
            _parameter = parameter;
            _classes = Classes;
            itemsList = new ObservableCollection<NewItems>();
            AvailableTypes = new ObservableCollection<String>();
            itemsToAdd = new List<Model.Attribute>();

        }

        public void Execute()
        {

          PopupWindow PopupWindow =  new PopupWindow();
          PopupWindow.dgList.ItemsSource = itemsList;

            //this works!
          PopupWindow.TestType.ItemsSource = AvailableTypes;


          //Make sure you can choose type void if adding method
          if ((String)_parameter == "Method")
          {
             AvailableTypes.Add("void");
          }

          AvailableTypes.Add("Int");
          AvailableTypes.Add("String");
          AvailableTypes.Add("Float");
          AvailableTypes.Add("Double");


          //Add Classnames as available types
          foreach (NodeViewModel node in _classes)
          {
             AvailableTypes.Add(node.ClassName);
          }


          PopupWindow.ShowDialog();

         

          if (PopupWindow.DialogResult.HasValue && PopupWindow.DialogResult.Value)
          {
            
              foreach (NewItems n in itemsList)
              {
                  //Dont add empty selections
                  if (n.ClassName == null)
                  {
                      continue;
                  }
                  //Convert the inputted data into attribute type, that we can do undo on aswell.
                  itemsToAdd.Add(new UMLDesigner.Model.Attribute { Name = n.ClassName, Modifier = n.Visibility, Type = n.Type });

                 
              }
              foreach (UMLDesigner.Model.Attribute a in itemsToAdd)
              {
                  //Cast object to String, since we know the commandparameter is String, this is possible
                  if ((String)_parameter == "Attribute")
                  {
                      _focusedClass.Attributes.Add(a);
                  }
                  //Cast object to String, since we know the commandparameter is String, this is possible
                  else if ((String)_parameter == "Method")
                  {
                      _focusedClass.Methods.Add(a);

                  }
              }
              }

          else
              //Cancel was pressed
          {
              return;
          }
        }


        public void UnExecute()
        {
            foreach (UMLDesigner.Model.Attribute a in itemsToAdd)
            {


                if ((String)_parameter == "Method")
                {
                    _focusedClass.Methods.Remove(a);
                }
                else if ((String)_parameter == "Attribute")
                {
                    _focusedClass.Attributes.Remove(a);
                }
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
