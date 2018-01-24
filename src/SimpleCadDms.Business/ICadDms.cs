using SimpleCadDms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.Business
{
    public interface ICadDms
    {
        string CreateNewDocumentId();

        OperationResult DownloadDocument(string documentId);

        OperationResult UploadDocument(DmsDocument dmsDocument);

        OperationResult DeleteDocument(string documentId);
    }
}
