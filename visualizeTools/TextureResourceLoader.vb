﻿Imports System.Drawing
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Imaging

<PackageNamespace("Texture.Resource.Loader", Publisher:="xie.guigang@gmail.com")>
Public Module TextureResourceLoader

    ''' <summary>
    ''' 按照指定的资源图片和参数进行纹理资源的剪裁处理
    ''' </summary>
    ''' <param name="Resource"></param>
    ''' <param name="Size"></param>
    ''' <param name="IntervalWidth">纹理模块之间在水平上的间隔宽度</param>
    ''' <param name="IntervalHeight">纹理模块之间在竖直方向上的间隔宽度</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    <ExportAPI("Load.TextureResource")>
    Public Function LoadTextureResource(Resource As Image, Size As Size, IntervalWidth As Integer, IntervalHeight As Integer) As Image()
        Dim ChunkBuffer As New List(Of Image)
        Dim X As Integer = 0, Y As Integer = 0

        Do While True
            Dim resToken As Image = Resource.ImageCrop(New Point(X, Y), Size)
            Call ChunkBuffer.Add(resToken)

            X += IntervalWidth + Size.Width

            If X >= Resource.Width Then
                X = 0
                Y += Size.Height + IntervalHeight
            End If

            If Y >= Resource.Height Then
                Exit Do
            End If
        Loop

        Return ChunkBuffer.ToArray
    End Function

    <ExportAPI("Texture.Color.Adjust")>
    Public Function AdjustColor(Image As Image, Color As Color) As Image
        Dim res As Bitmap = CType(Image.Clone, Bitmap)
        Dim X, Y As Integer

        Do While True
            Dim p = res.GetPixel(X, Y)
            Dim R As Integer = (CInt(p.R) + CInt(Color.R)) / 2
            Dim G As Integer = (CInt(p.G) + CInt(Color.G)) / 2
            Dim B As Integer = (CInt(p.B) + CInt(Color.B)) / 2
            Dim A As Integer = (CInt(p.A) + CInt(Color.A)) / 2
            Call res.SetPixel(X, Y, System.Drawing.Color.FromArgb(A, R, G, B))

            X += 1

            If X >= res.Width Then
                X = 0
                Y += 1
            End If

            If Y >= res.Height Then
                Exit Do
            End If
        Loop

        Return res
    End Function

    <ExportAPI("LoadResource.InternalDefault")>
    Public Function LoadInternalDefaultResource() As Image()
        Return TextureResourceLoader.LoadTextureResource(My.Resources.DefaultTexture, New Size(27, 19), 6, 6)
    End Function
End Module
