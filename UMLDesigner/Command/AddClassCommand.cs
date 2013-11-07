using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UMLDesigner.Model;
using UMLDesigner.ViewModel;

namespace UMLDesigner.Command
{
    class AddClassCommand : IUndoRedoCommand
    {

        private ObservableCollection<NodeViewModel> classes;
        private NodeViewModel _class;


        public AddClassCommand(ObservableCollection<NodeViewModel> _classes)
        {
            classes = _classes;
        }

        public void Execute()
        {
            classes.Add(_class = new NodeViewModel() { ClassName = "AddedClass", X = 100, Y = 100 });
        }

        public void UnExecute()
        {
            classes.Remove(_class);
        }

    }
}
