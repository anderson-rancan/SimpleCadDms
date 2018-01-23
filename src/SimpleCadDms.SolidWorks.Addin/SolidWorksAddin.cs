using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using SolidWorksTools.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.SolidWorks.Addin
{
    [ComVisible(true)]
    [Guid(AddinGuid)]
    [SwAddin(Description = AddinDescription, LoadAtStartup = AddinLoadAtStartup, Title = AddinTitle)]
    public class SolidWorksAddin : ISwAddin
    {
        public const string AddinGuid = "662A148C-5F10-4D6A-9670-5D378C1C1A97";
        public const string AddinDescription = "SimpleCadDms SolidWorks Addin";
        public const string AddinTitle = "SimpleCadDms";
        public const bool AddinLoadAtStartup = true;

        private readonly int _userGroupId = 1;

        private CommandManager _commandManager;
        private BitmapHandler _bitmapHandler;

        public ISldWorks SldWorks { get; private set; }

        public int ID { get; private set; }

        public bool ConnectToSW(object thisSW, int cookie)
        {
            SldWorks = (ISldWorks)thisSW;
            ID = cookie;

            SldWorks.SetAddinCallbackInfo(0, this, ID);

            SetupCommandManager();

            return true;
        }

        public bool DisconnectFromSW()
        {
            DisposeCommandManager();

            return true;
        }

        private void SetupCommandManager()
        {
            _commandManager = SldWorks.GetCommandManager(ID);
            _bitmapHandler = new BitmapHandler();

            var createCommandGroup2Errors = 0;
            var cmdGroup = _commandManager.CreateCommandGroup2(_userGroupId, AddinTitle, AddinDescription, string.Empty, -1, false, ref createCommandGroup2Errors);

            var _generateNewIdCmdId = 0;
            var _saveWithNewIdCmdId = 1;
            var _saveWithNewIdAndUploadCmdId = 2;
            var _saveAndUploadToServerCmdId = 3;
            var _downloadFromServerCmdId = 4;

            var menuToolbarOption = (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem);
            cmdGroup.AddCommandItem2("NewId", -1, "Generates a new document ID", "Generates ID", 0, "GenerateNewId", "CommandAlwaysEnabled", _generateNewIdCmdId, menuToolbarOption);
            cmdGroup.AddCommandItem2("SaveWithNewId", -1, "Saves the current document with a new ID", "Saves with a new ID", 0, "SaveWithNewId", "CommandAlwaysEnabled", _saveWithNewIdCmdId, menuToolbarOption);
            cmdGroup.AddCommandItem2("SaveWithNewIdAndUpload", -1, "Saves the current document with a new ID and uploads it", "Saves with a new ID and upload", 0, "SaveWithNewIdAndUpload", "CommandAlwaysEnabled", _saveWithNewIdAndUploadCmdId, menuToolbarOption);
            cmdGroup.AddSpacer2(-1, menuToolbarOption);
            cmdGroup.AddCommandItem2("SaveAndUpload", -1, "Saves the current document and uploads it", "Saves and uploads the document", 0, "SaveAndUpload", "CommandAlwaysEnabled", _saveAndUploadToServerCmdId, menuToolbarOption);
            cmdGroup.AddCommandItem2("DownloadFromServer", -1, "Downloads a document from the server", "Downloads a document", 0, "DownloadFromServer", "CommandAlwaysEnabled", _downloadFromServerCmdId, menuToolbarOption);

            cmdGroup.HasToolbar = true;
            cmdGroup.HasMenu = true;
            cmdGroup.Activate();
        }

        private void DisposeCommandManager()
        {
            _commandManager.RemoveCommandGroup(_userGroupId);

            Marshal.ReleaseComObject(_commandManager);
            _commandManager = null;
        }

        public void HelloWorld()
        {
            System.Windows.Forms.MessageBox.Show("Hello world!");
        }

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            try
            {
                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + AddinGuid + "}";
                Microsoft.Win32.RegistryKey addinkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyname);
                addinkey.SetValue(null, 0);

                addinkey.SetValue("Description", AddinDescription);
                addinkey.SetValue("Title", AddinTitle);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + AddinGuid + "}";
                addinkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(keyname);
                addinkey.SetValue(null, Convert.ToInt32(AddinLoadAtStartup), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("There was a problem registering the function: \n\"" + ex + "\"");
            }
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            try
            {
                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + AddinGuid + "}";
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(keyname);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + AddinGuid + "}";
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(keyname);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("There was a problem unregistering this dll: \n\"" + ex + "\"");
            }
        }
    }
}
