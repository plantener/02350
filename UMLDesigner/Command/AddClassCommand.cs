using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UMLDesigner.Model;

namespace UMLDesigner.Command
{
    class AddClassCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;
        private ObservableCollection<Class> classes;
        private Class _class;


        public AddClassCommand(ObservableCollection<Class> _classes)
        {
            classes = _classes;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        //    throw new NotImplementedException();
        }

   

        public void Execute(object parameter)
        {
            classes.Add(_class = new Class() { ClassName = "AddedClass", X = 100, Y = 100 });
        }
    }
}
