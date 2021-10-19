Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class ChangePassword

    Public conn As New System.Data.SqlClient.SqlConnection
    Dim ds As SqlDataReader
    Private Sub txtContinue_Click(sender As Object, e As EventArgs) Handles txtContinue.Click
        Dim db As New LibraryDataContext()
        Dim ask As MsgBoxResult
        ask = MsgBox("Are you sure you want to change your password?", MsgBoxStyle.YesNo, "")
        Dim isExist As Boolean = db.Accounts.Any(Function(o) o.Password = txtOldPassword.Text)
        If ask = MsgBoxResult.Yes Then
            If IsValidPasswordFormat(txtNewPassword.Text) = False Then
                MessageBox.Show("Invalid new password format ! Must be more than 8 characters, must contain at least one alphabet & one number.")
            ElseIf isExist = False Then
                MessageBox.Show("Wrong Old Password", "Error", MessageBoxButtons.OK)
            ElseIf txtConfirmPassword.Text <> txtNewPassword.Text Then
                MessageBox.Show("Please enter the same confirm password as the new password !")
            Else
                If OpenConnection() = True Then
                    Dim strSql2 As New SqlCommand("UPDATE Account SET Password = '" + txtNewPassword.Text + "' WHERE Password = '" + txtOldPassword.Text + "'", conn)
                    ds = strSql2.ExecuteReader
                    MessageBox.Show("Password Change, Please Login Again!", "Information", MessageBoxButtons.OK)
                    CloseConnection()

                End If
                Homepage.Close()
                Me.Close()
            End If
        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
        End If

    End Sub

    Function IsValidPasswordFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")
    End Function


    Public Function OpenConnection() As Boolean
        Dim strMySQLConnectionString As String

        strMySQLConnectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Library.mdf;Integrated Security=True"
        Try
            conn.ConnectionString = strMySQLConnectionString
            conn.Open()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub CloseConnection()
        conn.Close()
    End Sub
End Class