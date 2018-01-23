using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;

namespace SimpleCadDms.AutoCad.Addin
{
    public class AddinRibbonExtensionApplication : IExtensionApplication
    {
        public void Initialize()
        {
            AcadApp.Idle += new EventHandler(Application_OnIdle);
        }

        public void Terminate()
        {
            
        }

        void Application_OnIdle(object sender, EventArgs e)
        {
            AcadApp.Idle -= new EventHandler(Application_OnIdle);

            CreateRibbon();
        }

        private void CreateRibbon()
        {
            var addinTab = new RibbonTab
            {
                Title = "SimpleCadDms Ribbon",
                Id = "SimpleCadDms"
            };

            ComponentManager.Ribbon.Tabs.Add(addinTab);

            var documentPanelSource = new RibbonPanelSource { Title = "Document" };
            var documentPanel = new RibbonPanel { Source = documentPanelSource };
            addinTab.Panels.Add(documentPanel);

            var newIdButton = new RibbonButton
            {
                Text = "Get new ID",
                ShowText = true,
                ShowImage = true,
                Image = Images.getBitmap(Properties.Resources.icons8_starbucks_16),
                LargeImage = Images.getBitmap(Properties.Resources.icons8_starbucks_32),
                Orientation = Orientation.Vertical,
                Size = RibbonItemSize.Large,
                CommandHandler = new DummyButtonCommand()
            };

            var saveNewIdButton = new RibbonButton
            {
                Text = "Save with new ID",
                ShowText = true,
                ShowImage = true,
                Image = Images.getBitmap(Properties.Resources.icons8_starbucks_16),
                LargeImage = Images.getBitmap(Properties.Resources.icons8_starbucks_32),
                CommandHandler = new DummyButtonCommand()
            };

            var saveNewIdUploadButton = new RibbonButton
            {
                Text = "Save with new ID and upload",
                ShowText = true,
                ShowImage = true,
                Image = Images.getBitmap(Properties.Resources.icons8_starbucks_16),
                LargeImage = Images.getBitmap(Properties.Resources.icons8_starbucks_32),
                CommandHandler = new DummyButtonCommand()
            };

            var deleteDocButton = new RibbonButton
            {
                Text = "Delete document",
                ShowText = true,
                ShowImage = true,
                Image = Images.getBitmap(Properties.Resources.icons8_starbucks_16),
                LargeImage = Images.getBitmap(Properties.Resources.icons8_starbucks_32),
                CommandHandler = new DummyButtonCommand()
            };

            var documentRibbonRow = new RibbonRowPanel();
            documentRibbonRow.Items.Add(saveNewIdButton);
            documentRibbonRow.Items.Add(new RibbonRowBreak());
            documentRibbonRow.Items.Add(saveNewIdUploadButton);
            documentRibbonRow.Items.Add(new RibbonRowBreak());
            documentRibbonRow.Items.Add(deleteDocButton);

            documentPanelSource.Items.Add(newIdButton);
            documentPanelSource.Items.Add(new RibbonSeparator());
            documentPanelSource.Items.Add(documentRibbonRow);

            var transferPanelSource = new RibbonPanelSource { Title = "Transfer" };
            var transferPanel = new RibbonPanel { Source = transferPanelSource };
            addinTab.Panels.Add(transferPanel);

            var uploadButton = new RibbonButton
            {
                Text = "Upload",
                ShowText = true,
                ShowImage = true,
                Image = Images.getBitmap(Properties.Resources.icons8_starbucks_16),
                LargeImage = Images.getBitmap(Properties.Resources.icons8_starbucks_32),
                Size = RibbonItemSize.Large,
                Orientation = Orientation.Vertical,
                CommandHandler = new DummyButtonCommand()
            };

            var downloadButton = new RibbonButton
            {
                Text = "Download",
                ShowText = true,
                ShowImage = true,
                Image = Images.getBitmap(Properties.Resources.icons8_starbucks_16),
                LargeImage = Images.getBitmap(Properties.Resources.icons8_starbucks_32),
                Size = RibbonItemSize.Large,
                Orientation = Orientation.Vertical,
                CommandHandler = new DummyButtonCommand()
            };

            transferPanelSource.Items.Add(uploadButton);
            transferPanelSource.Items.Add(downloadButton);
        }
    }

    public class Images
    {
        public static BitmapImage getBitmap(Bitmap image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = stream;
            bmp.EndInit();

            return bmp;
        }
    }
}
