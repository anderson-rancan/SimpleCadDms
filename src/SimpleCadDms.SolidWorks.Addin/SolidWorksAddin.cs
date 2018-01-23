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

        private CommandManagerHandler _commandManagerHandler;

        public ISldWorks SldWorks { get; private set; }

        public int ID { get; private set; }

        public bool ConnectToSW(object thisSW, int cookie)
        {
            SldWorks = (ISldWorks)thisSW;
            ID = cookie;

            _commandManagerHandler = new CommandManagerHandler(SldWorks, ID);
            _commandManagerHandler.Setup();

            return true;
        }

        public bool DisconnectFromSW()
        {
            _commandManagerHandler.Dispose();

            return true;
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
