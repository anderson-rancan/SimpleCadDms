using SimpleCadDms.AutoCad.Addin.CommandHandlers;
using System;
using System.Windows;
using System.Windows.Input;

namespace SimpleCadDms.AutoCad.Addin.Ribbon
{
    internal class GenerateNewIdCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var documentId = CommandHandlerFactory
                .Instance
                .GetNewDocumentIdHandler()
                .CreateNewId();

            Clipboard.SetText(documentId);

            MessageBox.Show(
                string.Format("The {0} document ID was generated and copied to the clipboard.", documentId),
                "New Document ID",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}