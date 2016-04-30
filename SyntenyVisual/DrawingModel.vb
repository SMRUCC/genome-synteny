Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Linq

Public Class DrawingModel

    Public Property margin As Size
    Public Property Links As Line()
    Public Property size As Size
    ''' <summary>
    ''' 在这里控制基因组共线性的绘制的连线的粗细
    ''' </summary>
    ''' <returns></returns>
    Public Property penWidth As Integer
    Public Property briefs As GenomeBrief()

    Public Function Visualize() As Image
        Dim font As New Font(FontFace.MicrosoftYaHei, 12, FontStyle.Italic)  ' 默认的字体
        Dim texts As Vectors.Text() = briefs.ToArray(Function(x) Vectors.GetText(x.Name, font))
        Dim szs As SizeF() = __getSize(texts)
        Dim maxtLen As Integer = szs.Select(Function(x) x.Width).Max
        Dim cl As SolidBrush = New SolidBrush(Color.Black)
        Dim dh As Integer = GDIPlusExtensions.MeasureString(briefs.First.Name, font).Height / 2

        Using gdi As GDIPlusDeviceHandle = New Size(size.Width + maxtLen * 1.5, size.Height).CreateGDIDevice
            For Each lnk As Line In Links   ' 首先绘制连线
                Call lnk.Draw(gdi, penWidth)
            Next

            For Each x As GenomeBrief In briefs   '然后绘制基因组的简单表示，以及显示标题
                Call gdi.DrawString(x.Name, font, cl, New Point(size.Width, x.Y - dh))
                Call gdi.DrawLine(New Pen(Color.Gray, 10), New Point(margin.Width, x.Y), New Point(size.Width - margin.Width, x.Y))
            Next

            Return gdi.ImageResource
        End Using
    End Function

    Private Function __getSize(texts As Vectors.Text()) As SizeF()
        Using gdi As GDIPlusDeviceHandle = New Size(10, 10).CreateGDIDevice
            Return texts.ToArray(Function(x) x.MeasureString(gdi))
        End Using
    End Function
End Class

''' <summary>
''' The simple abstract of the genome drawing data.
''' </summary>
Public Class GenomeBrief

    Public Property Y As Integer
    ''' <summary>
    ''' The display title.(由于需要兼容html文本，所以这里是被当做html文本来对待了)
    ''' </summary>
    ''' <returns></returns>
    Public Property Name As String
    ''' <summary>
    ''' The length of the genome nt sequence
    ''' </summary>
    ''' <returns></returns>
    Public Property Size As String

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class