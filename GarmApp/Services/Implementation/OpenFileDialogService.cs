using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace GarmApp.Services
{
    class OpenFileDialogService : IOpenFileDialogService
    {
        public string? GetFileName(string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.Multiselect = false;

            var tryToOpenFileDialog = openFileDialog.ShowDialog();
            return (tryToOpenFileDialog.HasValue && tryToOpenFileDialog.Value) ? openFileDialog.FileName : null;
        }
    }
}
