Imports LANS.SystemsBiology.Assembly.NCBI.GenBank
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.NCBI.Extensions.Analysis

Public Module ModelAPI

    Public Function GetDrawsModel(path As String) As DrawingModel
        Dim model As DeviceModel = Serialization.LoadJsonFile(Of DeviceModel)(path)
        Dim DIR As New Directory(path.ParentPath)
        Dim bbhMeta As BestHit = DIR.GetFullPath(model.Meta).LoadXml(Of BestHit)
        Dim PTT As Dictionary(Of String, PTT) =
            model.PTT.ToDictionary(Function(x) x.Key,
                                   Function(x) TabularFormat.PTT.Load(x.Value))
        Dim height As Integer = model.Size.Height - model.Margin.Height * 2
        Dim width As Integer = model.Size.Width - model.Margin.Width * 2

        height /= PTT.Count

    End Function
End Module

Public Class DrawingModel

    Public Property Links As Line()


End Class