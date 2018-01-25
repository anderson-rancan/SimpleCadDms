using System;
using System.IO;
using System.Windows;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class DownloadHandler : CommandHandlerBase
    {
        public void Download()
        {
            var userInput = new UserInterface.UserInputWindow("Document ID");
            if (!userInput.ShowDialog().Value) return;

            Download(userInput.Answer);
        }

        public void Download(string documentId)
        {
            var result = CadDms.DownloadDocument(documentId);

            if (result.Success)
            {
                File.WriteAllBytes(
                    string.Format("{0}.{1}", result.Document.DocumentId, result.Document.FileExtension),
                    result.Document.FileContent);
            }
            else
            {
                MessageBox.Show(
                    string.Format("It was not possible to download the file!{0}{0}{1}", Environment.NewLine, result.Message),
                    "Download document",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

    }
}
