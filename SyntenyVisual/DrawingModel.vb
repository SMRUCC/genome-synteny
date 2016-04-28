Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization

Public Class DrawingModel

    Public Property Links As Line()
    Public Property size As Size
    Public Property penWidth As Integer
    Public Property briefs As GenomeBrief()

    Public Function Visualize() As Image
        Dim font As New Font(FontFace.MicrosoftYaHei, 12, FontStyle.Italic)
        Dim maxtLen As Integer = briefs _
            .Select(Function(x) GDIPlusExtensions.MeasureString(x.Name, font).Width).Max
        Dim cl As SolidBrush = New SolidBrush(Color.Black)
        Dim dh As Integer = GDIPlusExtensions.MeasureString(briefs.First.Name, font).Height / 2

        Using gdi As GDIPlusDeviceHandle = New Size(size.Width + maxtLen * 1.5, size.Height).CreateGDIDevice
            For Each lnk As Line In Links
                Call lnk.Draw(gdi, penWidth)
            Next

            For Each x As GenomeBrief In briefs
                Call gdi.DrawString(x.Name, font, cl, New Point(size.Width, x.Y - dh))
            Next

            Return gdi.ImageResource
        End Using
    End Function
End Class

Public Class GenomeBrief

    Public Property Y As Integer
    Public Property Name As String
    Public Property Size As String

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class