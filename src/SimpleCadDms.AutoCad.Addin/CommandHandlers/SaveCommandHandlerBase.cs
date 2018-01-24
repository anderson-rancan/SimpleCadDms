using System.Text;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace SimpleCadDms.AutoCad.Addin.CommandHandlers
{
    internal abstract class SaveCommandHandlerBase : CommandHandlerBase
    {
        protected void Save()
        {
            var previousCmdEcho = AcadApp.GetSystemVariable("CMDECHO");

            var lispCommand = new StringBuilder();
            lispCommand
                .Append("(setvar \"CMDECHO\" 0)")
                .Append("(command \"_.QSAVE\"")
                .AppendFormat("(setvar \"CMDECHO\" {0})", previousCmdEcho)
                .Append("(princ) ");

            AcadApp
                .DocumentManager
                .MdiActiveDocument
                .SendStringToExecute(lispCommand.ToString(), false, false, false);
        }

        protected void Save(string documentId)
        {
            var previousCmdEcho = AcadApp.GetSystemVariable("CMDECHO");

            var lispCommand = new StringBuilder();
            lispCommand
                .Append("(setvar \"CMDECHO\" 0)")
                .AppendFormat("(command \"_.SAVEAS\" \"\" \"{0}.DWG\")", documentId)
                .AppendFormat("(setvar \"CMDECHO\" {0})", previousCmdEcho)
                .Append("(princ) ");

            AcadApp
                .DocumentManager
                .MdiActiveDocument
                .SendStringToExecute(lispCommand.ToString(), false, false, false);

            UpdateCustomProperties(documentId);
        }

        private void UpdateCustomProperties(string documentId)
        {
            var documentProperties = DocumentProperties.FromActiveDocument();

            documentProperties.DocumentId = documentId;

            documentProperties.SetOnActiveDocument();
        }
    }
}
