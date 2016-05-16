Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel.Loci
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Imaging

Public MustInherit Class CurvesModel

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

    Public Function Draw(source As Image, buf As Double(), location As Point, size As Size, loci As Location) As Image
        Using g As IGraphics = source.GdiFromImage
            Call Draw(g, buf, location, size, loci)
        End Using

        Return source
    End Function

    Protected MustOverride Sub Draw(source As IGraphics, buf As Double(), location As Point, size As Size, loci As Location)
End Class
