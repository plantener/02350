using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using UMLDesigner.View;

namespace UMLDesigner.ViewModel
{
    class AddItemToNodeCommand<T> : ICommand
    {
        public bool CanExecute(object parameter)
        {
            Debug.WriteLine("fokuselement er: " + Keyboard.FocusedElement);    
           return true;
          //  throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
          PopupWindow PopupWindow =  new PopupWindow();
          PopupWindow.ShowDialog();

          if (PopupWindow.DialogResult.HasValue && PopupWindow.DialogResult.Value)
          {
              Debug.WriteLine("HEY HO DET VIRKER");
          }
          else
          {
              Debug.WriteLine("Der blev trykket cancel!");
          }


            Debug.WriteLine("Parameter er: " + parameter);
            Debug.WriteLine("fokuselement er: " + Keyboard.FocusedElement);

            //throw new NotImplementedException();
        }
    }
}
