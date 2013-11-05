using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using UMLDesigner.Command;

namespace UMLDesigner {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window {
      public MainWindow() {
         InitializeComponent();
      }

      private bool _pressed = false;
      private void Window_KeyDown(object sender, KeyEventArgs e) {
         if (Keyboard.Modifiers == ModifierKeys.Control && _pressed == false) {
            if (Keyboard.IsKeyDown(Key.Z)) {
               UndoRedoController.GetInstance().Undo();
               _pressed = true;
            }
            if (Keyboard.IsKeyDown(Key.Y)) {
               UndoRedoController.GetInstance().Redo();
               _pressed = true;
            }


         }

         e.Handled = true;
      }

      private void Window_KeyUp(object sender, KeyEventArgs e) {
         if (_pressed == true) {
            System.Diagnostics.Debug.WriteLine("unpressed");
            _pressed = false;
         }

      }



   }
}
