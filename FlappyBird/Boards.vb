Public Class Scoreboardmoving
    Private Height As Integer
    Private Width As Integer
    Private CurrentLC As Integer
    Private Speed As Integer
    Private Score As Integer
    Private CurrentScore As Double
    Private Middle As Integer
    Private BiggestScore As Integer
    Private FinBoard, FinScore As Boolean
    Private IsPress As Boolean
    Private Const MaxHeight As Integer = SCOREBOARD_MAXHEIGHT
    Public ReadOnly Property ReadyPress As Boolean
        Get
            Return Me.FinScore And Me.FinBoard
        End Get
    End Property

    Public Sub New(nHeight As Integer, nWidth As Integer, _BiggestScore As Integer, Optional nScore As Integer = 0, Optional nSpeed As Integer = BESTSPEED * 3)
        With Me
            .Height = nHeight
            .Width = nWidth
            .CurrentLC = nHeight
            .Speed = nSpeed
            .Score = nScore
            .CurrentScore = 0
            .Middle = Me.Width / 2 - SCOREBOARD_WIDTH / 2
            .FinBoard = False
            .FinScore = False
            .IsPress = False
            .BiggestScore = _BiggestScore
        End With
    End Sub

    Public Sub SetScore(nScore As Integer)
        Me.Score = nScore
    End Sub

    Public Function ReloadBoard() As Boolean
        If Me.CurrentLC <= MaxHeight Then
            Me.FinBoard = True
            Return Me.ReloadScore()
        End If
        Me.CurrentLC -= Speed
        Return False
    End Function

    Public Sub DrawBoard(objBoardG As Graphics)
        objBoardG.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        Dim TextX As Integer = Me.Width / 2 - TEXT_GAMEOVER_WIDTH / 2
        Dim TextY As Integer = TEXT_HEIGHT

        objBoardG.DrawImage(textGameOver, TextX, TextY, TEXT_GAMEOVER_WIDTH, TEXT_GAMEOVER_HEIGHT)
        objBoardG.DrawImage(ScoreBoard, Middle, Me.CurrentLC, SCOREBOARD_WIDTH, SCOREBOARD_HEIGHT)
        If Me.FinBoard Then
            Me.DrawScore(objBoardG)
            If Me.FinScore Then
                Me.DrawButton(objBoardG)
            End If
        End If
    End Sub

    Public Sub MouseDown(nX As Integer, nY As Integer)
        If Me.Clicked(nX, nY) Then
            IsPress = True
        End If
    End Sub

    Public Function MouseUp(nX As Integer, nY As Integer) As Boolean
        If IsPress Then
            Me.IsPress = False
            Return Me.Clicked(nX, nY)
        End If
        Return False
    End Function

    Private Function Clicked(nX As Integer, nY As Integer) As Boolean
        Dim BtnX As Integer = Me.Width / 2 - BTN_BIG_WIDTH / 2
        Dim BtnY As Integer = Scoreboardmoving.MaxHeight + SCOREBOARD_HEIGHT + 20
        Dim BtnEndX As Integer = BtnX + BTN_BIG_WIDTH
        Dim BtnEndY As Integer = BtnY + BTN_BIG_HEIGHT
        Return nX > BtnX And nY > BtnY And nX < BtnEndX And nY < BtnEndY
    End Function

    Private Function ReloadScore() As Boolean
        Dim IsFinished As Boolean = Me.CurrentScore >= Me.Score
        If IsFinished Then
            Me.FinScore = True
            Return True
        Else
            Me.CurrentScore += 0.3
            Return False
        End If
    End Function

    Private Sub DrawButton(objButtonG As Graphics)
        Dim ButtonX As Integer = Me.Width / 2 - BTN_BIG_WIDTH / 2
        Dim ButtonY As Integer = Scoreboardmoving.MaxHeight + SCOREBOARD_HEIGHT + 20 + IIf(IsPress, 5, 0)

        objButtonG.DrawImage(btnStart, ButtonX, ButtonY, BTN_BIG_WIDTH, BTN_BIG_HEIGHT)
    End Sub

    Private Sub DrawScore(objScoreG As Graphics)
        Dim sScores As String = Fix(Me.CurrentScore).ToString
        Dim sScore2 As String = Math.Max(Me.BiggestScore, Me.Score).ToString
        Dim EachScore(Len(sScores) - 1) As Byte
        Dim EachScore2(Len(sScore2) - 1) As Byte
        Dim i As Integer
        Dim TextX As Integer = Me.Width / 2 - TEXT_GAMEOVER_WIDTH / 2
        Dim TextY As Integer = TEXT_HEIGHT

        Dim objMedal As New Bitmap(1, 1)
        Dim MedalCount As Integer = 0

        For i = 1 To Len(sScores)
            EachScore(Len(sScores) - (i)) = Val(Mid(sScores, i, 1))
        Next
        For i = 0 To EachScore.GetLength(0) - 1
            objScoreG.DrawImage(ContextScore(EachScore(i)), SCOREBOARD_SCORE_X - (i + 1) * (SCORE_CONTEXT_WIDTH) - i * SCOREBOARD_SCORE_BETWEEN, SCOREBOARD_SCORE_Y, SCORE_CONTEXT_WIDTH, SCORE_CONTEXT_HEIGHT)
        Next
        For i = 1 To Len(sScore2)
            EachScore2(Len(sScore2) - (i)) = Val(Mid(sScore2, i, 1))
        Next
        For i = 0 To EachScore2.GetLength(0) - 1
            objScoreG.DrawImage(ContextScore(EachScore2(i)), SCOREBOARD_SCORE_X - (i + 1) * (SCORE_CONTEXT_WIDTH) - i * SCOREBOARD_SCORE_BETWEEN, SCOREBOARD_SCORE_Y + 42, SCORE_CONTEXT_WIDTH, SCORE_CONTEXT_HEIGHT)
        Next

        If Not Me.FinScore Then
            Return
        End If
        If Me.Score > BiggestScore Then
            objScoreG.DrawImage(textNew, Middle + 140, Me.CurrentLC + 60)
        End If
        Select Case Me.Score
            Case Is >= 40
                MedalCount = 3
            Case Is >= 30
                MedalCount = 2
            Case Is >= 20
                MedalCount = 1
            Case Is >= 10
                MedalCount = 0
            Case Else
                Return
        End Select
        objMedal = Medals(MedalCount)
        objScoreG.DrawImage(objMedal, MEDAL_X + Me.Middle, MEDAL_Y + MaxHeight, MEDAL_LENTH, MEDAL_LENTH)
    End Sub
