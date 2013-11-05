using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UMLDesigner.Model;
using UMLDesigner.View;

namespace UMLDesigner.Command
{
    class AddItemToNodeCommand : IUndoRedoCommand
    {
        private object _parameter;
        private Node _focusedClass;
        private UMLDesigner.Model.Attribute itemToAdd;
        private ObservableCollection<Node> _classes;
        public ObservableCollection<NewItems> itemsList { get; set; }

        public AddItemToNodeCommand(Node focusedClass,ObservableCollection<Node> Classes, object parameter)
        {
            _focusedClass = focusedClass;
            _parameter = parameter;
            _classes = Classes;
            itemsList = new ObservableCollection<NewItems>();
        }

        public void Execute()
        {
          PopupWindow PopupWindow =  new PopupWindow();
          PopupWindow.dgList.ItemsSource = itemsList;
      
            //Populate type list on popupwindow
          //PopupWindow.cmbTypes.Items.Add("Int");
          //PopupWindow.cmbTypes.Items.Add("String");
          //PopupWindow.cmbTypes.Items.Add("Float");
          //PopupWindow.cmbTypes.Items.Add("Double");

          //PopupWindow.dgList.Items.Add("Int");
          //PopupWindow.dgList.Items.Add("String");
          //PopupWindow.dgList.Items.Add("Float");
          //PopupWindow.dgList.Items.Add("Double");

            //Mae sure you can choose type void if adding attribute
          //if ((String)_parameter == "Method")
          //{
          //    PopupWindow.dgList.Items.Add("void");
          //}


          //foreach (Node node in _classes)
          //{
          //    PopupWindow.cmbTypes.Items.Add(node.ClassName);
          //}

          //PopupWindow.cmbTypes.SelectedIndex = 0;
         
          //if ((String)_parameter == "Method")
          //{
          //    PopupWindow.cmbTypes.Items.Add("void");
          //}

          PopupWindow.ShowDialog();

         

          if (PopupWindow.DialogResult.HasValue && PopupWindow.DialogResult.Value)
          {

              //Get info from popup
              String _selectedType = PopupWindow.GetSelectedItem;
              String _selectedName = PopupWindow.GetName;
              bool _visibility = PopupWindow.visibility;

              itemToAdd = new UMLDesigner.Model.Attribute { Name = _selectedName, Modifier = _visibility, Type = _selectedType };

              //Cast object to String, since we know the commandparameter is String, this is possible
              if ((String)_parameter == "Attribute")
              {      
                  _focusedClass.Attributes.Add(itemToAdd);
              }
              else if ((String)_parameter == "Method")
              {
                  _focusedClass.Methods.Add(itemToAdd);

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
            if ((String)_parameter == "Method")
            {
                _focusedClass.Methods.Remove(itemToAdd);
            }else if ((String)_parameter == "Attribute")
            {
                _focusedClass.Attributes.Remove(itemToAdd);
            }
        }

       
       
    }
     public class NewItems
        {
         public String ClassName  { get;  set; }
         public String Type { get; set; }
         public bool Visibility { get; set; }
        }
}
