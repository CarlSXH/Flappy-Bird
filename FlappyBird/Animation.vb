Class Animation

    Private Structure MyPipe
        Dim nPipe As Pipe
        Dim IsPast As Boolean
    End Structure
    Private Width As Integer
    Private Height As Integer
    Private Speed As Integer
    Private LandLC As Integer
    Private MyBirdKind As BirdColor
    Private Wing() As BirdWing =
        {BirdWing.Middle, BirdWing.Middle, BirdWing.Down, BirdWing.Middle, BirdWing.Up}
    Private WingCount As Double = 0
    Private Pipes As List(Of MyPipe)
    Private BirdCurrentLC As Integer
    Private DropSpeed As Double
    Private Score As Integer
    Private Didstart As Boolean
    Private IsDead As Boolean
    Private WhiteCount As Double
    Const WhiteCounter As Double = 10

    Public Sub New(nWidth As Integer, nHeight As Integer, Optional Speed As Integer = BESTSPEED)
        SetProperty(nWidth, nHeight, Speed)
    End Sub
    Public Sub New()
        SetProperty(BGWIDTH, BGHEIGHT, BESTSPEED)
    End Sub
    Public Sub Restart()
        SetProperty(Me.Width, Me.Height, Me.Speed)
    End Sub
    Public Sub Click()
        If Me.IsDead Then
            Return
        End If
        Didstart = True
        DropSpeed = -6.5
        WingCount = 1
    End Sub
    Public Sub Reload()
        If Me.IsOnGround() Then
            WhiteCount = 0
            Return
        End If
        If WhiteCount > 0 Then
            WhiteCount -= WhiteCounter
        End If
        Me.BirdCurrentLC += Fix(DropSpeed)
        If DropSpeed <= 10 Then
            DropSpeed += 0.3
        End If
        If WingCount > 0 Then
            If WingCount < Wing.GetLength(0) - 1 Then
                WingCount += 0.1
            Else
                WingCount = 0
            End If
        End If

        If Me.IsDead Then
            Return
        End If
        Me.LandLC -= Speed
        If -Me.LandLC >= BGWIDTH Then
            Me.LandLC += BGWIDTH
        End If
        If Pipes.Count >= 1 Then
            If Pipes(0).nPipe.LCOnTopLeft <= -PIPE_LENTH Then
                Pipes.RemoveAt(0)
            End If
            For i = 0 To Pipes.Count - 1
                Dim CurrentPipe As MyPipe = Pipes(i)
                CurrentPipe.nPipe.LCOnTopLeft -= Speed
                If Not CurrentPipe.IsPast Then
                    If CurrentPipe.nPipe.LCOnTopLeft + PIPE_LENTH - BIRD_BACK <= BIRD_LC Then
                        Me.Score += 1
                        CurrentPipe.IsPast = True
                    End If
                End If
                Pipes(i) = CurrentPipe
            Next
        End If
        If Me.IsAddPipe Then
            Me.AddNewPipe()
        End If
    End Sub
    Public Function IsStart() As Boolean
        Return Me.Didstart
    End Function
    Public Function GetScore() As Integer
        Return Me.Score
    End Function
    Private Sub SetProperty(nWidth As Integer, nHeight As Integer, Speed As Integer)
        With Me
            Randomize()
            .Width = nWidth
            .Height = nHeight
            .Speed = Speed
            .LandLC = 0
            '.BoardLC = BGHEIGHT
            '.TextLC = 0
            .MyBirdKind = (Rnd() Mod 1) * 2
            .Pipes = New List(Of MyPipe)
            If .IsAddPipe Then
                .AddNewPipe()
            End If
            .BirdCurrentLC = (.Height - LAND_HEIGHT) / 2 - HEIGHTDIS
            .DropSpeed = 0
            .Score = 0
            .Didstart = False
            .IsDead = False
            .WhiteCount = 0
        End With
    End Sub
    Private Sub AddNewPipe(Optional IsMoving As Boolean = False)
        Dim NewPipe As MyPipe
        Dim nheight As Integer

        'Randomize()
        nheight = CInt(Rnd() * (LAND_LINE - PIPE_LENTH * 2 - PIPE_BETWEEN))
        With NewPipe
            .nPipe.LCOnTopLeft = Width
            .nPipe.nHeight = nheight
            .IsPast = False
        End With
        Me.Pipes.Add(NewPipe)
    End Sub
    Private Function IsAddPipe() As Boolean
        If Pipes.Count <= 0 Then
            Return True
        End If
        Dim LastPipe As MyPipe = Pipes(Pipes.Count - 1)
        Return Width - (LastPipe.nPipe.LCOnTopLeft + PIPE_LENTH) >= PIPE_EACH_BETWEEN
    End Function

    'Public Function DrawBoard(objBoardG As Graphics) As Boolean
    '    Dim Text As Image = textGameOver
    '    Dim objBoard As Image = ScoreBoard
    '    Dim textPoint As New Point(Me.Width / 2 - (Text.Width / 2), 70)
    '    Dim BoardPoint As New Point(Me.Width / 2 - (objBoard.Width / 2), Me.Height - (IIf(Me.Speed >= 0, Me.Speed, 0)))
    '    Me.Speed += 25
    '    objBoardG.DrawImage(Text, textPoint.X, textPoint.Y, Text.Width, Text.Height)
    '    objBoardG.DrawImage(objBoard, BoardPoint.X, BoardPoint.Y, objBoard.Width, objBoard.Height)
    '    Return (Me.Height - Me.Speed) <= (textPoint.Y + Text.Height + 25)
    'End Function

    Public Sub DrawEveryThing(objG As Graphics)
        'objG.RotateTransform(0)
        objG.InterpolationMode = Drawing2D.InterpolationMode.Bilinear

        objG.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

        Dim j As Boolean = True Xor True

        DrawPipes(objG)
        DrawLand(objG)
        DrawBird(objG)

        If Not Didstart Then
            Dim TextX As Integer = TEXT_HEIGHT
            Dim TextY As Integer = Me.Width / 2 - TEXT_READY_WIDTH / 2
            objG.DrawImage(textReady, TextX, TextY, TEXT_READY_WIDTH, TEXT_READY_HEIGHT)
            Return
        End If

        If Not Me.IsOnGround Then
            GetScoreNumbers(objG, Me.Score, Me.Width)
        End If

        If Me.WhiteCount > 0 Then
            objG.FillRectangle(New SolidBrush(Color.FromArgb(Me.WhiteCount, Color.White)), 0, 0, Me.Width, Me.Height)
        End If
        'objG.RotateTransform(0)
        'objG.DrawImage(GetScoreNumbers(2, Me.Width), 0, 0)
    End Sub

    Private Sub DrawPipes(objPipeG As Graphics)
        Dim i As Integer
        If Pipes.Count <= 0 Then
            Return
        End If

        For i = 0 To Pipes.Count - 1
            Dim DrawingPipe As MyPipe = Pipes(i)
            Dim UpPipePoint, DownPipePoint As Integer
            UpPipePoint = -(PIPE_MAXHEIGHT - DrawingPipe.nPipe.nHeight)
            DownPipePoint = PIPE_MAXHEIGHT - (-UpPipePoint) + PIPE_BETWEEN
            objPipeG.DrawImage(PipeGDown, DrawingPipe.nPipe.LCOnTopLeft, UpPipePoint, PIPE_LENTH, PIPE_MAXHEIGHT)
            objPipeG.DrawImage(PipeGUp, DrawingPipe.nPipe.LCOnTopLeft, DownPipePoint, PIPE_LENTH, PIPE_MAXHEIGHT)
        Next
    End Sub

    Private Sub DrawBird(objBirdG As Graphics)
        Dim Angle As Integer
        Dim birdImage As Image = BirdImages(Me.MyBirdKind, Me.Wing(Fix(Me.WingCount)))
        Dim nP As Integer = -BIRD_LENTH / 2
        Dim nX As Integer = BIRD_LC + BIRD_LENTH / 2
        Dim nY As Integer = Me.BirdCurrentLC + BIRD_LENTH / 2

        Select Case Me.DropSpeed
            Case Is >= 3
                Angle = BirdDiraction.Down
            Case Is >= 2
                Angle = BirdDiraction.DownrightMore
            Case Is >= 1
                Angle = BirdDiraction.Downright
            Case Is >= 0
                Angle = BirdDiraction.Right
            Case Else
                Angle = BirdDiraction.Upright
        End Select
        objBirdG.TranslateTransform(nX, nY)
        objBirdG.RotateTransform(Angle)

        objBirdG.DrawImage(birdImage, nP, nP, BIRD_LENTH, BIRD_LENTH) ', New Point(BIRD_LENTH * 0.5, 0), New Point(0, BIRD_LENTH * 0.5)}) 'New Point(BIRD_LC, Me.BirdCurrentLC)) ' - BIRD_LENTH / 2))

        objBirdG.RotateTransform(-Angle)

        If Not Didstart Then
            Dim X As Integer = 0
            Dim Y As Integer = -TUTORIAL_HEIGHT / 2 + 10
            objBirdG.DrawImage(Tutorial, X, Y, TUTORIAL_WIDTH, TUTORIAL_HEIGHT)
        End If

        objBirdG.TranslateTransform(-nX, -nY)

        'Dim HalfLenth As Integer = BIRD_LENTH / 2
        'Dim arcAngle As Integer
        'Dim Startx As Integer = BIRD_LC
        'Dim Starty As Integer = Me.BirdCurrentLC
        'Dim MiddlePoint As New Point(BIRD_LC + BIRD_LENTH / 2, Me.BirdCurrentLC + BIRD_LENTH / 2)
        'Dim MiddlePLenth As Double = (MiddlePoint.X ^ 2 + MiddlePoint.Y ^ 2) ^ 0.5
        'Dim NewAngle As Double = 180 / Math.PI * Math.Atan(MiddlePoint.Y / MiddlePoint.X)
        'Dim SwingLenth As Double
        'Dim NewPoint As Point
        'Exit Select
        'Exit Select
        'Exit Select
        'Case Is >= -1
        '   Angle = BirdDiraction.RightMore
        'Exit Select
        '>= -1
        '    Angle = BirdDiraction.Right
        '    'Exit Select
        'Case Is >= -2
        '    Angle = BirdDiraction.Uprightmore
        '    'Exit Select
        'Case Is >= -3

        'Case Else
        'Angle = BirdDiraction.Up
        'NewAngle = (180 - Angle) / 2 - NewAngle
        'arcAngle = Math.PI / 180 * NewAngle
        'SwingLenth = Math.Cos(Math.PI / 180 * ((180 - Angle) / 2)) * MiddlePLenth

        'NewPoint = New Point(Math.Cos(arcAngle) * SwingLenth - BIRD_LENTH / 2, Math.Sin(arcAngle) * SwingLenth - BIRD_LENTH / 2)
        'f.RotateFlip(RotateFlipType.RotateNoneFlipX)
        'New Point((-Math.Sin(arcAngle) + (Math.Cos(arcAngle))) * -(BIRD_LENTH / 2) + Startx, (Math.Sin(arcAngle) + (-Math.Cos(arcAngle))) * -(BIRD_LENTH / 2) + Starty)

        'Dim Angel As Double = 1.11111 '360
        'Dim rectAngel1 As Integer = 90 - Angel
        'Dim rectAngel2 As Integer = Angel
        'Dim Birdwidth As Integer = BIRD_LENTH
        'Dim Birdheight As Integer = BIRD_LENTH
        'Dim MiddleLine As Integer = (Birdheight ^ 2 + Birdwidth ^ 2) ^ 0.5
        'Dim nX As Integer = Math.Sin(rectAngel2) * MiddleLine
        'Dim nY As Integer = Math.Sin(rectAngel1) * MiddleLine
        'Dim Point As Double

        'objBirdG.RotateTransform(Angle)
        'objBirdG.ScaleTransform(-(BIRD_LC + BIRD_LENTH / 2), -(Me.BirdCurrentLC + BIRD_LENTH / 2))
        'objBirdG.RotateTransform(-Angle)
        'objBirdG.RotateTransform(-10)
        'objBirdG.DrawImage(BirdImages(Me.MyBirdKind.BirdColor, Me.MyBirdKind.BirdWings), {New Point(10, 10), New Point(0, BIRD_LENTH + 20), New Point(10, BIRD_LENTH + 20)})

        'objBirdG.DrawImage(BirdImages(MyBirdKind.BirdColor, MyBirdKind.BirdWings), BIRD_LC, Me.BirdLCLCLC - HalfLenth)
    End Sub

    Private Sub DrawLand(objLandG As Graphics)
        Dim i As Integer
        For i = 0 To Fix((Width + (-Me.LandLC)) / BGWIDTH) + 1
            objLandG.DrawImage(Land, New Point(Me.LandLC + i * BGWIDTH, LAND_LINE))
        Next
    End Sub

    Public Sub CheckDead()
        If Me.IsDead Then
            Return
        End If
        Me.IsDead = (BirdCurrentLC + BIRD_BOTTONLINE >= LAND_LINE)
        If Me.IsDead Then
            Return
        End If
        Dim nBirdLC As Integer = Me.BirdCurrentLC
        For i = 0 To Pipes.Count - 1
            Dim CurrentPipe As MyPipe = Pipes(i)
            Dim nLC As Integer = CurrentPipe.nPipe.LCOnTopLeft
            Dim nTop As Integer = CurrentPipe.nPipe.nHeight
            Dim nBotton As Integer = nTop + PIPE_BETWEEN

            If nLC - BIRD_LENTH + (BIRD_LENTH - BIRD_FRONT) > BIRD_LC Then
                Continue For
            End If
            If nLC + PIPE_LENTH < (BIRD_LC + BIRD_BACK) Then
                Continue For
            End If

            Me.IsDead = (nBirdLC + BIRD_BOTTONLINE + BIRD_HARDMORE >= nBotton) Or (nBirdLC + BIRD_TOPLINE - BIRD_HARDMORE <= nTop)

            If Me.IsDead Then
                WhiteCount = 255 / 2
                Return
            End If
        Next
    End Sub

    Public Function IsOnGround() As Boolean
        'If Not Me.IsOver Then
        '    Return False
        'End If
        If Me.BirdCurrentLC + BIRD_BOTTONLINE >= LAND_LINE Then
            WhiteCount = 0
            Return True
        End If
        Return False
    End Function

