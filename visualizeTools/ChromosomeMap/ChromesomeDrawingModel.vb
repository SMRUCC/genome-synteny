﻿Imports System.Drawing

Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.CommandLine

Imports Oracle.Java.IO.Properties.Reflector
Imports System.Text
Imports Microsoft.VisualBasic.Extensions
Imports Microsoft.VisualBasic.DocumentFormat.Csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv.Extensions
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.CsvExports
Imports LANS.SystemsBiology.SequenceModel.NucleotideModels

Namespace ChromosomeMap.DrawingModels

    ''' <summary>
    ''' Data model for described a chromosome drawing action invoked.(用于描述如何绘制一个基因组的图形数据的数据模型)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChromesomeDrawingModel : Inherits LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat.Rpt

        ''' <summary>
        ''' 所需要进行绘制的基因组之中的基因对象，整个基因组之中的基本框架
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GeneObjects As DrawingModels.SegmentObject()
        ''' <summary>
        ''' 基因的突变点的数据
        ''' </summary>
        ''' <returns></returns>
        Public Property MutationDatas As DrawingModels.MultationPointData()
        ''' <summary>
        ''' 绘图设备的配置数据
        ''' </summary>
        ''' <returns></returns>
        Public Property DrawingConfigurations As Conf
        ''' <summary>
        ''' 转录调控位点
        ''' </summary>
        ''' <returns></returns>
        Public Property MotifSites As DrawingModels.MotifSite()
        ''' <summary>
        ''' 基因的转录起始位点
        ''' </summary>
        ''' <returns></returns>
        Public Property TSSs As DrawingModels.TSSs()
        Public Property Loci As DrawingModels.Loci()

        ''' <summary>
        ''' COG分类的颜色配置
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MyvaCogColorProfile As Dictionary(Of String, Brush)
        ''' <summary>
        ''' Motif位点的颜色配置
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotifSiteColorProfile As Dictionary(Of String, Color)

        Public Overrides Function ToString() As String
            Dim sBuilder As StringBuilder = New StringBuilder
            Call sBuilder.AppendLine("Drawing Model Brief Information:" & vbCrLf)
            Call sBuilder.AppendLine($"GeneObject Segments:  {GeneObjects.GetElementCounts}")
            Call sBuilder.AppendLine($"Mutation Sites Data:  {MutationDatas.GetElementCounts}")
            Call sBuilder.AppendLine($"Motif Sites:          {MotifSites.GetElementCounts}")
            Call sBuilder.AppendLine($"Loci Sites:           {Loci.GetElementCounts}")
            Call sBuilder.AppendLine($"TSSs Sites:          {TSSs.GetElementCounts}")

            Return sBuilder.ToString
        End Function
    End Class

    <[Namespace]("PlasmidAnnotations")>
    Public Class PlasmidAnnotation : Implements IGeneBrief

        ''' <summary>
        ''' 基因号
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ORF_ID As String Implements sIdEnumerable.Identifier
        Public Property Strand As String
        <Column("Gene-Length")> Public Property Length As Integer Implements ICOGDigest.Length
        ''' <summary>
        ''' 基因核酸序列
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Column("Gene")> Public Property GeneNA As String
        ''' <summary>
        ''' 蛋白质序列
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Protein As String
        Public Property ST As Integer
        Public Property SP As Integer
        ''' <summary>
        ''' 简写的基因名
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Gene_name As String
        ''' <summary>
        ''' 蛋白质功能注释
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property product As String Implements ICOGDigest.Product
        <Column("GO-I")> Public Property GOI As String
        <Column("GO-ID")> Public Property GO_ID As String
        Public Property GO_name As String
        Public Property GO_discription As String
        <Column("Protein family membership")> Public Property Family As String
        <Column("Protein family membership discription")> Public Property discription As String
        <Column("COG-NO.")> Public Property COG_NO As String Implements ICOGDigest.COG
        <Column("COG-cat")> Public Property COG_cat As String
        <Column("COG-annotation")> Public Property COG_annotation As String
        Public Property Identity As String
        <Column("qst.")> Public Property qst As String
        <Column("qsp.")> Public Property qsp As String
        <Column("sst.")> Public Property sst As String
        <Column("ssp.")> Public Property ssp As String
        Public Property subject_length As String
        <Column("E-value")> Public Property Evalue As String
        Public Property Protein_len As String

        Public Property Location As LANS.SystemsBiology.ComponentModel.Loci.NucleotideLocation Implements IGeneBrief.Location
            Get
                Return New LANS.SystemsBiology.ComponentModel.Loci.NucleotideLocation(ST, SP, Strand)
            End Get
            Set(value As LANS.SystemsBiology.ComponentModel.Loci.NucleotideLocation)
                If Not value Is Nothing Then
                    ST = value.Left
                    SP = value.Right
                End If
            End Set
        End Property

        ''' <summary>
        ''' 导出蛋白质的序列信息
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        <ExportAPI("Export.Orf")>
        Public Shared Function ExportProteinFasta(data As IEnumerable(Of PlasmidAnnotation),
                                                  <Parameter("Id.Only")>
                                                  Optional JustId As Boolean = False) As SequenceModel.FASTA.FastaFile
            Dim GetTitle As Func(Of PlasmidAnnotation, String()) = JustId.[If](
                Function(Gene As PlasmidAnnotation) {Gene.ORF_ID},
                Function(Gene As PlasmidAnnotation) New String() {Gene.ORF_ID, Gene.Gene_name, Gene.product})

            Dim LQuery = (From PlasmidGene As PlasmidAnnotation
                          In data
                          Where Not String.IsNullOrEmpty(PlasmidGene.Protein)
                          Select New SequenceModel.FASTA.FastaToken With {
                              .Attributes = GetTitle(PlasmidGene),
                              .SequenceData = PlasmidGene.Protein}).ToArray
            Return CType(LQuery, SequenceModel.FASTA.FastaFile)
        End Function

        <ExportAPI("read.csv.plasmid_data")>
        Public Shared Function READ_PlasmidData(path As String) As PlasmidAnnotation()
            Return path.LoadCsv(Of PlasmidAnnotation)(False).ToArray
        End Function

        <ExportAPI("export.as_ncbi_annotation")>
        Public Shared Function ExportAnnotations(data As Generic.IEnumerable(Of PlasmidAnnotation), Optional saveto As String = "") As GeneDumpInfo()
            Dim LQuery = (From item As PlasmidAnnotation In data
                          Where Not String.IsNullOrEmpty(item.Protein)
                          Let gc As Double = GC_Content(New NucleicAcid(item.GeneNA).ToArray)
                          Select New GeneDumpInfo With {
                              .CDS = item.GeneNA,
                              .GC_Content = gc,
                              .COG = item.COG_NO,
                              .Strand = item.Strand,
                              .CommonName = item.product,
                              .GeneName = item.Gene_name,
                              .LocusID = item.ORF_ID,
                              .Translation = item.Protein,
                              .Left = item.Location.Left,
                              .Right = item.Location.Right}).ToArray
            If Not String.IsNullOrEmpty(saveto) Then
                Call LQuery.SaveTo(saveto, False)
            End If

            Return LQuery
        End Function
    End Class
End Namespace