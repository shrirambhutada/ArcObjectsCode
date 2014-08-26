using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;

namespace ArcMapAddin_WireAllActiveEvents
{
    public class WireAllEvents : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event m_docEvents = null;  

        public WireAllEvents()
        {
        }

        protected override void OnClick()
        {
            //
            //  TODO: Sample code showing how to access button host
            //
            SetUpDocumentEvent((IDocument) ArcMap.Document);
            ArcMap.Application.CurrentTool = null;
        }

        #region "DocumentEvents"

        private void SetUpDocumentEvent(ESRI.ArcGIS.Framework.IDocument myDocument)
        {
            m_docEvents = myDocument as ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event;  
            m_docEvents.NewDocument += new ESRI.ArcGIS.ArcMapUI.IDocumentEvents_NewDocumentEventHandler(OnNewDocument);
            m_docEvents.OpenDocument += new ESRI.ArcGIS.ArcMapUI.IDocumentEvents_OpenDocumentEventHandler(OnOpenDocument);
        }

        /// <summary>
        /// The NewDocument event handler. 
        /// </summary>
        /// <remarks></remarks>
        void OnNewDocument()
        {
          //ESRI.ArcGIS.Framework.IDocument theDocument = m_docEvents as ESRI.ArcGIS.Framework.IDocument;

            IMxDocument m_MxDoc = (IMxDocument)ArcMap.Document;
          SetupActiveViewEvents(m_MxDoc.FocusMap);
        //  System.Windows.Forms.MessageBox.Show("newDoc");
        }

        /// <summary>
        /// The OpenDocument event handler.
        /// </summary>
        /// <remarks></remarks>
        void OnOpenDocument()
        {
          //ESRI.ArcGIS.Framework.IDocument theDocument = m_docEvents as ESRI.ArcGIS.Framework.IDocument;
          IMxDocument m_MxDoc = (IMxDocument)ArcMap.Document;
          SetupActiveViewEvents(m_MxDoc.FocusMap);
        }

        #endregion


        #region "ActiveEvents"
        private ESRI.ArcGIS.Carto.IActiveViewEvents_AfterDrawEventHandler m_ActiveViewEventsAfterDraw;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_AfterItemDrawEventHandler m_ActiveViewEventsAfterItemDraw;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsChangedEventHandler m_ActiveViewEventsContentsChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsClearedEventHandler m_ActiveViewEventsContentsCleared;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_FocusMapChangedEventHandler m_ActiveViewEventsFocusMapChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler m_ActiveViewEventsItemAdded;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ItemDeletedEventHandler m_ActiveViewEventsItemDeleted;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ItemReorderedEventHandler m_ActiveViewEventsItemReordered;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler m_ActiveViewEventsSelectionChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_SpatialReferenceChangedEventHandler m_ActiveViewEventsSpatialReferenceChanged;
        private ESRI.ArcGIS.Carto.IActiveViewEvents_ViewRefreshedEventHandler m_ActiveViewEventsViewRefreshed;

        private void SetupActiveViewEvents(ESRI.ArcGIS.Carto.IMap map)
        {
            //parameter check
            if (map == null)
            {
                return;

            }
            ESRI.ArcGIS.Carto.IActiveViewEvents_Event activeViewEvents = map as ESRI.ArcGIS.Carto.IActiveViewEvents_Event;
            // Create an instance of the delegate, add it to AfterDraw event
            m_ActiveViewEventsAfterDraw = new ESRI.ArcGIS.Carto.IActiveViewEvents_AfterDrawEventHandler(OnActiveViewEventsAfterDraw);
            activeViewEvents.AfterDraw += m_ActiveViewEventsAfterDraw;

            // Create an instance of the delegate, add it to AfterItemDraw event
            m_ActiveViewEventsAfterItemDraw = new ESRI.ArcGIS.Carto.IActiveViewEvents_AfterItemDrawEventHandler(OnActiveViewEventsItemDraw);
            activeViewEvents.AfterItemDraw += m_ActiveViewEventsAfterItemDraw;

            // Create an instance of the delegate, add it to ContentsChanged event
            m_ActiveViewEventsContentsChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsChangedEventHandler(OnActiveViewEventsContentsChanged);
            activeViewEvents.ContentsChanged += m_ActiveViewEventsContentsChanged;

            // Create an instance of the delegate, add it to ContentsCleared event
            m_ActiveViewEventsContentsCleared = new ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsClearedEventHandler(OnActiveViewEventsContentsCleared);
            activeViewEvents.ContentsCleared += m_ActiveViewEventsContentsCleared;

            // Create an instance of the delegate, add it to FocusMapChanged event
            m_ActiveViewEventsFocusMapChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_FocusMapChangedEventHandler(OnActiveViewEventsFocusMapChanged);
            activeViewEvents.FocusMapChanged += m_ActiveViewEventsFocusMapChanged;

            // Create an instance of the delegate, add it to ItemAdded event
            m_ActiveViewEventsItemAdded = new ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler(OnActiveViewEventsItemAdded);
            activeViewEvents.ItemAdded += m_ActiveViewEventsItemAdded;

            // Create an instance of the delegate, add it to ItemDeleted event
            m_ActiveViewEventsItemDeleted = new ESRI.ArcGIS.Carto.IActiveViewEvents_ItemDeletedEventHandler(OnActiveViewEventsItemDeleted);
            activeViewEvents.ItemDeleted += m_ActiveViewEventsItemDeleted;

            // Create an instance of the delegate, add it to ItemReordered event
            m_ActiveViewEventsItemReordered = new ESRI.ArcGIS.Carto.IActiveViewEvents_ItemReorderedEventHandler(OnActiveViewEventsItemReordered);
            activeViewEvents.ItemReordered += m_ActiveViewEventsItemReordered;

            // Create an instance of the delegate, add it to SelectionChanged event
            m_ActiveViewEventsSelectionChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler(OnActiveViewEventsSelectionChanged);
            activeViewEvents.SelectionChanged += m_ActiveViewEventsSelectionChanged;

            // Create an instance of the delegate, add it to SpatialReferenceChanged event
            m_ActiveViewEventsSpatialReferenceChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_SpatialReferenceChangedEventHandler(OnActiveViewEventsSpatialReferenceChanged);
            activeViewEvents.SpatialReferenceChanged += m_ActiveViewEventsSpatialReferenceChanged;

            // Create an instance of the delegate, add it to ViewRefreshed event
            m_ActiveViewEventsViewRefreshed = new ESRI.ArcGIS.Carto.IActiveViewEvents_ViewRefreshedEventHandler(OnActiveViewEventsViewRefreshed);
            activeViewEvents.ViewRefreshed += m_ActiveViewEventsViewRefreshed;
        }

