﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextCap
{
    [Transaction(TransactionMode.Manual)]
    public class UpperCaseCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Access the active Revit document
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            try
            {
                var selectedElements = uiDoc.Selection.GetElementIds();

                if (selectedElements.Count > 0)
                {
                    using (Transaction tx = new Transaction(doc, "Change TextNote to Uppercase"))
                    {
                        tx.Start();
                        foreach (var selectedElementId in selectedElements)
                        {
                            Element selectedElement = doc.GetElement(selectedElementId);

                            if (selectedElement is TextNote textNote)
                            {
                                // Get text from the TextNote and convert to uppercase
                                string originalText = textNote.Text;
                                string upperCaseText = originalText.ToUpper();

                                // Set the text of the TextNote to uppercase

                                textNote.Text = upperCaseText;


                                // Do something with the text (e.g., print it)
                                Debug.WriteLine("Text from the selected TextNote: " + upperCaseText);
                            }
                            else
                            {
                                continue;
                            }
                        }

                        tx.Commit();
                    }
                }
                else
                {
                    Element pickedElement = null;

                    try
                    {
                        // Prompt user to select an element
                        var pickedElementRef = uiDoc.Selection.PickObject(ObjectType.Element);
                        pickedElement = doc.GetElement(pickedElementRef);

                        // Check if the picked element is a TextNote
                        if (pickedElement is TextNote textNote)
                        {
                            // Get text from the TextNote
                            string text = textNote.Text;

                            // Get text from the TextNote and convert to uppercase
                            string originalText = textNote.Text;
                            string upperCaseText = originalText.ToUpper();

                            // Set the text of the TextNote to uppercase
                            using (Transaction tx = new Transaction(doc, "Change TextNote to Uppercase"))
                            {
                                tx.Start();
                                textNote.Text = upperCaseText;
                                tx.Commit();
                            }



                            // Do something with the text (e.g., print it)
                            Debug.WriteLine("Text from the selected TextNote: " + text);
                        }
                        else
                        {
                            Debug.WriteLine("The selected element is not a TextNote.");
                            return Result.Failed;
                        }
                    }
                    catch (Exception ex)
                    {

                        Debug.WriteLine(ex.Message);
                        return Result.Failed;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}
