Imports System.Drawing
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization

Public Class DeviceModel : Inherits ClassObject

    Public Property Margin As Size
    Public Property Size As Size
    Public Property Meta As String
    Public Property penWidth As Integer

    ''' <summary>
    ''' {基因组的名称, PTT的文件路径}
    ''' </summary>
    ''' <returns></returns>
    Public Property PTT As Dictionary(Of String, String)
    Public Property Orders As String()
    Public Property Titles As String

    Public Function GetTitles(DIR As Directory) As Title()
        Dim path As String

        If String.IsNullOrEmpty(Titles) Then
            path = DIR.GetFullPath("./titles.Csv")
        Else
            path = DIR.GetFullPath(Titles)
        End If

        If Not path.FileExists Then
            Return Nothing
        Else
            Return path.LoadCsv(Of Title)
        End If
    End Function

    Public Shared Function Template() As DeviceModel
        Return New DeviceModel With {
            .Size = New Size(1920, 1600),
            .Meta = "./bbh.Xml",
            .PTT = New Dictionary(Of String, String) From {
                {"xcb", "./Xanthomonas_campestris_8004_uid15.PTT"},
                {"xor", "./Xanthomonas_oryzae_oryzicola_BLS256_uid16740.PTT"}
            },
            .Margin = New Size(25, 25),
            .Orders = {"xcb", "xor"},
            .penWidth = 3
        }
    End Function
End Class

Public Class Title
    Public Property key As String
    ''' <summary>
    ''' 用来控制标题格式的html文本
    ''' </summary>
    ''' <returns></returns>
    Public Property Title As String

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class