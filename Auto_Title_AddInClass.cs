using EA;
using System;
using System.CodeDom;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Auto_Title_AddIn
{
    public class Auto_Title_AddInClass : EAAddinFramework.EAAddinBase
    {
        // define menu constants
        const string menuHeader = "-&Diagram Title";
        const string titleElement = "Auto Title Generator";



        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        ///
        public override object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {
            switch (MenuName)
            {
                // defines the top level menu option
                case "":
                    return menuHeader;
                // defines the submenu options
                case menuHeader:
                    string[] subMenus = { };
                    return subMenus;
            }
            return "";
        }

        public override void EA_OnNotifyContextItemModified(EA.Repository repository, string GUID, EA.ObjectType ot)
        {
            int x = 400;
            int y = -5;
            //int width = 350;
            int height = 100;


            if (ot == EA.ObjectType.otDiagram) // check that the modified object is diagram type.
            {
                EA.Diagram diagram = repository.GetDiagramByGuid(GUID); //get the diagram by the GUID.

                if (diagram != null)
                {
                    // loop over all the diagram objects in the diagram and search for the objects that represent the 'AUTO TITLE' object
                    foreach (DiagramObject diagramObject in diagram.DiagramObjects)
                    {
                        if (diagramObject.ElementID > 0)
                        {
                            Element element = repository.GetElementByID(diagramObject.ElementID); // get each element by his unique ID.
                            if (element != null && element.Name == titleElement) //checks the element is not null and his name match the 'AUTO TITLE' object name.
                            {
                                // if the element found his text note is being updated to the with the right modified time.
                                element.Notes = $"Diagram Name: {diagram.Name}.   Last Modified: {diagram.ModifiedDate.ToString().Substring(0, 9)}.";
                                element.Update();

                                diagramObject.left = x;
                                diagramObject.right = x + (element.Notes.Length * 5);
                                diagramObject.top = y;
                                diagramObject.bottom = y - height;
                                diagramObject.Update();
                                base.EA_OnNotifyContextItemModified(repository, GUID, ot); // call the base EA_OnNotifyContextItemModified function to continue the event as expected.
                                return;

                            }
                        }
                    }

                    //if the search for the matching object is not ended, that means no element was found there for we create a new title element 
                    EA.Package pac = repository.GetPackageByID(diagram.PackageID);

                    for (short i = 0; i < pac.Elements.Count; i++)
                    // loop over all the elements in the package and deletes all the elements that are duplicates of the auto title element.
                    {
                        Element elm = pac.Elements.GetAt(i);
                        if (elm != null && elm.Type == "Text" && elm.Name == titleElement)
                        {
                            if (elm.Notes.Substring(0, elm.Notes.IndexOf('.')) == $"Diagram Name: {diagram.Name}")
                            {
                                pac.Elements.DeleteAt(i, false);
                            }
                        }

                    }
                    pac.Elements.Refresh();

                    Element elem = pac.Elements.AddNew(titleElement, "Text"); // create the new text object in the packages broswer with the correct name for future look up and updated.
                    elem.Notes = $"Diagram Name: {diagram.Name}.   Last Modified: {diagram.ModifiedDate.ToString().Substring(0, 9)}."; //write the title text.
                    elem.Update();

                    // Create a diagram object for the title to be assign to.
                    string bounds = $"l={x};r={x + (elem.Notes.Length * 5)};t={y};b={y - height};";
                    DiagramObject diagramTextElem = (DiagramObject)diagram.DiagramObjects.AddNew(bounds, "Text");


                    // Set diagram object properties
                    diagramTextElem.ElementID = elem.ElementID;// set the diagram object id to be as the new element created.

                    diagramTextElem.Update();

                    repository.RefreshOpenDiagrams(true);

                    diagram.DiagramObjects.Refresh();

                    diagram.Update();

                    repository.RefreshOpenDiagrams(false);

                    base.EA_OnNotifyContextItemModified(repository, GUID, ot); // call the base EA_OnNotifyContextItemModified function to continue the event as expected.

                }
            }
        }

        public override bool EA_OnPostNewDiagram(Repository Repository, EventProperties Info)
        {
            Repository.GetCurrentDiagram().Update();
            return base.EA_OnPostNewDiagram(Repository, Info);

        }



        public override bool EA_OnPreDeleteDiagram(Repository Repository, EventProperties Info)
        {

            EA.Diagram diagram = Repository.GetDiagramByID(Info.Get(0).Value); //get the diagram by the GUID.

            if (diagram != null)
            {
                //if the search for the matching object is not ended, that means no element was found there for we create a new title element 
                EA.Package pac = Repository.GetPackageByID(diagram.PackageID);

                for (short i = 0; i < pac.Elements.Count; i++)
                // loop over all the elements in the package and deletes all the elements that are duplicates of the auto title element.
                {
                    Element elm = pac.Elements.GetAt(i);
                    if (elm != null && elm.Type == "Text" && elm.Name == titleElement)
                    {
                        if (elm.Notes.Substring(0, elm.Notes.IndexOf('.')) == $"Diagram Name: {diagram.Name}")
                        {
                            pac.Elements.DeleteAt(i, false);
                        }
                    }

                }
                pac.Elements.Refresh();
            }
            return base.EA_OnPreDeleteDiagram(Repository, Info);
        }



        /// EA calls this operation when it exists. Can be used to do some cleanup work.
        ///
        public override void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}

