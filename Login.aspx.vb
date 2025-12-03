Imports System.Data.SqlClient

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub btnGiris_Click(sender As Object, e As EventArgs) Handles btnGiris.Click

        If String.IsNullOrWhiteSpace(txtKullanici.Text) OrElse String.IsNullOrWhiteSpace(txtParola.Text) Then

            lblMesaj.ForeColor = Drawing.Color.Red
            lblMesaj.Text = "Lütfen tüm alanları doldurun."
            Return
        End If


        Dim con As New SqlConnection("Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1")

        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM Login WHERE Kullanici_Adi=@kul AND Parola=@par", con)
        cmd.Parameters.AddWithValue("@kul", txtKullanici.Text)
        cmd.Parameters.AddWithValue("@par", txtParola.Text)

        Try
            con.Open()
            Dim sonuc As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If sonuc > 0 Then
                ' Giriş Başarılı
                Session("Kullanici") = txtKullanici.Text
                Response.Redirect("Anasayfa.aspx")
            Else
                ' Giriş Başarısız (Kullanıcı adı/Parola hatalı)
                lblMesaj.ForeColor = Drawing.Color.Red
                lblMesaj.Text = "Kullanıcı adı veya parola hatalı!"
            End If

        Catch ex As Exception
            lblMesaj.ForeColor = Drawing.Color.Red
            lblMesaj.Text = "Giriş işlemi sırasında bir hata oluştu: " & ex.Message
        Finally
            If con.State = Data.ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub
End Class