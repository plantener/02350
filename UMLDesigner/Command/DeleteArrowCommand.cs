using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLDesigner.ViewModel;

namespace UMLDesigner.Command
{
    class DeleteArrowCommand : IUndoRedoCommand
    {
        private ObservableCollection<EdgeViewModel> _edges;
        private EdgeViewModel _focusedEdge;

        public DeleteArrowCommand(ObservableCollection<EdgeViewModel> edges, EdgeViewModel focusedEdge)
        {
            _edges = edges;
            _focusedEdge = focusedEdge;
        }

        public void Execute()
        {
            _edges.Remove(_focusedEdge);
        }

        public void UnExecute()
        {
            _edges.Add(_focusedEdge);
        }


    }
}
