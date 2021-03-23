using GarmApp.ViewModel;
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

namespace GarmApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainView_Loaded;
            Closing += MainView_Closing;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is MainViewModel))
                throw new Exception("Data context in main view is not valid.");
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var userDecision = MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButton.YesNo);
            if (userDecision == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)            
                DragMove();            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class FileViewDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? SoundLevelMeterDataTemplate { get; set; }

        public DataTemplate? ExtechVibrationMeterDataTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            var flashInterfaceType = (FileType)item;

            if (flashInterfaceType == FileType.SoundLevelMeter)
                return SoundLevelMeterDataTemplate;
            if (flashInterfaceType == FileType.ExtechVibrationMeter)
                return ExtechVibrationMeterDataTemplate;
            else
                throw new NotImplementedException();
        }
    }
}
