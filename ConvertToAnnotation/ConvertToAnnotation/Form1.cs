using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using   ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;

namespace ConvertToAnnotation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler m_ActiveViewEventsSelectionChanged;
        private ICompositeLayer m_CompoLyr;
        private Form_LabelEdit m_Form_LabelEdit = new Form_LabelEdit();
        private IMap m_Map;
        private IActiveViewEvents_Event activeViewEvents;
        private void Form1_Load(object sender, EventArgs e)
        {
            //write events
            m_Map = axMapControl1.Map; 
            activeViewEvents = m_Map as IActiveViewEvents_Event;

            m_ActiveViewEventsSelectionChanged = new ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler(OnActiveViewEventsSelectionChanged);
            activeViewEvents.SelectionChanged += m_ActiveViewEventsSelectionChanged;
            

        }

        private void OnActiveViewEventsSelectionChanged()
        {
           
            m_CompoLyr = this.axMapControl1.Map.ActiveGraphicsLayer as ICompositeLayer;

            for (int i = 0; i < m_CompoLyr.Count; i++)
            {
            IGraphicsLayer m_GraphicsLyr =(IGraphicsLayer) m_CompoLyr.Layer[i];

            IGraphicsContainerSelect m_GConatinerSelect = m_GraphicsLyr as IGraphicsContainerSelect;

            if (m_GConatinerSelect.ElementSelectionCount != 1)
            {
                //MessageBox.Show("Select only one graphic element");
            }

            else
            {
                IElement m_Element = (IElement) m_GConatinerSelect.SelectedElements.Next();

                if (m_Element is ITextElement)
                {
                    if (this.m_Form_LabelEdit == null)
                    {
                        m_Form_LabelEdit = new Form_LabelEdit();
                        m_Form_LabelEdit.m_ActiveView = this.axMapControl1.ActiveView;
                        m_Form_LabelEdit.m_TextElement = (ITextElement)m_Element;

                        m_Form_LabelEdit.Show();
                    }

                    else
                    {
                        m_Form_LabelEdit = new Form_LabelEdit();
                        m_Form_LabelEdit.m_ActiveView = this.axMapControl1.ActiveView;
                        m_Form_LabelEdit.m_TextElement = (ITextElement)m_Element;

                        m_Form_LabelEdit.Show();

                    }
                }
            }

            this.axMapControl1.ActiveView.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConvertLabelsToAnnotationSingleLayerMapAnno(this.axMapControl1.Map, 0);
        }

     

        private void ConvertLabelsToAnnotationSingleLayerMapAnno(IMap pMap, int layerIndex)
        {


            //label features by OBJECTID FEILD

            IGeoFeatureLayer pGeoFeatLayer;
            pGeoFeatLayer = pMap.Layer[layerIndex] as IGeoFeatureLayer;
            IAnnotateLayerPropertiesCollection pAnnotationLayerProColl = pGeoFeatLayer.AnnotationProperties;
            pGeoFeatLayer.DisplayAnnotation = true;
            IAnnotateLayerProperties pAnnoLayPro;
            IElementCollection pElecol1;
            IElementCollection pElecol2;
            pAnnotationLayerProColl.QueryItem(0, out pAnnoLayPro, out pElecol1, out pElecol2);
            ILabelEngineLayerProperties pLabelEngineLayerPro;
            pLabelEngineLayerPro = pAnnoLayPro as ILabelEngineLayerProperties;
            pLabelEngineLayerPro.Expression = "[OBJECTID]";
            pAnnoLayPro.DisplayAnnotation = true;

            //convert to annotation

            IConvertLabelsToAnnotation pConvertLabelsToAnnotation = new
                ConvertLabelsToAnnotationClass();
            ITrackCancel pTrackCancel = new CancelTrackerClass();
            //Change global level options for the conversion by sending in different parameters to the next line.
            pConvertLabelsToAnnotation.Initialize(pMap,
                esriAnnotationStorageType.esriMapAnnotation,
                esriLabelWhichFeatures.esriVisibleFeatures, true, pTrackCancel, null);
            ILayer pLayer = pMap.get_Layer(layerIndex);
            IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            if (pGeoFeatureLayer != null)
            {
                IFeatureClass pFeatureClass = pGeoFeatureLayer.FeatureClass;
                //Add the layer information to the converter object. Specify the parameters of the output annotation feature class here as well.
                pConvertLabelsToAnnotation.AddFeatureLayer(pGeoFeatureLayer,
                    pGeoFeatureLayer.Name + "_Anno", null, null, false, false, false, false,
                    false, "");
                //Do the conversion.
                pConvertLabelsToAnnotation.ConvertLabels();
                //Turn off labeling for the layer converted.
                pGeoFeatureLayer.DisplayAnnotation = false;
                //Refresh the map to update the display.
                IActiveView pActiveView = pMap as IActiveView;
                pActiveView.Refresh();



            }


          
        }
    }
}
