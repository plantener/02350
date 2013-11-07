using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UMLDesigner.Model;
using UMLDesigner.ViewModel;

namespace UMLDesigner.Command
{
    class MoveNodeCommand : IUndoRedoCommand
    {
        private ObservableCollection<EdgeViewModel> edges;
        private NodeViewModel node;
        private int newX, newY, oldX, oldY;

        public MoveNodeCommand(NodeViewModel _node, int _newX, int _newY, int _oldX, int _oldY, ObservableCollection<EdgeViewModel> _edges)
        {
            this.node = _node;
            this.newX = _newX;
            this.newY = _newY;
            this.oldX = _oldX;
            this.oldY = _oldY;
            this.edges = _edges;
        }

        public void Execute()
        {
            node.X = newX;
            node.Y = newY;
            for (int i = 0; i < edges.Count; i++)
            {
                if (node == edges[i].NVMEndA || node == edges[i].NVMEndB)
                {
                    edges[i].newPath();
                }
            }
        }

        public void UnExecute()
        {
            node.X = oldX;
            node.Y = oldY;
            for (int i = 0; i < edges.Count; i++)
            {
                if (node == edges[i].NVMEndA || node == edges[i].NVMEndB)
                {
                    edges[i].newPath();
                }
            }
        }

    }
}
