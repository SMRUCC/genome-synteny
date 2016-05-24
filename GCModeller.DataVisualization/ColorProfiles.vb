Imports System.Drawing
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic

Public Class ColorProfiles

    ReadOnly _defaultColor As Color

    Public ReadOnly Property ColorProfiles As Dictionary(Of String, Color)

    Default Public ReadOnly Property Color(Name As String) As Color
        Get
            If _ColorProfiles.ContainsKey(Name) Then
                Return _ColorProfiles(Name)
            Else
                Return _defaultColor
            End If
        End Get
    End Property

    Sub New(ColorProfiles As IEnumerable(Of String), Optional DefaultColor As Color = Nothing)
        _ColorProfiles = GenerateColorProfiles(ColorProfiles:=ColorProfiles.ToArray)
        _defaultColor = DefaultColor

        If _defaultColor = Nothing Then _defaultColor = System.Drawing.Color.Black
    End Sub

    Public Overrides Function ToString() As String
        Return String.Join(__describ, ColorProfiles.Count, _defaultColor.ToString)
    End Function

    Const __describ As String =
        "{0} color(s) in the rendering profile, default color is ""{1}"""

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ColorProfiles"></param>
    ''' <param name="removeUsed">是否移除已经使用过的元素，这样子就会产生不重复的颜色</param>
    ''' <returns></returns>
    Public Shared Function GenerateColorProfiles(ColorProfiles As String(), Optional removeUsed As Boolean = True) As Dictionary(Of String, Color)
        Dim colos As New Dictionary(Of String, Color)

        Dim Rs = 255.Sequence.Randomize.ToList
        Dim Gs = 255.Sequence.Randomize.ToList
        Dim Bs = 255.Sequence.Randomize.ToList

        For Each Color As String In From s As String In ColorProfiles Where Not String.IsNullOrEmpty(s) Select s
            Dim R, G, B As Integer

            Call VBMath.Randomize() : R = RandomDouble() * (Rs.Count - 1)
            Call VBMath.Randomize() : G = RandomDouble() * (Gs.Count - 1)
            Call VBMath.Randomize() : B = RandomDouble() * (Bs.Count - 1)

            Call colos.Add(Color, System.Drawing.Color.FromArgb(Rs(R), Gs(G), Bs(B)))

            If removeUsed Then
                Call Rs.RemoveAt(R)
                Call Gs.RemoveAt(G)
                Call Bs.RemoveAt(B)
            End If
        Next

        Return colos
    End Function
End Class
