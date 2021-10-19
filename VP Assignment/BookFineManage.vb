Imports System.Data.SqlClient

Public Class BookFineManage
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub BookFineManage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindData()
    End Sub

    Public conn As New System.Data.SqlClient.SqlConnection
    Private ds As DataSet = New DataSet()
    Private da As SqlDataAdapter
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
    Private Sub BindData()
        'Dim db As New LibraryDataContext()
        'DataGridView1.Columns.Clear()
        'Dim rs = From o In db.BookFines Where o.BookFineStatus.Contains("Unpaid")
        'DataGridView1.DataSource = rs


        Dim strSql As String
        If OpenConnection() = True Then
            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
            strSql = "Select BookFineID,LendReturnID, BookID, StudentID,OverDueDate, FineValue, BookFineStatus  From BookFine Where BookFineStatus Like '%Unpaid%'"
            da = New SqlDataAdapter(strSql, conn)
            ds.Clear()
            da.Fill(ds, "BookFine")

            If ds.Tables("BookFine").Rows.Count > 0 Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = "BookFine"
            End If
            CloseConnection()
        Else
            MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        End If
        lblFineID.Text = ""
        lblStudentID.Text = ""
        lblBookID.Text = ""
        lblOverdueDateTime.Text = ""
        lblFineValue.Text = ""
        lblBookFineStatus.Text = ""
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        Dim strSql As String
        If OpenConnection() = True Then
            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
            strSql = "Select BookFineID,LendReturnID, BookID, StudentID,OverDueDate, FineValue, BookFineStatus  From BookFine Where StudentID LIKE '%" & txtStudentID.Text & "%' And BookFineStatus LIKE 'Unpaid%' "
            da = New SqlDataAdapter(strSql, conn)
            ds.Clear()
            da.Fill(ds, "BookFine")

            If ds.Tables("BookFine").Rows.Count > 0 Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = "BookFine"
            End If
            CloseConnection()
        Else
            MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        End If
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        lblFineID.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString()
        lblStudentID.Text = DataGridView1.CurrentRow.Cells(3).Value.ToString()
        lblBookID.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString()
        lblOverdueDateTime.Text = DataGridView1.CurrentRow.Cells(4).Value.ToString()
        lblBookFineStatus.Text = DataGridView1.CurrentRow.Cells(6).Value.ToString()

    End Sub

    Private Sub btnCalculate_Click(sender As Object, e As EventArgs) Handles btnCalculate.Click
        If lblOverdueDateTime.Text = "" Then
            MessageBox.Show("No Fine Selected!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim db As New LibraryDataContext()
            Dim i As BookFine = db.BookFines.FirstOrDefault(Function(o) o.OverDueDate = CDec(lblOverdueDateTime.Text) And o.BookFineID = lblFineID.Text)
            lblFineValue.Text = CStr(i.OverDueDate * 0.2)
        End If

    End Sub

    Private Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        If lblOverdueDateTime.Text = "" Then
            MessageBox.Show("No Fine Selected!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim db As New LibraryDataContext()
            Dim afterStudent As Student = db.Students.FirstOrDefault(Function(o) o.StudentID = lblStudentID.Text And o.StudentFine.Contains("Yes"))
            Dim afterBookFine As BookFine = db.BookFines.FirstOrDefault(Function(o) o.BookFineID = lblFineID.Text And o.BookFineStatus.Contains("Unpaid"))
            afterBookFine.BookFineStatus = "Paid"
            afterStudent.StudentFine = "No"
            db.SubmitChanges()
            BindData()
        End If
    End Sub

    Private Sub PrintReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintReportToolStripMenuItem.Click

        Dim db As New LibraryDataContext()
        Dim user As String = "User"
        Dim isDuplicated As Boolean = db.Accounts.Any(Function(o) o.Username = Login.sessionUser)
        If isDuplicated = True Then
            Dim isUnauthorized As Boolean = db.Accounts.Any(Function(o) o.Roles = user And o.Username = Login.sessionUser)
            If isUnauthorized = True Then
                MessageBox.Show("Unauthorized", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                BookFineReport.Show()
            End If
        Else
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub BookFineManage_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Response As DialogResult
        Dim strMessage As String = "Are you sure you want to exit?"


        Response = MessageBox.Show(strMessage, "", MessageBoxButtons.YesNo)

        If Response = DialogResult.No Then
            e.Cancel = True
            MessageBox.Show("Action has been canceled.", "", MessageBoxButtons.OK)
        End If

    End Sub
End Class