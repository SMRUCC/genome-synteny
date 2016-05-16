Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.SequenceModel
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Imaging

<[PackageNamespace]("NT.GC.Curve", Publisher:="amethyst.asuka@gcmodeller.org")>
Public Module GCSkew

    <DataFrameColumn("slide_Window.size")>
    Public Property WindowSize As Integer = 500
    <DataFrameColumn("slide_Window.steps")>
    Public Property Steps As Integer = 20
    <DataFrameColumn("plot.height")>
    Public Property PlotsHeight As Integer = 100
    <DataFrameColumn("offset.aix_y")>
    Public Property YValueOffset As Integer = 40
    <DataFrameColumn("average.shows")>
    Public Property ShowAverageLine As Boolean = True

    Dim PlotBrush As SolidBrush = Brushes.DarkGreen

    <ExportAPI("Plot.Set.Color")>
    Public Function SetPlotBrush(r As Integer, g As Integer, b As Integer, Optional alpha As Integer = 220) As Color
        Dim cl = Color.FromArgb(alpha, r, g, b)
        PlotBrush = New SolidBrush(cl)
        Return cl
    End Function

    ''' <summary>
    ''' 将GC含量曲线绘制到目标比对图形<paramref name="source"></paramref>之上
    ''' </summary>
    ''' <param name="source"></param> 
    ''' <param name="nt">Attributes的 1 和 2 分别为nt的开始和结束的位置</param>
    ''' <param name="Location">坐标轴原点的位置</param>
    ''' <param name="Width">坐标轴纵轴的宽度</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    <ExportAPI("GC.Content.Drawing")>
    Public Function InvokeDrawingGCContent(source As Image, nt As FASTA.FastaToken, Location As Point, Width As Integer) As Image
        Dim GCContent As Double() = NucleotideModels.GCContent(nt, WindowSize, Steps, True)
        Return InvokeDrawingCurve(source, GCContent, Location, New Size(Width, PlotsHeight), loci:=__getLoci(nt))
    End Function

    Private Function __getLoci(nt As FASTA.FastaToken) As Loci.Location
        If nt.Attributes.Length < 3 Then
            Dim List As New List(Of String)
            Call List.AddRange(nt.Attributes)
            Call List.Add("")
            Call List.Add("")
            nt.Attributes = List.ToArray
        End If

        Dim Loci = New Loci.Location(Val(nt.Attributes(1)), Val(nt.Attributes(2)))
        If Loci.Right = 0 Then
            Loci.Right = nt.Length
        End If

        Return Loci
    End Function

    ''' <summary>
    ''' 绘制基本的坐标轴
    ''' </summary>
    ''' <param name="g"></param>
    ''' <param name="location"></param>
    ''' <param name="size"></param>
    ''' <param name="tagFont"></param>
    Public Sub DrawAixs(g As GDIPlusDeviceHandle, location As Point, size As Size, tagFont As Font, min As Double, max As Double)
        Dim aixsSize As Size = "0".MeasureString(tagFont)
        Dim vertex As Point = New Point(location.X, location.Y - size.Height)
        Dim tmpLoci As New Point(location.X - YValueOffset,
                                 location.Y - aixsSize.Height / 2)

        Call g.DrawString(Mid(min.ToString, 1, 5),
                          font:=tagFont,
                          brush:=Brushes.Black,
                          point:=tmpLoci)
        Call g.DrawString(Mid(max.ToString, 1, 5),
                          tagFont,
                          Brushes.Black,
                          New Point(vertex.X - YValueOffset, vertex.Y - aixsSize.Height / 2))
    End Sub

    <ExportAPI("Curve.Drawing")>
    Public Function InvokeDrawingCurve(source As Image, buf As Double(), location As Point, size As Size, loci As Loci.Location) As Image
        Dim Gr As GDIPlusDeviceHandle = source.GdiFromImage
        Dim Max As Double = buf.Max
        Dim Min As Double = buf.Min
        Dim LinePen As New Pen(color:=Color.FromArgb(30, Color.LightGray), width:=0.3)
        Dim tagFont As New Font(FontFace.Ubuntu, 12)

        Call DrawAixs(Gr, location, size, tagFont, Min, Max)

        'Y箭头向上
        'Call Gr.Gr_Device.DrawLine(LinePen, Vertex, New Point(Vertex.X - 5, Vertex.Y + 20))
        'Vertex = New Point(Location.X + size.Width, Location.Y)
        'Call Gr.Gr_Device.DrawLine(LinePen, Location, Vertex) 'X
        'Call Gr.Gr_Device.DrawString(Loci.Left & " bp", TagFont, Brushes.Black, New Point(Location.X, Location.Y + 30)) '基因组上面的位置信息
        'Call Gr.Gr_Device.DrawString(Loci.Right & " bp", TagFont, Brushes.Black, New Point(Vertex.X, Vertex.Y + 30))
        'X箭头向右
        'Call Gr.Gr_Device.DrawLine(LinePen, Vertex, New Point(Vertex.X - 20, Vertex.Y - 5))

        Dim X_ScaleFactor As Double = size.Width / buf.Count
        Dim Y_ScaleFactor As Double = size.Height / (Max - Min)
        Dim X As Double = location.X, Y As Integer
        Dim avg As Double = buf.Average
        Dim Y_avg As Double = location.Y - (avg - Min) * Y_ScaleFactor
        Dim dddd As Double = size.Height / 10

        Y = location.Y - size.Height

        For i As Integer = 0 To 9
            'Call Gr.Gr_Device.DrawLine(LinePen, New Point(Location.X, Y), New Point(Location.X + size.Width, Y))
            Y += dddd
        Next

        Dim prePt As Point = New Point(X, location.Y - (buf.First - Min) * Y_ScaleFactor)

        LinePen = New Pen(PlotBrush, 2)

        If ShowAverageLine Then  ' 绘制中间的平均线
            Call Gr.Gr_Device.DrawLine(New Pen(Brushes.LightGray, 3), New Point(location.X, Y_avg), New Point(location.X + size.Width, Y_avg))
            Call Gr.Gr_Device.DrawString(Mid(avg, 1, 5), tagFont, Brushes.Black, New Point(location.X - YValueOffset, Y_avg - "0".MeasureString(tagFont).Height / 2))
        End If

        Dim Region As Rectangle

        For Each n As Double In buf
            Y = location.Y - (n - Min) * Y_ScaleFactor

            If Y > Y_avg Then '小于平均值，则Y颠倒过来
                Dim pt As New Point(X, Y_avg)
                Region = New Rectangle(pt, New Size(X_ScaleFactor, Y - Y_avg))
                pt = New Point(X + 0.5 * X_ScaleFactor, Region.Bottom)
                Call Gr.Gr_Device.DrawLine(LinePen, prePt, pt)
                prePt = pt
            Else
                Dim pt As New Point(X, Y)
                Region = New Rectangle(pt, New Size(X_ScaleFactor, Y_avg - Y))
                pt = New Point(X + 0.5 * X_ScaleFactor, Region.Top)
                Call Gr.Gr_Device.DrawLine(LinePen, prePt, pt)
                prePt = pt
            End If

            'Call Gr.Gr_Device.FillRectangle(PlotBrush, Region)

            X += X_ScaleFactor
        Next

        Return Gr.ImageResource
    End Function

    ''' <summary>
    ''' 将GC偏移曲线绘制到目标比对图形<paramref name="source"></paramref>之上
    ''' </summary>
    ''' <param name="source"></param> 
    ''' <param name="nt"></param>
    ''' <param name="Location">坐标轴原点的位置</param>
    ''' <param name="Width">坐标轴纵轴的宽度</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    <ExportAPI("GCSkew.Drawing")>
    Public Function InvokeDrawing(source As Image, nt As FASTA.FastaToken, Location As Point, Width As Integer) As Image
        ' 绘制gc偏移曲线
        Dim Skew As Double() = NucleotideModels.GCSkew(nt, WindowSize, Steps, True)
        Return InvokeDrawingCurve(source,
                                  buf:=Skew,
                                  location:=Location,
                                  size:=New Size(Width, PlotsHeight),
                                  loci:=__getLoci(nt))
    End Function
End Module