End Class

'Module PipeGenerate
'End Module

'Structure u

'End Structure


'Class Animation
'    Private Width As Integer
'    Private Height As Integer
'    Private Speed As Integer
'    Private LandLC As Integer
'    Private IsOver As Boolean
'    'Private BoardLC As Integer
'    'Private TextLC As Integer
'    Private MyBirdKind As BirdColor
'    Private Wing() As BirdWing =
'        {BirdWing.Middle, BirdWing.Middle, BirdWing.Down, BirdWing.Middle, BirdWing.Up}
'    Private WingCount As Double = 0
'    Private Pipes As List(Of Pipe)
'    Private BirdCurrentLC As Integer
'    Private DropSpeed As Double
'    Private Board As ScoreBoardMoving
'    Private Score As Integer
'    Private Didstart As Boolean


'    Public Sub New(nWidth As Integer, nHeight As Integer, Optional Speed As Integer = BESTSPEED)
'        SetProperty(nWidth, nHeight, Speed)
'    End Sub
'    Public Sub New()
'        SetProperty(BGWIDTH, BGHEIGHT, BESTSPEED)
'    End Sub
'    Public Sub Restart()
'        SetProperty(Me.Width, Me.Height, Me.Speed)
'    End Sub
'    Private Sub SetProperty(nWidth As Integer, nHeight As Integer, Speed As Integer)
'        With Me
'            .Width = nWidth
'            .Height = nHeight
'            .Speed = Speed
'            .LandLC = 0
'            .IsOver = False
'            '.BoardLC = BGHEIGHT
'            '.TextLC = 0
'            .MyBirdKind = BirdWing.Middle
'            .Pipes = New List(Of Pipe)
'            .BirdCurrentLC = (.Height - LAND_HEIGHT) / 2 - HEIGHTDIS
'            .DropSpeed = 0
'            .Board = New ScoreBoardMoving(nHeight, nWidth, , )
'            .Score = 0
'            .Didstart = False
'        End With
'    End Sub
'    Public Sub AddNewPipe(Optional IsMoving As Boolean = False)
'        Dim NewPipe As Pipe
'        Dim nheight As Integer

