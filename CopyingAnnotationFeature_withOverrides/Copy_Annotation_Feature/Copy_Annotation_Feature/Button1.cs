using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.ArcMap;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Desktop;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;


namespace Copy_Annotation_Feature
{
    public class Button1 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public Button1()
        {
        }

        protected override void OnClick()
        {

            CopyAnnotationFeatures();
        }

        public void CopyAnnotationFeatures()
        {

            try
            {
                IMxDocument pMxDoc = (IMxDocument)ArcMap.Application.Document;
                IMap pMap = pMxDoc.FocusMap;
                ILayer SourceLayer = pMap.Layer[0];
                ILayer TargetLayer = pMap.Layer[1];

                IFeatureLayer SourceFLayer = (IFeatureLayer)SourceLayer;
                IFeatureLayer TargetFLayer = (IFeatureLayer)TargetLayer;

                IFeatureClass SourceFeatureClass = SourceFLayer.FeatureClass;
                IFeatureClass TargetFeatureClass = TargetFLayer.FeatureClass;

                IFields TargetFields = TargetFeatureClass.Fields;
                IFields SourceFields = SourceFeatureClass.Fields;

                IDictionary<int, int> symbSourceTargIdXref = new Dictionary<int, int>();
                symbSourceTargIdXref.Add(4, 17);
                symbSourceTargIdXref.Add(5, 22);
                symbSourceTargIdXref.Add(43, 45);
                symbSourceTargIdXref.Add(45, 45);
                ISymbolCollection targetSymbolsColl = (TargetFeatureClass.Extension as IAnnoClass).SymbolCollection;

                IField pFld;

                int[] lSrcFlds;
                int[] lTarFlds;

                int lFld, lExFld, i;
                lExFld = 0;
                for (lFld = 0; lFld <= (SourceFields.FieldCount - 1); lFld++)
                {
                    pFld = SourceFields.Field[lFld];

                    if (pFld.Type != esriFieldType.esriFieldTypeOID && pFld.Type != esriFieldType.esriFieldTypeGeometry && pFld.Name.ToUpper() != "ELEMENT" && pFld.Name.ToUpper() != "ZORDER" && pFld.Editable == true)
                    {
                        lExFld = lExFld + 1;

                    }
                }


                lSrcFlds = new int[lExFld];
                lTarFlds = new int[lExFld];

                i = 0;

                for (lFld = 0; lFld <= (SourceFields.FieldCount - 1); lFld++)
                {
                    pFld = SourceFields.Field[lFld];
                    if (pFld.Type != esriFieldType.esriFieldTypeOID && pFld.Type != esriFieldType.esriFieldTypeGeometry && pFld.Name.ToUpper() != "ELEMENT" && pFld.Name.ToUpper() != "ZORDER" && pFld.Editable == true)
                    {
                        lSrcFlds[i] = lFld;
                        lTarFlds[i] = TargetFields.FindField(pFld.Name);
                        i = i + 1;
                    }
                }


                IFeatureCursor pICursor;
                pICursor = SourceFeatureClass.Search(null, true);

                IFeature pIFeat;
                pIFeat = pICursor.NextFeature();

                IFDOGraphicsLayerFactory pGLF;
                pGLF = new FDOGraphicsLayerFactory();

                IDataset pDataset;
                pDataset = (IDataset)TargetFeatureClass;

              

                IAnnotationFeature pAnnoFeature;
                IClone pAClone;
                IElement pElement;


                IAnnoClass pTargetAnnoClass = (IAnnoClass)TargetFeatureClass.Extension;


                while (pIFeat != null)
                {
                    pAnnoFeature = (IAnnotationFeature)pIFeat;

                    if (pAnnoFeature.Annotation != null)
                    {
                       
                        ITextElement sourceTextElement = pAnnoFeature.Annotation as ITextElement;
                        pAClone = (IClone)sourceTextElement;
                        pElement = (IElement)pAClone.Clone();

                        ITextElement ptempTxt = (ITextElement)pAClone.Clone();
                        //set id
                       IFeature pTempFeat = TargetFeatureClass.CreateFeature();
                      
                     

                        ISymbolCollectionElement TargetSymbCollElem = (ISymbolCollectionElement)ptempTxt;
                        
                        ISymbolCollectionElement sourceSymbCollElem = sourceTextElement as ISymbolCollectionElement;
                      
                        int sourceSymbID = sourceSymbCollElem.SymbolID;
                       
                        int symbolID = symbSourceTargIdXref[sourceSymbID];
                       
                        ISymbolIdentifier2 pSymbI ;
                        ISymbolCollection2 pSymbolColl2 = ( ISymbolCollection2 ) pTargetAnnoClass.SymbolCollection;

                       pSymbolColl2.GetSymbolIdentifier(symbolID, out pSymbI );

                     //reset the desired symbol id

                       TargetSymbCollElem.set_SharedSymbol(pSymbI.ID, pSymbI.Symbol);

                     
                        //save the annotation feature
                       IAnnotationFeature pAnnoFeat = (IAnnotationFeature)pTempFeat;

                       pAnnoFeat.Annotation = ptempTxt as IElement;
                       pTempFeat.Store();

                        //reset the over rided property by analysing the override code

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideXOffset) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.XOffset = sourceSymbCollElem.XOffset;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideYOffset) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.YOffset = sourceSymbCollElem.YOffset;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideHorzAlignment) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.HorizontalAlignment = sourceSymbCollElem.HorizontalAlignment;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideVertAlignment) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.VerticalAlignment = sourceSymbCollElem.VerticalAlignment;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideFlipAngle) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.FlipAngle = TargetSymbCollElem.FlipAngle;
                       }
                       
                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideSize) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           
                           TargetSymbCollElem.Size = sourceSymbCollElem.Size;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideColor) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.Color = sourceSymbCollElem.Color;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideCharSpacing) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.CharacterSpacing = sourceSymbCollElem.CharacterSpacing;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideCharWidth) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.CharacterWidth = sourceSymbCollElem.CharacterWidth;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideWordSpacing) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.WordSpacing = sourceSymbCollElem.WordSpacing;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideLeading) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.Leading = sourceSymbCollElem.Leading;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideBold) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.Bold = sourceSymbCollElem.Bold;
                       }
                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideItalic) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.Italic = sourceSymbCollElem.Italic;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideUnderline) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.Underline = sourceSymbCollElem.Underline;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideBackground) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.Background = sourceSymbCollElem.Background;
                       }

                       if ((sourceSymbCollElem.OverriddenProperties ^ (int)esriSymbolOverrideEnum.esriSymbolOverrideFontName) <= sourceSymbCollElem.OverriddenProperties)
                       {
                           TargetSymbCollElem.FontName = sourceSymbCollElem.FontName;
                       }

                        //save the feature again
                       pAnnoFeat.Annotation = ptempTxt as IElement;
                       pTempFeat.Store();
                     

                      
                    }
                    pIFeat = pICursor.NextFeature();

                }

               
                pMxDoc.UpdateContents();
                pMxDoc.ActiveView.Refresh();




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        public void CopyAnnotationFeatures1()
        {


            try
            {
                IMxDocument pMxDoc = (IMxDocument)ArcMap.Application.Document;
                IMap pMap = pMxDoc.FocusMap;
                ILayer FromLayer = pMap.Layer[0];
                ILayer ToLayer = pMap.Layer[1];

                IFeatureLayer FromLayerFeature = (IFeatureLayer)FromLayer;
                IFeatureLayer ToLayerFeature = (IFeatureLayer)ToLayer;

                IFeatureClass FromFeatureClass = FromLayerFeature.FeatureClass;
                IFeatureClass ToFeatureClass = ToLayerFeature.FeatureClass;

                IFeatureBuffer featureBuffer = ToFeatureClass.CreateFeatureBuffer();
                IFeatureCursor insertCursor = ToFeatureClass.Insert(true);

                int iFeatureID = ToFeatureClass.FindField("FeatureID");
                int iZOrder = ToFeatureClass.FindField("ZOrder");
                int iAnnotationClassid = ToFeatureClass.FindField("AnnotationClassID");
                int iSymbolId = ToFeatureClass.FindField("SymbolID");
                int iStatus = ToFeatureClass.FindField("Status");
                int iTextString = ToFeatureClass.FindField("TextString");
                int iFontName = ToFeatureClass.FindField("FontName");
                int iFontSize = ToFeatureClass.FindField("FontSize");
                int iBold = ToFeatureClass.FindField("Bold");
                int iItalic = ToFeatureClass.FindField("Italic");
                int iUnderline = ToFeatureClass.FindField("Underline");
                int iVerticalAlignment = ToFeatureClass.FindField("VerticalAlignment");
                int iHorizontalAlignment = ToFeatureClass.FindField("HorizontalAlignment");
                int iXoffset = ToFeatureClass.FindField("XOffset");
                int iYoffset = ToFeatureClass.FindField("YOffset");
                int iAngle = ToFeatureClass.FindField("FontLeading");
                int iWordspacing = ToFeatureClass.FindField("WordSpacing");
                int iCharacterWidth = ToFeatureClass.FindField("CharacterWidth");
                int iCharacterSpacing = ToFeatureClass.FindField("CharacterSpacing");
                int iFlipAngle = ToFeatureClass.FindField("FlipAngle");
                int iOverride = ToFeatureClass.FindField("Override");
               

                IFeatureCursor searchCursor = FromFeatureClass.Search(null, true);

                int sFeatureID = FromFeatureClass.FindField("FeatureID");
                int sZOrder = FromFeatureClass.FindField("ZOrder");
                int sAnnotationClassid = FromFeatureClass.FindField("AnnotationClassID");
                int sSymbolId = FromFeatureClass.FindField("SymbolID");
                int sStatus = FromFeatureClass.FindField("Status");
                int sTextString = FromFeatureClass.FindField("TextString");
                int sFontName = FromFeatureClass.FindField("FontName");
                int sFontSize = FromFeatureClass.FindField("FontSize");
                int sBold = FromFeatureClass.FindField("Bold");
                int sItalic = FromFeatureClass.FindField("Italic");
                int sUnderline = FromFeatureClass.FindField("Underline");
                int sVerticalAlignment = FromFeatureClass.FindField("VerticalAlignment");
                int sHorizontalAlignment = FromFeatureClass.FindField("HorizontalAlignment");
                int sXoffset = FromFeatureClass.FindField("XOffset");
                int sYoffset = FromFeatureClass.FindField("YOffset");
                int sAngle = FromFeatureClass.FindField("FontLeading");
                int sWordspacing = FromFeatureClass.FindField("WordSpacing");
                int sCharacterWidth = FromFeatureClass.FindField("CharacterWidth");
                int sCharacterSpacing = FromFeatureClass.FindField("CharacterSpacing");
                int sFlipAngle = FromFeatureClass.FindField("FlipAngle");
                int sOverride = FromFeatureClass.FindField("Override");
               

                IFeature feature;
                while ((feature = searchCursor.NextFeature()) != null)
                {
                    featureBuffer.Shape = feature.Shape;
                    featureBuffer.Value[iFeatureID] = feature.Value[sFeatureID];
                    featureBuffer.Value[iZOrder] = feature.Value[sZOrder];
                    featureBuffer.Value[iAnnotationClassid] = feature.Value[sAnnotationClassid];
                    featureBuffer.Value[iSymbolId] = feature.Value[sSymbolId];
                    featureBuffer.Value[iStatus] = feature.Value[sStatus];
                    featureBuffer.Value[iTextString] = feature.Value[sTextString];
                    featureBuffer.Value[iFontName] = feature.Value[sFontName];
                    featureBuffer.Value[iFontSize] = feature.Value[sFontSize];
                    featureBuffer.Value[iBold] = feature.Value[sBold];
                    featureBuffer.Value[iItalic] = feature.Value[sItalic];
                    featureBuffer.Value[iUnderline] = feature.Value[sUnderline];
                    featureBuffer.Value[iVerticalAlignment] = feature.Value[sVerticalAlignment];
                    featureBuffer.Value[iHorizontalAlignment] = feature.Value[sHorizontalAlignment];
                    featureBuffer.Value[iXoffset] = feature.Value[sXoffset];
                    featureBuffer.Value[iYoffset] = feature.Value[sYoffset];
                    featureBuffer.Value[iAngle] = feature.Value[sAngle];
                    featureBuffer.Value[iWordspacing] = feature.Value[sWordspacing];
                    featureBuffer.Value[iCharacterWidth] = feature.Value[sCharacterWidth];
                    featureBuffer.Value[iCharacterSpacing] = feature.Value[sCharacterSpacing];
                    featureBuffer.Value[iFlipAngle] = feature.Value[sFlipAngle];
                    

                    insertCursor.InsertFeature(featureBuffer);

                    insertCursor.Flush();
                }

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(searchCursor);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(insertCursor);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
