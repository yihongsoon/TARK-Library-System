Imports System.Data.SqlClient

Public Class BookFineReport
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
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim strSql As String
        If OpenConnection() = True Then
            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
            strSql = "Select BookFineID,LendReturnID, BookID, StudentID,OverDueDate, FineValue, BookFineStatus  From BookFine Where StudentID LIKE '%" & txtStudentID.Text & "%'"
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

    Private Sub BookFineReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub
    Private Sub LoadData()
        Dim strSql As String
        If OpenConnection() = True Then
            'strSql = "Select BookFineID, OverDueDate, FineValue, BookFineStatus, BookID, StudentID, LendReturnID From BookFine Where BookFineStatus = 'Unpaid'"
            strSql = "Select BookFineID,LendReturnID, BookID, StudentID,OverDueDate, FineValue, BookFineStatus  From BookFine"
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

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtStudentID.Text = ""
        LoadData()
    End Sub

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
        e.Graphics.DrawString("Summary Book Fine Report", fntHeading, Brushes.Black, 50, sngYLocation)
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
        e.Graphics.DrawString("Book Fine ID", fntHeading, Brushes.Black, 50, sngYLocation)
        e.Graphics.DrawString("Lend Return ID", fntHeading, Brushes.Black, 200, sngYLocation)
        e.Graphics.DrawString("Book ID", fntHeading, Brushes.Black, 355, sngYLocation)
        e.Graphics.DrawString("Student ID", fntHeading, Brushes.Black, 500, sngYLocation)
        e.Graphics.DrawString("OverDue Date", fntHeading, Brushes.Black, 650, sngYLocation)
        e.Graphics.DrawString("Fine Value", fntHeading, Brushes.Black, 800, sngYLocation)
        e.Graphics.DrawString("Book Fine Status", fntHeading, Brushes.Black, 950, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("=============================================================================================", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        'set up detail
        'Do

        For i As Integer = 0 To DataGridView1.Rows.Count - 1

            e.Graphics.DrawString(CType(DataGridView1.Item(0, i).Value, String), fntPrint, Brushes.Black, 50, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(1, i).Value, String), fntPrint, Brushes.Black, 200, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(2, i).Value, String), fntPrint, Brushes.Black, 355, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(3, i).Value, String), fntPrint, Brushes.Black, 500, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(4, i).Value, String), fntPrint, Brushes.Black, 650, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(5, i).Value, String), fntPrint, Brushes.Black, 800, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(6, i).Value, String), fntPrint, Brushes.Black, 950, sngYLocation)
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
End Class