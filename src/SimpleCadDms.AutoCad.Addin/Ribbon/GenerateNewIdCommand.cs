using SimpleCadDms.AutoCad.Addin.CommandHandlers;
using System;
using System.Windows.Input;

using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

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

            var doc = AcadApp.DocumentManager.MdiActiveDocument;
            doc.Editor.WriteMessage(string.Format("{0}The {1} document ID was successfully generated!{0}", Environment.NewLine, documentId));
        }
    }
}