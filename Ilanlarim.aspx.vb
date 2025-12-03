Imports System.Data.SqlClient
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Globalization

Public Class Ilanlarim
    Inherits System.Web.UI.Page

    Private ReadOnly connectionString As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1"

    ' Sayfanın kendi ilanları mı yoksa favoriler mi olduğunu tutan değişken
    Protected _isKendiIlanlarim As Boolean = True

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' OTURUM KONTROLÜ
        If Session("Kullanici") Is Nothing Then
            Response.Redirect("Login.aspx")
            Return
        End If

        If Not IsPostBack Then

            Dim tip As String = Request.QueryString("tip") ' URL'den tip parametresini alır

            If tip = "favoriler" Then
                _isKendiIlanlarim = False ' Sayfa modunu favori olarak ayarla
                litBaslik.Text = Session("Kullanici").ToString() & " - Favori İlanlarım"
                FavoriIlanlariYukle()
            Else
                _isKendiIlanlarim = True ' Sayfa modunu kendi ilanlarım olarak ayarla
                litBaslik.Text = Session("Kullanici").ToString() & " - İlanlarım"
                KullaniciIlanlariYukle()
            End If

        End If

    End Sub

    ' Repeater'ın ItemTemplate'inde PlaceHolder'ın görünürlüğünü kontrol etmek için kullanılan yardımcı fonksiyon
    Public Function IsKendiIlanlarimPage() As Boolean
        Return _isKendiIlanlarim
    End Function

    ' Mevcut metot (Kendi İlanlarını Yükler)
    Private Sub KullaniciIlanlariYukle()
        Dim kullaniciAdi As String = Session("Kullanici").ToString()

        Dim selectQuery As String = "SELECT ilanID, Kategori, Baslik, Aciklama, Resim_Yolu, Fiyat, Sehir, Durum, Iletisim_Tel, Tarih FROM Ilanlar WHERE Kullanici_Adi = @kul ORDER BY Tarih DESC"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(selectQuery, con)
                cmd.Parameters.AddWithValue("@kul", kullaniciAdi)

                Try
                    con.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    rptrIlanlar.DataSource = reader
                    rptrIlanlar.DataBind()

                    If Not reader.HasRows Then
                        litIlanYok.Text = "<div class='no-ilan'>" &
                                         "<h3>Henüz Yayınlanmış İlanınız Bulunmamaktadır.</h3>" &
                                         "<p>Hemen <a href='IlanYukle.aspx'>yeni bir ilan yükleyin!</a></p>" &
                                         "</div>"
                    End If

                    reader.Close()

                Catch ex As Exception
                    litIlanYok.Text = "<div class='no-ilan' style='color:red; background-color:#ffdddd; border:1px solid red;'><strong>KRİTİK HATA:</strong> İlanlar yüklenemedi. Detay: " & ex.Message & "</div>"
                End Try
            End Using
        End Using
    End Sub

    ' Favori İlanları Yükler
    Private Sub FavoriIlanlariYukle()
        Dim kullaniciAdi As String = Session("Kullanici").ToString()

        Dim selectQuery As String = "SELECT I.* FROM Ilanlar I " &
                                    "INNER JOIN Favoriler F ON I.ilanID = F.ilanID " &
                                    "WHERE F.Kullanici_Adi = @kul ORDER BY F.EklemeTarihi DESC"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(selectQuery, con)
                cmd.Parameters.AddWithValue("@kul", kullaniciAdi)

                Try
                    con.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    rptrIlanlar.DataSource = reader
                    rptrIlanlar.DataBind()

                    If Not reader.HasRows Then
                        litIlanYok.Text = "<div class='no-ilan'>" &
                                         "<h3>Henüz Favori İlanınız Bulunmamaktadır.</h3>" &
                                         "<p>Hemen <a href='Anasayfa.aspx'>Anasayfadan</a> ilan beğenmeye başlayın!</p>" &
                                         "</div>"
                    End If

                    reader.Close()

                Catch ex As Exception
                    litIlanYok.Text = "<div class='no-ilan' style='color:red; background-color:#ffdddd; border:1px solid red;'><strong>KRİTİK HATA:</strong> Favori İlanlar yüklenemedi. Detay: " & ex.Message & "</div>"
                End Try
            End Using
        End Using
    End Sub

    ' Repeater'ın ItemCommand olayı (Buton tıklamalarını işler)
    Protected Sub rptrIlanlar_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Dim ilanId As Integer = Convert.ToInt32(e.CommandArgument)

        If e.CommandName = "Sil" Then
            ' Kendi ilanlarımızı silmek için (sadece Kendi İlanlarım sayfasında görünür)
            IlanSil(ilanId)

        ElseIf e.CommandName = "Cikart" Then
            ' Favori sayfasından ilan çıkartma (sadece Favorilerim sayfasında görünür)
            FavoridenCikart(ilanId)
        End If
    End Sub

    ' Favoriden Çıkartma Metodu
    Private Sub FavoridenCikart(ilanId As Integer)
        Dim kullaniciAdi As String = Session("Kullanici").ToString()

        Dim deleteQuery As String = "DELETE FROM Favoriler WHERE Kullanici_Adi = @kul AND ilanID = @id"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(deleteQuery, con)
                cmd.Parameters.AddWithValue("@kul", kullaniciAdi)
                cmd.Parameters.AddWithValue("@id", ilanId)

                Try
                    con.Open()
                    cmd.ExecuteNonQuery()

                    ' İşlem başarılı olunca sayfayı yenile
                    Response.Redirect(Request.RawUrl)

                Catch ex As Exception
                    litIlanYok.Text = "<div class='no-ilan' style='color:red;'>Favoriden çıkartılırken bir hata oluştu. Detay: " & ex.Message & "</div>"
                End Try
            End Using
        End Using
    End Sub

    ' İlan Silme Metodu (Kendi İlanlarım sayfasında kullanılır)
    Private Sub IlanSil(ilanId As Integer)
        Dim kullaniciAdi As String = Session("Kullanici").ToString()
        Dim resimYolu As String = ""

        Using con As New SqlConnection(connectionString)
            Try
                con.Open()
            Catch
                Return
            End Try

            Dim getResimQuery As String = "SELECT Resim_Yolu FROM Ilanlar WHERE ilanID = @id AND Kullanici_Adi = @kul"
            Using cmd As New SqlCommand(getResimQuery, con)
                cmd.Parameters.AddWithValue("@id", ilanId)
                cmd.Parameters.AddWithValue("@kul", kullaniciAdi)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        resimYolu = reader("Resim_Yolu").ToString()
                    End If
                    reader.Close()
                End Using
            End Using

            Dim deleteQuery As String = "DELETE FROM Ilanlar WHERE ilanID = @id AND Kullanici_Adi = @kul"
            Using cmdDelete As New SqlCommand(deleteQuery, con)
                cmdDelete.Parameters.AddWithValue("@id", ilanId)
                cmdDelete.Parameters.AddWithValue("@kul", kullaniciAdi)

                Try
                    Dim silinenSatirSayisi As Integer = cmdDelete.ExecuteNonQuery()

                    If silinenSatirSayisi > 0 Then
                        If Not String.IsNullOrEmpty(resimYolu) AndAlso Not resimYolu.ToLower().Contains("default.png") Then
                            Dim fullPath As String = Server.MapPath("~/" & resimYolu)
                            If File.Exists(fullPath) Then
                                File.Delete(fullPath)
                            End If
                        End If

                        Response.Redirect(Request.RawUrl)
                    End If

                Catch ex As Exception
                    ' Silme sırasında hata olursa
                End Try
            End Using
        End Using
    End Sub

    ' Fiyat formatlama fonksiyonu
    Public Function FormatFiyat(ByVal fiyat As Object) As String
        If IsDBNull(fiyat) OrElse fiyat Is Nothing OrElse String.IsNullOrWhiteSpace(fiyat.ToString()) Then
            Return "Fiyat Yok"
        Else
            Try
                Dim fiyatDeger As Decimal = Convert.ToDecimal(fiyat)
                Dim culture As New System.Globalization.CultureInfo("tr-TR")
                Return String.Format(culture, "{0:N0}", fiyatDeger)

            Catch ex As Exception
                Return "Format Hatası"
            End Try
        End If
    End Function

End Class