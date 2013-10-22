using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

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


            Debug.WriteLine("Parameter er: " + parameter);
            Debug.WriteLine("fokuselement er: " + Keyboard.FocusedElement);

            //throw new NotImplementedException();
        }
    }
}
