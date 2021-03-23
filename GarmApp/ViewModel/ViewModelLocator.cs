using System;
using System.Collections.Generic;
using System.Text;
using GarmApp.Services;
using GalaSoft.MvvmLight.Ioc;

namespace GarmApp.ViewModel
{
    class ViewModelLocator
    {
        static ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IOpenFileDialogService, OpenFileDialogService>();
            SimpleIoc.Default.Register<SoundLevelMeterViewModel>();
            SimpleIoc.Default.Register<ExtechVibrationMeterViewModel>();

        }

        public MainViewModel MainVM
        {
            get => SimpleIoc.Default.GetInstance<MainViewModel>();
        }

        public SoundLevelMeterViewModel SoundLevelMeterVM
        {
            get => SimpleIoc.Default.GetInstance<SoundLevelMeterViewModel>();
        }

        public ExtechVibrationMeterViewModel ExtechVibrationMeterVM
        {
            get => SimpleIoc.Default.GetInstance<ExtechVibrationMeterViewModel>();
        }
    }
}
