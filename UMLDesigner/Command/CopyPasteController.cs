using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLDesigner.Model;
using UMLDesigner.ViewModel;

namespace UMLDesigner.Command
{
    class CopyPasteController
    {
        private static CopyPasteController controller = new CopyPasteController();

        private CopyPasteController()
        {
        }

        public static CopyPasteController GetInstance() { return controller; }

        public bool CanCopy(Node FocusedClass)
        {
            if (FocusedClass is Node)
            {
                return true;
            }
            return false;
        }

        public void Copy()
        {
            
        }
    }
}
