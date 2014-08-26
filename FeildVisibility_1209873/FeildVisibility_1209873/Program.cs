using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.GISClient;
using ESRI.ArcGIS.Server;

namespace FeildVisibility_1209873
{
    class Program
    {
        private static LicenseInitializer m_AOLicenseInitializer = new LicenseInitializer();
    
        static void Main(string[] args)
        {
            LicenseApplication();

            IMapDocument pMapDoc = new MapDocument();

            pMapDoc.Open("C:\\junk\\incidents\\1209873\\1209873_fieldvisibility.mxd", "");

            IMap pMap = null;
            pMap = pMapDoc.get_Map(0);

            IFieldInfo pFieldInfo;
            ILayerFields pLyrFields;

            pLyrFields = (ILayerFields) pMap.get_Layer(0);
            pFieldInfo = pLyrFields.get_FieldInfo(4);
            pFieldInfo.Visible = false;

            pMapDoc.Save(true, true);
            pMapDoc.Close();






            m_AOLicenseInitializer.ShutdownApplication();



 
        }

    

        static void LicenseApplication()
        {
            //ESRI License Initializer generated code.
            m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeAdvanced },
            new esriLicenseExtensionCode[] { esriLicenseExtensionCode.esriLicenseExtensionCode3DAnalyst, esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork, esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst });
            //ESRI License Initializer generated code.
            //Do not make any call to ArcObjects after ShutDownApplication()
         //   
        }
    }
}
