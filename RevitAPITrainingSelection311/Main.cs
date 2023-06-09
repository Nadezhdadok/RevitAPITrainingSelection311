﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingSelection311
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            IList<Reference> selectedRef = uidoc.Selection.PickObjects(ObjectType.Face, "Выберите элементы");
            var volumeList = new List<double>();
            foreach (var selectedElement in selectedRef)
            {
                var element = doc.GetElement(selectedElement);
                
                            
                if (element is Wall)
                {
                    Parameter volumeParameter = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    if (volumeParameter.StorageType == StorageType.Double)
                    {
                        double volume = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                        volumeList.Add(volume);
                    }
                    
                }
            }
            TaskDialog.Show("Volume", $"Объем выбранных стен: {volumeList.Sum()} m3");
            return Result.Succeeded;
        }
    }
}
