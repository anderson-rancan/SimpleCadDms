using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SimpleCadDms.AutoCad.Addin
{
    [ComVisible(true)]
    public class AddinRegistration
    {
        private const string AutoCadRegistryPath = "Software\\Autodesk\\AutoCAD\\R20.1\\ACAD-F001:409\\Applications\\";

        private const string ApplicationName = "SimpleCadDms";

        private const string DescriptionTag = "DESCRIPTION";
        private const string DescriptionValue = "SimpleCadDms AutoCad addin";

        private const string LoadControlsTag = "LOADCTRLS";
        private const string LoadControlsValue = "2";

        private const string ManagedTag = "MANAGED";
        private const string ManagedValue = "1";

        private const string Loader = "LOADER";

        [ComRegisterFunction]
        public static void ComRegisterFunction(Type type)
        {
            try
            {
                using (var hkcu = Microsoft.Win32.Registry.CurrentUser)
                using (var appKey = hkcu.CreateSubKey(AutoCadRegistryPath + ApplicationName))
                {
                    appKey.SetValue(DescriptionTag, DescriptionValue, Microsoft.Win32.RegistryValueKind.String);
                    appKey.SetValue(LoadControlsTag, LoadControlsValue, Microsoft.Win32.RegistryValueKind.DWord);
                    appKey.SetValue(ManagedTag, ManagedValue, Microsoft.Win32.RegistryValueKind.DWord);
                    appKey.SetValue(Loader, Assembly.GetExecutingAssembly().Location);
                }
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
                using (var hkcu = Microsoft.Win32.Registry.CurrentUser)
                {
                    hkcu.DeleteSubKey(AutoCadRegistryPath + ApplicationName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem unregistering this dll: " + e.Message);
                System.Windows.Forms.MessageBox.Show("There was a problem unregistering this dll: \n\"" + e.Message + "\"");
            }
        }
    }
}
