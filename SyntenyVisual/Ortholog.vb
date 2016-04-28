Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization

''' <summary>
''' 直系同源的绘图模型
''' </summary>
Public MustInherit Class Line

    Public ReadOnly Property From As Point
    Public ReadOnly Property [To] As Point
    Public ReadOnly Property Color As Color

    Sub New(from As Point, toPoint As Point, cl As Color)
        Me.From = from
        Me.To = toPoint
        Me.Color = cl
    End Sub

    Public MustOverride Sub Draw(ByRef gdi As GDIPlusDeviceHandle, width As Integer)

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class

''' <summary>
''' 
''' </summary>
''' <remarks>
''' 这个绘图模型最简单
''' </remarks>
Public Class StraightLine : Inherits Line

    Sub New(from As Point, toPoint As Point, cl As Color)
        Call MyBase.New(from, toPoint, cl)
    End Sub

    Public Overrides Sub Draw(ByRef gdi As GDIPlusDeviceHandle, width As Integer)
        Call gdi.DrawLine(New Pen(Color, width), From, [To])
    End Sub
End Class

''' <summary>
''' 
''' </summary>
''' <remarks>
'''    +
'''    |
'''    |
'''    \
'''     \
'''      \
'''       \
'''        \
'''         \
'''         |
'''         |
'''         +
''' </remarks>
Public Class Polyline : Inherits Line

    ''' <summary>
    ''' 出现转折的长度的百分比
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Turnp As Double

    ''' <summary>
    ''' 在高度的多少百分比处开始转折？
    ''' </summary>
    ''' <param name="p"></param>
    Sub New(from As Point, toPoint As Point, cl As Color, Optional p As Double = 0.2)
        Call MyBase.New(from, toPoint, cl)
        Turnp = p
    End Sub

    Public Overrides Sub Draw(ByRef gdi As GDIPlusDeviceHandle, width As Integer)
        Dim height As Integer = [To].Y - From.Y
        Dim t As Integer = height * Turnp
        Dim t1 As New Point(From.X, From.Y + t)
        Dim t2 As New Point([To].X, [To].Y - t)
        Dim pen As New Pen(Color, width)

        ' 绘图是从上到下进行的
        Call gdi.DrawLine(pen, From, t1)
        Call gdi.DrawLine(pen, t1, t2)
        Call gdi.DrawLine(pen, t2, [To])
    End Sub
End Class

''' <summary>
''' 两个同源的基因之间的相连的线的样式
''' </summary>
Public Enum LineStyle
    ''' <summary>
    ''' 直线
    ''' </summary>
    Straight
    ''' <summary>
    ''' 折线
    ''' </summary>
    Polyline
    ''' <summary>
    ''' 贝塞尔曲线
    ''' </summary>
    Bezier
End Enum

''' <summary>
''' 直系同源的绘图数据模型
''' </summary>
Public Class Ortholog

End Class