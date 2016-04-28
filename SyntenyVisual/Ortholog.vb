﻿Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging

''' <summary>
''' 直系同源的绘图模型
''' </summary>
Public MustInherit Class Line

    Public ReadOnly Property From As Point
    Public ReadOnly Property [To] As Point
    Public ReadOnly Property Color As Color

    Public MustOverride Sub Draw(ByRef gdi As GDIPlusDeviceHandle, width As Integer)

End Class

''' <summary>
''' 
''' </summary>
''' <remarks>
''' 这个绘图模型最简单
''' </remarks>
Public Class StraightLine : Inherits Line

    Public Overrides Sub Draw(ByRef gdi As GDIPlusDeviceHandle, width As Integer)
        Call gdi.DrawLine(New Pen(Color, width), From, [To])
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