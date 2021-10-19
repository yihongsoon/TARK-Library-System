Imports System.Data.SqlClient

Public Class LoginReport

    Private Sub LoginReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TopMost = True
        BindData()
    End Sub

    Public conn As New System.Data.SqlClient.SqlConnection
    Private ds As DataSet = New DataSet()
    Private da As SqlDataAdapter
    Private dr As SqlDataReader
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
        Dim strSql As String
        If OpenConnection() = True Then
            strSql = "Select AccountID, Username, LoginTime From LogInTime"
            da = New SqlDataAdapter(strSql, conn)
            ds.Clear()
            da.Fill(ds, "LogInTime")

            If ds.Tables("LogInTime").Rows.Count > 0 Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = "LogInTime"
            End If
            CloseConnection()
        Else
            MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        End If
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        dlgPreview.Document = doc
        dlgPreview.ShowDialog(Me)
    End Sub
    Private Sub doc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles doc.PrintPage
        '--------------Print multiple pages-------------
        Dim fntPrint As New Font("Arial", 12)
        Dim fntHeading As New Font("Arial", 14, FontStyle.Bold)
        Dim sngLineHeight As Single = fntPrint.GetHeight + 2
        Dim sngXLocation As Single = e.MarginBounds.Left
        Dim sngYLocation As Single = e.MarginBounds.Top
        Dim todayDate As Date
        Static intCountPage As Integer = 1


        'set up header
        e.Graphics.DrawImage(My.Resources.tarklogo, 30, 10, 100, 100)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("Summary Log In Report", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("====================", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("Prepare by TARK Library System", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("Date : " & todayDate.Now.ToString("dd/MM/yyyy "), fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("Page : " & intCountPage, fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("Account ID", fntHeading, Brushes.Black, 50, sngYLocation)
        e.Graphics.DrawString("Username", fntHeading, Brushes.Black, 200, sngYLocation)
        e.Graphics.DrawString("Log In Time", fntHeading, Brushes.Black, 350, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("=================================================================", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        'set up detail
        'Do

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            e.Graphics.DrawString(CType(DataGridView1.Item(0, i).Value, String), fntPrint, Brushes.Black, 50, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(1, i).Value, String), fntPrint, Brushes.Black, 200, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(2, i).Value, String), fntPrint, Brushes.Black, 350, sngYLocation)
            'e.Graphics.DrawString(CType(DataGridView1.Item(5, i).Value, String), fntPrint, Brushes.Black, 480, sngYLocation)
            sngYLocation += sngLineHeight
        Next

        'Loop Until sngYLocation >= e.MarginBounds.Bottom

        'intCountPage += 1
        'If intCountPage <= 2 Then
        '    e.HasMorePages = True
        'Else
        '    e.HasMorePages = False
        '    intCountPage = 1
        'End If
        '--------------End of Print multiple pages-------------
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("--- End ---", fntPrint, Brushes.Black, 50, sngYLocation)
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        '        Dim strSql As String
        '        If OpenConnection() = True Then
        '            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
        '            strSql = "Select AccountID, Username, LoginTime From LogInTime Where (LoginTime BETWEEN '" + dateFrom.Value.ToString() + "' AND '" + dateTo.Value.ToString() + "') OR 
        '(LoginTime >= '" + dateFrom.Value.ToString() + "' AND LoginTime <= '" + dateTo.Value.ToString() + "')"

        '            da = New SqlDataAdapter(strSql, conn)
        '            ds.Clear()
        '            da.Fill(ds, "LogInTime")

        '            If ds.Tables("LogInTime").Rows.Count > 0 Then
        '                DataGridView1.DataSource = ds
        '                DataGridView1.DataMember = "LogInTime"
        '            End If
        '            CloseConnection()
        '        Else
        '            MessageBox.Show("Error in connecting to database server", "Error", MessageBoxButtons.OK)
        '        End If
        Dim datediff As Integer = CInt((dateTo.Value.Date - dateFrom.Value.Date).Days)
        If datediff < 0 Then
            MessageBox.Show("Invalid Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Else
            Dim strsql As String
        If OpenConnection() = True Then
            'strsql = "select bookfineid, overduedate, finevalue, bookfinestatus, bookid, studentid, lendreturnid from bookfine where bookfinestatus = 'unpaid'"
            strsql = "select accountid, username, logintime from logintime where logintime BETWEEN @dateFrom And @dateTo"
            Dim cmd As New SqlCommand(strsql, conn)
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Value)
            cmd.Parameters.AddWithValue("@dateTo", dateTo.Value)
            da.SelectCommand = cmd
            ds.Clear()
            da.Fill(ds, "LogInTime")

            If ds.Tables("LogInTime").Rows.Count > 0 Then
                DataGridView1.DataSource = ds
                DataGridView1.DataMember = "LogInTime"
            End If
            CloseConnection()
        Else
            MessageBox.Show("error in connecting to database server", "error", MessageBoxButtons.OK)
        End If
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        BindData()
        dateFrom.ResetText()
        dateTo.ResetText()
    End Sub
End Class