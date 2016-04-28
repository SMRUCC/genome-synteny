Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging

Module Program

    Sub Main()
        Dim gdi As GDIPlusDeviceHandle = New Size(1200, 800).CreateGDIDevice

        Dim pol As New Polyline(New Point(100, 10), New Point(600, 120), Color.Red)
        Call pol.Draw(gdi, 5)
        Call gdi.Save("./testPol.png", ImageFormats.Png)
    End Sub
End Module
