Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.Analysis
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application.BBH
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Linq
Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging

Public Module ModelAPI

    Public Function GetDrawsModel(path As String) As DrawingModel
        Dim model As DeviceModel = Serialization.LoadJsonFile(Of DeviceModel)(path)
        Dim DIR As New Directory(path.ParentPath)
        Dim bbhMeta As Analysis.BestHit =
            DIR.GetFullPath(model.Meta).LoadXml(Of Analysis.BestHit)
        Dim PTT As Dictionary(Of String, PTT) =
            model.PTT.ToDictionary(Function(x) x.Key,
                                   Function(x) TabularFormat.PTT.Load(DIR.GetFullPath(x.Value)))
        Dim height As Integer = model.Size.Height - model.Margin.Height * 2
        Dim width As Integer = model.Size.Width - model.Margin.Width * 2

        height /= PTT.Count

        Dim maps As SlideWindowHandle(Of String)() = model.Orders.CreateSlideWindows(2)
        Dim bbhs As BBHIndex() =
            LinqAPI.Exec(Of BBHIndex) <= From hits As HitCollection
                                         In bbhMeta.hits
                                         Select From tag As SlideWindowHandle(Of String)
                                                In maps
                                                Let o As BBHIndex = IsOrtholog(tag.Elements.First, tag.Elements.Last, hits, bbhMeta.sp)
                                                Where Not o Is Nothing
                                                Select o
        Dim spGroups = (From x As BBHIndex
                        In bbhs
                        Let sp As String = x.Properties("query")
                        Select sp,
                            x
                        Group By sp Into Group)
        Dim h1 As Integer = model.Margin.Height
        Dim h2 As Integer = h1 + height
        Dim links As New List(Of Line)
        Dim genomes As New List(Of GenomeBrief)
        Dim i As Integer
        Dim last As PTT = Nothing

        For Each buf In spGroups
            Dim sp As String = buf.sp
            Dim hit As String = maps(i.MoveNext).Elements.Last
            Dim query As PTT = PTT(sp)
            Dim hitBrief As PTT = PTT(hit)

            links += OrthologAPI.FromBBH(
                buf.Group.ToArray(Function(x) x.x),
                query,
                hitBrief,
                Function() Color.Red,
                h1,
                h2,
                width,
                model.Margin.Width)
            genomes += New GenomeBrief With {
                .Name = query.Title,
                .Size = query.Size,
                .Y = h1
            }
            h1 += height
            h2 += height
            last = hitBrief
        Next

        genomes += New GenomeBrief With {
            .Name = last.Title,
            .Size = last.Size,
            .Y = h1
        }

        Return New DrawingModel With {
            .Links = links,
            .size = model.Size,
            .penWidth = model.penWidth,
            .briefs = genomes
        }
    End Function

    ''' <summary>
    ''' 空值表示没有同源关系
    ''' </summary>
    ''' <param name="query">基因组标识符</param>
    ''' <param name="hit">基因组标识符</param>
    ''' <param name="hits"></param>
    ''' <param name="hitsTag">基因组标识符</param>
    ''' <returns></returns>
    Public Function IsOrtholog(query As String, hit As String, hits As HitCollection, hitsTag As String) As BBHIndex
        Dim qsp As String = query

        query = hits.__getName(hitsTag, query)
        hit = hits.__getName(hitsTag, hit)

        If String.IsNullOrEmpty(query) OrElse String.IsNullOrEmpty(hit) Then
            Return Nothing
        Else
            Return New BBHIndex With {
                .QueryName = query,
                .HitName = hit,
                .Properties = New Dictionary(Of String, String) From {
                    {NameOf(query), qsp}
                }
            }
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="hits"></param>
    ''' <param name="hitsTag"></param>
    ''' <param name="query">基因组标识符</param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Private Function __getName(hits As HitCollection, hitsTag As String, query As String) As String
        If String.Equals(query, hitsTag) Then
            Return hits.QueryName
        Else
            Dim hitX As Hit = hits.GetHitByTagInfo(query)

            If hitX Is Nothing Then
                Return Nothing
            Else
                Return hitX.HitName
            End If
        End If
    End Function
End Module
