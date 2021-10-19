Imports System.Data.SqlClient

Public Class Homepage
    Private Sub btnBookManagement_Click(sender As Object, e As EventArgs) Handles btnBookManagement.Click
        BookManagement.ShowDialog()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to quit the system?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            Me.Close()
            Login.Close()
        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
        End If

    End Sub

    Private Sub btnBookFine_Click(sender As Object, e As EventArgs) Handles btnBookFine.Click
        BookFineManage.ShowDialog()
    End Sub

    Private Sub btnLendandReturn_Click(sender As Object, e As EventArgs) Handles btnLendandReturn.Click
        LendAndReturn.ShowDialog()
    End Sub

    Private Sub btnStudent_Click(sender As Object, e As EventArgs) Handles btnStudent.Click
        Dim db As New LibraryDataContext()
        Dim user As String = "User"
        Dim isDuplicated As Boolean = db.Accounts.Any(Function(o) o.Username = Login.sessionUser)
        If isDuplicated = True Then
            Dim isUnauthorized As Boolean = db.Accounts.Any(Function(o) o.Roles = user And o.Username = Login.sessionUser)
            If isUnauthorized = True Then
                MessageBox.Show("Unauthorized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                btnStudent.Enabled = False
                btnRoles.Enabled = False
            Else
                StudentManage.show()
            End If
        Else
        End If

    End Sub

    Private Sub btnRoles_Click(sender As Object, e As EventArgs) Handles btnRoles.Click

        Dim db As New LibraryDataContext()
        Dim user As String = "User"
        Dim isDuplicated As Boolean = db.Accounts.Any(Function(o) o.Username = Login.sessionUser)
        If isDuplicated = True Then
            Dim isUnauthorized As Boolean = db.Accounts.Any(Function(o) o.Roles = user And o.Username = Login.sessionUser)
            If isUnauthorized = True Then
                MessageBox.Show("Unauthorized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                btnStudent.Enabled = False
                btnRoles.Enabled = False
            Else
                Roles.ShowDialog()
            End If
        Else
        End If
    End Sub

    Private Sub Homepage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim digit As Integer
        Dim result As String

        Using con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Library.mdf;Integrated Security=True")

            con.Open()
            'check product id
            Dim cmd = New SqlCommand("Select MAX(ID) FROM LoginTime", con)
            result = cmd.ExecuteScalar().ToString()

            If String.IsNullOrEmpty(result) Then
                result = "I1000"
            End If

            result = result.Substring(1)
            Int32.TryParse(result, digit)
            digit = digit + 1
            result = "I" + digit.ToString("D4")

            Dim db As New LibraryDataContext()
            Dim Log As New LogInTime()
            Log.AccountID = Login.sessionAccountID
            Log.Username = Login.sessionUser
            Log.LoginTime = DateTime.Now.Date
            Log.ID = result
            db.LogInTimes.InsertOnSubmit(Log)
            db.SubmitChanges()
        End Using
    End Sub

    Private Sub ChangePasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangePasswordToolStripMenuItem.Click
        ChangePassword.ShowDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim db As New LibraryDataContext()
        Dim user As String = "User"
        Dim isDuplicated As Boolean = db.Accounts.Any(Function(o) o.Username = Login.sessionUser)
        If isDuplicated = True Then
            Dim isUnauthorized As Boolean = db.Accounts.Any(Function(o) o.Roles = user And o.Username = Login.sessionUser)
            If isUnauthorized = True Then
                MessageBox.Show("Unauthorized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                LoginReport.Show()
            End If
        Else
        End If

    End Sub

    Private Sub Homepage_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Login.Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub
End Class