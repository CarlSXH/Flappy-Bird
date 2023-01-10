Class Bird
    Private BirdLC As Integer
    Private StartPoint As Integer
    Private IsUp As Boolean
    Private Speed As Integer
    Private Pipes As List(Of Pipe)

    Private Const JumpHeight As Integer = 50
    Public Sub New(_BirdLC As Integer, _Speed As Integer)
        Me.BirdLC = _BirdLC
        Me.Speed = _Speed
        Me.StartPoint = -1
        Me.IsUp = False
    End Sub
    Public Sub InitPipe(_Pipes() As Pipe)
        Me.Pipes.Clear()
        For i = 0 To _Pipes.Count - 1
            Me.Pipes.Add(_Pipes(i))
        Next
    End Sub
    Public Sub Clicked()
        Me.StartPoint = Me.BirdLC
        IsUp = True
    End Sub

    Public Sub Reload()
        If IsUp Then

        End If
    End Sub


End Class
