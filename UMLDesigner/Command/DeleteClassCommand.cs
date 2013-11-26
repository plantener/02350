using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLDesigner.ViewModel;

namespace UMLDesigner.Command
{
    class DeleteClassCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> _classes;
        private NodeViewModel _focusedClass;
        private ObservableCollection<EdgeViewModel> _edges;
        private ObservableCollection<EdgeViewModel> _removedEdges;

        public DeleteClassCommand(ObservableCollection<NodeViewModel> classes, NodeViewModel focusedClass, ObservableCollection<EdgeViewModel> edges)
        {
            _classes = classes;
            _focusedClass = focusedClass;
            _edges = edges;
            _removedEdges = new ObservableCollection<EdgeViewModel>();
        }

        public void Execute()
        {
            foreach (EdgeViewModel edge in _edges)
            {
                if (_focusedClass == edge.NVMEndA || _focusedClass == edge.NVMEndB)
                {
                    _removedEdges.Add(edge);
                }
            }
            foreach (EdgeViewModel edge in _removedEdges)
            {
                _edges.Remove(edge);
            }
            _classes.Remove(_focusedClass);
        }

        public void UnExecute()
        {
            if (_edges.Count == 1)
            {
                _edges.Clear();
            }
            foreach (EdgeViewModel edge in _removedEdges)
            {
                _edges.Add(edge);
            }
            _classes.Add(_focusedClass);
        }
    }
}
