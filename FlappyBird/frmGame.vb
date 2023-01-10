Public Class frmGame

    Dim objBG As PictureBox = Nothing
    Dim objGame As PictureBox = Nothing
    Dim Moving As Animation = Nothing
    Dim Board As Scoreboardmoving = Nothing
    Dim StartPage As MainForm = Nothing
    Dim BiggestScore As Integer = 0
    Dim UpdateTime As Timer = Nothing

    Private Sub Frm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With Me
            .Height = BGHEIGHT
            .Width = BGWIDTH
            .FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        End With
        StartPage = New MainForm(Me.Width)

        Dim NewMainPage As New PictureBox()
        Dim NewMPTimer As New Timer()
        With NewMainPage
            .Width = Me.Width
            .Height = Me.Handle
            .Location = Point.Empty
            .BackColor = Color.Transparent
            AddHandler .MouseDown, AddressOf StartBoard_MouseDown
            AddHandler .MouseUp, AddressOf StartBoard_MouseUp
            AddHandler .Paint, AddressOf StartBoard_Paint
        End With
        With NewMPTimer
            .Interval = 20
            .Enabled = True
            AddHandler .Tick, AddressOf StartBoard_TimerTick
        End With

        Dim NewBG As New PictureBox()
        With NewBG
            Dim CurrentTime As Date = Now

            .Width = Me.Width
            .Height = Me.Handle
            .Location = Point.Empty
            .BackColor = Color.Transparent
            .BackgroundImage = IIf(CurrentTime.Hour - 7 < 12, BGDay, BGNight)
            .Controls.Add(NewMainPage)
        End With

        objBG = NewBG
        With Me
            .Controls.Add(objBG)
            .Height += HEIGHTDIS
            .Width += WIDTHDIS
        End With
        NewMPTimer.Start()
    End Sub

    Private Sub GameStartSetting()
        Dim NewGamePic As New PictureBox()
        Dim NewGameTimer As New Timer()

        Moving = New Animation(Me.Width - WIDTHDIS, Me.Height - HEIGHTDIS, )
        With NewGamePic
            .Width = Me.Width - WIDTHDIS
            .Height = Me.Handle - HEIGHTDIS
            .Location = Point.Empty
            .BackColor = Color.Transparent
            AddHandler .Paint, AddressOf Game_Paint
            AddHandler .MouseDown, AddressOf Game_MouseDown
        End With
        With NewGameTimer
            .Enabled = True
            .Interval = 15
            AddHandler .Tick, AddressOf Game_TimerTick
        End With
        If Not IsNothing(objGame) Then
            objGame.Dispose()
        End If
        objGame = NewGamePic
        Me.objBG.Controls.Add(objGame)
        With Me
            AddHandler .KeyUp, AddressOf Game_KeyDown
        End With
        UpdateTime = NewGameTimer
        NewGameTimer.Start()
    End Sub

    Private Sub GameEndSetting()
        Dim NewBoard As New PictureBox()
        Dim NewBoardTimer As New Timer()

        Board = New Scoreboardmoving(Me.Height - HEIGHTDIS, Me.Width - WIDTHDIS, BiggestScore, Moving.GetScore, )
        BiggestScore = Math.Max(Moving.GetScore, BiggestScore)
        With NewBoard
            .Width = Me.Width - WIDTHDIS
            .Height = Me.Height - HEIGHTDIS
            .Location = Point.Empty
            .BackColor = Color.Transparent
            AddHandler .MouseDown, AddressOf Board_MouseDown
            AddHandler .MouseUp, AddressOf Board_MouseUp
            AddHandler .Paint, AddressOf Board_Paint
        End With
        With NewBoardTimer
            .Interval = 1
            AddHandler .Tick, AddressOf Board_TimerTick
        End With
        objGame.Controls.Add(NewBoard)
        NewBoardTimer.Start()
    End Sub

    Private Sub StartBoard_MouseDown(sender As PictureBox, e As MouseEventArgs)
        StartPage.MouseDown(e.X, e.Y)
    End Sub

    Private Sub StartBoard_MouseUp(sender As PictureBox, e As MouseEventArgs)
        If StartPage.MouseUp(e.X, e.Y) Then
            GameStartSetting()
            sender.Dispose()
        End If
    End Sub

    Private Sub StartBoard_Paint(sender As PictureBox, e As PaintEventArgs)
        'StartPage.Reload()
        StartPage.DrawForm(e.Graphics)
    End Sub

    Private Sub StartBoard_TimerTick(sender As Timer, e As EventArgs)
        StartPage.Reload()
        Me.Refresh()
    End Sub

    Private Sub Game_TimerTick(sender As Timer, e As EventArgs)
        If Not Moving.IsStart() Then
            Return
        End If
        Moving.Reload()
        objGame.Refresh()
        If Not Moving.IsOnGround Then
            Return
        End If
        GameEndSetting()
        sender.Dispose()

    End Sub

    Private Sub Game_KeyDown(sender As Object, e As KeyEventArgs)

        If e.KeyCode = Keys.Space Then
            UpdateTime.Stop()
        End If
        If e.KeyCode = Keys.S Then
            UpdateTime.Start()
        End If

    End Sub

    Private Sub Game_MouseDown(sender As PictureBox, e As MouseEventArgs)
        If e.Button <> Windows.Forms.MouseButtons.Left Then
            Return
        End If
        Moving.Click()
    End Sub

    Private Sub Game_Paint(sender As PictureBox, e As PaintEventArgs)
        'Me.Refresh()
        Dim G As Graphics = e.Graphics

        Moving.CheckDead()
        Moving.DrawEveryThing(G)
        'G.DrawImage(btnPause, Me.Width - WIDTHDIS - BTN_SHORT_WIDTH, 0, BTN_SHORT_WIDTH, BTN_SHORT_HEIGHT)
    End Sub

    Private Sub Board_MouseDown(sender As PictureBox, e As MouseEventArgs)
        If Board.ReadyPress Then
            Board.MouseDown(e.X, e.Y)
            Me.Refresh()
        End If
    End Sub

    Private Sub Board_MouseUp(sender As PictureBox, e As MouseEventArgs)
        If Board.MouseUp(e.X, e.Y) Then
            sender.Dispose()
            Moving.Restart()
            GameStartSetting()
        End If
        Me.Refresh()
    End Sub

    Private Sub Board_Paint(sender As PictureBox, e As PaintEventArgs)
        Board.DrawBoard(e.Graphics)
    End Sub

    Private Sub Board_TimerTick(sender As Timer, e As EventArgs)
        'Board.ReloadBoard()
        If Board.ReloadBoard() Then
            sender.Dispose()
        End If
        objGame.Refresh()
    End Sub

    'Private Sub CreatePicBox(Optional IsChangeBG As Boolean = False)
    '    'Dim i As Integer
    '    Randomize()
    '    Dim imageDayNight() As Image = {BGDay, BGNight}

    '    Dim objImage As Image = imageDayNight(Fix(Rnd() * imageDayNight.Count) Mod imageDayNight.Count)
    '    If IsChangeBG Then
    '        If IsNothing(objBG) Then
    '            Return
    '        End If
    '        objBG.BackgroundImage = objImage
    '    End If
    '    'Dim BGBitmap As New Bitmap(Me.Width, Me.Height)
    '    'Dim objBGG As Graphics

    '    'objBGG = Graphics.FromImage(BGBitmap)
    '    'For i = 0 To Fix(BGBitmap.Width / BGWIDTH) + 1
    '    '    objBGG.DrawImage(objImage, New Point(i * BGWIDTH, 0))
    '    'Next
    '    'objBGG.Dispose()
    '    Dim NewBackRound As New PictureBox
    '    Dim NewGamePicBox As New PictureBox

    '    With NewBackRound
    '        .BackgroundImage = objImage
    '        .Location = New Point(0, 0)
    '        .Height = Me.Height
    '        .Width = Me.Width
    '    End With

    '    With NewGamePicBox
    '        .Location = New Point(0, 0)
    '        .Height = Me.Height
    '        .Width = Me.Width
    '        .BackColor = Color.Transparent
    '    End With

    '    AddHandler NewGamePicBox.MouseUp, AddressOf Game_MouseUp
    '    AddHandler NewGamePicBox.Paint, AddressOf GameRefresh

    '    objBG = NewBackRound
    '    objPic = NewGamePicBox

    '    objBG.Controls.Add(objPic)
    '    Me.Controls.Add(objBG)
    'End Sub

    'Private Sub PicRefresh(sender As Object, e As EventArgs)
    '    'If Moving.IsAddPipe Then
    '    '    Moving.AddNewPipe()
    '    'End If
    '    'Moving.Reload()
    '    'objPic.Image = Moving.MoveEveryThing
    '    Me.Refresh()
    'End Sub

    'Private Sub Game_MouseUp(sender As Object, e As MouseEventArgs)
    '    If Not DidStart Then
    '        'If StartPage.Clicked(e.X, e.Y) Then
    '        StartPage = Nothing
    '        DidStart = True
    '        'Timer1.Enabled = False
    '        Me.Refresh()
    '        'End If
    '    Else
    '        Clicked()
    '    End If
    'End Sub

    'Private Sub Clicked()
    '    'Moving.SetBirdLC(e.Y)

    '    If DidStart Then
    '        'Timer1.Enabled = True
    '    End If

    '    If DidDead Then

    '    End If

    '    If Not Moving.IsDead Then
    '        'If Timer1.Enabled Then
    '        Moving.Click()
    '        'Else
    '        'Timer1.Enabled = True
    '        'End If
    '        'Moving.Restart()
    '        'Timer1.Enabled = True
    '        'Return
    '    Else
    '        'If Not Timer1.Enabled Then
    '        Moving.Restart()
    '        'Timer1.Enabled = True
    '        'End If
    '    End If


    'End Sub
    ''Dim count As Boolean = False
    'Private Sub GameRefresh(sender As Object, e As PaintEventArgs)

    '    Dim MyGraphic As Graphics = e.Graphics
    '    If Not DidStart Then
    '        StartPage.Reload()
    '        StartPage.DrawForm(MyGraphic)
    '        Return
    '    End If
    '    'count = Not count
    '    If Moving.IsAddPipe Then
    '        Moving.AddNewPipe()
    '    End If
    '    'If Not Moving.IsDead Then
    '    Moving.Reload()
    '    DidDead = Moving.MoveEveryThing(MyGraphic)
    '    'End If
    '    'If Moving.IsOnGround Then
    '    '    'If Moving.DrawBoard(MyGraphic) Then
    '    '    Board.SetScore(Moving.GetScore)
    '    '    If Not Board.ReloadBoard() Then
    '    '        Board.DrawBoard(MyGraphic)
    '    '    Else
    '    '        If Not Board.ReloadScore() Then
    '    '            Board.DrawScore(MyGraphic)
    '    '        Else
    '    '            Board.DrawScore(MyGraphic)
    '    '            Board = New ScoreBoardMoving(Me.Height, Me.Width)
    '    '            Timer1.Enabled = False
    '    '        End If
    '    '    End If
    '    'Else
    '    '    GetScoreNumbers(e.Graphics, Moving.GetScore, Me.Width)

    '    '    'End If
    '    'End If

    '    'MyGraphic.TranslateTransform(100, 100)
    '    'MyGraphic.RotateTransform(45)
    '    'MyGraphic.DrawImage(textReady, 0, 0, TEXT_READY_WIDTH, TEXT_READY_HEIGHT)
    '    'MyGraphic.DrawImage(textGameOver, 0, 0, TEXT_GAMEOVER_WIDTH, TEXT_GAMEOVER_HEIGHT)
    '    'MyGraphic.DrawImage(btnOk, 0, 0)
    '    'MyGraphic.DrawImage(ScoreBoard, 0, 0, SCOREBOARD_WIDTH, SCOREBOARD_HEIGHT)
    '    'MyGraphic.DrawImage(Medals(0), MEDAL_X, MEDAL_Y, MEDAL_LENTH, MEDAL_LENTH)

    '    'f -= 1
    '    'If f <= -BGWIDTH Then
    '    '    f = 0
    '    'End If
    '    'e.Graphics.DrawImage(BGDay, f, 0)
    '    'e.Graphics.DrawImage(BGDay, f + BGWIDTH, 0)
    'End Sub

    'Private Sub frmGame_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    '    If DidStart Then
    '        If e.KeyCode = Keys.Space Then
    '            Clicked()
    '        End If
    '    End If
    'End Sub
End Class
