using SimpleCadDms.Models;
using System;
using System.IO;
using System.Windows;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class UploadHandler : SaveCommandHandlerBase
    {
        public void Upload()
        {
            Save();

            var fullpath = AcadApp.DocumentManager.MdiActiveDocument.Name;
            var documentProperties = DocumentProperties.FromActiveDocument();

            var dmsDocument = new DmsDocument
            {
                DocumentId = documentProperties.DocumentId,
                FileContent = File.ReadAllBytes(fullpath),
                FileExtension = "DWG"
            };

            var result = CadDms.UploadDocument(dmsDocument);

            if (!result.Success)
            {
                MessageBox.Show(
                    string.Format("It was not possible to upload the file!{0}{0}{1}", Environment.NewLine, result.Message),
                    "Save and upload",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
