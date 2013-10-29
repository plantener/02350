using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using UMLDesigner.View;

namespace UMLDesigner.Command
{
    class AddItemToNodeCommand : IUndoRedoCommand
    {
        private object _parameter;

        public AddItemToNodeCommand(object parameter)
        {
            _parameter = parameter;
        }

        public void Execute()
        {

            MessageBox.Show("parameteren er: " + _parameter);
          PopupWindow PopupWindow =  new PopupWindow();
          PopupWindow.ShowDialog();

         

          if (PopupWindow.DialogResult.HasValue && PopupWindow.DialogResult.Value)
          {
              
              Debug.WriteLine("HEY HO DET VIRKER");
              String selectedType = PopupWindow.getSelectedItem;
              Debug.WriteLine("TYPEN ER: " + selectedType);
             
          }
          else
          {
              Debug.WriteLine("Der blev trykket cancel!");
          }
          


           // Debug.WriteLine("Parameter er: " + parameter);
            Debug.WriteLine("fokuselement er: " + Keyboard.FocusedElement);

            //throw new NotImplementedException();
        }


        public void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
