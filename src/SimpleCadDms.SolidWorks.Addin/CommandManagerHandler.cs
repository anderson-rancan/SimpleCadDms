using SimpleCadDms.Business;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksTools.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleCadDms.SolidWorks.Addin
{
    public sealed class CommandManagerHandler : IDisposable
    {
        private int _addinId;
        private ISldWorks _sldWorks;
        private CommandManager _commandManager;
        private BitmapHandler _bitmapHandler;
        private bool _disposed;
        private ICadDms _cadDms;

        private readonly int _userGroupId = 1;
        private readonly int _generateNewIdCmdId = 0;
        private readonly int _saveWithNewIdCmdId = 1;
        private readonly int _saveWithNewIdAndUploadCmdId = 2;
        private readonly int _deleteDocument = 3;
        private readonly int _saveAndUploadToServerCmdId = 4;
        private readonly int _downloadFromServerCmdId = 5;

        private readonly int _lastItemIndex = -1;

        private CommandManager CommandManager
        {
            get
            {
                if (_commandManager == null) _commandManager = _sldWorks.GetCommandManager(_addinId);
                return _commandManager;
            }
        }

        private BitmapHandler BitmapHandler
        {
            get
            {
                if (_bitmapHandler == null) _bitmapHandler = new BitmapHandler();
                return _bitmapHandler;
            }
        }

        private ICadDms CadDms
        {
            get
            {
                if (_cadDms == null) _cadDms = CadDmsFactory.CreateNew();
                return _cadDms;
            }
        }

        public CommandManagerHandler(ISldWorks sldWorks, int addinId)
        {
            _sldWorks = sldWorks;
            _addinId = addinId;
        }

        public void Setup()
        {
            _sldWorks.SetAddinCallbackInfo(0, this, _addinId);

            var knownIds = new[] 
            {
                _generateNewIdCmdId,
                _saveWithNewIdCmdId,
                _saveWithNewIdAndUploadCmdId,
                _saveWithNewIdAndUploadCmdId,
                _deleteDocument,
                _downloadFromServerCmdId
            };

            var ignorePrevious = ShouldIgnorePrevious(knownIds, out bool groupExistsInRegistry);

            var commandGroup = CreateCommandGroup(ignorePrevious, out Dictionary<int, int> commandsDictionary);

            CreateCommandTab(commandGroup, groupExistsInRegistry, ignorePrevious, commandsDictionary);
        }

        private bool ShouldIgnorePrevious(int[] knownIds, out bool groupExistsInRegistry)
        {
            groupExistsInRegistry = CommandManager.GetGroupDataFromRegistry(_userGroupId, out object registryIDs);

            return
                groupExistsInRegistry &&
                !knownIds.SequenceEqual((int[])registryIDs);
        }

        private CommandGroup CreateCommandGroup(bool ignorePrevious, out Dictionary<int, int> commandsDictionary)
        {
            var createCommandGroup2Errors = 0;
            var commandGroup = CommandManager.CreateCommandGroup2(_userGroupId, SolidWorksAddin.AddinTitle, SolidWorksAddin.AddinDescription, string.Empty, _lastItemIndex, ignorePrevious, ref createCommandGroup2Errors);

            var icons = new string[3];
            icons[0] = BitmapHandler.CreateFileFromResourceBitmap("SimpleCadDms.SolidWorks.Addin.Images.IconList_20.png", Assembly.GetExecutingAssembly());
            icons[1] = BitmapHandler.CreateFileFromResourceBitmap("SimpleCadDms.SolidWorks.Addin.Images.IconList_32.png", Assembly.GetExecutingAssembly());
            icons[2] = BitmapHandler.CreateFileFromResourceBitmap("SimpleCadDms.SolidWorks.Addin.Images.IconList_40.png", Assembly.GetExecutingAssembly());

            var mainIcons = new string[3];
            mainIcons[0] = BitmapHandler.CreateFileFromResourceBitmap("SimpleCadDms.SolidWorks.Addin.Images.MainIconList_20.png", Assembly.GetExecutingAssembly());
            mainIcons[1] = BitmapHandler.CreateFileFromResourceBitmap("SimpleCadDms.SolidWorks.Addin.Images.MainIconList_32.png", Assembly.GetExecutingAssembly());
            mainIcons[2] = BitmapHandler.CreateFileFromResourceBitmap("SimpleCadDms.SolidWorks.Addin.Images.MainIconList_40.png", Assembly.GetExecutingAssembly());

            commandGroup.IconList = icons;
            commandGroup.MainIconList = mainIcons;

            commandsDictionary = new Dictionary<int, int>();
            var menuToolbarOption = (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem);

            commandsDictionary[_generateNewIdCmdId] = AddGenerateNewIdCommand(commandGroup);
            commandsDictionary[_saveWithNewIdCmdId] = AddSaveWithNewIdCommand(commandGroup);
            commandsDictionary[_saveWithNewIdAndUploadCmdId] = AddSaveWithNewIdAndUploadCommand(commandGroup);
            commandsDictionary[_deleteDocument] = AddDeleteDocumentCommand(commandGroup);
            commandGroup.AddSpacer2(_lastItemIndex, menuToolbarOption);
            commandsDictionary[_saveAndUploadToServerCmdId] = AddSaveAndUploadCommand(commandGroup);
            commandsDictionary[_downloadFromServerCmdId] = AddDownloadFromServerCommand(commandGroup);

            commandGroup.HasToolbar = true;
            commandGroup.HasMenu = true;
            commandGroup.Activate();

            return commandGroup;
        }

        private void CreateCommandTab(CommandGroup commandGroup, bool groupExistsInRegistry, bool ignorePrevious, Dictionary<int, int> commandsDictionary)
        {
            var docTypes = new int[]{ (int)swDocumentTypes_e.swDocPART,
                                      (int)swDocumentTypes_e.swDocASSEMBLY,
                                      (int)swDocumentTypes_e.swDocDRAWING };

            foreach (var docType in docTypes)
            {
                var commandTab = CommandManager.GetCommandTab(docType, SolidWorksAddin.AddinTitle);

                if (commandTab != null && !groupExistsInRegistry || ignorePrevious)
                {
                    CommandManager.RemoveCommandTab(commandTab);
                    commandTab = null;
                }

                if (commandTab == null)
                {
                    commandTab = CommandManager.AddCommandTab(docType, SolidWorksAddin.AddinTitle);

                    var commandTabBoxDoc = commandTab.AddCommandTabBox();
                    var commandDocIds = new int[4];
                    var commandDocTextTypes = new int[4];

                    commandDocIds[0] = commandGroup.get_CommandID(commandsDictionary[_generateNewIdCmdId]);
                    commandDocTextTypes[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                    commandDocIds[1] = commandGroup.get_CommandID(commandsDictionary[_saveWithNewIdCmdId]);
                    commandDocTextTypes[1] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                    commandDocIds[2] = commandGroup.get_CommandID(commandsDictionary[_saveWithNewIdAndUploadCmdId]);
                    commandDocTextTypes[2] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                    commandDocIds[3] = commandGroup.get_CommandID(commandsDictionary[_deleteDocument]);
                    commandDocTextTypes[3] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                    commandTabBoxDoc.AddCommands(commandDocIds, commandDocTextTypes);

                    var commandTabBoxTransfer = commandTab.AddCommandTabBox();
                    var commandTransferIds = new int[2];
                    var commandTransferTextTypes = new int[2];

                    commandTransferIds[0] = commandGroup.get_CommandID(commandsDictionary[_saveAndUploadToServerCmdId]);
                    commandTransferTextTypes[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                    commandTransferIds[1] = commandGroup.get_CommandID(commandsDictionary[_downloadFromServerCmdId]);
                    commandTransferTextTypes[1] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                    commandTabBoxTransfer.AddCommands(commandTransferIds, commandTransferTextTypes);
                }
            }
        }

        private int AddGenerateNewIdCommand(CommandGroup commandGroup)
        {
            return commandGroup.AddCommandItem2(
                "NewId",
                _lastItemIndex,
                "Generates a new document ID",
                "Generates ID",
                3,
                nameof(GenerateNewId),
                nameof(CommandAlwaysEnabled),
                _generateNewIdCmdId,
                (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem));
        }

        private int AddSaveWithNewIdCommand(CommandGroup commandGroup)
        {
            return commandGroup.AddCommandItem2(
                "SaveWithNewId",
                _lastItemIndex,
               "Saves the current document with a new ID",
               "Saves with a new ID",
               5,
               nameof(SaveWithNewId),
               nameof(CommandAlwaysEnabled),
               _saveWithNewIdCmdId,
               (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem));
        }

        private int AddSaveWithNewIdAndUploadCommand(CommandGroup commandGroup)
        {
            return commandGroup.AddCommandItem2(
                "SaveWithNewIdAndUpload",
                _lastItemIndex,
                "Saves the current document with a new ID and uploads it",
                "Saves with a new ID and upload",
                5,
                nameof(SaveWithNewIdAndUpload),
                nameof(CommandAlwaysEnabled),
                _saveWithNewIdAndUploadCmdId,
                (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem));
        }

        private int AddDeleteDocumentCommand(CommandGroup commandGroup)
        {
            return commandGroup.AddCommandItem2(
                "DeleteDocument",
                _lastItemIndex,
               "Deletes a document",
               "Deletes a document",
               2,
               nameof(DeleteDocument),
               nameof(CommandAlwaysEnabled),
               _deleteDocument,
               (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem));
        }

        private int AddSaveAndUploadCommand(CommandGroup commandGroup)
        {
            return commandGroup.AddCommandItem2(
                "SaveAndUpload",
                _lastItemIndex,
                "Saves the current document and uploads it",
                "Saves and uploads the document",
                6,
                nameof(SaveAndUpload),
                nameof(CommandAlwaysEnabled),
                _saveAndUploadToServerCmdId,
                (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem));
        }

        private int AddDownloadFromServerCommand(CommandGroup commandGroup)
        {
            return commandGroup.AddCommandItem2(
                "DownloadFromServer",
                _lastItemIndex,
               "Downloads a document from the server",
               "Downloads a document",
               1,
               nameof(DownloadFromServer),
               nameof(CommandAlwaysEnabled),
               _downloadFromServerCmdId,
               (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem));
        }

        public int CommandAlwaysEnabled()
        {
            return (int)CommandState.Enabled;
        }

        public void GenerateNewId()
        {
            var documentId = CadDms.CreateNewDocumentId();

            Clipboard.SetText(documentId);

            MessageBox.Show(
                string.Format("The {0} document ID was generated and copied to the clipboard.", documentId),
                "New Document ID",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void SaveWithNewId()
        {
            if (_sldWorks.ActiveDoc is IModelDoc2 activeDoc)
            {
                var dependencies = activeDoc.GetDependencies2(true, true, false) as string[];

                for (int counter = 1; counter < dependencies.Length; counter += 2)
                {
                    var filename = dependencies[counter];

                    var openDoc6Errors = 0;
                    var openDoc6Warnings = 0;
                    var document = _sldWorks.OpenDoc6(
                        filename,
                        (int)GetDocumentType(filename),
                        (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
                        string.Empty,
                        ref openDoc6Errors,
                        ref openDoc6Warnings);

                    var depSaveAsErrors = 0;
                    var depSaveAsWarnings = 0;
                    document.Extension.SaveAs(
                        Path.Combine(Path.GetDirectoryName(filename), CadDms.CreateNewDocumentId() + Path.GetExtension(filename)),
                        (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                        (int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                        null,
                        ref depSaveAsErrors,
                        ref depSaveAsWarnings);
                }

                var saveAsErrors = 0;
                var saveAsWarnings = 0;
                activeDoc.Extension.SaveAs(
                    Path.Combine(Path.GetDirectoryName(activeDoc.GetPathName()), CadDms.CreateNewDocumentId() + Path.GetExtension(activeDoc.GetPathName())),
                    (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                    (int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                    null,
                    ref saveAsErrors,
                    ref saveAsWarnings);
            }
        }

        public void SaveWithNewIdAndUpload()
        {
            MessageBox.Show("Save with new ID and upload");
        }

        public void DeleteDocument()
        {
            MessageBox.Show("Deletes a document");
        }

        public void SaveAndUpload()
        {
            MessageBox.Show("Save and upload");
        }

        public void DownloadFromServer()
        {
            MessageBox.Show("Download from server");
        }

        private swDocumentTypes_e GetDocumentType(string filename)
        {
            if (filename.EndsWith(".SLDPRT")) return swDocumentTypes_e.swDocPART;
            if (filename.EndsWith(".SLDASM")) return swDocumentTypes_e.swDocASSEMBLY;
            if (filename.EndsWith(".SLDDRW")) return swDocumentTypes_e.swDocDRAWING;

            return swDocumentTypes_e.swDocNONE;
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (_bitmapHandler != null)
            {
                _bitmapHandler.Dispose();
                _bitmapHandler = null;
            }

            if (_commandManager != null)
            {
                _commandManager.RemoveCommandGroup(_userGroupId);
                Marshal.ReleaseComObject(_commandManager);
                _commandManager = null;
            }

            _disposed = true;
        }
    }
}
