using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class CommandHandlerFactory
    {
        private NewDocumentIdHandler _newDocumentIdHandler;

        public static CommandHandlerFactory Instance { get; } = new CommandHandlerFactory();

        private CommandHandlerFactory()
        {

        }

        public NewDocumentIdHandler GetNewDocumentIdHandler()
        {
            if (_newDocumentIdHandler == null)
                _newDocumentIdHandler = new NewDocumentIdHandler();

            return _newDocumentIdHandler;
        }
    }
}
