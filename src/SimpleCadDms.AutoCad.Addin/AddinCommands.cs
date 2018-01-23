using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.Runtime;

namespace SimpleCadDms.AutoCad.Addin
{
    public class AddinCommands
    {
        private const string _commandGenerateNewId = "scd_NewId";
        private const string _commandSaveWithNewId = "scd_SaveWithNewId";
        private const string _commandSaveWithNewIdAndUpload = "scd_SaveWithNewIdAndUpload";
        private const string _commandSaveAndUploadToServer = "scd_SaveAndUpload";
        private const string _commandDownloadFromServer = "scd_Download";

        [CommandMethod(_commandGenerateNewId)]
        public void GenerateNewId()
        {
            WriteMessage("This method will generate a new number");
        }

        [CommandMethod(_commandSaveWithNewId)]
        public void SaveWithNewId()
        {
            WriteMessage("This method will generate a new number and save the file");
        }

        [CommandMethod(_commandSaveWithNewIdAndUpload)]
        public void SaveWithNewIdAndUpload()
        {
            WriteMessage("This method will generate a new number, save the file and upload it to the server");
        }

        [CommandMethod(_commandSaveAndUploadToServer)]
        public void SaveAndUploadToServer()
        {
            WriteMessage("This method will save the file and upload it to the server");
        }

        [CommandMethod(_commandDownloadFromServer)]
        public void DownloadFromServer()
        {
            WriteMessage("This method will download a file from the server");
        }

        private void WriteMessage(string message)
        {
            AcadApp
                .DocumentManager
                .MdiActiveDocument
                .Editor
                .WriteMessage(message + Environment.NewLine);
        }
    }
}
