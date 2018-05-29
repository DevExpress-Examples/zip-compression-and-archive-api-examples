Namespace CompressionLibraryExamples
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.textBox1 = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            ' 
            ' textBox1
            ' 
            Me.textBox1.Location = New System.Drawing.Point(54, 41)
            Me.textBox1.Multiline = True
            Me.textBox1.Name = "textBox1"
            Me.textBox1.Size = New System.Drawing.Size(177, 67)
            Me.textBox1.TabIndex = 1
            Me.textBox1.Text = "This project " & ControlChars.CrLf & "contains sample methods" & ControlChars.CrLf & " to illustrate " & ControlChars.CrLf & "Compression Library API"
            Me.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 262)
            Me.Controls.Add(Me.textBox1)
            Me.Name = "Form1"
            Me.Text = "Form1"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        #End Region

        Private textBox1 As System.Windows.Forms.TextBox

    End Class
End Namespace

