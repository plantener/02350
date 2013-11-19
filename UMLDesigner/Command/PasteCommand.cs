using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLDesigner.ViewModel;

namespace UMLDesigner.Command
{
    class PasteCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> _classes;
        private NodeViewModel _copyClass = null;

        public PasteCommand(ObservableCollection<NodeViewModel> classes, NodeViewModel copyClass)
        {
            _classes = classes;
            _copyClass = copyClass;
        }

        public void Execute()
        {
            _classes.Add(_copyClass);
        }

        public void UnExecute()
        {
            _classes.Remove(_copyClass);
        }
    }
}
