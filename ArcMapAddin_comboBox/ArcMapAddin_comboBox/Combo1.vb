
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Geometry




Public Class Combo1
    Inherits ESRI.ArcGIS.Desktop.AddIns.ComboBox
    Private m_docEvents As ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event
    Private m_ActiveViewEventsItemAdded As ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler
    Private m_ActiveViewEventsSelectionChanged As ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler
    Private m_ActiveViewEvents_ContentsChanged As ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsChangedEventHandler
    Public SelectedCookie As Integer
    Dim pMap As IMap
    Public Sub New()
        Dim pDoc As IMxDocument
        Dim pMap As IMap

        m_docEvents = CType(My.ArcMap.Document, ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event)


        AddHandler m_docEvents.NewDocument, AddressOf OnNewDocument
        AddHandler m_docEvents.OpenDocument, AddressOf OnOpenDocument

        ' Dim pCOM As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
        '  pCOM.ManageLifetime(m_docEvents)

        pMap = My.ArcMap.Document.FocusMap

        For i = 0 To pMap.LayerCount - 1
            Me.Add(pMap.Layer(i).Name)

        Next


    End Sub

    Private Sub SetUpDocumentEvent(ByVal myDocument As ESRI.ArcGIS.Framework.IDocument)
        m_docEvents = CType(myDocument, ESRI.ArcGIS.ArcMapUI.IDocumentEvents_Event)
        AddHandler m_docEvents.NewDocument, AddressOf OnNewDocument
        AddHandler m_docEvents.OpenDocument, AddressOf OnOpenDocument
    End Sub

    Private Sub OnNewDocument()
        Dim theDocument As ESRI.ArcGIS.Framework.IDocument = CType(m_docEvents, ESRI.ArcGIS.Framework.IDocument)

        pMap = CType(theDocument, IMxDocument).FocusMap
        Dim activeViewEvents As ESRI.ArcGIS.Carto.IActiveViewEvents_Event = TryCast(pMap, ESRI.ArcGIS.Carto.IActiveViewEvents_Event)

        m_ActiveViewEventsItemAdded = New ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler(AddressOf OnActiveViewEventsItemAdded)
        'Create an instance of the delegate, add it to SelectionChanged event
        m_ActiveViewEventsSelectionChanged = New ESRI.ArcGIS.Carto.IActiveViewEvents_SelectionChangedEventHandler(AddressOf OnActiveViewEventsSelectionChanged)
        m_ActiveViewEvents_ContentsChanged = New ESRI.ArcGIS.Carto.IActiveViewEvents_ContentsChangedEventHandler(AddressOf OnActiveViewEventsContentsChanged)

        AddHandler activeViewEvents.SelectionChanged, m_ActiveViewEventsSelectionChanged


        AddHandler activeViewEvents.ContentsChanged, m_ActiveViewEvents_ContentsChanged

        AddHandler activeViewEvents.ItemAdded, m_ActiveViewEventsItemAdded

        updateLayers()
    End Sub

    Private Sub OnOpenDocument()
        Dim theDocument As ESRI.ArcGIS.Framework.IDocument = CType(m_docEvents, ESRI.ArcGIS.Framework.IDocument)
        Dim pMap As IMap
        pMap = CType(theDocument, IMxDocument).FocusMap
        m_ActiveViewEventsItemAdded = New ESRI.ArcGIS.Carto.IActiveViewEvents_ItemAddedEventHandler(AddressOf OnActiveViewEventsItemAdded)



        Dim activeViewEvents As ESRI.ArcGIS.Carto.IActiveViewEvents_Event = TryCast(pMap, ESRI.ArcGIS.Carto.IActiveViewEvents_Event)
        AddHandler activeViewEvents.ItemAdded, m_ActiveViewEventsItemAdded
        updateLayers()
    End Sub

    Private Sub OnActiveViewEventsItemAdded(ByVal Item As System.Object)
        ' TODO: Add your code here
        System.Windows.Forms.MessageBox.Show("Item Added")
        updateLayers()
    End Sub

    Private Sub OnActiveViewEventsSelectionChanged()
        ' TODO: Add your code here
        Dim pGCS As IGraphicsContainerSelect
        pGCS = pMap

        Dim pEnumEle As IEnumElement
        pEnumEle = pGCS.SelectedElements()
        Dim pEle As IElement
        pEle = pEnumEle.Next

        If Not pEle Is Nothing Then
            If TypeOf pEle Is IMarkerElement Then
                Dim pGeo As IGeometry
                pGeo = pEle.Geometry

                If TypeOf pGeo Is IPoint Then
                    Dim pPt As IPoint
                    pPt = pGeo

                    System.Windows.Forms.MessageBox.Show(pPt.X.ToString() & " : " & pPt.Y.ToString())
                Else
                    System.Windows.Forms.MessageBox.Show("other geo")
                End If
            End If
        End If


    End Sub


    Public Function GetSelectedLayer() As ILayer

        Return Me.GetItem(Me.SelectedCookie).Tag

    End Function



    Protected Overrides Sub OnSelChange(ByVal cookie As Integer)

        Me.SelectedCookie = cookie
        MyBase.OnSelChange(cookie)
    End Sub
    Protected Overrides Sub OnFocus(ByVal [set] As Boolean)

        MyBase.OnFocus([set])
    End Sub

    Protected Overrides Sub OnUpdate()

        Enabled = My.ArcMap.Application IsNot Nothing
    End Sub


    Private Sub updateLayers()
        Me.Clear()
        Dim pDoc As IMxDocument
        Dim pMap As IMap

        pMap = My.ArcMap.Document.FocusMap

        For i = 0 To pMap.LayerCount - 1
            Me.Add(pMap.Layer(i).Name, pMap.Layer(i))

        Next
    End Sub

    Private Sub OnActiveViewEventsContentsChanged()
        System.Windows.Forms.MessageBox.Show("active view changed")
    End Sub

End Class