'        Randomize()
'        nheight = (Rnd() Mod 1) * (PIPE_MAXHEIGHT - PIPE_EDGE * 2 - PIPE_BETWEEN / 2) + PIPE_EDGE
'        With NewPipe
'            .LCOnTopLeft = Width
'            .nHeight = nheight
'            .IsPast = False
'        End With
'        Me.Pipes.Add(NewPipe)
'    End Sub
'    Public Function GetScore() As Integer
'        Return Me.Score
'    End Function
'    Public Sub SetWidth(NewWidth As Integer)
'        Me.Width = NewWidth
'    End Sub
'    Public Sub SetHeight(NewHeight As Integer)
'        Me.Height = NewHeight
'    End Sub
'    Public Sub SetBirdColor(Color As BirdColor)
'        MyBirdKind = Color
'    End Sub
'    Public Sub Click()
'        DropSpeed = -6.5
'        WingCount = 1
'    End Sub
'    Public Sub Reload()
'        If Me.IsOnGround Then
'            Return
'        End If
'        If Me.BirdCurrentLC + BIRD_BOTTONLINE <= LAND_LINE Then
'            Me.BirdCurrentLC += Fix(DropSpeed)
'        End If
'        If DropSpeed <= 10 Then
'            DropSpeed += 0.3
'        End If
'        If WingCount > 0 Then
'            If WingCount < Wing.GetLength(0) - 1 Then
'                WingCount += 0.1
'            Else
'                WingCount = 0
'            End If
'        End If
'        If Me.IsDead Then
'            Return
'        End If
'        Me.LandLC -= Speed
'        If -Me.LandLC >= Width Then
'            Me.LandLC += BGWIDTH
'        End If
'        If Pipes.Count >= 1 Then
'            If Pipes(0).LCOnTopLeft <= -PIPE_LENTH Then
'                Pipes.RemoveAt(0)
'            End If
'            For i = 0 To Pipes.Count - 1
'                Dim CurrentPipe As Pipe = Pipes(i)
'                CurrentPipe.LCOnTopLeft -= Speed
'                If Not CurrentPipe.IsPast Then
'                    If CurrentPipe.LCOnTopLeft + PIPE_LENTH - BIRD_BACK <= BIRD_LC Then
'                        Me.Score += 1
'                        Me.Board.SetScore(Me.Score)
'                        CurrentPipe.IsPast = True
'                    End If
'                End If
'                Pipes(i) = CurrentPipe
'            Next
'        End If
'    End Sub

