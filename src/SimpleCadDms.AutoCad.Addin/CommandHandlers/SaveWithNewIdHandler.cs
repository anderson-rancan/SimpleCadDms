namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class SaveWithNewIdHandler : SaveCommandHandlerBase
    {
        public void SaveWithNewId()
        {
            Save(CadDms.CreateNewDocumentId());
        }
    }
}