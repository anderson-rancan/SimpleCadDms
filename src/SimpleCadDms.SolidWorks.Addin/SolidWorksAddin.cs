using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
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

        public ISldWorks SldWorks { get; private set; }
        public int ID { get; private set; }
        private CommandManager CommandManager { get; set; }

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
            CommandManager = SldWorks.GetCommandManager(ID);

            int[] docTypes = new int[]{(int)swDocumentTypes_e.swDocASSEMBLY,
                                       (int)swDocumentTypes_e.swDocDRAWING,
                                       (int)swDocumentTypes_e.swDocPART};

            var createCommandGroup2Errors = 0;
            var cmdGroup = CommandManager.CreateCommandGroup2(_userGroupId, AddinTitle, AddinDescription, string.Empty, -1, false, ref createCommandGroup2Errors);

            cmdGroup.AddCommandItem2("HelloWorld", -1, "Our first command", "Our first command", 0, "HelloWorld", string.Empty, 0, (int)swCommandItemType_e.swMenuItem);

            cmdGroup.HasMenu = true;
            cmdGroup.Activate();
        }

        private void DisposeCommandManager()
        {
            CommandManager.RemoveCommandGroup(_userGroupId);
        }

        public void HelloWorld()
        {
            var form = new Form1();
            form.ShowDialog();
        }

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            try
            {
                Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + type.GUID + "}";
                Microsoft.Win32.RegistryKey addinkey = hklm.CreateSubKey(keyname);
                addinkey.SetValue(null, 0);

                addinkey.SetValue("Description", AddinDescription);
                addinkey.SetValue("Title", AddinTitle);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + type.GUID + "}";
                addinkey = hkcu.CreateSubKey(keyname);
                addinkey.SetValue(null, Convert.ToInt32(AddinLoadAtStartup), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                System.Windows.Forms.MessageBox.Show("There was a problem registering the function: \n\"" + e.Message + "\"");
            }
        }

        [ComUnregisterFunction]
        public static void ComUnregisterFunction(Type type)
        {
            try
            {
                Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;

                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + type.GUID + "}";
                hklm.DeleteSubKey(keyname);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + type.GUID + "}";
                hkcu.DeleteSubKey(keyname);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem unregistering this dll: " + e.Message);
                System.Windows.Forms.MessageBox.Show("There was a problem unregistering this dll: \n\"" + e.Message + "\"");
            }
        }
    }
}