'    Public Function IsAddPipe() As Boolean
'        If Pipes.Count <= 0 Then
'            Return True
'        End If
'        Dim LastPipe As Pipe = Pipes(Pipes.Count - 1)
'        Return Width - (LastPipe.LCOnTopLeft + PIPE_LENTH) >= PIPE_EACH_BETWEEN
'    End Function

'    'Public Function DrawBoard(objBoardG As Graphics) As Boolean
'    '    Dim Text As Image = textGameOver
'    '    Dim objBoard As Image = ScoreBoard
'    '    Dim textPoint As New Point(Me.Width / 2 - (Text.Width / 2), 70)
'    '    Dim BoardPoint As New Point(Me.Width / 2 - (objBoard.Width / 2), Me.Height - (IIf(Me.Speed >= 0, Me.Speed, 0)))
'    '    Me.Speed += 25
'    '    objBoardG.DrawImage(Text, textPoint.X, textPoint.Y, Text.Width, Text.Height)
'    '    objBoardG.DrawImage(objBoard, BoardPoint.X, BoardPoint.Y, objBoard.Width, objBoard.Height)
'    '    Return (Me.Height - Me.Speed) <= (textPoint.Y + Text.Height + 25)
'    'End Function

'    Public Function MoveEveryThing(objG As Graphics) As Boolean
'        'objG.RotateTransform(0)
'        objG.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
'        DrawPipes(objG)
'        DrawLand(objG)
'        DrawBird(objG)

