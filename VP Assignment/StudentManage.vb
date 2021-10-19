Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class StudentManage
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        Me.Close()

    End Sub

    Private Sub StudentManage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindData()
        IDload()
    End Sub

    Private Sub BindData()
        Dim db As New LibraryDataContext()
        DataGridView1.Columns.Clear()
        DataGridView1.DataSource = db.Students
        lblBookFine.Text = "No"
        lblCurrentStatus.Text = "Available"
    End Sub

    Private Sub IDload()
        Dim digit As Integer
        Dim result As String

        Using con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Library.mdf;Integrated Security=True")

            con.Open()
            'check product id
            Dim cmd = New SqlCommand("Select MAX(StudentID) FROM Student", con)
            result = cmd.ExecuteScalar().ToString()

            If String.IsNullOrEmpty(result) Then
                result = "S1000"
                lblStudentID.Text = result
            End If

            result = result.Substring(1)
            Int32.TryParse(result, digit)
            digit = digit + 1
            result = "S" + digit.ToString("D4")

            lblStudentID.Text = result
        End Using
    End Sub

    Function IsValidEmailFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^[\w-.]+@([\w-]+.)+[\w-]{2,4}$")
    End Function

    Function IsValidNameFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^[a-zA-Z]+(\s[a-zA-Z]+)?$")
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim errormsg As String = ""
        Dim db As New LibraryDataContext()
        Dim i As New Student()

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to add the respective student details to the system?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            Try
                If lblStudentID.Text <> "" And txtName.Text <> "" And msktxtHPNO.Text <> "" And lblCurrentStatus.Text <> "" And txtEmail.Text <> "" And lblBookFine.Text <> "" Then

                    'Dim isDuplicated As Boolean = db.Items.Any(Function(o) o.Id = i.Id)'
                    Dim isDuplicated As Boolean = db.Students.Any(Function(o) o.StudentHPNO = msktxtHPNO.Text)
                    Dim isDuplicatedEmail As Boolean = db.Students.Any(Function(o) o.StudentEmail = txtEmail.Text)

                    If isDuplicated = True Then
                        'Console.WriteLine("ID already exist!", MessageBoxIcon.Error)'
                        MessageBox.Show("Phone Number Already Exist !")
                    ElseIf isDuplicatedEmail = True Then
                        MessageBox.Show("Email Already Exist !")
                    ElseIf msktxtHPNO.MaskCompleted = False Then
                        MessageBox.Show("Wrong Phone Number !")
                    ElseIf IsValidEmailFormat(txtemail.text) = False Then
                        MessageBox.Show("Invalid Email Format !")
                    ElseIf IsValidNameFormat(txtName.Text) = False Then
                        MessageBox.Show("Invalid name, only alphabets are allowed !")
                    Else

                        i.StudentID = lblStudentID.Text
                        i.StudentName = txtName.Text
                        i.StudentHPNO = msktxtHPNO.Text
                        i.StudentStatus = lblCurrentStatus.Text
                        i.StudentEmail = txtEmail.Text
                        i.StudentFine = lblBookFine.Text


                        db.Students.InsertOnSubmit(i)
                        db.SubmitChanges()
                        MessageBox.Show("Successfully Added!")
                        BindData()
                        txtName.Text = ""
                        txtEmail.Text = ""
                        msktxtHPNO.Text = ""
                        lblBookFine.Text = "No"
                        lblCurrentStatus.Text = "Available"
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

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        lblStudentID.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString()
        txtName.Text = DataGridView1.CurrentRow.Cells(1).Value.ToString()
        msktxtHPNO.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString()
        lblCurrentStatus.Text = DataGridView1.CurrentRow.Cells(3).Value.ToString()
        txtEmail.Text = DataGridView1.CurrentRow.Cells(4).Value.ToString()
        lblBookFine.Text = DataGridView1.CurrentRow.Cells(5).Value.ToString()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim db As New LibraryDataContext()
        Dim i As Student = db.Students.FirstOrDefault(Function(o) o.StudentID = lblStudentID.Text)
        Dim errormsg As String = ""

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to edit the respective student details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            If i Is Nothing Then
                'Console.WriteLine("Record Not Found!")
                MessageBox.Show("Record Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Try
                    If lblStudentID.Text <> "" And txtName.Text <> "" And msktxtHPNO.Text <> "" And lblCurrentStatus.Text <> "" And txtEmail.Text <> "" And lblBookFine.Text <> "" Then
                        If msktxtHPNO.MaskCompleted = False Then
                            MessageBox.Show("Wrong Phone Number !")
                        ElseIf IsValidEmailFormat(txtEmail.Text) = False Then
                            MessageBox.Show("Invalid Email Format !")
                        ElseIf IsValidNameFormat(txtName.Text) = False Then
                            MessageBox.Show("Invalid name, only alphabets are allowed !")
                        Else

                            i.StudentName = txtName.Text
                            i.StudentHPNO = msktxtHPNO.Text
                            i.StudentEmail = txtEmail.Text

                            db.SubmitChanges()
                            MessageBox.Show("Record Updated!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            BindData()
                        End If
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
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtName.Text = ""
        msktxtHPNO.Text = ""
        txtEmail.Text = ""
        BindData()
        IDload()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim db As New LibraryDataContext()

        Dim i As Student = db.Students.FirstOrDefault(Function(o) o.StudentID = lblStudentID.Text)

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to delete the selected student details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            If i Is Nothing Then
                MessageBox.Show("Record Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                db.Students.DeleteOnSubmit(i)
                db.SubmitChanges()
                MessageBox.Show("Record Deleted!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                BindData()
                txtName.Text = ""
                txtEmail.Text = ""
                msktxtHPNO.Text = ""
                lblBookFine.Text = "No"
                lblCurrentStatus.Text = "Available"
                IDload()
            End If
        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
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
                StudentReport.Show()
            End If
        Else
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub StudentManage_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Response As DialogResult
        Dim strMessage As String = "Are you sure you want to exit?"


        Response = MessageBox.Show(strMessage, "", MessageBoxButtons.YesNo)

        If Response = DialogResult.No Then
            e.Cancel = True
            MessageBox.Show("Action has been canceled.", "", MessageBoxButtons.OK)
        End If

    End Sub
End Class