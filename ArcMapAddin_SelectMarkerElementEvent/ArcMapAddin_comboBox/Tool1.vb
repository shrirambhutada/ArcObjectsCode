Imports ArcMapAddin_comboBox.My
Imports ESRI.ArcGIS.Framework

Public Class Tool1
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

    Private m_docEvents As ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event

    Public Sub New()

    End Sub
    Protected Overrides Sub OnMouseDown(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)

        Dim pCombo1 As Combo1
        pCombo1 = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of Combo1)(My.ThisAddIn.IDs.Combo1)

        MsgBox(pCombo1.GetSelectedLayer().Name)


        Dim pCmmd As ICommandItem
        pCmmd = ArcMap.Application.Document.CommandBars.Find("Query_ZoomToSelected")
        pCmmd.Execute()

        MyBase.OnMouseDown(arg)
    End Sub
    Protected Overrides Sub OnUpdate()
        Enabled = My.ArcMap.Application IsNot Nothing
    End Sub


End Class
