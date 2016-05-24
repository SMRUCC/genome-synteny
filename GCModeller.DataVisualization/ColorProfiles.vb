Imports System.Drawing
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic

Public Class ColorProfiles

    Dim _DefaultColor As Color

    Public ReadOnly Property ColorProfiles As Dictionary(Of String, Color)

    Default Public ReadOnly Property Color(Name As String) As Color
        Get
            If _ColorProfiles.ContainsKey(Name) Then
                Return _ColorProfiles(Name)
            Else
                Return _DefaultColor
            End If
        End Get
    End Property

    Sub New(ColorProfiles As IEnumerable(Of String), Optional DefaultColor As Color = Nothing)
        _ColorProfiles = GenerateColorProfiles(ColorProfiles:=ColorProfiles.ToArray)
        _DefaultColor = DefaultColor

        If _DefaultColor = Nothing Then _DefaultColor = System.Drawing.Color.Black
    End Sub

    Public Overrides Function ToString() As String
        Return $"{_ColorProfiles.Count} color(s) in the rendering profile, default color is ""{_DefaultColor.ToString}"""
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ColorProfiles"></param>
    ''' <param name="RemoveUsed">是否移除已经使用过的元素，这样子就会产生不重复的颜色</param>
    ''' <returns></returns>
    Public Shared Function GenerateColorProfiles(ColorProfiles As String(), Optional RemoveUsed As Boolean = True) As Dictionary(Of String, Color)
        Dim _ColorProfiles As Dictionary(Of String, Color) = New Dictionary(Of String, Color)

        Dim Rs = 255.Sequence.Randomize.ToList
        Dim Gs = 255.Sequence.Randomize.ToList
        Dim Bs = 255.Sequence.Randomize.ToList

        For Each Color As String In (From s As String In ColorProfiles Where Not String.IsNullOrEmpty(s) Select s).ToArray
            Dim R, G, B As Integer

            Call VBMath.Randomize() : R = RandomDouble() * (Rs.Count - 1)
            Call VBMath.Randomize() : G = RandomDouble() * (Gs.Count - 1)
            Call VBMath.Randomize() : B = RandomDouble() * (Bs.Count - 1)

            Call _ColorProfiles.Add(Color, System.Drawing.Color.FromArgb(Rs(R), Gs(G), Bs(B)))

            If RemoveUsed Then
                Call Rs.RemoveAt(R)
                Call Gs.RemoveAt(G)
                Call Bs.RemoveAt(B)
            End If
        Next

        Return _ColorProfiles
    End Function
End Class
