using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GarmApp.Model;
using GarmApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GarmApp.ViewModel
{
    class ExtechVibrationMeterViewModel : ViewModelBase
    {
        #region Constants

        #endregion


        #region Fields

        readonly IOpenFileDialogService _openFileDialogService;

        #endregion


        #region Constructors

        public ExtechVibrationMeterViewModel(IOpenFileDialogService openFileDialogService)
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

        ExtechVibrationMeterFile? _selectedExtechVibrationeterFile;
        public ExtechVibrationMeterFile? SelectedExtechVibrationMeterFile
        {
            get => _selectedExtechVibrationeterFile;
            set => Set(nameof(SelectedExtechVibrationMeterFile), ref _selectedExtechVibrationeterFile, value);
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
                        SelectedExtechVibrationMeterFile = new ExtechVibrationMeterFile(fileName);
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
                SelectedExtechVibrationMeterFile = null;
            });
        }

        #endregion


        #region Public Methods

        public string CreateConvertedFile()
        {
            if (SelectedExtechVibrationMeterFile == null)
                return string.Empty;

            return ExtechVibrationMeterFile.CreateFile(SelectedExtechVibrationMeterFile, UseAbsoluteTimeFormat);
        }

        #endregion


        #region Private Methods

        #endregion
    }
}
