Imports System.Runtime.InteropServices
Public Class Form1

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HTCAPTION As Integer = &H2

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function ReleaseCapture() As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 3000
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim p As Process()
        p = Process.GetProcessesByName("Spotify")
        For Each w As Process In p
            If w.MainWindowTitle.Length > 0 Then
                If w.MainWindowTitle.Contains("Spotify") Then
                    If My.Settings.BlankOnNotPlaying = True Then
                        Me.PictureBox1.Visible = True
                        Me.Label1.Text = ""
                    Else
                        Me.Label1.Text = "Nothing Playing"
                        Me.PictureBox1.Visible = False
                    End If
                Else
                    Me.Label1.Text = w.MainWindowTitle
                    Me.PictureBox1.Visible = True
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
    End Sub
End Class