'        If Me.IsOnGround Then
'            If Not Me.Board.ReloadBoard Then
'                Board.DrawBoard(objG)
'            Else
'                If Board.ReloadScore() Then
'                    Board.DrawButton(objG)
'                    Return True
'                Else
'                    Board.DrawScore(objG)
'                End If
'            End If
'        Else
'            GetScoreNumbers(objG, Me.Score, Me.Width)
'        End If
'        Return False
'        'objG.RotateTransform(0)
'        'objG.DrawImage(GetScoreNumbers(2, Me.Width), 0, 0)
'    End Function

'    Private Sub DrawPipes(objPipeG As Graphics)
'        Dim i As Integer
'        If Pipes.Count <= 0 Then
'            Return
'        End If

'        For i = 0 To Pipes.Count - 1
'            Dim DrawingPipe As Pipe = Pipes(i)
'            Dim UpPipePoint, DownPipePoint As Integer
'            UpPipePoint = -(PIPE_MAXHEIGHT - DrawingPipe.nHeight)
'            DownPipePoint = PIPE_MAXHEIGHT - (-UpPipePoint) + PIPE_BETWEEN
'            objPipeG.DrawImage(PipeGDown, DrawingPipe.LCOnTopLeft, UpPipePoint, PIPE_LENTH, PIPE_MAXHEIGHT)
'            objPipeG.DrawImage(PipeGUp, DrawingPipe.LCOnTopLeft, DownPipePoint, PIPE_LENTH, PIPE_MAXHEIGHT)
'        Next
'    End Sub

