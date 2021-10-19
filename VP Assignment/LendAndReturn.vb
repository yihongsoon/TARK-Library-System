Imports System.Data.SqlClient

Public Class LendAndReturn
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub LendAndReturn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindData()
        IDload()

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
        'DataGridView1.DataSource = db.LendReturns

        Dim strSql As String
        If OpenConnection() = True Then
            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
            strSql = "Select LendReturnID, BookID, StudentID,LendFrom, LendTo, ReturnDate, OverdueDate, AccountID  From LendReturn"
            da = New SqlDataAdapter(strSql, conn)
            ds.Clear()
            da.Fill(ds, "LendReturn")

            If ds.Tables("LendReturn").Rows.Count > 0 Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = "LendReturn"
            End If
            CloseConnection()
        Else
            MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        End If
        dateFrom.Format = DateTimePickerFormat.Short
        dateTo.Format = DateTimePickerFormat.Short
        dateReturn.Value = DateTimePicker.MinimumDateTime
        txtBookID.Text = ""
        txtStudentID.Text = ""
        dateReturn.Enabled = False
    End Sub

    Private Sub IDload()
        Dim digit As Integer
        Dim result As String

        Using con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Library.mdf;Integrated Security=True")

            con.Open()
            'check product id
            Dim cmd = New SqlCommand("Select MAX(LendReturnID) FROM LendReturn", con)
            result = cmd.ExecuteScalar().ToString()

            If String.IsNullOrEmpty(result) Then
                result = "L1000"
                lblLendReturnID.Text = result
            End If

            result = result.Substring(1)
            Int32.TryParse(result, digit)
            digit = digit + 1
            result = "L" + digit.ToString("D4")

            lblLendReturnID.Text = result
        End Using
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim errormsg As String = ""
        Dim db As New LibraryDataContext()
        Dim i As New LendReturn()
        Dim afterStudent As Student = db.Students.FirstOrDefault(Function(o) o.StudentID = txtStudentID.Text)
        Dim afterBook As Book = db.Books.FirstOrDefault(Function(o) o.BookID = txtBookID.Text)

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to add new student's lend & return details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then

            Try
                If lblLendReturnID.Text <> "" And txtBookID.Text <> "" And txtStudentID.Text <> "" And dateFrom.Text <> "" And dateTo.Text <> "" Then
                    Dim datediff As Integer = CInt((dateTo.Value.Date - dateFrom.Value.Date).Days)
                    'Dim isDuplicated As Boolean = db.Items.Any(Function(o) o.Id = i.Id)'
                    Dim isDuplicated As Boolean = db.LendReturns.Any(Function(o) o.LendReturnID = lblLendReturnID.Text)
                    If isDuplicated = True Then
                        MessageBox.Show("ID Already Existed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ElseIf datediff < 0 Then
                        MessageBox.Show("Invalid Date!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    Else
                        Dim forBook As Book = db.Books.FirstOrDefault(Function(o) o.BookID = txtBookID.Text)
                        Dim forStudent As Student = db.Students.FirstOrDefault(Function(o) o.StudentID = txtStudentID.Text)
                        Dim isDuplicatedBookStatus As Boolean = db.Books.Any(Function(o) o.BookStatus = "Lending" And o.BookID = txtBookID.Text)
                        Dim isDuplicatedStudentStatus As Boolean = db.Students.Any(Function(o) o.StudentStatus = "Lending" And o.StudentID = txtStudentID.Text)
                        Dim isDuplicatedStudentFine As Boolean = db.Students.Any(Function(o) o.StudentFine = "Yes" And o.StudentID = txtStudentID.Text)
                        If forBook Is Nothing Then
                            'Console.WriteLine("Record Not Found!")
                            MessageBox.Show("Book Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ElseIf forStudent Is Nothing Then
                            MessageBox.Show("Student Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ElseIf isDuplicatedBookStatus = True Then
                            MessageBox.Show("Book Already Lended", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        ElseIf isDuplicatedStudentStatus = True Then
                            MessageBox.Show("Student Already Lending Book", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        ElseIf isDuplicatedStudentFine = True Then
                            MessageBox.Show("Student Need to Pay Book Fine First!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else

                            i.LendReturnID = lblLendReturnID.Text
                            i.BookID = txtBookID.Text
                            i.StudentID = txtStudentID.Text
                            i.LendFrom = dateFrom.Value
                            i.LendTo = dateTo.Value
                            i.ReturnDate = dateReturn.Value
                            i.OverdueDate = lblOverdue.Text
                            i.AccountID = Login.sessionAccountID
                            afterBook.BookStatus = "Lending"
                            afterStudent.StudentStatus = "Lending"
                            db.LendReturns.InsertOnSubmit(i)
                            db.SubmitChanges()
                            MessageBox.Show("Successfully Added!")
                            txtBookID.Text = ""
                            txtStudentID.Text = ""
                            lblOverdue.Text = ""
                            BindData()
                            IDload()
                            txtBookID.Enabled = True
                            txtStudentID.Enabled = True
                            dateFrom.Enabled = True
                            dateTo.Enabled = True

                        End If
                    End If
                Else
                    errormsg = "Please enter all required fields"
                    MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try

        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
        End If

    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick 
        lblLendReturnID.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString()
        txtBookID.Text = DataGridView1.CurrentRow.Cells(1).Value.ToString()
        txtStudentID.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString()
        dateFrom.Value = CDate(DataGridView1.CurrentRow.Cells(3).Value)
        dateTo.Value = CDate(DataGridView1.CurrentRow.Cells(4).Value)
        dateReturn.Value = CDate(DataGridView1.CurrentRow.Cells(5).Value)
        lblOverdue.Text = CStr(DataGridView1.CurrentRow.Cells(6).Value)
        txtBookID.Enabled = False
        txtStudentID.Enabled = False
        dateFrom.Enabled = False
        dateTo.Enabled = False
        dateReturn.Enabled = True
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim db As New LibraryDataContext()
        Dim i As LendReturn = db.LendReturns.FirstOrDefault(Function(o) o.LendReturnID = lblLendReturnID.Text)
        Dim afterStudent As Student = db.Students.FirstOrDefault(Function(o) o.StudentID = txtStudentID.Text)
        Dim afterBook As Book = db.Books.FirstOrDefault(Function(o) o.BookID = txtBookID.Text)
        Dim overdue As New BookFine()
        Dim afterOverdue As BookFine = db.BookFines.FirstOrDefault(Function(o) o.LendReturnID = lblLendReturnID.Text)
        Dim overdueEdit As Boolean = db.BookFines.Any(Function(o) o.LendReturnID = lblLendReturnID.Text)

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to edit the respective student's lend & return details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then

            If i Is Nothing Then
                'Console.WriteLine("Record Not Found!")
                MessageBox.Show("Record Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Try

                    afterBook.BookStatus = "Available"
                    afterStudent.StudentStatus = "Available"
                    i.ReturnDate = dateReturn.Value
                    Dim datediff As Integer = CInt((dateReturn.Value.Date - dateTo.Value.Date).Days)
                    If datediff = 0 Then
                        i.OverdueDate = datediff.ToString()
                        afterStudent.StudentFine = "No"
                        If overdueEdit = True Then
                            Dim deleteBookFine As BookFine = db.BookFines.FirstOrDefault(Function(o) o.LendReturnID = lblLendReturnID.Text)
                            db.BookFines.DeleteOnSubmit(deleteBookFine)

                        End If
                        db.SubmitChanges()
                    ElseIf datediff >= 1 Then

                        i.OverdueDate = datediff.ToString()
                        afterStudent.StudentFine = "Yes"
                        Dim digit As Integer
                        Dim result As String

                        Using con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Library.mdf;Integrated Security=True")

                            con.Open()
                            'check product id
                            Dim cmd = New SqlCommand("Select MAX(BookFineID) FROM BookFine", con)
                            result = cmd.ExecuteScalar().ToString()

                            If String.IsNullOrEmpty(result) Then
                                result = "F1000"
                            End If

                            result = result.Substring(1)
                            Int32.TryParse(result, digit)
                            digit = digit + 1
                            result = "F" + digit.ToString("D4")


                        End Using

                        overdue.BookFineID = result
                        overdue.StudentID = txtStudentID.Text
                        overdue.BookID = txtBookID.Text
                        overdue.OverDueDate = datediff
                        overdue.BookFineStatus = "Unpaid"
                        overdue.LendReturnID = lblLendReturnID.Text
                        db.BookFines.InsertOnSubmit(overdue)
                        db.SubmitChanges()
                    ElseIf datediff < 0 Then
                        MessageBox.Show("Invalid Date!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    db.SubmitChanges()
                    MessageBox.Show("Record Updated!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    BindData()
                    IDload()
                    txtBookID.Enabled = True
                    txtStudentID.Enabled = True
                    dateFrom.Enabled = True
                    dateTo.Enabled = True
                    lblOverdue.Text = ""
                Catch ex As Exception
                    MessageBox.Show("Please fill in the blank/ Insert wrong data type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            End If

        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
        End If
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtBookID.Text = ""
        txtStudentID.Text = ""
        lblOverdue.Text = ""
        BindData()
        IDload()
        txtBookID.Enabled = True
        txtStudentID.Enabled = True
        dateFrom.Enabled = True
        dateTo.Enabled = True
        dateFrom.ResetText()
        dateTo.ResetText()
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
                LendReturnReport.Show()
            End If
        Else
        End If
    End Sub


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'Dim strSql As String
        'If OpenConnection() = True Then
        '    'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
        '    strSql = ""

        '    da = New SqlDataAdapter(strSql, conn)
        '    ds.Clear()
        '    da.Fill(ds, "LendReturn")

        '    If ds.Tables("LendReturn").Rows.Count > 0 Then
        '        DataGridView1.DataSource = ds
        '        DataGridView1.DataMember = "LendReturn"
        '    End If
        '    CloseConnection()
        'Else
        '    MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        'End If
        Dim datediff As Integer = CInt((dateToSearch.Value.Date - dateFromSearch.Value.Date).Days)
        If datediff < 0 Then
            MessageBox.Show("Invalid Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Else
            Dim strsql As String
            If OpenConnection() = True Then
                'strsql = "select bookfineid, overduedate, finevalue, bookfinestatus, bookid, studentid, lendreturnid from bookfine where bookfinestatus = 'unpaid'"
                strsql = "Select LendReturnID, BookID, StudentID,LendFrom, LendTo, ReturnDate, OverdueDate, AccountID  From LendReturn Where StudentID LIKE '%" & txtSearch.Text & "%' AND LendTo BETWEEN @dateFrom And @dateTo AND LendFrom BETWEEN @dateFromF and @dateToF"
                Dim cmd As New SqlCommand(strsql, conn)
                cmd.Parameters.AddWithValue("@dateFrom", dateFromSearch.Value)
                cmd.Parameters.AddWithValue("@dateTo", dateToSearch.Value)
                cmd.Parameters.AddWithValue("@dateFromF", dateFromSearch.Value)
                cmd.Parameters.AddWithValue("@dateToF", dateToSearch.Value)
                da.SelectCommand = cmd
                ds.Clear()
                da.Fill(ds, "LendReturn")

                If ds.Tables("LendReturn").Rows.Count > 0 Then
                    DataGridView1.DataSource = ds
                    DataGridView1.DataMember = "LendReturn"
                End If
                CloseConnection()
            Else
                MessageBox.Show("error in connecting to database server", "error", MessageBoxButtons.OK)
            End If
        End If

    End Sub



    Private Sub dateReturn_ValueChanged(sender As Object, e As EventArgs) Handles dateReturn.ValueChanged
        If dateReturn.Value = DateTimePicker.MinimumDateTime Then
            dateReturn.Value = DateTime.Now
            dateReturn.Format = DateTimePickerFormat.Custom
            dateReturn.CustomFormat = " "
        Else
            dateReturn.Format = DateTimePickerFormat.Short
        End If

    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtSearch.Text = ""
        dateFromSearch.ResetText()
        dateToSearch.ResetText()
        Dim strSql As String
        If OpenConnection() = True Then
            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
            strSql = "Select LendReturnID, BookID, StudentID,LendFrom, LendTo, ReturnDate, OverdueDate, AccountID  From LendReturn"

            da = New SqlDataAdapter(strSql, conn)
            ds.Clear()
            da.Fill(ds, "LendReturn")

            If ds.Tables("LendReturn").Rows.Count > 0 Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = "LendReturn"
            End If
            CloseConnection()
        Else
            MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub LendAndReturn_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Response As DialogResult
        Dim strMessage As String = "Are you sure you want to exit?"


        Response = MessageBox.Show(strMessage, "", MessageBoxButtons.YesNo)

        If Response = DialogResult.No Then
            e.Cancel = True
            MessageBox.Show("Action has been canceled.", "", MessageBoxButtons.OK)
        End If

    End Sub
End Class