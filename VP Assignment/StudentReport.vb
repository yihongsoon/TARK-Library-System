Public Class StudentReport
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        dlgPreview.Document = doc
        dlgPreview.ShowDialog(Me)
    End Sub

    Private Sub StudentReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BindData()
    End Sub
    Private Sub BindData()
        Dim db As New LibraryDataContext()
        DataGridView1.Columns.Clear()
        DataGridView1.DataSource = db.Students
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
        e.Graphics.DrawString("Summary Student Report", fntHeading, Brushes.Black, 50, sngYLocation)
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
        e.Graphics.DrawString("Student ID", fntHeading, Brushes.Black, 50, sngYLocation)
        e.Graphics.DrawString("Student Name", fntHeading, Brushes.Black, 150, sngYLocation)
        e.Graphics.DrawString("Phone Number", fntHeading, Brushes.Black, 300, sngYLocation)
        e.Graphics.DrawString("Student Email", fntHeading, Brushes.Black, 500, sngYLocation)
        e.Graphics.DrawString("Student Fine", fntHeading, Brushes.Black, 700, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("====================================================================", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        'set up detail
        'Do

        For i As Integer = 0 To DataGridView1.Rows.Count - 1

            e.Graphics.DrawString(CType(DataGridView1.Item(0, i).Value, String), fntPrint, Brushes.Black, 50, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(1, i).Value, String), fntPrint, Brushes.Black, 150, sngYLocation)
            'e.Graphics.DrawString(CType(DataGridView1.Item(3, i).Value, String), fntPrint, Brushes.Black, 300, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(2, i).Value, String), fntPrint, Brushes.Black, 300, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(4, i).Value, String), fntPrint, Brushes.Black, 500, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(5, i).Value, String), fntPrint, Brushes.Black, 700, sngYLocation)
            sngYLocation += sngLineHeight
        Next
        sngYLocation += sngLineHeight
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
End Class