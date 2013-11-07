using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
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
        private UMLDesigner.Model.Attribute itemToAdd;

        public AddItemToNodeCommand(NodeViewModel focusedClass, object parameter)
        {
            _focusedClass = focusedClass;
            _parameter = parameter;
        }

        public void Execute()
        {
          PopupWindow PopupWindow =  new PopupWindow();

            //Populate type list on popupwindow
          PopupWindow.cmbTypes.Items.Add("Int");
          PopupWindow.cmbTypes.Items.Add("String");
          PopupWindow.cmbTypes.Items.Add("Float");
          PopupWindow.cmbTypes.Items.Add("Double");

          PopupWindow.cmbTypes.SelectedIndex = 0;
         
          if ((String)_parameter == "Method")
          {
              PopupWindow.cmbTypes.Items.Add("void");
          }

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
}
