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
    class AddClassCommand : IUndoRedoCommand
    {

        private ObservableCollection<Node> classes;
        private Node _class = null;


        public AddClassCommand(ObservableCollection<Node> _classes)
        {
            classes = _classes;
        }

        public void Execute()
        {
            if (_class == null)
            {
                classes.Add(_class = new Node() { ClassName = "AddedClass", X = 100, Y = 100 });
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
