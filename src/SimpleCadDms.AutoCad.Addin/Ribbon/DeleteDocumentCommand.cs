using SimpleCadDms.AutoCad.Addin.CommandHandlers;
using System;
using System.Windows.Input;

namespace SimpleCadDms.AutoCad.Addin.Ribbon
{
    public class DeleteDocumentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(DocumentProperties.FromActiveDocument().DocumentId);
        }

        public void Execute(object parameter)
        {
            CommandHandlerFactory
                .Instance
                .GetDeleteDocumentHandler()
                .Delete();
        }
    }
}
