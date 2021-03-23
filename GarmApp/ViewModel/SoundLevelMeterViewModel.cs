using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using GarmApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GarmApp.Model;
using System.Windows;

namespace GarmApp.ViewModel
{
    class SoundLevelMeterViewModel : ViewModelBase
    {
        #region Constants

        #endregion


        #region Fields

        readonly IOpenFileDialogService _openFileDialogService;

        #endregion


        #region Constructors

        public SoundLevelMeterViewModel(IOpenFileDialogService openFileDialogService)
        {
            _openFileDialogService = openFileDialogService;
        }

        #endregion


        #region Properties

        string? _filePath;
        public string? FilePath
        {
            get => _filePath;
            set => Set(nameof(FilePath), ref _filePath, value);
        }

        SoundLevelMeterFile? _selectedSoundLevelMeterFile;
        public SoundLevelMeterFile? SelectedSoundLevelMeterFile
        {
            get => _selectedSoundLevelMeterFile;
            set => Set(nameof(SelectedSoundLevelMeterFile), ref _selectedSoundLevelMeterFile, value);
        }

        bool _useAbsoluteTimeFormat;
        public bool UseAbsoluteTimeFormat
        {
            get => _useAbsoluteTimeFormat;
            set => Set(nameof(UseAbsoluteTimeFormat), ref _useAbsoluteTimeFormat, value);
        }

        #endregion

        #region Commands

        ICommand? _browseForFileCommand;
        public ICommand BrowseForFileCommand
        {
            get => _browseForFileCommand ??= new RelayCommand(() =>
            {
                var fileName = _openFileDialogService.GetFileName("Sound Level Meter files | *.xls");
                if (fileName != null)
                {
                    try
                    {
                        FilePath = fileName;
                        SelectedSoundLevelMeterFile = new SoundLevelMeterFile(fileName);
                    }
                    catch { MessageBox.Show("Invalid file selected.", "Error"); }
                }
            });
        }


        ICommand? _clearFileCommand;
        public ICommand ClearFileCommand
        {
            get => _clearFileCommand ??= new RelayCommand(() =>
            {
                FilePath = null;
                SelectedSoundLevelMeterFile = null;
            });
        }

        #endregion


        #region Public Methods

        public string CreateConvertedFile()
        {
            if (SelectedSoundLevelMeterFile == null)
                return string.Empty;

            return SoundLevelMeterFile.CreateFile(SelectedSoundLevelMeterFile, UseAbsoluteTimeFormat);
        }

        #endregion


        #region Private Methods

        #endregion
    }
}
