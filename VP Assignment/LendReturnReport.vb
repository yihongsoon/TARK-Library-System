Imports System.Data.SqlClient

Public Class LendReturnReport
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        doc.DefaultPageSettings.Landscape = True
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
        e.Graphics.DrawString("Summary Lend/Return Report", fntHeading, Brushes.Black, 50, sngYLocation)
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
        e.Graphics.DrawString("Lend/Return ID", fntHeading, Brushes.Black, 50, sngYLocation)
        e.Graphics.DrawString("Book ID", fntHeading, Brushes.Black, 200, sngYLocation)
        e.Graphics.DrawString("Student ID", fntHeading, Brushes.Black, 350, sngYLocation)
        e.Graphics.DrawString("Lend From", fntHeading, Brushes.Black, 500, sngYLocation)
        e.Graphics.DrawString("Lend To", fntHeading, Brushes.Black, 650, sngYLocation)
        e.Graphics.DrawString("Return Date", fntHeading, Brushes.Black, 850, sngYLocation)
        e.Graphics.DrawString("Overdue Date", fntHeading, Brushes.Black, 1000, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("===============================================================================================", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        'set up detail
        'Do

        For i As Integer = 0 To DataGridView1.Rows.Count - 1

            e.Graphics.DrawString(CType(DataGridView1.Item(0, i).Value, String), fntPrint, Brushes.Black, 50, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(1, i).Value, String), fntPrint, Brushes.Black, 200, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(2, i).Value, String), fntPrint, Brushes.Black, 350, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(3, i).Value, String), fntPrint, Brushes.Black, 500, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(4, i).Value, String), fntPrint, Brushes.Black, 650, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(5, i).Value, String), fntPrint, Brushes.Black, 850, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(6, i).Value, String), fntPrint, Brushes.Black, 1000, sngYLocation)
            sngYLocation += sngLineHeight
        Next
        e.Graphics.DrawString("--- End ---", fntPrint, Brushes.Black, 50, sngYLocation)
        'Loop Until sngYLocation >= e.MarginBounds.Bottom

        'intCountPage += 1
        'If intCountPage <= 2 Then
        '    e.HasMorePages = True
        'Else
        '    e.HasMorePages = False
        '    intCountPage = 1
        'End If
        '--------------End of Print multiple pages-------------

    End Sub

    Private Sub LendReturnReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        BindData()
        dateFrom.ResetText()
        dateTo.ResetText()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'Dim strSql As String
        'If OpenConnection() = True Then
        '    'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
        '    strSql = "Select LendReturnID, BookID, StudentID,LendFrom, LendTo, ReturnDate, OverdueDate, AccountID  From ReturnDate Where StudentID LIKE '%" & txtUsername.Text & "%'"

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
        Dim datediff As Integer = CInt((dateTo.Value.Date - dateFrom.Value.Date).Days)
        If datediff < 0 Then
            MessageBox.Show("Invalid Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        Else
            Dim strsql As String
            If OpenConnection() = True Then
                'strsql = "select bookfineid, overduedate, finevalue, bookfinestatus, bookid, studentid, lendreturnid from bookfine where bookfinestatus = 'unpaid'"
                strsql = "Select LendReturnID, BookID, StudentID,LendFrom, LendTo, ReturnDate, OverdueDate, AccountID  From LendReturn Where ReturnDate BETWEEN @dateFrom And @dateTo"
                Dim cmd As New SqlCommand(strsql, conn)
                cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Value)
                cmd.Parameters.AddWithValue("@dateTo", dateTo.Value)
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
End Class