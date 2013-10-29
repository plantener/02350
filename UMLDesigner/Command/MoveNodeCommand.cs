using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UMLDesigner.Model;

namespace UMLDesigner.Command
{
    class MoveNodeCommand : IUndoRedoCommand
    {
        private ObservableCollection<Edge> edges;
        private Node node;
        private int newX, newY, oldX, oldY;

        public MoveNodeCommand(Node _node, int _newX, int _newY, int _oldX, int _oldY, ObservableCollection<Edge> _edges)
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
                if (node == edges[i].Start || node == edges[i].End)
                {
                    edges[i].Path = "";
                }
            }
        }

        public void UnExecute()
        {
            node.X = oldX;
            node.Y = oldY;
            for (int i = 0; i < edges.Count; i++)
            {
                if (node == edges[i].Start || node == edges[i].End)
                {
                    edges[i].Path = "";
                }
            }
        }

    }
}
