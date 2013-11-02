using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLDesigner.Model;

namespace UMLDesigner.Command
{
    class AddEdgeCommand : IUndoRedoCommand
    {

        private ObservableCollection<Edge> edges;
        private Edge _edge;

        public AddEdgeCommand(ObservableCollection<Edge> _edges, Node _start, Node _end)
        {
            edges = _edges;
            _edge = new Edge(_start, _end) {};
        }   

        public void Execute()
        {
            edges.Add(_edge);
        }

        public void UnExecute()
        {
            edges.Remove(_edge);
        }
    }
}
