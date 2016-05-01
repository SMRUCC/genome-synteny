Imports System
Imports System.Drawing
Imports LANS.SystemsBiology.GCModeller.DataVisualization.SyntenyVisual
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.DocumentFormat.HTML
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization

Module Program

    Sub New()
        VBDebugger.Mute = True
    End Sub

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine, executeFile:=AddressOf ExecuteFile)
    End Function

    Private Function ExecuteFile(file As String, args As CommandLine) As Integer
        Dim model As DrawingModel = ModelAPI.GetDrawsModel(file)
        Dim res As Drawing.Image = model.Visualize
        Dim png As String = file.TrimFileExt & ".png"
        Return res.SaveAs(png, ImageFormats.Png).CLICode
    End Function
End Module
