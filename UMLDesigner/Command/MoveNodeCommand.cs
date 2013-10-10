using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UMLDesigner.Model;

namespace UMLDesigner.Command
{
    class MoveNodeCommand : ICommand
    {

        private Node node;
        private int newX, newY, oldX, oldY;

        public MoveNodeCommand(Node _node, int _newX, int _newY, int _oldX, int _oldY)
        {
            this.node = _node;
            this.newX = _newX;
            this.newY = _newY;
            this.oldX = _oldX;
            this.oldY = _oldY;
        }

        public bool CanExecute(object parameter)
        {
           // throw new NotImplementedException();
            return true;
            //TODO Implementer den her
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            node.X = newX;
            node.Y = newY;
        }
    }
}