'    Private Sub DrawBird(objBirdG As Graphics)
'        Dim Angle As Integer
'        Dim birdImage As Image = BirdImages(Me.MyBirdKind, Me.Wing(Fix(Me.WingCount)))
'        Dim nP As Integer = -BIRD_LENTH / 2

'        Select Case Me.DropSpeed
'            Case Is >= 3
'                Angle = BirdDiraction.Down
'            Case Is >= 2
'                Angle = BirdDiraction.DownrightMore
'            Case Is >= 1
'                Angle = BirdDiraction.Downright
'            Case Is >= 0
'                Angle = BirdDiraction.Right
'            Case Else
'                Angle = BirdDiraction.Upright
'        End Select
'        objBirdG.TranslateTransform(BIRD_LC + BIRD_LENTH / 2, Me.BirdCurrentLC + BIRD_LENTH / 2)
'        objBirdG.RotateTransform(Angle)

'        objBirdG.DrawImage(birdImage, nP, nP, BIRD_LENTH, BIRD_LENTH) ', New Point(BIRD_LENTH * 0.5, 0), New Point(0, BIRD_LENTH * 0.5)}) 'New Point(BIRD_LC, Me.BirdCurrentLC)) ' - BIRD_LENTH / 2))

'        objBirdG.RotateTransform(-Angle)

'        If Not Didstart Then
'            Dim nX As Integer = 0
'            Dim nY As Integer = -TUTORIAL_HEIGHT / 2 + 10
'            Didstart = True
'            objBirdG.DrawImage(Tutorial, nX, nY, TUTORIAL_WIDTH, TUTORIAL_HEIGHT)
'        End If

'        objBirdG.TranslateTransform(-(BIRD_LC + BIRD_LENTH / 2), -(Me.BirdCurrentLC + BIRD_LENTH / 2))

'        'Dim HalfLenth As Integer = BIRD_LENTH / 2
'        'Dim arcAngle As Integer
'        'Dim Startx As Integer = BIRD_LC
'        'Dim Starty As Integer = Me.BirdCurrentLC
'        'Dim MiddlePoint As New Point(BIRD_LC + BIRD_LENTH / 2, Me.BirdCurrentLC + BIRD_LENTH / 2)
'        'Dim MiddlePLenth As Double = (MiddlePoint.X ^ 2 + MiddlePoint.Y ^ 2) ^ 0.5
'        'Dim NewAngle As Double = 180 / Math.PI * Math.Atan(MiddlePoint.Y / MiddlePoint.X)
'        'Dim SwingLenth As Double
'        'Dim NewPoint As Point
'        'Exit Select
'        'Exit Select
'        'Exit Select
'        'Case Is >= -1
'        '   Angle = BirdDiraction.RightMore
'        'Exit Select
'        '>= -1
'        '    Angle = BirdDiraction.Right
'        '    'Exit Select
'        'Case Is >= -2
'        '    Angle = BirdDiraction.Uprightmore
'        '    'Exit Select
'        'Case Is >= -3

'        'Case Else
'        'Angle = BirdDiraction.Up
'        'NewAngle = (180 - Angle) / 2 - NewAngle
'        'arcAngle = Math.PI / 180 * NewAngle
'        'SwingLenth = Math.Cos(Math.PI / 180 * ((180 - Angle) / 2)) * MiddlePLenth

