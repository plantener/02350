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
        private NodeViewModel _class = null;
        private int _index = 1;



        public AddClassCommand(ObservableCollection<NodeViewModel> _classes, int index)
        {
            classes = _classes;
            _index = index;
        }

        public void Execute()
        {
            if (_class == null)
            {
                classes.Add(_class = new NodeViewModel() { ClassName = "New Class", X = 100, Y = 100, Id = _index });
            }
            else
            {
                classes.Add(_class);
            }
        }

        public void UnExecute()
        {
            classes.Remove(_class);
        }

    }
}
