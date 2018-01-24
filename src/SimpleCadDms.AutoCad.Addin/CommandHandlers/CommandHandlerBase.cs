using SimpleCadDms.Business;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal abstract class CommandHandlerBase
    {
        private ICadDms _cadDms;

        protected ICadDms CadDms
        {
            get
            {
                if (_cadDms == null) _cadDms = CadDmsFactory.CreateNew();
                return _cadDms;
            }
        }
    }
}
