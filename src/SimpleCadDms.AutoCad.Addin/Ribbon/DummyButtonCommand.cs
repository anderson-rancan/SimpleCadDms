using System;
using System.Windows.Input;

using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.Windows;

namespace SimpleCadDms.AutoCad.Addin.Ribbon
{
    internal class DummyButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var doc = AcadApp.DocumentManager.MdiActiveDocument;

            if (parameter is RibbonButton)
            {
                RibbonButton button = parameter as RibbonButton;
                doc.Editor.WriteMessage("\nRibbonButton Executed: " + button.Text + "\n");
            }
        }
    }
}