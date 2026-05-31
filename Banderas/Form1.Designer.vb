<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Panel1 = New Panel()
        BtnCargar = New Button()
        BtnGuardar = New Button()
        ToolTip1 = New ToolTip(components)
        ToolTip2 = New ToolTip(components)
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.AutoScroll = True
        Panel1.BackColor = SystemColors.ActiveCaption
        Panel1.Location = New Point(1, 2)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(420, 500)
        Panel1.TabIndex = 0
        ' 
        ' BtnCargar
        ' 
        BtnCargar.Location = New Point(457, 12)
        BtnCargar.Name = "BtnCargar"
        BtnCargar.Size = New Size(44, 23)
        BtnCargar.TabIndex = 1
        BtnCargar.Text = "Cargar"
        BtnCargar.UseVisualStyleBackColor = True
        ' 
        ' BtnGuardar
        ' 
        BtnGuardar.Location = New Point(457, 41)
        BtnGuardar.Name = "BtnGuardar"
        BtnGuardar.Size = New Size(44, 23)
        BtnGuardar.TabIndex = 2
        BtnGuardar.Text = "Guardar"
        BtnGuardar.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        AutoScroll = True
        ClientSize = New Size(545, 525)
        Controls.Add(BtnGuardar)
        Controls.Add(BtnCargar)
        Controls.Add(Panel1)
        MaximizeBox = False
        Name = "Form1"
        Text = "Flags Move v1"
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents BtnCargar As Button
    Friend WithEvents BtnGuardar As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ToolTip2 As ToolTip

End Class
