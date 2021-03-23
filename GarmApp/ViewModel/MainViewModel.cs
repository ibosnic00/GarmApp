using Microsoft.Win32;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using System.IO;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows;

namespace GarmApp.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        #region Constants

        #endregion


        #region Fields

        readonly SoundLevelMeterViewModel _soundLevelMeterViewModel;
        readonly ExtechVibrationMeterViewModel _extechVibrationMeterViewModel;

        #endregion


        #region Constructors
        public MainViewModel(SoundLevelMeterViewModel soundLevelMeterViewModel, ExtechVibrationMeterViewModel extechVibrationMeterViewModel)
        {
            _soundLevelMeterViewModel = soundLevelMeterViewModel;
            _extechVibrationMeterViewModel = extechVibrationMeterViewModel;
        }

        #endregion


        #region Properties

        FileType _selectedFileType = FileType.SoundLevelMeter;
        public FileType SelectedFileType
        {
            get => _selectedFileType;
            set => Set(nameof(SelectedFileType), ref _selectedFileType, value);
        }

        #endregion


        #region Commands

        ICommand? _saveFileCommand;
        public ICommand SaveFileCommand
        {
            get => _saveFileCommand ??= new RelayCommand(() =>
            {
                switch (SelectedFileType)
                {
                    case FileType.SoundLevelMeter:
                        var file = _soundLevelMeterViewModel.CreateConvertedFile();
                        if (file != string.Empty)
                        {
                            SaveConvertedFile(file);
                        }
                        _soundLevelMeterViewModel.ClearFileCommand.Execute(null);
                        break;
                    case FileType.ExtechVibrationMeter:
                        var file1 = _extechVibrationMeterViewModel.CreateConvertedFile();
                        if (file1 != string.Empty)
                        {
                            SaveConvertedFile(file1);
                        }
                        _extechVibrationMeterViewModel.ClearFileCommand.Execute(null);
                        break;
                    default:
                        break;
                }


            });
        }

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        void SaveConvertedFile(string textFile)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                FileName = Path.ChangeExtension("placeholder_name", ".xls"),
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == true)
            {
                if (saveFileDialog1.FileName != null)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog1.FileName, textFile);
                    }
                    catch
                    { MessageBox.Show("Unable to save file. Invalid path, or insufficent permission.", "Error"); }
                }
            }
        }

        #endregion
    }

    public enum FileType
    {
        [Description("Sound Level Meter")]
        SoundLevelMeter,
        [Description("Extech Vibration Meter")]
        ExtechVibrationMeter,
    }
}
