using System;
using System.Windows;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class DeleteDocumentHandler : CommandHandlerBase
    {
        public bool Delete()
        {
            return Delete(DocumentProperties.FromActiveDocument().DocumentId);
        }

        public bool Delete(string documentId)
        {
            var result = CadDms
                .DeleteDocument(documentId);

            if (!result.Success)
            {
                MessageBox.Show(
                    string.Format("It was not possible to delete the document!{0}{0}{1}", Environment.NewLine, result.Message),
                    "Delete document",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return result.Success;
        }
    }
}