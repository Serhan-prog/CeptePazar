Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Guid

Public Class IlanYukle
    Inherits System.Web.UI.Page

    Private ReadOnly connectionString As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1"

    ' Sayfa modunu tutar: 0 = Yeni İlan, >0 = Düzenleme (İlan ID'si)
    Private _ilanID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' OTURUM KONTROLÜ
        If Session("Kullanici") Is Nothing Then
            Response.Redirect("Login.aspx")
            Return
        End If

        ' URL'de ilanID var mı kontrol et
        If Request.QueryString("ilanID") IsNot Nothing AndAlso IsNumeric(Request.QueryString("ilanID")) Then
            _ilanID = Convert.ToInt32(Request.QueryString("ilanID"))
        End If

        If Not IsPostBack Then
            lblMesaj.Text = ""
            lblMesaj.Style.Clear()

            ' DropDownList'leri doldur
            KategoriDoldur()
            SehirDoldur()

            If _ilanID > 0 Then
                ' DÜZENLEME MODU: İlan detaylarını yükle
                IlanGetir(_ilanID)
                btnIlanEkle.Text = "İlanı Güncelle"
                ' litBaslik.Text = "İlan Düzenleme" ' (Eğer ASPX sayfanızda bir litBaslik varsa)
            Else
                ' YENİ İLAN MODU
                btnIlanEkle.Text = "İlanı Yayınla"
                ' pnlResim.Visible = False ' (Eğer pnlResim ID'niz varsa, gizlenir)
                ' litBaslik.Text = "Yeni İlan Yükle" 
            End If
        End If
    End Sub

    ' YENİ: İlan ID'sine göre veritabanından bilgileri çekip form elemanlarına doldurur
    Private Sub IlanGetir(ByVal id As Integer)

        Dim selectQuery As String = "SELECT Kullanici_Adi, Kategori, Baslik, Aciklama, Resim_Yolu, Fiyat, Sehir, Durum, Iletisim_Tel FROM Ilanlar WHERE ilanID = @id AND Kullanici_Adi = @kul"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(selectQuery, con)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.Parameters.AddWithValue("@kul", Session("Kullanici").ToString()) ' Güvenlik

                Try
                    con.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' Form Kontrollerini Doldurma

                            ' String alanlar (Başlık, Açıklama, Telefon)
                            ddlKategori.SelectedValue = reader("Kategori").ToString()
                            txtBaslik.Text = reader("Baslik").ToString()
                            txtAciklama.Text = reader("Aciklama").ToString()
                            ddlSehir.SelectedValue = reader("Sehir").ToString()
                            ddlDurum.SelectedValue = reader("Durum").ToString()
                            txtTelefon.Text = reader("Iletisim_Tel").ToString()

                            ' Fiyat Alanı: Formatlama sorunu olmaması için sadece sayısal değeri atıyoruz
                            txtFiyat.Text = Convert.ToDecimal(reader("Fiyat")).ToString()

                            ' Mevcut Resim Gösterimi (Eğer ASPX'te imgMevcutResim ve pnlResim varsa)
                            ' Dim resimYolu As String = reader("Resim_Yolu").ToString()
                            ' If Not String.IsNullOrEmpty(resimYolu) Then
                            '    imgMevcutResim.ImageUrl = "~/" & resimYolu
                            '    pnlResim.Visible = True
                            ' End If

                        Else
                            lblMesaj.Text = "Düzenlenecek ilan bulunamadı veya yetkiniz yok."
                            lblMesaj.Style("background-color") = "#f8d7da"
                            lblMesaj.Style("color") = "#721c24"
                            _ilanID = 0 ' Yeni ilan moduna dön
                            btnIlanEkle.Text = "İlanı Yayınla"
                        End If
                    End Using
                Catch ex As Exception
                    lblMesaj.Text = "İlan yüklenirken hata oluştu: " & ex.Message
                    lblMesaj.Style("background-color") = "#f8d7da"
                    lblMesaj.Style("color") = "#721c24"
                End Try
            End Using
        End Using
    End Sub

    ' Sizin mevcut kodunuzdaki metot: Resim Validasyonu
    Protected Sub CustomValidatorResim_ServerValidate(source As Object, args As ServerValidateEventArgs)
        ' Düzenleme modunda, yeni resim yüklenmiyorsa geçerli sayılır.
        If _ilanID > 0 AndAlso Not fuResim.HasFile Then
            args.IsValid = True
            Return
        End If

        If Not fuResim.HasFile Then
            args.IsValid = False
        Else
            Dim dosyaUzantisi As String = Path.GetExtension(fuResim.FileName).ToLower()
            If Not (dosyaUzantisi = ".jpg" OrElse dosyaUzantisi = ".jpeg" OrElse dosyaUzantisi = ".png") Then
                args.IsValid = False
                source.ErrorMessage = "Sadece JPG veya PNG formatı kabul edilir."
            Else
                args.IsValid = True
            End If
        End If
    End Sub

    Protected Sub btnIlanEkle_Click(sender As Object, e As EventArgs) Handles btnIlanEkle.Click

        Page.Validate()

        If Not Page.IsValid Then
            lblMesaj.Text = "Lütfen formdaki tüm hataları düzeltin."
            lblMesaj.Style("background-color") = "#f8d7da"
            lblMesaj.Style("color") = "#721c24"
            Return
        End If

        Dim resimYoluDB As String = ""

        If fuResim.HasFile Then
            ' YENİ RESİM YÜKLENİYORSA
            Try
                Dim imagesDir As String = Server.MapPath("~/Images/")
                If Not Directory.Exists(imagesDir) Then
                    Directory.CreateDirectory(imagesDir)
                End If

                Dim dosyaUzantisi As String = Path.GetExtension(fuResim.FileName).ToLower()
                Dim newFileName As String = Guid.NewGuid().ToString() & dosyaUzantisi
                Dim savePath As String = Path.Combine(imagesDir, newFileName)
                fuResim.SaveAs(savePath)

                resimYoluDB = "Images/" & newFileName

                ' Düzenleme modunda eski resmi sil
                If _ilanID > 0 Then
                    Dim eskiResim As String = GetEskiResimYolu(_ilanID)
                    SilEskiResim(eskiResim)
                End If

            Catch ex As Exception
                lblMesaj.Text = "Resim yüklenirken bir hata oluştu: " & ex.Message
                lblMesaj.Style("background-color") = "#f8d7da"
                lblMesaj.Style("color") = "#721c24"
                Return
            End Try
        ElseIf _ilanID > 0 Then
            ' DÜZENLEME MODU, RESİM DEĞİŞMEDİYSE: Mevcut resim yolunu çek
            resimYoluDB = GetEskiResimYolu(_ilanID)
        Else
            ' YENİ İLAN, RESİM YÜKLENMEDİYSE (Bu durumda CustomValidator devreye girerdi, ama yine de):
            resimYoluDB = "Images/default.png"
        End If


        Dim connectionString As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1"
        Dim islemQuery As String = ""

        If _ilanID > 0 Then
            ' GÜNCELLEME SORGUSU
            If fuResim.HasFile Then
                ' Resim de güncelleniyorsa
                islemQuery = "UPDATE Ilanlar SET Kategori=@kat, Baslik=@bas, Aciklama=@ack, Resim_Yolu=@res, Fiyat=@fiyat, Sehir=@sehir, Durum=@durum, Iletisim_Tel=@tel WHERE ilanID=@id AND Kullanici_Adi=@kul"
            Else
                ' Sadece metin bilgileri güncelleniyorsa
                islemQuery = "UPDATE Ilanlar SET Kategori=@kat, Baslik=@bas, Aciklama=@ack, Fiyat=@fiyat, Sehir=@sehir, Durum=@durum, Iletisim_Tel=@tel WHERE ilanID=@id AND Kullanici_Adi=@kul"
            End If
        Else
            ' YENİ İLAN EKLEME SORGUSU
            islemQuery = "INSERT INTO Ilanlar (Kullanici_Adi, Kategori, Baslik, Aciklama, Resim_Yolu, Fiyat, Sehir, Durum, Iletisim_Tel, Tarih) VALUES (@kul, @kat, @bas, @ack, @res, @fiyat, @sehir, @durum, @tel, GETDATE())"
        End If


        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(islemQuery, con)

                Dim fiyatValue As Decimal
                ' Fiyatı Decimal'e dönüştürürken binlik ayraçları temizle
                If Not Decimal.TryParse(txtFiyat.Text.Replace(".", "").Replace(",", ""), NumberStyles.Any, CultureInfo.InvariantCulture, fiyatValue) Then
                    fiyatValue = 0D
                End If

                cmd.Parameters.AddWithValue("@kul", Session("Kullanici").ToString())
                cmd.Parameters.AddWithValue("@kat", ddlKategori.SelectedValue)
                cmd.Parameters.AddWithValue("@bas", txtBaslik.Text)
                cmd.Parameters.AddWithValue("@ack", txtAciklama.Text)
                cmd.Parameters.AddWithValue("@fiyat", fiyatValue)
                cmd.Parameters.AddWithValue("@sehir", ddlSehir.SelectedValue)
                cmd.Parameters.AddWithValue("@durum", ddlDurum.SelectedValue)
                cmd.Parameters.AddWithValue("@tel", txtTelefon.Text)

                If _ilanID > 0 Then
                    cmd.Parameters.AddWithValue("@id", _ilanID)
                End If

                ' Resim yolu sadece INSERT işleminde veya UPDATE sırasında resim değişiyorsa eklenir.
                If Not (_ilanID > 0 AndAlso Not fuResim.HasFile) Then
                    cmd.Parameters.AddWithValue("@res", resimYoluDB)
                End If

                Try
                    con.Open()
                    cmd.ExecuteNonQuery()

                    lblMesaj.Style("background-color") = "#d4edda"
                    lblMesaj.Style("color") = "#155724"

                    If _ilanID > 0 Then
                        lblMesaj.Text = "İlanınız başarıyla güncellendi! İlanlarıma yönlendiriliyorsunuz..."
                        Dim redirectScript As String = "setTimeout(function() { window.location.href = 'Ilanlarim.aspx'; }, 2500);"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RedirectToIlanlarim", redirectScript, True)
                    Else
                        lblMesaj.Text = "İlanınız başarıyla yayınlandı! Ana sayfaya yönlendiriliyorsunuz..."
                        Dim redirectScript As String = "setTimeout(function() { window.location.href = 'Anasayfa.aspx'; }, 2500);"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RedirectToHome", redirectScript, True)
                    End If


                Catch ex As Exception
                    lblMesaj.Text = "Veritabanına kayıt sırasında bir hata oluştu: " & ex.Message
                    lblMesaj.Style("background-color") = "#f8d7da"
                    lblMesaj.Style("color") = "#721c24"
                Finally
                    If con.State = Data.ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End Using
        End Using
    End Sub

    ' Kategori DropDownList'lerini doldurur
    Private Sub KategoriDoldur()
        If ddlKategori.Items.Count = 0 Then
            ddlKategori.Items.Add(New ListItem("Seçiniz", ""))
            ddlKategori.Items.Add(New ListItem("Elektronik", "Elektronik"))
            ddlKategori.Items.Add(New ListItem("Emlak", "Emlak"))
            ddlKategori.Items.Add(New ListItem("Vasıta", "Vasıta"))
            ddlKategori.Items.Add(New ListItem("Giyim", "Giyim"))
            ddlKategori.Items.Add(New ListItem("Ev & Yaşam", "Ev_ve_Yasam"))
            ddlKategori.Items.Add(New ListItem("Diğer", "Diğer"))
        End If

        If ddlDurum.Items.Count = 0 Then
            ddlDurum.Items.Add(New ListItem("Seçiniz", ""))
            ddlDurum.Items.Add(New ListItem("Sıfır", "Sifir"))
            ddlDurum.Items.Add(New ListItem("İkinci El", "Ikinci_El"))
        End If
    End Sub

    ' Şehir DropDownList'ini doldurur
    Private Sub SehirDoldur()
        If ddlSehir.Items.Count = 0 Then
            ddlSehir.Items.Add(New ListItem("Seçiniz", ""))
            ddlSehir.Items.Add(New ListItem("İstanbul", "İstanbul"))
            ddlSehir.Items.Add(New ListItem("Ankara", "Ankara"))
            ddlSehir.Items.Add(New ListItem("İzmir", "İzmir"))
            ' ... diğer şehirler
        End If
    End Sub

    ' Eski Resim Yolunu Bulur (Düzenleme sırasında)
    Private Function GetEskiResimYolu(ByVal id As Integer) As String
        Dim query As String = "SELECT Resim_Yolu FROM Ilanlar WHERE ilanID = @id"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@id", id)
                Try
                    con.Open()
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return result.ToString()
                    End If
                Catch ex As Exception

                End Try
            End Using
        End Using
        Return String.Empty
    End Function

    ' Eski Resmi Siler (default.png değilse)
    Private Sub SilEskiResim(ByVal resimYolu As String)
        If Not String.IsNullOrEmpty(resimYolu) AndAlso Not resimYolu.ToLower().Contains("default.png") Then
            Dim fullPath As String = Server.MapPath("~/" & resimYolu)
            If File.Exists(fullPath) Then
                File.Delete(fullPath)
            End If
        End If
    End Sub
End Class