Imports System.Runtime.InteropServices
Imports System.Drawing
Public Class Form1

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HTCAPTION As Integer = &H2

    Private showLogo As Boolean = False
    Private nowPlaying As String = ""
    Private scrollText As Boolean = False
    Private firstHit As Boolean = False
    Private nowPlayingLen As Integer = 0
    Private subtractedChars As Integer = 0
    Private trimmedCharCount As Integer = 0
    Private antiScrollPauseMillisecs As Integer = 2000
    Private waitedFor As Integer = 0
    Private zString As String = "z"


    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function ReleaseCapture() As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 3000
        Timer1.Start()
        Timer2.Interval = 150
        Timer2.Stop()

        Try
            Dim fontSize As Single
            Single.TryParse(My.Settings.FontSize, fontSize)
            If IsValidFont(My.Settings.FontName) Then
                Me.Label1.Font = New Font(My.Settings.FontName, fontSize, FontStyle.Bold)
            Else
                Me.Label1.Font = New Font("Arial Narrow", 16.0F, FontStyle.Bold)
            End If
            If My.Settings.ShowLogo Then
                showLogo = True
            End If

            If My.Settings.FontSize > My.Settings.WindowHeight / 2 Then
                MessageBox.Show("Error: WindowHeight must be at least 2 times the value of FontSize. Please edit the config file. Exiting", My.Application.Info.AssemblyName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()
            End If
        Catch ex As Exception
            Me.Label1.Font = New Font("Arial Narrow", 16.0F, FontStyle.Bold)
        End Try

        Try
            If My.Settings.WindowHeight > 0 And My.Settings.WindowHeight <= Me.MaximumSize.Height Then
                Me.Size = New Size(Me.Size.Width, My.Settings.WindowHeight)
            End If
            If My.Settings.WindowWidth > 0 And My.Settings.WindowWidth <= Me.MaximumSize.Width Then
                Me.Size = New Size(My.Settings.WindowWidth, Me.Size.Height)
                Label1.Size = New Size(Me.Size.Width - 40, Me.Label1.Height)
            End If

        Catch ex As Exception
            Me.Size = New Size(588, 40)
        End Try

        Try
            Dim textHeight As Integer = GetTextHeight()
            Dim padValue As Integer = 0 'Math.Truncate(textHeight / (textHeight / (Me.Size.Height - 4)))
            'padValue = Math.Truncate(textHeight / 2) + Math.Truncate(textHeight / (Label1.Font.SizeInPoints / (Me.Size.Height - 4)))
            padValue = Math.Truncate((Me.Size.Height - textHeight) / 2)

            Label1.Padding = New Padding(Label1.Padding.Left, Math.Truncate(textHeight / 8) + 1, Label1.Padding.Right, Label1.Padding.Bottom)
            Label1.Padding = New Padding(Label1.Padding.Left, padValue, Label1.Padding.Right, padValue)
        Catch ex As Exception
            Label1.Padding = New Padding(0, 1, 0, 1)
        End Try


        Try
            Me.Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml(My.Settings.FontColor)
        Catch ex As Exception
            Me.Label1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#32CD32")
        End Try

        Try
            scrollText = My.Settings.AllowScrollingText
        Catch ex As Exception
            scrollText = False
        End Try

        Me.PictureBox1.Visible = showLogo

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim p As Process()
        p = Process.GetProcessesByName("Spotify")
        For Each w As Process In p
            If w.MainWindowTitle.Length > 0 Then
                If w.MainWindowTitle.Contains("Spotify") Then
                    If My.Settings.BlankOnNotPlaying = True Then
                        Me.PictureBox1.Visible = showLogo
                        Me.Label1.Text = ""
                    Else
                        Me.Label1.Text = "Nothing Playing"
                        Me.PictureBox1.Visible = False
                    End If
                    Me.nowPlaying = Nothing
                Else
                    Dim titleText As String = w.MainWindowTitle
                    If titleText.Contains("&") Then
                        titleText = titleText.Replace("&", "&&")
                    End If

                    If Not Me.nowPlaying = titleText Then
                        NewSong(titleText)
                    Else
                        Exit Sub
                    End If
                End If
            End If
        Next
        p = Nothing
        GC.Collect()
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
        If e.Button = MouseButtons.Right Then
            Me.ContextMenuStrip1.Show(New Point(Me.Location.X + e.Location.X, Me.Location.Y + e.Location.Y))
        End If
    End Sub

    Private Sub Label1_MouseDown(sender As Object, e As MouseEventArgs) Handles Label1.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
        If e.Button = MouseButtons.Right Then
            'Me.ContextMenuStrip1.Show(New Point(Me.Location.X + e.Location.X, Me.Location.Y + e.Location.Y))
        End If
    End Sub
    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
        If e.Button = MouseButtons.Right Then
            'Me.ContextMenuStrip1.Show(New Point(Me.Location.X + e.Location.X, Me.Location.Y + e.Location.Y))
        End If
    End Sub
    Private Function IsValidFont(fontName As String)
        Try
            Dim testFont As New Font(fontName, 12.0F, FontStyle.Regular)
            If testFont.Name = fontName Then
                Return True
                Exit Function
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub NewSong(ByVal newSongName As String)
        Me.Label1.Text = newSongName
        Me.nowPlaying = newSongName
        Me.PictureBox1.Visible = showLogo
        Timer2.Stop()
        trimmedCharCount = 0
        subtractedChars = 0
        nowPlayingLen = nowPlaying.Length
        Do While IsTextTooBig(Me.Label1.Text) And Not Me.Label1.Text = ""
            Me.Label1.Text = Me.Label1.Text.Substring(0, Me.Label1.Text.Length - 1)
            trimmedCharCount += 1
        Loop
        zString = "z"
        For i = 1 To trimmedCharCount
            zString &= "z"
        Next
        waitedFor = 0
        If scrollText Then
            Timer2.Start()
        End If
    End Sub

    Private Function IsTextTooBig(input As String) As Boolean
        Dim measuredTextWidth As Integer = TextRenderer.MeasureText(input, Label1.Font).Width

        If measuredTextWidth > Label1.Width - (5) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function GetTextHeight() As Integer
        Dim measuredTextHeight As Integer = 0

        Try
            measuredTextHeight = TextRenderer.MeasureText(Label1.Text, Label1.Font).Height
        Catch ex As Exception
            measuredTextHeight = 0
        End Try

        Return measuredTextHeight
    End Function

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        waitedFor += Timer2.Interval
        If Not scrollText Then
            Exit Sub
        End If
        If waitedFor < antiScrollPauseMillisecs Then
            Exit Sub
        End If
        ' If Not IsTextTooBig(Label1.Text & zString) Then
        If Not IsTextTooBig(nowPlaying) Then
            Exit Sub
        End If

        'If scrollText And IsTextTooBig(Label1.Text & zString) Then
        If scrollText And IsTextTooBig(nowPlaying) Then
            If subtractedChars = 0 And trimmedCharCount <= 0 Then
                Me.Label1.Text = Me.Label1.Text.Substring(5, Me.Label1.Text.Length - 5) & "    " & nowPlaying.Substring(subtractedChars, 1)
                DBG("hit1")
            ElseIf trimmedCharCount > 0 Then
                Me.Label1.Text = Me.Label1.Text.Substring(1, Me.Label1.Text.Length - 1) & nowPlaying.Substring(nowPlaying.Length - trimmedCharCount, 1)
                firstHit = False
                trimmedCharCount += -1
                subtractedChars = -1
                DBG("hit2")
            Else
                Me.Label1.Text = Me.Label1.Text.Substring(1, Me.Label1.Text.Length - 1) & nowPlaying.Substring(subtractedChars, 1)
                DBG("hit3")
            End If
            DBG(trimmedCharCount & "," & nowPlayingLen & "," & subtractedChars & "," & Label1.Text & "," & Label1.Text.Length)
            subtractedChars += 1
            If subtractedChars >= nowPlayingLen Then
                subtractedChars = 0
            End If
        End If
    End Sub

    Private Sub DBG(text As String)
        If Debugger.IsAttached Then
            Debug.Print(text)
        End If
    End Sub

End Class
