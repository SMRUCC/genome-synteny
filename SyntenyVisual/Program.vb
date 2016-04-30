Imports System.Drawing
Imports Microsoft.VisualBasic.DocumentFormat.HTML
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization

Module Program

    Sub Main()

        Dim html As String = "<font face=""Microsoft YaHei"" size=""5.5""><strong>text</strong><b> &lt;&lt;&lt; <i>value</i></b></font> "


        Dim strings = TextAPI.TryParse(html)
        strings = TextAPI.TryParse("1234   56789    &lt;")

        Dim gdi As GDIPlusDeviceHandle = New Size(1200, 800).CreateGDIDevice

        Dim pol As New Polyline(New Point(100, 10), New Point(600, 120), Color.Red)
        Call pol.Draw(gdi, 5)

        Dim bez As New Bézier(New Point(120, 10), New Point(200, 120), Color.Blue)
        Call bez.Draw(gdi, 5)

        bez = New Bézier(New Point(560, 10), New Point(200, 120), Color.Green)
        Call bez.Draw(gdi, 5)

        Call gdi.Save("./testPol.png", ImageFormats.Png)


        Dim model As DeviceModel = DeviceModel.Template
        Call model.GetJson.SaveTo("./test.json")

        VBDebugger.Mute = True

        Dim res = ModelAPI.GetDrawsModel("G:\Xanthomonas_campestris_8004_uid15\xcc.json", LineStyles.Straight).Visualize
        Call res.Save("G:\Xanthomonas_campestris_8004_uid15\Xanthomonadales.png")
    End Sub
End Module
