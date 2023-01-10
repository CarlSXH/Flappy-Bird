Module ControlsGet
    Public Sub GetScoreNumbers(objScoreG As Graphics, Score As Integer, Width As Integer)
        Dim ScoreInstring As String = Score.ToString
        Dim Scores As New List(Of Integer)
        Dim MiddlePoint As Integer
        Dim StartPoint As Integer
        For i = 1 To Len(ScoreInstring)
            Dim OneScore As Integer = Val(Mid(ScoreInstring, i, 1))
            Scores.Add(OneScore)
        Next
        'StartPoint = New Point(MiddlePoint.Y - SCORE_WIDTH * ScoreImages.Count / 2, MiddlePoint.X - SCORE_HEIGHT / 2)

        MiddlePoint = Width / 2
        StartPoint = MiddlePoint - ((Scores.Count * SCORE_WIDTH + (Scores.Count - 1) * SCORE_EACH_DIS) / 2) - 10

        For i = 0 To Scores.Count - 1
            Dim CurrentImage As Image = ScoreImage(Scores(i))
            objScoreG.DrawImage(CurrentImage, StartPoint + i * SCORE_WIDTH + i * SCORE_EACH_DIS, TEXT_HEIGHT, SCORE_WIDTH, SCORE_HEIGHT)
        Next
    End Sub


End Module
