Imports System.Data.SqlClient

Public Class IlanDetay
    Inherits System.Web.UI.Page

    Private ReadOnly connectionString As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1"

    ' Sayfa boyunca ilan ID'sini tutmak için bir değişken
    Private _ilanID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' *** OTURUM KONTROLÜ ***
        If Session("Kullanici") Is Nothing Then
            Response.Redirect("Login.aspx")
            Return
        End If
        ' *************************

        If Request.QueryString("ilanID") IsNot Nothing AndAlso IsNumeric(Request.QueryString("ilanID")) Then
            _ilanID = Convert.ToInt32(Request.QueryString("ilanID")) ' ID'yi değişkene atıyoruz
        Else
            _ilanID = -1 ' Geçersiz ID
        End If

        If Not IsPostBack Then
            If _ilanID > 0 Then
                IlanDetaylariYukle(_ilanID)
            Else
                pnlDetay.Visible = False
                litHataMesaj.Text = "<div style='text-align:center; padding: 50px; color: #3b5998; font-size: 20px; font-weight: bold;'>Lütfen görüntülemek istediğiniz ilanı Ana Sayfa üzerinden seçiniz.</div>"
            End If
        End If
    End Sub

    Private Sub IlanDetaylariYukle(ByVal id As Integer)

        Dim selectQuery As String = "SELECT Kullanici_Adi, Kategori, Baslik, Aciklama, Resim_Yolu, Tarih, Fiyat, Sehir, Durum, Iletisim_Tel FROM Ilanlar WHERE ilanID = @id"
        Dim ilanKullaniciAdi As String = "" ' İlan sahibini tutmak için

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(selectQuery, con)
                cmd.Parameters.AddWithValue("@id", id)

                Try
                    con.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            pnlDetay.Visible = True

                            ' Detayları doldur
                            ilanKullaniciAdi = reader("Kullanici_Adi").ToString()
                            litBaslikTitle.Text = reader("Baslik").ToString() & " - İlan Detay"
                            litBaslik.Text = reader("Baslik").ToString()
                            litAciklama.Text = Replace(reader("Aciklama").ToString(), vbCrLf, "<br/>")
                            litFiyat.Text = Convert.ToDecimal(reader("Fiyat")).ToString("N0")
                            litSehir.Text = reader("Sehir").ToString()
                            litDurum.Text = reader("Durum").ToString().Replace("_", " ")
                            litTelefon.Text = reader("Iletisim_Tel").ToString()
                            litKategori.Text = reader("Kategori").ToString()
                            litKullanici.Text = ilanKullaniciAdi
                            litTarih.Text = Convert.ToDateTime(reader("Tarih")).ToString("dd MMMM yyyy HH:mm")
                            imgIlan.ImageUrl = reader("Resim_Yolu").ToString()

                            reader.Close() ' Reader'ı kapat

                            ' Favori butonunu ayarla
                            FavoriDurumuKontrolEt(ilanKullaniciAdi)
                        Else
                            pnlDetay.Visible = False
                            litHataMesaj.Text = "<div style='text-align:center; padding: 50px; color: #333; font-size: 18px;'>Üzgünüz, aradığınız ilan bulunamadı veya yayından kaldırılmış.</div>"
                        End If
                    End Using
                Catch ex As Exception
                    pnlDetay.Visible = False
                    litHataMesaj.Text = "<div style='text-align:center; padding: 50px; color: red;'>Veritabanı hatası: " & ex.Message & "</div>"
                End Try
            End Using
        End Using
    End Sub

    ' YENİ: İlanın favorilerde olup olmadığını kontrol eder ve butonu ayarlar
    Private Sub FavoriDurumuKontrolEt(ByVal ilanSahibi As String)
        Dim kullaniciAdi As String = Session("Kullanici").ToString()

        ' Kendi ilanımızı favoriye eklemeyi engelle
        If kullaniciAdi = ilanSahibi Then
            btnFavori.Visible = False
            litFavoriMesaj.Text = "<div class='mesaj-hata'>Kendi ilanın favorilere eklenemez.</div>"
            Return
        End If

        Dim isFavori As Boolean = False
        Dim selectQuery As String = "SELECT COUNT(*) FROM Favoriler WHERE Kullanici_Adi = @kul AND ilanID = @id"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(selectQuery, con)
                cmd.Parameters.AddWithValue("@kul", kullaniciAdi)
                cmd.Parameters.AddWithValue("@id", _ilanID)

                Try
                    con.Open()
                    isFavori = Convert.ToInt32(cmd.ExecuteScalar()) > 0
                Catch ex As Exception
                    ' Hata durumunda butonu gizleyebiliriz
                    btnFavori.Visible = False
                    litFavoriMesaj.Text = "<div class='mesaj-hata'>Favori kontrol hatası!</div>"
                    Return
                End Try
            End Using
        End Using

        If isFavori Then
            btnFavori.Text = "★ Favoriden Kaldır"
            btnFavori.CssClass = "btn-kaldir"
        Else
            btnFavori.Text = "★ Favoriye Ekle"
            btnFavori.CssClass = "btn-ekle"
        End If
    End Sub

    ' YENİ: Favori Butonu Tıklama Olayı
    Protected Sub btnFavori_Click(sender As Object, e As EventArgs) Handles btnFavori.Click
        Dim kullaniciAdi As String = Session("Kullanici").ToString()

        If _ilanID <= 0 Then
            litFavoriMesaj.Text = "<div class='mesaj-hata'>İlan kimliği bulunamadı.</div>"
            Return
        End If

        If btnFavori.CssClass = "btn-ekle" Then
            ' Favoriye Ekleme İşlemi
            Dim insertQuery As String = "INSERT INTO Favoriler (Kullanici_Adi, ilanID) VALUES (@kul, @id)"

            Using con As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(insertQuery, con)
                    cmd.Parameters.AddWithValue("@kul", kullaniciAdi)
                    cmd.Parameters.AddWithValue("@id", _ilanID)

                    Try
                        con.Open()
                        cmd.ExecuteNonQuery()
                        litFavoriMesaj.Text = "<div class='mesaj-basari'>İlan favorilerinize eklendi!</div>"

                    Catch ex As Exception
                        litFavoriMesaj.Text = "<div class='mesaj-hata'>Ekleme hatası: İlan zaten favorilerinizde.</div>"
                    End Try
                End Using
            End Using

        Else ' Buton "Favoriden Kaldır" ise
            ' Favoriden Kaldırma İşlemi
            Dim deleteQuery As String = "DELETE FROM Favoriler WHERE Kullanici_Adi = @kul AND ilanID = @id"

            Using con As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(deleteQuery, con)
                    cmd.Parameters.AddWithValue("@kul", kullaniciAdi)
                    cmd.Parameters.AddWithValue("@id", _ilanID)

                    Try
                        con.Open()
                        cmd.ExecuteNonQuery()
                        litFavoriMesaj.Text = "<div class='mesaj-basari'>İlan favorilerinizden kaldırıldı.</div>"

                    Catch ex As Exception
                        litFavoriMesaj.Text = "<div class='mesaj-hata'>Kaldırma hatası!</div>"
                    End Try
                End Using
            End Using
        End If

        ' İşlemden sonra butonu tekrar güncelleyerek durumunu değiştir
        Response.Redirect(Request.RawUrl)
    End Sub

End Class