Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class Roles
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Roles_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbRoles.Items.Add("Admin")
        cmbRoles.Items.Add("User")
        BindData()
        IDload()
    End Sub

    Private Sub BindData()
        Dim db As New LibraryDataContext()
        DataGridView1.Columns.Clear()
        DataGridView1.DataSource = db.Accounts
        txtUsername.Text = ""
        msktxtPassword.Text = ""
        txtEmail.Text = ""
        cmbRoles.SelectedItem = cmbRoles.Items(1)
    End Sub

    Private Sub IDload()
        Dim digit As Integer
        Dim result As String

        Using con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Library.mdf;Integrated Security=True")

            con.Open()
            'check product id
            Dim cmd = New SqlCommand("Select MAX(AccountID) FROM Account", con)
            result = cmd.ExecuteScalar().ToString()

            If String.IsNullOrEmpty(result) Then
                result = "Z1000"
                lblAccountID.Text = result
            End If

            result = result.Substring(1)
            Int32.TryParse(result, digit)
            digit = digit + 1
            result = "Z" + digit.ToString("D4")

            lblAccountID.Text = result
        End Using
    End Sub

    Function IsValidEmailFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^[\w-.]+@([\w-]+.)+[\w-]{2,4}$")
    End Function

    Function IsValidPasswordFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")
    End Function

    Function IsValidUserNameFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^[a-zA-Z]+(\s[a-zA-Z]+)?$")
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        'HASHING PASSWORD
        'Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(msktxtPassword.Text)
        'Dim hashOfBytes() As Byte = New System.Security.Cryptography.SHA1Managed().ComputeHash(bytes)
        'Dim strHash As String = Convert.ToBase64String(hashOfBytes) 'HASH PASSWORD VALUE
        Dim errormsg As String = ""
        Dim db As New LibraryDataContext()
        Dim i As New Account()

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to add new role into the system?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            Try
                If lblAccountID.Text <> "" And cmbRoles.Text <> "" And txtUsername.Text <> "" And msktxtPassword.Text <> "" Then

                    'Dim isDuplicated As Boolean = db.Items.Any(Function(o) o.Id = i.Id)'
                    Dim isDuplicated As Boolean = db.Accounts.Any(Function(o) o.Username.ToUpper = txtUsername.Text.ToUpper)
                    Dim isDuplicatedEmail As Boolean = db.Accounts.Any(Function(o) o.Email.ToUpper = txtEmail.Text.ToUpper)
                    If isDuplicated = True Then
                        'Console.WriteLine("ID already exist!", MessageBoxIcon.Error)'
                        MessageBox.Show("Username Already Exist")
                    ElseIf isDuplicatedEmail = True Then
                        MessageBox.Show("Email Already Exist")
                    ElseIf IsValidPasswordFormat(msktxtPassword.Text) = False Then
                        MessageBox.Show("Invalid password format ! Must be more than 8 characters, must contain at least one alphabet & one number.")
                    ElseIf IsValidEmailFormat(txtEmail.Text) = False Then
                        MessageBox.Show("Invalid Email Format !")
                    ElseIf IsValidUserNameFormat(txtUsername.Text) = False Then
                        MessageBox.Show("Invalid UserName, only alphabets are allowed !")
                    Else

                        i.AccountID = lblAccountID.Text
                        i.Roles = cmbRoles.Text
                        i.Username = txtUsername.Text.ToUpper
                        i.Password = msktxtPassword.Text
                        i.Email = txtEmail.Text

                        db.Accounts.InsertOnSubmit(i)
                        db.SubmitChanges()
                        MessageBox.Show("Successfully Added!")
                        BindData()
                        txtUsername.Text = ""
                        msktxtPassword.Text = ""
                        IDload()
                    End If
                Else
                    errormsg = "Please enter all required fields"
                    MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                errormsg = "Username Already Exist"
                MessageBox.Show(errormsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try

        Else
            If ask = MsgBoxResult.No Then
                MsgBox("Action has been canceled.")
            End If
        End If

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        lblAccountID.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString()
        cmbRoles.SelectedItem = DataGridView1.CurrentRow.Cells(3).Value.ToString()
        txtUsername.Text = DataGridView1.CurrentRow.Cells(1).Value.ToString()
        msktxtPassword.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString()
        txtEmail.Text = DataGridView1.CurrentRow.Cells(4).Value.ToString()
    End Sub
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        'HASHING PASSWORD
        'Dim bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(msktxtPassword.Text)
        'Dim hashOfBytes() As Byte = New System.Security.Cryptography.SHA1Managed().ComputeHash(bytes)
        'Dim strHash As String = Convert.ToBase64String(hashOfBytes) 'HASH PASSWORD VALUE
        Dim db As New LibraryDataContext()
        Dim i As Account = db.Accounts.FirstOrDefault(Function(o) o.AccountID = lblAccountID.Text)
        Dim errormsg As String = ""

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to edit the respective role details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            If i Is Nothing Then
                'Console.WriteLine("Record Not Found!")
                MessageBox.Show("Record Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Try
                    If lblAccountID.Text <> "" And cmbRoles.Text <> "" And txtUsername.Text <> "" And msktxtPassword.Text <> "" Then

                        'Dim isDuplicated As Boolean = db.Items.Any(Function(o) o.Id = i.Id)'
                        'Dim isDuplicated As Boolean = db.Accounts.Any(Function(o) o.Username.ToUpper = txtUsername.Text.ToUpper)
                        'If isDuplicated = True Then
                        '    'Console.WriteLine("ID already exist!", MessageBoxIcon.Error)'
                        '    MessageBox.Show("Username Already Exist")
                        If IsValidPasswordFormat(msktxtPassword.Text) = False Then
                            MessageBox.Show("Invalid password format ! Must be more than 8 characters, must contain at least one alphabet & one number.")
                        ElseIf IsValidEmailFormat(txtEmail.Text) = False Then
                            MessageBox.Show("Invalid Email Format !")
                        ElseIf IsValidUserNameFormat(txtUsername.Text) = False Then
                            MessageBox.Show("Invalid UserName, only alphabets are allowed !")
                        Else
                            i.Roles = cmbRoles.Text
                            i.Username = txtUsername.Text
                            i.Password = msktxtPassword.Text
                            i.Email = txtEmail.Text
                            db.SubmitChanges()
                            MessageBox.Show("Record Updated!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            BindData()
                            IDload()
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
        cmbRoles.SelectedItem = cmbRoles.Items(1)
        txtUsername.Text = ""
        msktxtPassword.Text = ""
        BindData()
        IDload()
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim db As New LibraryDataContext()

        Dim i As Account = db.Accounts.FirstOrDefault(Function(o) o.AccountID = lblAccountID.Text)

        Dim ask As MsgBoxResult
        ask = MsgBox("Would you like to delete the selected role details?", MsgBoxStyle.YesNo, "")

        If ask = MsgBoxResult.Yes Then
            If i Is Nothing Then
                MessageBox.Show("Record Not Found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                db.Accounts.DeleteOnSubmit(i)
                db.SubmitChanges()
                MessageBox.Show("Record Deleted!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                BindData()
                cmbRoles.SelectedItem = cmbRoles.Items(1)
                txtUsername.Text = ""
                msktxtPassword.Text = ""
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
                RolesReport.Show()
            End If
        Else
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub Roles_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Response As DialogResult
        Dim strMessage As String = "Are you sure you want to exit?"


        Response = MessageBox.Show(strMessage, "", MessageBoxButtons.YesNo)

        If Response = DialogResult.No Then
            e.Cancel = True
            MessageBox.Show("Action has been canceled.", "", MessageBoxButtons.OK)
        End If

    End Sub
End Class