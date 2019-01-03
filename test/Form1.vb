Imports MySql.Data.MySqlClient
Public Class Form1
    '// ----------------- Config Connection -----------------//
    Dim strG As String = "server=127.0.0.1;" &
                         "uid=root;" &
                         "pwd=mastah;" &
                         "database=testupload"
    Dim conn As New MySqlConnection(strG)
    '// ------------------------------------------------------//
    Dim query, getName, _path, secretDtrows 'variable
    Dim cmnd As MySqlCommand ' command sql
    Private Sub ViewDG()
        Dim dt_table As New DataTable
        Dim dtAdptr As New MySqlDataAdapter
        Dim bndSource As New BindingSource
        With dtAdptr
            Try
                conn.Open()
                query = "select _namefile as 'Name File', location as 'Path Dircetory' from pdfupload"

                cmnd = New MySqlCommand(
                                        query, conn
                                        )
                .SelectCommand = cmnd
                .Fill(dt_table)
                bndSource.DataSource = dt_table

                DataGridView1.DataSource = dt_table

            Catch ex As Exception
                MsgBox("Check Your Connection!!",
                                MsgBoxStyle.Exclamation, "Lost Conn")
            End Try
            conn.Close()
        End With
    End Sub
    Private Sub fm_Load() Handles MyBase.Load
        silent.Visible = False
        silent2.Visible = False
        ViewDG()
    End Sub
    Private Sub BrowseFile() Handles Button2.Click
        _ResetText()
        Button1.Enabled = True
        OpenFileDialog1.Filter = " Chose Fucking File (*.pdf) | *.pdf;"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Title = "Select File Pdf.."
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpfD() Handles OpenFileDialog1.FileOk
        _path = System.IO.Path.
                GetFullPath(
                            OpenFileDialog1.FileName
                            )
        getName = System.IO.Path.
            GetFileName(OpenFileDialog1.FileName
                        )


        Label1.Text = "Path : " & _path
        nm_file.Text = "Name File : " & getName

        AxAcroPDF1.src = _path


        silent2.Text = _path
        silent.Text = silent2.Text.Replace("\", "\\")
    End Sub
    Private Sub sendtodb() Handles Button1.Click
        If Label1.Text = "Path :" Then
            MsgBox("Upload ditolak!" & vbNewLine &
                   "Anda Belum memilih file", MsgBoxStyle.Exclamation, "")
        Else

            With Label2
                Try
                    conn.Open()
                    query = "insert into pdfupload (_namefile, location) values ('" & getName & "', '" & silent.Text & "')"

                    cmnd = New MySqlCommand(query,
                                             conn)

                    Dim _read As MySqlDataReader
                    _read = cmnd.ExecuteReader
                    .Text = "Status : Sucesfully"
                    If .Text = "Status : Sucesfully" Then
                        .ForeColor = Color.Green
                        Button1.Enabled = False
                    End If
                Catch ex As Exception
                    .Text = "Status : Error | Check Your Conection!!"
                    .ForeColor = Color.Red
                End Try
                conn.Close()
                ViewDG() 'refresh Datagrid
                x()
            End With
        End If
    End Sub

    Private Sub _ResetText()
        Label1.Text = "Path : "
        nm_file.Text = "Name File : "
        Label2.Text = "Status : "
        Label2.ForeColor = Color.Black

        silent.Text = ""
        silent2.Text = ""
    End Sub
#Region "jaja"
    Private Sub x()
        If Not Me.Text = "Form1" Then
            MsgBox(Me.Text & "???" & vbNewLine &
                   "Hiks :(", MsgBoxStyle.Critical, "")
        End If
    End Sub
#End Region
    Private Sub Dataiew(sender As Object, e As EventArgs) Handles btn_view.Click
        If Label1.Text = "Path :" Then
            MsgBox("Anda Belum memilih data Pada Datagrid", MsgBoxStyle.
                                                         Exclamation, "Access Danied!!")
        Else
            '// Lakukan Prosses

            AxAcroPDF1.src = secretDtrows
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            Dim indx As Integer
            Dim _selectrow As DataGridViewRow
            indx = e.RowIndex

            _selectrow = DataGridView1.
                         Rows(indx)

            Label1.Text = "Path : " & _selectrow.Cells(0).
                                      Value.ToString()
            nm_file.Text = "Name File : " & _selectrow.Cells(1).
                                      Value.ToString()



            secretDtrows = _selectrow.Cells(1).
                                      Value.ToString() 'get data click DG
        Catch ex As Exception
            'errorbypass
        End Try
    End Sub
End Class
