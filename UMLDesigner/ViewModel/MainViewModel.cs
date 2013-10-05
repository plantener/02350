using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UMLDesigner.Model;

namespace UMLDesigner.ViewModel
{
 
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Class> Classes { get; set; }

        // Kommandoer som UI bindes til.
        public ICommand AddClassCommand { get; private set; }


        public MainViewModel()
        {

            // Her fyldes listen af classes med to classes. Her benyttes et alternativ til konstruktorer med syntaksen 'new Type(){ Attribut = V�rdi }'
            // Det der sker er at der f�rst laves et nyt object og s� s�ttes objektets attributer til de givne v�rdier.
            Classes = new ObservableCollection<Class>()
            { 
                new Class() { ClassName = "TestClass", X = 30, Y = 40, Attributes = {"Hej","Test"}, Methods = {"Vi","tester","mere"}, Properties={"properties"}},
                new Class() { ClassName = "TestClass", X = 140, Y = 230, Methods = {"Endnu", "En", "test"}, Attributes = {"Attribut"}, Properties= {"properties", "her"}},
                new Class() {ClassName = "NewClass", Attributes = {"Attributtest", "Attributtest2"}, Methods = { "MethodTest", "MethodTest2"}, Properties = {"PropertiesTest", "ProperTiesTest2"}}
            };
        }
    }
}