Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ComponentModel.DataStructures

Namespace ChromosomeMap.DrawingModels

    Public Module RenderingColor

        Public Function ApplyingCOGCategoryColor(Of T As I_COGEntry)(MyvaCog As T(),
                                                 Chromesome As DrawingModels.ChromesomeDrawingModel) As DrawingModels.ChromesomeDrawingModel

            Dim ColorProfiles = InternalInitialize_COGColors(Nothing).ToDictionary(Function(obj) obj.Key, elementSelector:=Function(obj) CType(New SolidBrush(obj.Value), Brush))
            Dim DefaultCogColor As SolidBrush = New SolidBrush(Chromesome.DrawingConfigurations.NoneCogColor)

            For Each GeneObject In Chromesome.GeneObjects
                Dim Cog = MyvaCog.GetItem(GeneObject.LocusTag)
                If Not Cog Is Nothing Then
                    If Not String.IsNullOrEmpty(Cog.Category) Then
                        If Cog.Category.Count > 1 Then
                            GeneObject.Color = New SolidBrush(NeutralizeColor((From c As Char In Cog.Category Select DirectCast(ColorProfiles(c.ToString), SolidBrush).Color).ToArray))
                        Else
                            GeneObject.Color = ColorProfiles(Cog.Category)
                        End If
                    Else
                        GeneObject.Color = DefaultCogColor
                    End If
                Else
                    GeneObject.Color = DefaultCogColor
                End If
            Next

            Call Chromesome.MyvaCogColorProfile.InvokeSet(ColorProfiles)

            Return Chromesome
        End Function

        Public Function ApplyingCOGNumberColors(Of T As I_COGEntry)(MyvaCog As T(),
                                                Chromesome As DrawingModels.ChromesomeDrawingModel) As ChromesomeDrawingModel
            Dim ColorProfiles = InternalInitialize_COGColors((From cogAlign In MyvaCog
                                                              Select cogAlign.COG
                                                              Distinct).ToArray).ToDictionary(Function(obj) obj.Key, elementSelector:=Function(obj) DirectCast(New SolidBrush(obj.Value), Brush))
            Dim DefaultCogColor = New SolidBrush(Chromesome.DrawingConfigurations.NoneCogColor)

            For Each GeneObject In Chromesome.GeneObjects
                Dim Cog = MyvaCog.GetItem(GeneObject.LocusTag)
                If Not Cog Is Nothing Then
                    If Not String.IsNullOrEmpty(Cog.COG) Then
                        GeneObject.Color = ColorProfiles(Cog.COG)
                    Else
                        GeneObject.Color = DefaultCogColor
                    End If
                Else
                    GeneObject.Color = DefaultCogColor
                End If
            Next

            Call Chromesome.MyvaCogColorProfile.InvokeSet(ColorProfiles)

            Return Chromesome
        End Function

        Private Function NeutralizeColor(data As Color()) As Color
            Dim _r = (From item In data Select item.R).ToArray.Average(Function(n As Byte) CType(n, Integer))
            Dim _g = (From item In data Select item.G).ToArray.Average(Function(n As Byte) CType(n, Integer))
            Dim _b = (From item In data Select item.B).ToArray.Average(Function(n As Byte) CType(n, Integer))
            Return Color.FromArgb(data.First.A, _r, _g, _b)
        End Function

        ''' <summary>
        ''' 材质映射
        ''' </summary>
        ''' <param name="categories"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InternalInitialize_COGColors(categories As String(), Textures As Image()) As Dictionary(Of String, Brush)
            If categories.IsNullOrEmpty Then
                categories = (From Category
                    In LANS.SystemsBiology.Assembly.NCBI.COG.Function.Default.Categories
                              Select (From [Class] In Category.SubClasses Select [Class].Key).ToArray).ToArray.MatrixToList.Distinct.ToArray
            End If

            Dim mapping = If(categories.Count > Textures.Count,
                InterpolateMapping(categories, Textures), ' 材质不足，则会使用颜色来绘制
                DirectlyMapping(categories, Textures))    ' 直接映射

            Return mapping
        End Function

        ''' <summary>
        ''' 材质不足，则会使用颜色来绘制
        ''' </summary>
        ''' <returns></returns>
        Private Function InterpolateMapping(categories As String(), Textures As Image()) As Dictionary(Of String, Brush)
            Dim TempChunk As String() = New String(Textures.Count - 1) {}
            Dim DictData As Dictionary(Of String, Brush) = New Dictionary(Of String, Brush)

            Call Array.ConstrainedCopy(categories, 0, TempChunk, 0, TempChunk.Length)

            For i As Integer = 0 To TempChunk.Count - 1
                Call DictData.Add(TempChunk(i), New TextureBrush(Textures(i)))
            Next

            '剩余的使用颜色
            Dim ColorList As List(Of Color) = AllDotNetPrefixColors.ToList
            categories = categories.Skip(TempChunk.Count).ToArray
            Dim ChunkBuffer = categories.CreateSlideWindows(Textures.Count, Textures.Count)
            Dim J As Integer = 0

            Do While True
                For Each CatList In ChunkBuffer
                    Dim Color = ColorList(J)

                    For i As Integer = 0 To CatList.Elements.Count - 1
                        Dim res = TextureResourceLoader.AdjustColor(Textures(i), Color)
                        Call DictData.Add(CatList(i), New TextureBrush(res))
                    Next

                    J += 1
                Next
            Loop

            Return DictData
        End Function

        ''' <summary>
        ''' 直接映射
        ''' </summary>
        ''' <param name="categories"></param>
        ''' <returns></returns>
        Private Function DirectlyMapping(categories As String(), Textures As Image()) As Dictionary(Of String, Brush)
            Dim DictData As Dictionary(Of String, Brush) = New Dictionary(Of String, Brush)

            For i As Integer = 0 To categories.Count - 1
                Call DictData.Add(categories(i), New TextureBrush(Textures(i)))
            Next

            Return DictData
        End Function

        ''' <summary>
        ''' 这是一个很通用的颜色谱创建函数
        ''' </summary>
        ''' <param name="categories">当不为空的时候，会返回一个列表，其中空字符串会被排除掉，故而在返回值之中需要自己添加一个空值的默认颜色</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InternalInitialize_COGColors(categories As String()) As Dictionary(Of String, System.Drawing.Color)
            If Not categories.IsNullOrEmpty Then
                Return ComponentModel.ColorProfiles.GenerateColorProfiles(categories)
            End If

            Dim CogCategory = LANS.SystemsBiology.Assembly.NCBI.COG.Function.Default
            Dim f = 255 / CogCategory.Categories.Count
            Dim ColorProfile = New Dictionary(Of String, Color)

            Dim R = f
            For Each cata In CogCategory.Categories
                Dim f2 = 255 / cata.SubClasses.Count
                Dim G = f2
                For Each [class] In cata.SubClasses
                    Call ColorProfile.Add([class].Key, Color.FromArgb(220, R, G, 255 * RandomDouble()))
                    G += f2
                Next

                R += f
            Next

            Return ColorProfile
        End Function
    End Module
End Namespace