'        'NewPoint = New Point(Math.Cos(arcAngle) * SwingLenth - BIRD_LENTH / 2, Math.Sin(arcAngle) * SwingLenth - BIRD_LENTH / 2)
'        'f.RotateFlip(RotateFlipType.RotateNoneFlipX)
'        'New Point((-Math.Sin(arcAngle) + (Math.Cos(arcAngle))) * -(BIRD_LENTH / 2) + Startx, (Math.Sin(arcAngle) + (-Math.Cos(arcAngle))) * -(BIRD_LENTH / 2) + Starty)

'        'Dim Angel As Double = 1.11111 '360
'        'Dim rectAngel1 As Integer = 90 - Angel
'        'Dim rectAngel2 As Integer = Angel
'        'Dim Birdwidth As Integer = BIRD_LENTH
'        'Dim Birdheight As Integer = BIRD_LENTH
'        'Dim MiddleLine As Integer = (Birdheight ^ 2 + Birdwidth ^ 2) ^ 0.5
'        'Dim nX As Integer = Math.Sin(rectAngel2) * MiddleLine
'        'Dim nY As Integer = Math.Sin(rectAngel1) * MiddleLine
'        'Dim Point As Double

'        'objBirdG.RotateTransform(Angle)
'        'objBirdG.ScaleTransform(-(BIRD_LC + BIRD_LENTH / 2), -(Me.BirdCurrentLC + BIRD_LENTH / 2))
'        'objBirdG.RotateTransform(-Angle)
'        'objBirdG.RotateTransform(-10)
'        'objBirdG.DrawImage(BirdImages(Me.MyBirdKind.BirdColor, Me.MyBirdKind.BirdWings), {New Point(10, 10), New Point(0, BIRD_LENTH + 20), New Point(10, BIRD_LENTH + 20)})

'        'objBirdG.DrawImage(BirdImages(MyBirdKind.BirdColor, MyBirdKind.BirdWings), BIRD_LC, Me.BirdLCLCLC - HalfLenth)
'    End Sub

'    Private Sub DrawLand(objLandG As Graphics)
'        Dim i As Integer
'        For i = 0 To Fix((Width + (-Me.LandLC)) / BGWIDTH) + 1
'            objLandG.DrawImage(Land, New Point(Me.LandLC + i * BGWIDTH, LAND_LINE))
'        Next
'    End Sub

'    Public Function IsDead() As Boolean
'        If Me.IsOver Then
'            Return True
'        End If
'        Me.IsOver = (BirdCurrentLC + BIRD_BOTTONLINE >= LAND_LINE)
'        If Me.IsOver Then
'            Return Me.IsOver
'        End If

'        If Pipes.Count <= 0 Then
'            Me.IsOver = False
'            Return Me.IsOver
'        End If

'        Dim nBirdLC As Integer = Me.BirdCurrentLC
'        For i = 0 To Pipes.Count - 1
'            Dim CurrentPipe As Pipe = Pipes(i)
'            Dim nLC As Integer = CurrentPipe.LCOnTopLeft
'            Dim nTop As Integer = CurrentPipe.nHeight
'            Dim nBotton As Integer = nTop + PIPE_BETWEEN

'            If nLC - BIRD_LENTH + (BIRD_LENTH - BIRD_FRONT) > BIRD_LC Then
'                Continue For
'            End If
'            If nLC + PIPE_LENTH < (BIRD_LC + BIRD_BACK) Then
'                Continue For
'            End If

'            Me.IsOver = (nBirdLC + BIRD_BOTTONLINE + BIRD_HARDMORE >= nBotton) Or (nBirdLC + BIRD_TOPLINE - BIRD_HARDMORE <= nTop)

'            If Me.IsOver Then
'                Return True
'            End If
'        Next
'        Me.IsOver = False
'        Return False

'    End Function
'    Public Function IsOnGround() As Boolean
'        'If Not Me.IsOver Then
'        '    Return False
'        'End If
'        Return Me.BirdCurrentLC + BIRD_BOTTONLINE >= LAND_LINE
'    End Function

'End Class

'Module PipeGenerate
'End Module

'Structure u

'End Structure
