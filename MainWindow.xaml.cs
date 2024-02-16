using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

namespace Simulator.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// @Author Liza Danielsson.
    /// </summary>
    public partial class MainWindow : Window
    {

        private TaskManager taskManager;
        private MusicPlayer musicPlayer;

        public MainWindow()
        {
            InitializeComponent();
            taskManager = new TaskManager(Application.Current.Dispatcher);
            AssetsListView.ItemsSource = taskManager.Assets;

            musicPlayer = new MusicPlayer("backgroundMusic.wav");
        }


        /// <summary>
        /// Event handler for the Play Music button click. Starts music playback.
        /// </summary>
        private void PlayMusicButton_Click(object sender, EventArgs e)
        {
            musicPlayer.StartMusic();
        }


        /// <summary>
        /// Event handler for the Stop Music button click. Stops music playback.
        /// </summary>
        private void StopMusicButton_Click(object sender, EventArgs e)
        {
            musicPlayer.StopMusic();
        }


        /// <summary>
        /// Event handler for the Add Item button click. Adds a new asset to the simulation.
        /// Validates user input before adding the asset.
        /// </summary>
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            string itemName = assetNameInput.Text.Trim();
            string itemDescription = assetDescriptionInput.Text.Trim();

            if (!string.IsNullOrEmpty(itemName) && !string.IsNullOrEmpty(itemDescription))
            {
                // Add the asset through TaskManager
                taskManager.AddAsset(itemName, itemDescription);

                assetNameInput.Clear();
                assetDescriptionInput.Clear();
            }
            else
            {
                // Display an error message or some validation response
                MessageBox.Show("Please enter both name and description for the asset.");
            }
        }


        /// <summary>
        /// Event handler for the Start Threads button click. Starts the asset borrowing simulation.
        /// Disables the Start Threads button to prevent multiple clicks.
        /// </summary>
        private void StartThreadsButton_Click(object sender, RoutedEventArgs e)
        {
            StartThreadsButton.IsEnabled = false;

            taskManager.StartAllThreads();
        }


        /// <summary>
        /// Event handler for the Stop Threads button click. Stops the asset borrowing simulation.
        /// Re-enables the Start Threads button after stopping.
        /// </summary>
        private void StopThreadsButton_Click(object sender, RoutedEventArgs e)
        {
            taskManager.StopAllThreads();

            StartThreadsButton.IsEnabled = true;
        }

    }
}
