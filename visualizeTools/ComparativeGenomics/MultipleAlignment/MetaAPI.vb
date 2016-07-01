﻿Imports System.Runtime.CompilerServices
Imports LANS.SystemsBiology.Assembly.KEGG.DBGET.bGetObject.Organism
Imports LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat
Imports LANS.SystemsBiology.NCBI.Extensions.Analysis
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.UnixBash

Namespace ComparativeAlignment

    Public Module MetaAPI

        <Extension>
        Public Function CompilePTT(source As BestHit, DIR As String) As Dictionary(Of String, PTT)
            Dim lst As IEnumerable(Of String) = ls - l - r - wildcards("*.PTT") <= DIR
            Dim allSp As String() = LinqAPI.Exec(Of String) <= From prot As HitCollection
                                                               In source.hits
                                                               Select From hit As Hit
                                                                      In prot.Hits
                                                                      Select hit.tag
            Dim PTTHash As Dictionary(Of String, String) = lst.ToDictionary(Function(x) x.BaseName.ToLower.Trim)
            Dim names As Dictionary(Of String, String) =
                LinqAPI.BuildHash(Of String, String, TagValue(Of String))(Function(x) x.tag, Function(x) x.Value) <=
                    From name As String
                    In allSp.Distinct
                    Let sp = EntryAPI.GetValue(name)
                    Where Not sp Is Nothing
                    Select New TagValue(Of String)(name, sp.Species.ToLower.Trim)

            Dim files = (From x In names
                         Where PTTHash.ContainsKey(x.Value)
                         Select x.Key,
                             path = PTTHash(x.Value)).ToDictionary(Function(x) x.Key,
                                                                   Function(x) PTT.Load(x.path))
            Return files
        End Function

        <Extension>
        Public Function FromMetaData(source As BestHit, PTT_DIR As String) As DrawingModel
            Dim res As Dictionary(Of String, PTT) = source.CompilePTT(PTT_DIR)
            Dim result As DrawingModel = source.FromMetaData(res)
            Return result
        End Function

        <Extension>
        Public Function FromMetaData(source As BestHit, PTT As Dictionary(Of String, PTT)) As DrawingModel

        End Function
    End Module
End Namespace