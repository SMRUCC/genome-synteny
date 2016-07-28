﻿#Region "Microsoft.VisualBasic::4c2af86aebaf5e532fe73267faecc9da, ..\GCModeller\visualize\Synteny\Program.vb"

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

Imports System
Imports System.Drawing
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization
Imports SMRUCC.genomics.Visualize
Imports SMRUCC.genomics.Visualize.SyntenyVisualize

Module Program

    Sub New()
        VBDebugger.Mute = True
    End Sub

    Public Function Main() As Integer

        Dim list As Double() = "G:\5.14.circos\03.ZIKV_45_2015_updated_mafft_named.GCSkew.txt".ReadVector
        Dim img As New Bitmap(3000, 1000)
        Dim res = GCSkew.InvokeDrawingCurve(img, list, New Point(200, 850), New Size(2500, 800), GraphicTypes.Histogram)

        Call res.SaveAs("x:\dddd.png", ImageFormats.Png)

        Return GetType(Program).RunCLI(App.CommandLine, executeFile:=AddressOf ExecuteFile)
    End Function

    Private Function ExecuteFile(file As String, args As CommandLine) As Integer
        Dim model As DrawingModel = ModelAPI.GetDrawsModel(file)
        Dim res As Drawing.Image = model.Visualize
        Dim png As String = file.TrimSuffix & ".png"
        Return res.SaveAs(png, ImageFormats.Png).CLICode
    End Function
End Module