End Class

Public Class MainForm
    Dim Middle As Integer
    Dim LandLC As Integer
    Dim Count As Double
    Dim Counter As Double
    Dim btnX As Integer
    Dim btnY As Integer
    Dim DidClicked As Boolean

    Public Sub New(nWidth As Integer)
        With Me
            .Middle = nWidth / 2
            .LandLC = 0
            .Count = 0
            .Counter = 0.8
            .btnX = Middle - BTN_BIG_WIDTH / 2 - WIDTHDIS / 2
            .btnY = 250
        End With
    End Sub

    Public Sub Reload()
        Me.LandLC -= 4
        If Me.LandLC <= -BGWIDTH Then
            Me.LandLC += BGWIDTH
        End If
        Me.Count += Me.Counter
        If Math.Abs(Me.Count) >= 10 Then
            Me.Counter = -Me.Counter
        End If
    End Sub

    Public Sub DrawForm(objG As Graphics)
        Dim i As Integer
        Dim TextY As Integer = Middle - TEXT_TITLE_WIDTH / 2 - WIDTHDIS / 2
        Dim BirdX As Integer = Middle - BIRD_LENTH / 2 - WIDTHDIS / 2
        Dim BirdY As Integer = (BGHEIGHT - LAND_HEIGHT) / 2 - HEIGHTDIS + Fix(Me.Count)
        objG.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        For i = 0 To Fix((BGWIDTH + (-Me.LandLC)) / BGWIDTH) + 1
            objG.DrawImage(Land, New Point(Me.LandLC + i * BGWIDTH, LAND_LINE))
        Next
        objG.DrawImage(textTitle, TEXT_HEIGHT, TextY, TEXT_TITLE_WIDTH, TEXT_TITLE_HEIGHT)
        objG.DrawImage(BirdImages(2, 1), BirdX, BirdY, BIRD_LENTH, BIRD_LENTH)
        objG.DrawImage(btnStart, Me.btnX, Me.btnY + IIf(Me.DidClicked, 5, 0), BTN_BIG_WIDTH, BTN_BIG_HEIGHT)
    End Sub

    Private Function Clicked(nX As Integer, nY As Integer) As Boolean
        Return nX >= btnX And nY >= btnY And nX <= btnX + BTN_BIG_WIDTH And nY <= btnY + BTN_BIG_HEIGHT
    End Function

    Public Sub MouseDown(nX As Integer, nY As Integer)
        If Clicked(nX, nY) Then
            Me.DidClicked = True
        End If
    End Sub

    Public Function MouseUp(nX As Integer, nY As Integer) As Boolean
        If Me.DidClicked And Me.Clicked(nX, nY) Then
            Me.DidClicked = False
            Return True
        End If
        Me.DidClicked = False
        Return False
    End Function
End Class


