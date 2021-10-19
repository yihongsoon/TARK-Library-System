Imports System.Data.SqlClient

Public Class Login
    Public sessionUser As String
    Public sessionAccountID As String
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        Dim username As String = txtUsername.Text
        Dim password As String = msktxtPassword.Text
        'HASHING PASSWORD
        'Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(password)
        'Dim hashOfBytes() As Byte = New System.Security.Cryptography.SHA1Managed().ComputeHash(bytes)
        'Dim strHash As String = Convert.ToBase64String(hashOfBytes) 'HASH PASSWORD VALUE
        Dim db As New LibraryDataContext()
        Dim i As New Account()
        Try
            Dim isValid As Boolean = db.Accounts.Any(Function(o) o.Username.ToUpper = username.ToUpper)
            Dim isValidPassword As Boolean = db.Accounts.Any(Function(o) o.Password = msktxtPassword.Text)
            If isValid Then
                If isValidPassword Then


                    sessionUser = username.ToUpper
                    If OpenConnection() = True Then
                        'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
                        Dim strSql As New SqlCommand("Select AccountID From Account Where Username='" + sessionUser + "' ", conn)
                        ds = strSql.ExecuteReader
                        If ds.HasRows Then
                            While ds.Read
                                sessionAccountID = ds.Item("AccountID").ToString().ToUpper
                                Homepage.Show()
                                Me.Hide()
                            End While
                        End If

                        CloseConnection()
                    Else
                        MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
                    End If


                Else
                    MessageBox.Show("Wrong Username or Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("Wrong Username or Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If


        Catch ex As Exception
            MessageBox.Show("Wrong Username or Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try



    End Sub

    Public conn As New System.Data.SqlClient.SqlConnection
    Dim ds As SqlDataReader
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

    Private Sub lnklblForgetPassword_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnklblForgetPassword.LinkClicked
        ForgetPassword.ShowDialog()
    End Sub
End Class