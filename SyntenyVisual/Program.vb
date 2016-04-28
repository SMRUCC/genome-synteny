Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Serialization

Module Program

    Sub Main()
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


        Call ModelAPI.GetDrawsModel("F:\GCModeller\GCI Project\DataVisualization\genome-synteny\data\xcc.json").Visualize.SaveAs("G:\Xanthomonas_campestris_8004_uid15\Xanthomonadales.png", ImageFormats.Png)
    End Sub
End Module
