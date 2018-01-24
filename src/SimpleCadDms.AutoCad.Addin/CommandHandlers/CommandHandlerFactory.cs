namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal class CommandHandlerFactory
    {
        private NewDocumentIdHandler _newDocumentIdHandler;
        private SaveWithNewIdHandler _saveWithNewIdHandler;
        private SaveWithNewIdAndUploadHandler _saveWithNewIdAndUploadHandler;
        private DeleteDocumentHandler _deleteDocumentHandler;
        private UploadHandler _uploadHandler;
        private DownloadHandler _downloadHandler;

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

        public SaveWithNewIdHandler GetSaveWithNewIdHandler()
        {
            if (_saveWithNewIdHandler == null)
                _saveWithNewIdHandler = new SaveWithNewIdHandler();

            return _saveWithNewIdHandler;
        }

        public SaveWithNewIdAndUploadHandler GetSaveWithNewIdAndUpload()
        {
            if (_saveWithNewIdAndUploadHandler == null)
                _saveWithNewIdAndUploadHandler = new SaveWithNewIdAndUploadHandler();

            return _saveWithNewIdAndUploadHandler;
        }

        public DeleteDocumentHandler GetDeleteDocumentHandler()
        {
            if (_deleteDocumentHandler == null)
                _deleteDocumentHandler = new DeleteDocumentHandler();

            return _deleteDocumentHandler;
        }

        public UploadHandler GetUploadHandler()
        {
            if (_uploadHandler == null)
                _uploadHandler = new UploadHandler();

            return _uploadHandler;
        }

        public DownloadHandler GetDownloadHandler()
        {
            if (_downloadHandler == null)
                _downloadHandler = new DownloadHandler();

            return _downloadHandler;
        }
    }
}
