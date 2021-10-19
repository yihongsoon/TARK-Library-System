Imports System.Net.Mail
Imports System.Data.SqlClient
Public Class ForgetPassword
    Public sessionPassword As String
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
    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click

        If OpenConnection() = True Then
            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
            Dim strSql As New SqlCommand("Select Password From Account Where Username = '" + txtUsername.Text + "' and Email ='" + txtEmail.Text + "'  ", conn)
            ds = strSql.ExecuteReader
            If ds.HasRows Then
                While ds.Read
                    sessionPassword = ds.Item("Password").ToString()
                End While
            End If

            CloseConnection()
        Else
            MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        End If

        Dim db As New LibraryDataContext()
        Dim i As New Account()
        Dim isValid As Boolean = db.Accounts.Any(Function(o) o.Username.ToUpper = txtUsername.Text.ToUpper And o.Email.ToUpper = txtEmail.Text.ToUpper)
        If isValid = True Then

            Try
                Dim SmtpServer As New SmtpClient()
                Dim mail As New MailMessage()
                SmtpServer.Credentials = New System.Net.NetworkCredential("losblancostarc@gmail.com", "kolibrilimdingdong")
                SmtpServer.Port = 587
                SmtpServer.Host = "smtp.gmail.com"
                SmtpServer.EnableSsl = True
                mail = New MailMessage()
                mail.From = New MailAddress("losblancostarc@gmail.com", "TARK Library System")
                mail.To.Add(txtEmail.Text.ToString())
                mail.Subject = "Password Recovery"
                mail.Body = "This is your Password : " & sessionPassword.ToString()
                SmtpServer.Send(mail)
                MessageBox.Show("Password has send to your email!")
                Me.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message)

            End Try
        Else
            'Console.WriteLine("ID already exist!", MessageBoxIcon.Error)'
            MessageBox.Show("Username or Email Not Found")

        End If
    End Sub
End Class