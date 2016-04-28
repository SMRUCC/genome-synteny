Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.NCBI.Extensions
Imports LANS.SystemsBiology.NCBI.Extensions.Analysis
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application.BBH

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
        query = hits.__getName(hitsTag, query)
        hit = hits.__getName(hitsTag, hit)

        If String.IsNullOrEmpty(query) OrElse String.IsNullOrEmpty(hit) Then
            Return Nothing
        Else
            Return New BBHIndex With {
                .QueryName = query,
                .HitName = hit
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

Public Class DrawingModel

    Public Property Links As Line()

End Class