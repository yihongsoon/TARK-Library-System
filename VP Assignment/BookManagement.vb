Imports System.Text
Imports System.Data.SqlClient

Public Class BookManagement
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click


        'Dim Then errormsg As String = ""
        Dim errormsg As String = ""
        Dim strId As String = txtBookName.Text
        Dim db As New LibraryDataContext()
        Dim i As New Book()

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to add the new book details to the system?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then

            Try
                If txtBookID.Text <> "" And txtBookName.Text <> "" And txtBookDesc.Text <> "" And txtBookAuthor.Text <> "" And lblBookStatus.Text <> "" Then

                    'Dim isDuplicated As Boolean = db.Items.Any(Function(o) o.Id = i.Id)'
                    Dim isDuplicated As Boolean = db.Books.Any(Function(o) o.BookName = txtBookName.Text)

                    If isDuplicated = True Then
                        'Console.WriteLine("ID already exist!", MessageBoxIcon.Error)'
                        MessageBox.Show("Name Already Exist")
                    Else

                        i.BookID = txtBookID.Text
                        i.BookName = txtBookName.Text
                        i.BookDesc = txtBookDesc.Text
                        i.BookAuthor = txtBookAuthor.Text
                        i.BookStatus = "Available"


                        db.Books.InsertOnSubmit(i)
                        db.SubmitChanges()
                        MessageBox.Show("Successfully Added!")
                        LoadData()
                        txtBookName.Text = ""
                        txtBookDesc.Text = ""
                        txtBookAuthor.Text = ""
                        lblBookStatus.Text = "Available"
                        IDload()
                    End If
                Else
                    errormsg = "Please enter all required fields"
                    MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try

        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        Dim db As New LibraryDataContext()
        Dim i As Book = db.Books.FirstOrDefault(Function(o) o.BookID = txtBookID.Text)
        Dim errormsg As String = ""

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to edit the respective book details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            If i Is Nothing Then
                'Console.WriteLine("Record Not Found!")
                MessageBox.Show("Record Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Try
                    If txtBookID.Text <> "" And txtBookName.Text <> "" And txtBookDesc.Text <> "" And txtBookAuthor.Text <> "" And lblBookStatus.Text <> "" Then
                        i.BookName = txtBookName.Text
                        i.BookDesc = txtBookDesc.Text
                        i.BookAuthor = txtBookAuthor.Text
                        i.BookStatus = lblBookStatus.Text
                        db.SubmitChanges()
                        MessageBox.Show("Record Updated!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadData()
                    Else
                        errormsg = "Please enter all required fields"
                        MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If

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

    Private Sub IDload()
        Dim digit As Integer
        Dim result As String

        Using con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Library.mdf;Integrated Security=True")

            con.Open()
            'check product id
            Dim cmd = New SqlCommand("Select MAX(BookID) FROM Book", con)
            result = cmd.ExecuteScalar().ToString()

            If String.IsNullOrEmpty(result) Then
                result = "P1000"
                txtBookID.Text = result
            End If

            result = result.Substring(1)
            Int32.TryParse(result, digit)
            digit = digit + 1
            result = "P" + digit.ToString("D4")

            txtBookID.Text = result
        End Using
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim db As New LibraryDataContext()
        DataGridView1.DataSource = db.Books


        IDload()
        lblBookStatus.Text = "Available"
    End Sub

    Private Sub LoadData()
        'load the data grid view
        DataGridView1.Columns.Clear()

        Dim db As New LibraryDataContext()
        DataGridView1.DataSource = db.Books
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()

    End Sub

    Private Sub BindData()
        'search for name
        Dim db As New LibraryDataContext()

        Dim rs = From o In db.Books Where o.BookName.Contains(txtBookNameSearch.Text) And o.BookAuthor.Contains(txtAuthorSearch.Text)
        DataGridView1.DataSource = rs
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        BindData()
    End Sub
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtBookName.Text = ""
        txtBookDesc.Text = ""
        txtBookAuthor.Text = ""
        lblBookStatus.Text = "Available"
        LoadData()
        IDload()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim db As New LibraryDataContext()

        Dim i As Book = db.Books.FirstOrDefault(Function(o) o.BookID = txtBookID.Text)

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to delete the selected book details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            If i Is Nothing Then
                MessageBox.Show("Record Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                db.Books.DeleteOnSubmit(i)
                db.SubmitChanges()
                MessageBox.Show("Record Deleted!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadData()
                txtBookName.Text = ""
                txtBookDesc.Text = ""
                txtBookAuthor.Text = ""
                lblBookStatus.Text = "Available"
                IDload()
            End If
        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        txtBookID.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString()
        txtBookName.Text = DataGridView1.CurrentRow.Cells(1).Value.ToString()
        txtBookDesc.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString()
        txtBookAuthor.Text = DataGridView1.CurrentRow.Cells(3).Value.ToString()
        lblBookStatus.Text = DataGridView1.CurrentRow.Cells(4).Value.ToString()
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
                BookReport.Show()
            End If
        Else
        End If
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) 
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub BookManagement_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Response As DialogResult
        Dim strMessage As String = "Are you sure you want to exit?"


        Response = MessageBox.Show(strMessage, "", MessageBoxButtons.YesNo)

        If Response = DialogResult.No Then
            e.Cancel = True
            MessageBox.Show("Action has been canceled.", "", MessageBoxButtons.OK)
        End If

    End Sub
End Class