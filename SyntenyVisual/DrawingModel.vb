Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging

Public Class DrawingModel

    Public Property Links As Line()
    Public Property size As Size
    Public Property penWidth As Integer

    Public Function Visualize() As Image
        Using gdi As GDIPlusDeviceHandle = size.CreateGDIDevice
            For Each lnk As Line In Links
                Call lnk.Draw(gdi, penWidth)
            Next

            Return gdi.ImageResource
        End Using
    End Function
End Class