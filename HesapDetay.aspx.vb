Imports System.Data.SqlClient
Imports System.Web.UI

Public Class HesapDetay
    Inherits System.Web.UI.Page

    ' DİKKAT: Connection String, uygulama güvenliği açısından web.config'e taşınmalıdır.
    Private ReadOnly connectionString As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Oturum Kontrolü
        If Session("Kullanici") Is Nothing Then
            Response.Redirect("Login.aspx")
        Else
            txtKullaniciAdi.Text = Session("Kullanici").ToString()

            If Not IsPostBack Then
            End If
        End If
    End Sub


    Protected Sub btnKullaniciAdiDegistir_Click(sender As Object, e As EventArgs) Handles btnKullaniciAdiDegistir.Click
        lblMessage.Visible = False

        Dim mevcutKullaniciAdi As String = Session("Kullanici").ToString()
        Dim yeniKullaniciAdi As String = txtYeniKullaniciAdi.Text.Trim()
        Dim onayParolasi As String = txtKADDegisimParola.Text

        If String.IsNullOrWhiteSpace(yeniKullaniciAdi) OrElse String.IsNullOrWhiteSpace(onayParolasi) Then
            GosterMesaj("Yeni kullanıcı adı ve parola alanları boş bırakılamaz.", "error")
            Return
        End If

        If yeniKullaniciAdi.Equals(mevcutKullaniciAdi, StringComparison.OrdinalIgnoreCase) Then
            GosterMesaj("Yeni kullanıcı adı mevcut kullanıcı adınızla aynı olamaz.", "error")
            Return
        End If

        Using con As New SqlConnection(connectionString)
            con.Open()

            ' 1. Parola Kontrolü
            Dim checkParolaQuery As String = "SELECT COUNT(*) FROM Login WHERE Kullanici_Adi = @kAdi AND Parola = @parola"
            Using cmdCheckParola As New SqlCommand(checkParolaQuery, con)
                cmdCheckParola.Parameters.AddWithValue("@kAdi", mevcutKullaniciAdi)
                cmdCheckParola.Parameters.AddWithValue("@parola", onayParolasi)

                If CInt(cmdCheckParola.ExecuteScalar()) = 0 Then
                    GosterMesaj("Hata: Girdiğiniz parola yanlış.", "error")
                    Return
                End If
            End Using

            ' 2. Yeni Kullanıcı Adı Kullanımda mı?
            Dim checkKADQuery As String = "SELECT COUNT(*) FROM Login WHERE Kullanici_Adi = @yKAdi"
            Using cmdCheckKAD As New SqlCommand(checkKADQuery, con)
                cmdCheckKAD.Parameters.AddWithValue("@yKAdi", yeniKullaniciAdi)

                If CInt(cmdCheckKAD.ExecuteScalar()) > 0 Then
                    GosterMesaj("Hata: Bu kullanıcı adı zaten kullanımda. Lütfen başka bir ad seçin.", "error")
                    Return
                End If
            End Using

            ' 3. Login Tablosunu Güncelle
            Dim updateKADQuery As String = "UPDATE Login SET Kullanici_Adi = @yKAdi WHERE Kullanici_Adi = @eKAdi"
            Using cmdUpdateKAD As New SqlCommand(updateKADQuery, con)
                cmdUpdateKAD.Parameters.AddWithValue("@yKAdi", yeniKullaniciAdi)
                cmdUpdateKAD.Parameters.AddWithValue("@eKAdi", mevcutKullaniciAdi)

                cmdUpdateKAD.ExecuteNonQuery()
            End Using

            ' 4. Ilanlar Tablosunu Güncelle (İlişkili tüm kayıtları güncelle)
            Dim updateIlanlarQuery As String = "UPDATE Ilanlar SET Kullanici_Adi = @yKAdi WHERE Kullanici_Adi = @eKAdi"
            Using cmdUpdateIlanlar As New SqlCommand(updateIlanlarQuery, con)
                cmdUpdateIlanlar.Parameters.AddWithValue("@yKAdi", yeniKullaniciAdi)
                cmdUpdateIlanlar.Parameters.AddWithValue("@eKAdi", mevcutKullaniciAdi)

                cmdUpdateIlanlar.ExecuteNonQuery()
            End Using

            Session("Kullanici") = yeniKullaniciAdi ' Session'ı güncelle
            txtKullaniciAdi.Text = yeniKullaniciAdi ' Ekranda gösterilen adı güncelle
            txtYeniKullaniciAdi.Text = ""
            txtKADDegisimParola.Text = ""

            GosterMesaj("Kullanıcı adınız başarıyla " & yeniKullaniciAdi & " olarak değiştirildi!", "success")

        End Using

    End Sub


    Protected Sub btnParolaDegistir_Click(sender As Object, e As EventArgs) Handles btnParolaDegistir.Click
        lblMessage.Visible = False

        Dim kullaniciAdi As String = Session("Kullanici").ToString()
        Dim eskiParola As String = txtEskiParola.Text
        Dim yeniParola As String = txtYeniParola.Text
        Dim yeniParolaTekrar As String = txtYeniParolaTekrar.Text

        If String.IsNullOrWhiteSpace(eskiParola) OrElse String.IsNullOrWhiteSpace(yeniParola) OrElse String.IsNullOrWhiteSpace(yeniParolaTekrar) Then
            GosterMesaj("Lütfen tüm parola alanlarını doldurun.", "error")
            Return
        End If

        If yeniParola <> yeniParolaTekrar Then
            GosterMesaj("Yeni parolalar birbiriyle eşleşmiyor.", "error")
            Return
        End If

        Using con As New SqlConnection(connectionString)
            Dim checkQuery As String = "SELECT COUNT(*) FROM Login WHERE Kullanici_Adi = @kAdi AND Parola = @eParola"

            Using cmdCheck As New SqlCommand(checkQuery, con)
                cmdCheck.Parameters.AddWithValue("@kAdi", kullaniciAdi)
                cmdCheck.Parameters.AddWithValue("@eParola", eskiParola)

                Try
                    con.Open()
                    Dim userCount As Integer = CInt(cmdCheck.ExecuteScalar())

                    If userCount > 0 Then
                        Dim updateQuery As String = "UPDATE Login SET Parola = @yParola WHERE Kullanici_Adi = @kAdi"

                        Using cmdUpdate As New SqlCommand(updateQuery, con)
                            cmdUpdate.Parameters.AddWithValue("@yParola", yeniParola)
                            cmdUpdate.Parameters.AddWithValue("@kAdi", kullaniciAdi)

                            cmdUpdate.ExecuteNonQuery()
                            GosterMesaj("Parolanız başarıyla güncellendi!", "success")

                            txtEskiParola.Text = ""
                            txtYeniParola.Text = ""
                            txtYeniParolaTekrar.Text = ""
                        End Using
                    Else
                        GosterMesaj("Hata: Girdiğiniz eski parola yanlış.", "error")
                    End If

                Catch ex As Exception
                    GosterMesaj("Bir veritabanı hatası oluştu: " & ex.Message, "error")
                End Try
            End Using
        End Using
    End Sub

    ' YENİ EKLEME: Hesap Silme İşlevi
    Protected Sub btnHesabiSil_Click(sender As Object, e As EventArgs) Handles btnHesabiSil.Click
        lblMessage.Visible = False

        ' Oturum kontrolü
        If Session("Kullanici") Is Nothing Then
            Response.Redirect("Login.aspx")
            Return
        End If

        Dim kullaniciAdi As String = Session("Kullanici").ToString()

        Using con As New SqlConnection(connectionString)
            Try
                con.Open()

                ' 1. Kullanıcının ilanı varsa (Ilanlar tablosu) önce onları silmeliyiz (Foreign Key kısıtlamaları için)
                Dim deleteIlanlarQuery As String = "DELETE FROM Ilanlar WHERE Kullanici_Adi = @kAdi"
                Using cmdDeleteIlanlar As New SqlCommand(deleteIlanlarQuery, con)
                    cmdDeleteIlanlar.Parameters.AddWithValue("@kAdi", kullaniciAdi)
                    cmdDeleteIlanlar.ExecuteNonQuery()
                End Using

                ' 2. Ardından Login tablosundaki kullanıcı kaydını silelim
                Dim deleteUserQuery As String = "DELETE FROM Login WHERE Kullanici_Adi = @kAdi"
                Using cmdDeleteUser As New SqlCommand(deleteUserQuery, con)
                    cmdDeleteUser.Parameters.AddWithValue("@kAdi", kullaniciAdi)
                    Dim affectedRows As Integer = cmdDeleteUser.ExecuteNonQuery()
                End Using

                ' İşlem başarılıysa oturumu sonlandır ve giriş sayfasına yönlendir
                Session.Abandon()
                Response.Redirect("Login.aspx")

            Catch ex As Exception
                GosterMesaj("Hesap silinirken bir veritabanı hatası oluştu: " & ex.Message, "error")
            End Try
        End Using
    End Sub
    ' YENİ EKLEME SONU

    Protected Sub btnAnasayfa_Click(sender As Object, e As EventArgs) Handles btnAnasayfa.Click
        Response.Redirect("Anasayfa.aspx")
    End Sub

    Private Sub GosterMesaj(mesaj As String, tip As String)
        lblMessage.Text = mesaj
        lblMessage.CssClass = "message " & tip
        lblMessage.Visible = True
    End Sub

End Class