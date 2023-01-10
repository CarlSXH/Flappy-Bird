Module Dimesion
    Public Const WIDTHDIS As Integer = 16
    Public Const HEIGHTDIS As Integer = 39

    Public Const BGWIDTH As Integer = 288
    Public Const BGHEIGHT As Integer = 512

    Public Const BESTSPEED As Integer = 3

    Public Const LAND_HEIGHT As Integer = 112
    Public Const LAND_LINE As Integer = BGHEIGHT - LAND_HEIGHT

    Public Const PIPE_MAXHEIGHT As Integer = 320
    Public Const PIPE_EDGE As Integer = 100
    Public Const PIPE_LENTH As Integer = 52
    Public Const PIPE_BETWEEN As Integer = 112 '+ 200
    Public Const PIPE_EACH_BETWEEN As Integer = 130 '+ 100

    Public Const BIRD_LC As Integer = 50
    Public Const BIRD_LENTH As Integer = 48
    Public Const BIRD_TOPLINE As Integer = 14
    Public Const BIRD_BOTTONLINE As Integer = BIRD_LENTH - 12
    Public Const BIRD_HARDMORE As Integer = 0 '300
    Public Const BIRD_BACK As Integer = 7
    Public Const BIRD_FRONT As Integer = BIRD_LENTH - 10

    Public Const SCORE_HEIGHT As Integer = 44
    Public Const SCORE_WIDTH As Integer = 24
    Public Const SCORE_EACH_DIS As Integer = 2
    Public Const SCORE_CONTEXT_WIDTH As Integer = 14
    Public Const SCORE_CONTEXT_HEIGHT As Integer = 20

    Public Const SCOREBOARD_HEIGHT As Integer = 126
    Public Const SCOREBOARD_WIDTH As Integer = 238
    Public Const SCOREBOARD_MAXHEIGHT As Integer = 125
    Public Const SCOREBOARD_SCORE_X As Integer = 236
    Public Const SCOREBOARD_SCORE_Y As Integer = 161
    Public Const SCOREBOARD_SCORE_BETWEEN As Integer = 1

    Public Const MEDAL_X As Integer = 32
    Public Const MEDAL_Y As Integer = 44
    Public Const MEDAL_LENTH As Integer = 44

    Public Const TEXT_HEIGHT As Integer = 50
    Public Const TEXT_TITLE_HEIGHT As Integer = 48
    Public Const TEXT_TITLE_WIDTH As Integer = 178
    Public Const TEXT_READY_WIDTH As Integer = 196
    Public Const TEXT_READY_HEIGHT As Integer = 62
    Public Const TEXT_GAMEOVER_WIDTH As Integer = 204
    Public Const TEXT_GAMEOVER_HEIGHT As Integer = 54
    Public Const TEXT_NEW_WIDTH As Integer = 32
    Public Const TEXT_NEW_HEIGHT As Integer = 14

    Public Const TUTORIAL_WIDTH As Integer = 114
    Public Const TUTORIAL_HEIGHT As Integer = 98

    'Public Const BTN_LONG_WIDTH As Integer = 80
    'Public Const BTN_LONG_HEIGHT As Integer = 28
    'Public Const BTN_SHORT_WIDTH As Integer = 26
    'Public Const BTN_SHORT_HEIGHT As Integer = 28
    Public Const BTN_BIG_WIDTH As Integer = 116
    Public Const BTN_BIG_HEIGHT As Integer = 70

    Public Const BLINK_LENTH As Integer = 10

    Public Structure Pipe
        Dim LCOnTopLeft As Integer
        Dim nHeight As Integer
        Dim IsPast As Boolean
    End Structure

    Public Enum BirdDiraction
        Up = -90
        UpMore = -67.5
        Upright = -45
        Uprightmore = -22.5
        Right = 0
        RightMore = 22.5
        Downright = 45
        DownrightMore = 67.5
        Down = 90
    End Enum

    Public Enum BirdColor
        Blue = 0
        Red = 1
        Yellow = 2
    End Enum

    Public Enum BirdWing
        Up = 0
        Middle = 1
        Down = 2
    End Enum

End Module

Module ImageManager
    Public ReadOnly BGDay As Image = My.Resources.BGDay
    Public ReadOnly BGNight As Image = My.Resources.BGNight
    Public ReadOnly Land As Image = My.Resources.Land
    Public ReadOnly BirdImages(,) As Image =
    {
        {My.Resources.BirdB1, My.Resources.BirdB2, My.Resources.BirdB3},
        {My.Resources.BirdR1, My.Resources.BirdR2, My.Resources.BirdR3},
        {My.Resources.BirdY1, My.Resources.BirdY2, My.Resources.BirdY3}
    }
    Public ReadOnly PipeGUp As Image = My.Resources.PipeGUp
    Public ReadOnly PipeGDown As Image = My.Resources.PipeGDown
    Public ReadOnly ScoreImage() As Image =
    {
        My.Resources.Score0,
        My.Resources.Score1,
        My.Resources.Score2,
        My.Resources.Score3,
        My.Resources.Score4,
        My.Resources.Score5,
        My.Resources.Score6,
        My.Resources.Score7,
        My.Resources.Score8,
        My.Resources.Score9
    }
    Public ReadOnly ContextScore() As Image =
    {
        My.Resources.NumberContext0,
        My.Resources.NumberContext1,
        My.Resources.NumberContext2,
        My.Resources.NumberContext3,
        My.Resources.NumberContext4,
        My.Resources.NumberContext5,
        My.Resources.NumberContext6,
        My.Resources.NumberContext7,
        My.Resources.NumberContext8,
        My.Resources.NumberContext9
    }
    Public ReadOnly Medals() As Image =
    {
        My.Resources.Medals_Copper,
        My.Resources.Medals_Silver,
        My.Resources.Medals_Gold,
        My.Resources.Medals_WhiteGold
    }
    Public ReadOnly Blink() As Image =
    {
        My.Resources.Blink0,
        My.Resources.Blink1,
        My.Resources.Blink2
    }
    Public ReadOnly ScoreBoard As Image = My.Resources.ScoreBoard
    Public ReadOnly textNew As Image = My.Resources._New

    'Public ReadOnly btnPause As Image = My.Resources.btnPause
    'Public ReadOnly btnResume As Image = My.Resources.btnResume

    'Public ReadOnly btnMenu As Image = My.Resources.btnMenu
    'Public ReadOnly btnOk As Image = My.Resources.btnOk
    'Public ReadOnly btnShare As Image = My.Resources.btnShare

    Public ReadOnly btnStart As Image = My.Resources.btnStart
    'Public ReadOnly btnScore As Image = My.Resources.btnScore

    Public ReadOnly textGameOver As Image = My.Resources.textGameOver
    Public ReadOnly textReady As Image = My.Resources.textGetReady
    Public ReadOnly textTitle As Image = My.Resources.Title
    Public ReadOnly Tutorial As Image = My.Resources.Tutorial

End Module