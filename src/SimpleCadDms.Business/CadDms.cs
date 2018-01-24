using SimpleCadDms.Models;

namespace SimpleCadDms.Business
{
    internal class CadDms : ICadDms
    {
        private readonly string _documentIdFormat = "{0:0000}";

        private int _counter = 0;

        public string CreateNewDocumentId()
        {
            _counter = _counter + 1;
            return string.Format(_documentIdFormat, _counter);
        }

        public OperationResult DownloadDocument(string documentId)
        {
            return new OperationResult(
                false,
                "DownloadDocument() was not implemented!");
        }

        public OperationResult UploadDocument(DmsDocument dmsDocument)
        {
            return new OperationResult(
                false,
                "UploadDocument() was not implemented!");
        }

        public OperationResult DeleteDocument(string documentId)
        {
            return new OperationResult(
                false,
                "DeleteDocument() was not implemented!");
        }
    }
}