        private void RemoveActiveViewEvents(ESRI.ArcGIS.Carto.IMap map)
        {

            //parameter check
            if (map == null)
            {
                return;

            }
            ESRI.ArcGIS.Carto.IActiveViewEvents_Event activeViewEvents = map as ESRI.ArcGIS.Carto.IActiveViewEvents_Event;

            // Remove AfterDraw Event Handler
            activeViewEvents.AfterDraw -= m_ActiveViewEventsAfterDraw;

            // Remove AfterItemDraw Event Handler
            activeViewEvents.AfterItemDraw -= m_ActiveViewEventsAfterItemDraw;

            // Remove ContentsChanged Event Handler
            activeViewEvents.ContentsChanged -= m_ActiveViewEventsContentsChanged;

            // Remove ContentsCleared Event Handler
            activeViewEvents.ContentsCleared -= m_ActiveViewEventsContentsCleared;

            // Remove FocusMapChanged Event Handler
            activeViewEvents.FocusMapChanged -= m_ActiveViewEventsFocusMapChanged;

            // Remove ItemAdded Event Handler
            activeViewEvents.ItemAdded -= m_ActiveViewEventsItemAdded;

            // Remove ItemDeleted Event Handler
            activeViewEvents.ItemDeleted -= m_ActiveViewEventsItemDeleted;

            // Remove ItemReordered Event Handler
            activeViewEvents.ItemReordered -= m_ActiveViewEventsItemReordered;

            // Remove SelectionChanged Event Handler
            activeViewEvents.SelectionChanged -= m_ActiveViewEventsSelectionChanged;

            // Remove SpatialReferenceChanged Event Handler
            activeViewEvents.SpatialReferenceChanged -= m_ActiveViewEventsSpatialReferenceChanged;

            // Remove ViewRefreshed Event Handler
            activeViewEvents.ViewRefreshed -= m_ActiveViewEventsViewRefreshed;
        }

        /// <summary>
        /// AfterDraw Event handler
        /// </summary>
        /// <param name="Display"></param>
        /// <param name="phase"></param>
        /// <remarks>SECTION: Custom Functions that you write to add additionally functionality for the events</remarks>
        private void OnActiveViewEventsAfterDraw(ESRI.ArcGIS.Display.IDisplay display, ESRI.ArcGIS.Carto.esriViewDrawPhase phase)
        {

            ESRI.ArcGIS.Carto.esriViewDrawPhase m_phase = phase;
            System.Windows.Forms.MessageBox.Show(phase.ToString());
           
        }

        /// <summary>
        /// ItemDraw Event handler
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Display"></param>
        /// <param name="phase"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemDraw(short Index, ESRI.ArcGIS.Display.IDisplay display, ESRI.ArcGIS.esriSystem.esriDrawPhase phase)
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("ItemDraw"); 
        }

        /// <summary>
        /// ContentsChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsContentsChanged()
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("ContentsChanged"); 
        }

        /// <summary>
        /// ContentsCleared Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsContentsCleared()
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("ContentsCleared"); 
        }

        /// <summary>
        /// FocusMapChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsFocusMapChanged()
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("FocusMapChanged"); 
        }

        /// <summary>
        /// ItemAdded Event handler
        /// </summary>
        /// <param name="Item"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemAdded(System.Object Item)
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("ItemAdded"); 
        }

        /// <summary>
        /// ItemDeleted Event handler
        /// </summary>
        /// <param name="Item"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemDeleted(System.Object Item)
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("ItemDeleted"); 
        }

        /// <summary>
        /// ItemReordered Event handler
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="toIndex"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsItemReordered(System.Object Item, System.Int32 toIndex)
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("ItemReordered"); 
        }

        /// <summary>
        /// SelectionChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsSelectionChanged()
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("SelectionChanged"); 
        }

        /// <summary>
        /// SpatialReferenceChanged Event handler
        /// </summary>
        /// <remarks></remarks>
        private void OnActiveViewEventsSpatialReferenceChanged()
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("SpatialReferenceChanged"); 
        }

        /// <summary>
        /// ViewRefreshed Event handler
        /// </summary>
        /// <param name="view"></param>
        /// <param name="phase"></param>
        /// <param name="data"></param>
        /// <param name="envelope"></param>
        /// <remarks></remarks>
        private void OnActiveViewEventsViewRefreshed(ESRI.ArcGIS.Carto.IActiveView view, ESRI.ArcGIS.Carto.esriViewDrawPhase phase, System.Object data, ESRI.ArcGIS.Geometry.IEnvelope envelope)
        {
            // TODO: Add your code here
            // System.Windows.Forms.MessageBox.Show("ViewRefreshed");
        }


        #endregion

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
