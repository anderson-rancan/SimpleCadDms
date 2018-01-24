using SimpleCadDms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.Business
{
    public class OperationResult
    {
        public bool Success { get; }

        public DmsDocument Document { get; }

        public string Message { get; set; }

        public OperationResult()
            : this(false, null, string.Empty)
        {

        }

        public OperationResult(bool result, string message)
            : this(result, null, message)
        {

        }


        public OperationResult(bool success, DmsDocument document, string message)
        {
            Success = success;
            Document = document;
            Message = message;
        }
    }
}
