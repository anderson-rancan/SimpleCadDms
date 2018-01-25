using System;
using System.Linq;

using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.Runtime;
using SimpleCadDms.Business;
using SimpleCadDms.AutoCad.Addin.CommandHandlers;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace SimpleCadDms.AutoCad.Addin
{
    public class AddinCommands
    {
        private const string _commandGenerateNewId = "ScdNewId";
        private const string _lispGenerateNewId = "scd-newid";
        private const string _commandSaveWithNewId = "ScdSaveWithNewId";
        private const string _commandSaveWithNewIdAndUpload = "ScdSaveWithNewIdAndUpload";
        private const string _commandDeleteDocument = "ScdDeleteDocument";
        private const string _lispDeleteDocument = "scd-deletedocument";
        private const string _commandSaveAndUploadToServer = "ScdSaveAndUpload";
        private const string _commandDownloadFromServer = "ScdDownload";

        private ICadDms _cadDms;

        private ICadDms CadDms
        {
            get
            {
                if (_cadDms == null) _cadDms = CadDmsFactory.CreateNew();
                return _cadDms;
            }
        }

        [CommandMethod(_commandGenerateNewId)]
        public void GenerateNewIdCommand()
        {
            AcadApp
                .DocumentManager
                .MdiActiveDocument
                .Editor
                .WriteMessage(string.Format("{0}The {1} document ID was successfully generated!{0}", Environment.NewLine, GenerateNewId()));
        }

        [LispFunction(_lispGenerateNewId)]
        public ResultBuffer GenerateNewIdLisp(ResultBuffer incomingBuffer)
        {
            return new ResultBuffer(new TypedValue((int)LispDataType.Text, GenerateNewId()));
        }

        [CommandMethod(_commandSaveWithNewId)]
        public void SaveWithNewId()
        {
            CommandHandlerFactory
                .Instance
                .GetSaveWithNewIdHandler()
                .SaveWithNewId();
        }

        [CommandMethod(_commandSaveWithNewIdAndUpload)]
        public void SaveWithNewIdAndUpload()
        {
            CommandHandlerFactory
                .Instance
                .GetSaveWithNewIdAndUpload()
                .SaveAndUpload();
        }

        [CommandMethod(_commandDeleteDocument)]
        public void DeleteDocumentCommand()
        {
            CommandHandlerFactory
                .Instance
                .GetDeleteDocumentHandler()
                .Delete();
        }

        [LispFunction(_lispDeleteDocument)]
        public ResultBuffer DeleteDocumentLisp(ResultBuffer incomingBuffer)
        {
            var result = false;

            var documentId = incomingBuffer
                ?.AsArray()
                .FirstOrDefault(_ => _.TypeCode == (short)LispDataType.Text)
                .Value
                ?.ToString();

            if (!string.IsNullOrWhiteSpace(documentId))
            {
                result = CommandHandlerFactory
                    .Instance
                    .GetDeleteDocumentHandler()
                    .Delete(documentId);
            }
            else
            {
                result = CommandHandlerFactory
                    .Instance
                    .GetDeleteDocumentHandler()
                    .Delete();
            }

            return new ResultBuffer(new TypedValue(result ? (int)LispDataType.T_atom : (int)LispDataType.Nil));
        }

        [CommandMethod(_commandSaveAndUploadToServer)]
        public void SaveAndUploadToServer()
        {
            CommandHandlerFactory
                .Instance
                .GetUploadHandler()
                .Upload();
        }

        [CommandMethod(_commandDownloadFromServer)]
        public void DownloadFromServer()
        {
            var document = AcadApp.DocumentManager.MdiActiveDocument;

            var options = new PromptStringOptions(string.Format("{0}Enter document ID: ", Environment.NewLine));
            var result = document.Editor.GetString(options);

            if (!string.IsNullOrWhiteSpace(result.StringResult))
            {
                CommandHandlerFactory
                    .Instance
                    .GetDownloadHandler()
                    .Download(result.StringResult);
            }
        }

        private string GenerateNewId()
        {
            return CommandHandlerFactory
                .Instance
                .GetNewDocumentIdHandler()
                .CreateNewId();
        }
    }
}
