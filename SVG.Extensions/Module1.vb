﻿#Region "Microsoft.VisualBasic::f77c275eb68b78252408b73941d6ddd4, ..\GCModeller\visualize\SVG.Extensions\Module1.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Module Module1

    Sub Main()



        Dim doc As New SVG.SvgDocument

        Dim box As New SVG.SvgRectangle() With {.X = New SvgUnit(100), .Y = New SvgUnit(100), .Width = New SvgUnit(100), .Height = New SvgUnit(100)}

        Call doc.Children.Add(box)


        Call doc.Write("x:\sfsdf.svg")

    End Sub

End Module

