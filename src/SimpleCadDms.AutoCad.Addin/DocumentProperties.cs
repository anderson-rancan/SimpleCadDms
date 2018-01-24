using Autodesk.AutoCAD.DatabaseServices;
using System;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace SimpleCadDms.AutoCad.Addin
{
    internal class DocumentProperties
    {
        public const string DocumentIdKey = "scd_DocumentId";

        public string DocumentId { get; set; }

        public static DocumentProperties FromActiveDocument()
        {
            var result = new DocumentProperties();
            var activeDocument = AcadApp.DocumentManager.MdiActiveDocument;

            if (activeDocument != null)
            {
                var customProperties = activeDocument.Database.SummaryInfo.CustomProperties;

                while (customProperties.MoveNext())
                {
                    if (string.Equals(customProperties.Key.ToString(), DocumentIdKey, StringComparison.InvariantCultureIgnoreCase))
                        result.DocumentId = customProperties.Value.ToString();
                }
            }

            return result;
        }

        public void SetOnActiveDocument()
        {
            var activeDocument = AcadApp.DocumentManager.MdiActiveDocument;
            var database = activeDocument.Database;

            var summaryBuilder = new DatabaseSummaryInfoBuilder(database.SummaryInfo);

            summaryBuilder.CustomPropertyTable[DocumentIdKey] = DocumentId;

            using (activeDocument.LockDocument())
            {
                database.SummaryInfo = summaryBuilder.ToDatabaseSummaryInfo();
            }
        }
    }
}
