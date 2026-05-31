Imports System.IO
Imports System.Windows.Forms
Imports System.Drawing
Public Class Form1
    Public Class Bandera
        Public FileOffset As Integer
        Public Ancho As Byte
        Public Alto As Byte
        Public RecorteX As Byte
        Public RecorteY As Byte
        Public MovX As UShort
        Public MovY As Short
        Public Extra As Byte
        Public Id As Byte
    End Class

    Dim lista As New List(Of Bandera)
    Dim pics As New List(Of PictureBox)
    Dim toolTip As New ToolTip()
    Dim escala As Integer = 1
    Dim rutaActual As String = ""
    Dim seleccionado As PictureBox = Nothing
    Dim offsetMouse As Point

    Const MENU_W As Integer = 348
    Const MENU_H As Integer = 450
    Dim CANVAS_X As Integer = 30  ' Centrado H
    Dim CANVAS_Y As Integer = 490 ' Centrado V
    Dim PANEL_W As Integer = 420
    Dim PANEL_H As Integer = 560

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Panel1.AutoScroll = False
        Panel1.Size = New Size(PANEL_W, PANEL_H)
        Panel1.BackColor = Color.FromArgb(15, 15, 15)
        Panel1.BorderStyle = BorderStyle.FixedSingle
        Panel1.GetType().GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).SetValue(Panel1, True, Nothing)

        toolTip.IsBalloon = True
        toolTip.AutoPopDelay = 5000
    End Sub

    Function LeerTodo(ruta As String) As List(Of Bandera)
        Dim bytes = File.ReadAllBytes(ruta)
        Dim l As New List(Of Bandera)

        ScanRango(bytes, &H28D228, 650, l) ' Países
        ScanRango(bytes, &H28D4B2, 320, l) ' ML

        Return l
    End Function

    Sub ScanRango(bytes As Byte(), inicio As Integer, tamano As Integer, lista As List(Of Bandera))
        Dim fin As Integer = inicio + tamano
        Dim pos As Integer = inicio

        While pos + 9 < fin AndAlso pos + 9 < bytes.Length
            If bytes(pos) = &H14 AndAlso bytes(pos + 1) = &H10 Then
                Dim b As New Bandera
                b.FileOffset = pos
                b.Ancho = bytes(pos)
                b.Alto = bytes(pos + 1)
                b.RecorteX = bytes(pos + 2)
                b.RecorteY = bytes(pos + 3)
                b.MovX = BitConverter.ToUInt16(bytes, pos + 4)
                b.MovY = BitConverter.ToInt16(bytes, pos + 6)
                b.Extra = bytes(pos + 8)
                b.Id = bytes(pos + 9)

                If b.Ancho = &H14 AndAlso b.Alto = &H10 Then lista.Add(b)
            End If
            pos += 10
        End While
    End Sub

    Sub Crear()
        Panel1.Controls.Clear()
        pics.Clear()

        For Each b In lista
            Dim pic As New PictureBox
            Dim color As Color = If(b.FileOffset >= &H28D4B2, Color.FromArgb(220, 60, 60), Color.FromArgb(60, 130, 220))

            pic.BackColor = color
            pic.BorderStyle = BorderStyle.FixedSingle
            pic.Width = b.Ancho * escala
            pic.Height = b.Alto * escala
            pic.Cursor = Cursors.SizeAll

            ' Posicionar según coordenadas reales
            pic.Left = (b.MovX * escala) + CANVAS_X
            pic.Top = (b.MovY * escala) + CANVAS_Y - 122

            toolTip.SetToolTip(pic, $"ID: {b.Id.ToString("X2")} | X: {b.MovX} | Y: {b.MovY}")
            pic.Tag = b

            AddHandler pic.MouseDown, AddressOf Pic_MouseDown
            AddHandler pic.MouseMove, AddressOf Pic_MouseMove
            AddHandler pic.MouseUp, AddressOf Pic_MouseUp

            Panel1.Controls.Add(pic)
            pics.Add(pic)
        Next

        Panel1.Invalidate()
    End Sub

    Private Sub Pic_MouseDown(sender As Object, e As MouseEventArgs)
        seleccionado = CType(sender, PictureBox)
        offsetMouse = e.Location
    End Sub

    Private Sub Pic_MouseMove(sender As Object, e As MouseEventArgs)
        If seleccionado Is Nothing Then Exit Sub
        Dim b As Bandera = CType(seleccionado.Tag, Bandera)

        ' nueva posición
        Dim nuevoLeft As Integer = seleccionado.Left + e.X - offsetMouse.X
        Dim nuevoTop As Integer = seleccionado.Top + e.Y - offsetMouse.Y

        ' Limitar Area Canvas
        Dim minX As Integer = CANVAS_X
        Dim maxX As Integer = CANVAS_X + (MENU_W * escala) - seleccionado.Width
        Dim minY As Integer = CANVAS_Y - (MENU_H * escala) - 122
        Dim maxY As Integer = CANVAS_Y + (MENU_H * escala) - seleccionado.Height - 122

        seleccionado.Left = Math.Max(minX, Math.Min(maxX, nuevoLeft))
        seleccionado.Top = Math.Max(minY, Math.Min(maxY, nuevoTop))

        ' Actualizar x y
        Dim newX As Integer = (seleccionado.Left - CANVAS_X) \ escala
        Dim newY As Integer = (seleccionado.Top + 122 - CANVAS_Y) \ escala

        b.MovX = CUShort(Math.Max(0, Math.Min(65535, newX)))
        b.MovY = CShort(Math.Max(-32768, Math.Min(32767, newY)))

        toolTip.SetToolTip(seleccionado, $"ID: {b.Id.ToString("X2")} | X: {b.MovX} | Y: {b.MovY}")
    End Sub

    Private Sub Pic_MouseUp(sender As Object, e As MouseEventArgs)
        seleccionado = Nothing
    End Sub

    Sub Guardar()
        If rutaActual = "" Then Return
        Dim bytes = File.ReadAllBytes(rutaActual)

        For Each b In lista
            If b.FileOffset + 9 < bytes.Length Then
                bytes(b.FileOffset) = b.Ancho
                bytes(b.FileOffset + 1) = b.Alto
                bytes(b.FileOffset + 2) = b.RecorteX
                bytes(b.FileOffset + 3) = b.RecorteY
                Dim xB = BitConverter.GetBytes(b.MovX)
                bytes(b.FileOffset + 4) = xB(0) : bytes(b.FileOffset + 5) = xB(1)
                Dim yB = BitConverter.GetBytes(b.MovY)
                bytes(b.FileOffset + 6) = yB(0) : bytes(b.FileOffset + 7) = yB(1)
                bytes(b.FileOffset + 8) = b.Extra
                bytes(b.FileOffset + 9) = b.Id
            End If
        Next

        File.WriteAllBytes(rutaActual, bytes)
        MessageBox.Show("✅ Coordenadas guardadas correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BtnCargar_Click(sender As Object, e As EventArgs) Handles BtnCargar.Click
        Dim ofd As New OpenFileDialog With {.Filter = "Archivos de Juego|*.bin;*.dat;*.exe"}
        If ofd.ShowDialog() = DialogResult.OK Then
            rutaActual = ofd.FileName
            lista = LeerTodo(rutaActual)
            Crear()
            Text = $"FlagsMove WE - {Path.GetFileName(rutaActual)} ({lista.Count} banderas -by Zeta)"
        End If
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Guardar()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Dim menuRect As New Rectangle(CANVAS_X, CANVAS_Y - (MENU_H * escala), MENU_W * escala, MENU_H * escala)

        ' Borde
        Using pen As New Pen(Color.White, 2)
            e.Graphics.DrawRectangle(pen, menuRect)
        End Using

        ' Borde Inferior
        Using pen As New Pen(Color.FromArgb(0, 255, 0), 1) With {.DashStyle = Drawing2D.DashStyle.Dash}
            e.Graphics.DrawLine(pen, menuRect.X, CANVAS_Y, menuRect.Right, CANVAS_Y)
        End Using

        ' Borde Izq
        Using pen As New Pen(Color.FromArgb(0, 150, 255), 1) With {.DashStyle = Drawing2D.DashStyle.Dash}
            e.Graphics.DrawLine(pen, CANVAS_X, menuRect.Y, CANVAS_X, menuRect.Bottom)
        End Using
    End Sub

End Class