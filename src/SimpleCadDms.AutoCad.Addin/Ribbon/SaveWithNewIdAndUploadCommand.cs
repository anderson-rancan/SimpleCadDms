using SimpleCadDms.AutoCad.Addin.CommandHandlers;
using System;
using System.Windows.Input;

namespace SimpleCadDms.AutoCad.Addin.Ribbon
{
    internal class SaveWithNewIdAndUploadCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CommandHandlerFactory
                .Instance
                .GetSaveWithNewIdAndUpload()
                .SaveAndUpload();
        }
    }
}