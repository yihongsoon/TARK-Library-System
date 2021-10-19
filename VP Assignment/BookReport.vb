Public Class BookReport
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
        e.Graphics.DrawString("Summary Books Report", fntHeading, Brushes.Black, 50, sngYLocation)
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
        e.Graphics.DrawString("Book ID", fntHeading, Brushes.Black, 50, sngYLocation)
        e.Graphics.DrawString("Book Name", fntHeading, Brushes.Black, 200, sngYLocation)
        e.Graphics.DrawString("Book Author", fntHeading, Brushes.Black, 350, sngYLocation)
        e.Graphics.DrawString("Book Status", fntHeading, Brushes.Black, 550, sngYLocation)
        sngYLocation += sngLineHeight
        e.Graphics.DrawString("=================================================================", fntHeading, Brushes.Black, 50, sngYLocation)
        sngYLocation += sngLineHeight
        'set up detail
        'Do

        For i As Integer = 0 To DataGridView1.Rows.Count - 1

            e.Graphics.DrawString(CType(DataGridView1.Item(0, i).Value, String), fntPrint, Brushes.Black, 50, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(1, i).Value, String), fntPrint, Brushes.Black, 200, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(3, i).Value, String), fntPrint, Brushes.Black, 350, sngYLocation)
            e.Graphics.DrawString(CType(DataGridView1.Item(4, i).Value, String), fntPrint, Brushes.Black, 550, sngYLocation)
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

    Private Sub BookReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TopMost = True
        BindData()
    End Sub
    Private Sub BindData()
        Dim db As New LibraryDataContext()
        DataGridView1.DataSource = db.Books
    End Sub
End Class