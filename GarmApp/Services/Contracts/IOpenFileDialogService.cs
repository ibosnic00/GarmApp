using System;
using System.Collections.Generic;
using System.Text;

namespace GarmApp.Services
{
    interface IOpenFileDialogService
    {
        string? GetFileName(string filter);
    }
}
