Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel.Loci
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Imaging

Public Class Histogram : Inherits CurvesModel

    Protected Overrides Sub Draw(ByRef source As IGraphics, buf() As Double, location As Point, size As Size, range As DoubleRange)

        'Y箭头向上
        'Call Gr.Gr_Device.DrawLine(LinePen, Vertex, New Point(Vertex.X - 5, Vertex.Y + 20))
        'Vertex = New Point(Location.X + size.Width, Location.Y)
        'Call Gr.Gr_Device.DrawLine(LinePen, Location, Vertex) 'X
        'Call Gr.Gr_Device.DrawString(Loci.Left & " bp", TagFont, Brushes.Black, New Point(Location.X, Location.Y + 30)) '基因组上面的位置信息
        'Call Gr.Gr_Device.DrawString(Loci.Right & " bp", TagFont, Brushes.Black, New Point(Vertex.X, Vertex.Y + 30))
        'X箭头向右
        'Call Gr.Gr_Device.DrawLine(LinePen, Vertex, New Point(Vertex.X - 20, Vertex.Y - 5))
    End Sub
End Class

