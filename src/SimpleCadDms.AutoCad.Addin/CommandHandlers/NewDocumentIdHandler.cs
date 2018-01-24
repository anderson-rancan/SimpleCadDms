using SimpleCadDms.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class NewDocumentIdHandler
    {
        private ICadDms _cadDms;

        public NewDocumentIdHandler()
        {
            _cadDms = CadDmsFactory.CreateNew();
        }

        public string CreateNewId()
        {
            return _cadDms.CreateNewDocumentId();
        }
    }
}
