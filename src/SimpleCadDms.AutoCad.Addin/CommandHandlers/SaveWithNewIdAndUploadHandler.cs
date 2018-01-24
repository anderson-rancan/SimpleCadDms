using SimpleCadDms.Models;
using System;
using System.IO;
using System.Windows;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class SaveWithNewIdAndUploadHandler : SaveCommandHandlerBase
    {
        public void SaveAndUpload()
        {
            var documentId = CadDms.CreateNewDocumentId();

            Save(documentId);

            var fullpath = AcadApp.DocumentManager.MdiActiveDocument.Name;

            var dmsDocument = new DmsDocument
            {
                DocumentId = documentId,
                FileContent = File.ReadAllBytes(fullpath),
                FileExtension = "DWG"
            };

            var result = CadDms.UploadDocument(dmsDocument);

            if (!result.Success)
            {
                MessageBox.Show(
                    string.Format("It was not possible to upload the file!{0}{0}{1}", Environment.NewLine, result.Message),
                    "Save with new ID and upload",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}