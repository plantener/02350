using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace UMLDesigner.View
{
    /// <summary>
    /// Interaction logic for ClassUserControl.xaml
    /// </summary>
    public partial class ClassUserControl : UserControl
    {
        ObservableCollection<GameData> _GameCollection =
        new ObservableCollection<GameData>();

        public ClassUserControl()
        {
            _GameCollection.Add(new GameData
            {
                GameName = "World Of Warcraft",
                Creator = "Blizzard",
                Publisher = "Blizzard"
            });
            _GameCollection.Add(new GameData
            {
                GameName = "Halo",
                Creator = "Bungie",
                Publisher = "Microsoft"
            });
            _GameCollection.Add(new GameData
            {
                GameName = "Gears Of War",
                Creator = "Epic",
                Publisher = "Microsoft"
            });
            InitializeComponent();
        }

        public ObservableCollection<GameData> GameCollection
        { get { return _GameCollection; } }

        public class GameData
        {
            public string GameName { get; set; }
            public string Creator { get; set; }
            public string Publisher { get; set; }
        }
    }
}
