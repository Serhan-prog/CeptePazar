Imports System.Data.SqlClient

Public Class Register
    Inherits System.Web.UI.Page

    Protected Sub btnKayitOl_Click(sender As Object, e As EventArgs) Handles btnKayitOl.Click

        If String.IsNullOrEmpty(txtKullanici.Text) OrElse String.IsNullOrEmpty(txtParola.Text) Then
            lblMesaj.ForeColor = Drawing.Color.Red
            lblMesaj.Text = "Kullanıcı adı ve parola boş bırakılamaz."
            Return
        End If

        Dim con As New SqlConnection("Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1")

        Dim cmd As New SqlCommand("INSERT INTO Login (Kullanici_Adi, Parola) VALUES (@kul, @par)", con)
        cmd.Parameters.AddWithValue("@kul", txtKullanici.Text)
        cmd.Parameters.AddWithValue("@par", txtParola.Text)

        Try
            con.Open()
            cmd.ExecuteNonQuery()

            lblMesaj.ForeColor = Drawing.Color.Green
            lblMesaj.Text = "Kayıt başarılı! Giriş sayfasına yönlendiriliyorsunuz..."


            Dim redirectScript As String = "setTimeout(function() { window.location.href = 'Login.aspx'; }, 3000);"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Redirect", redirectScript, True)

        Catch ex As SqlException
            If ex.Number = 2627 OrElse ex.Number = 2601 Then
                lblMesaj.ForeColor = Drawing.Color.Red
                lblMesaj.Text = "Bu kullanıcı adı zaten mevcut! Lütfen başka bir tane deneyin."
            Else
                lblMesaj.ForeColor = Drawing.Color.Red
                lblMesaj.Text = "Kayıt sırasında bir hata oluştu: " & ex.Message
            End If
        Catch ex As Exception
            lblMesaj.ForeColor = Drawing.Color.Red
            lblMesaj.Text = "Beklenmedik bir hata oluştu: " & ex.Message
        Finally
            If con.State = Data.ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub
End Class