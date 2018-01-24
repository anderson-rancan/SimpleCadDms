using SimpleCadDms.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class NewDocumentIdHandler : CommandHandlerBase
    {
        public string CreateNewId()
        {
            return CadDms.CreateNewDocumentId();
        }
    }
}
