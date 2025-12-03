Imports System.Data.SqlClient
Imports System.IO
Imports System.Guid
Imports System.Web.UI
Imports System.Globalization

Public Class IlanDuzenle
    Inherits System.Web.UI.Page

    Private ReadOnly connectionString As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' OTURUM KONTROLÜ
        If Session("Kullanici") Is Nothing Then
            Response.Redirect("Login.aspx")
            Return
        End If

        If Not IsPostBack Then
            lblMesaj.Text = ""
            lblMesaj.Style.Clear()


            Dim ilanIDString As String = Request.QueryString("IlanID")

            If String.IsNullOrEmpty(ilanIDString) Then
                ilanIDString = Request.QueryString("Id")
            End If

            If Not String.IsNullOrEmpty(ilanIDString) Then
                Dim ilanID As Integer
                If Integer.TryParse(ilanIDString, ilanID) Then
                    LoadIlanDetails(ilanID)
                Else

                    lblMesaj.Text = "Geçersiz İlan Kimliği Formatı. Lütfen sayısal bir ID kullanın."
                    lblMesaj.Style("background-color") = "#f8d7da"
                    lblMesaj.Style("color") = "#721c24"
                End If
            Else
                lblMesaj.Text = "Düzenlenecek ilan seçilmedi. Lütfen bir ilan ID'si ile gelin."
                lblMesaj.Style("background-color") = "#f8d7da"
                lblMesaj.Style("color") = "#721c24"
            End If
        End If
    End Sub

    Private Sub LoadIlanDetails(ByVal ilanID As Integer)

        Dim selectQuery As String = "SELECT Kullanici_Adi, Kategori, Baslik, Aciklama, Resim_Yolu, Fiyat, Sehir, Durum, Iletisim_Tel FROM Ilanlar WHERE ilanID = @ilanID"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(selectQuery, con)
                cmd.Parameters.AddWithValue("@ilanID", ilanID)

                Try
                    con.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    If reader.Read() Then
                        Dim kullaniciAdi As String = reader("Kullanici_Adi").ToString()

                        If kullaniciAdi <> Session("Kullanici").ToString() Then
                            lblMesaj.Text = "Yetkisiz Erişim: Bu ilanı düzenleme yetkiniz yok."
                            lblMesaj.Style("background-color") = "#f8d7da"
                            lblMesaj.Style("color") = "#721c24"
                            reader.Close()
                            Response.Redirect("Anasayfa.aspx") ' Yetkisiz erişimi engelle
                            Return
                        End If

                        hfIlanID.Value = ilanID.ToString()
                        txtBaslik.Text = reader("Baslik").ToString()
                        txtAciklama.Text = reader("Aciklama").ToString()
                        txtTelefon.Text = reader("Iletisim_Tel").ToString()

                        Dim fiyatValue As Object = reader("Fiyat")
                        If Not Convert.IsDBNull(fiyatValue) Then
                            txtFiyat.Text = String.Format(CultureInfo.InvariantCulture, "{0}", fiyatValue)
                        End If

                        SetDropdownSelectedValue(ddlKategori, reader("Kategori").ToString())
                        SetDropdownSelectedValue(ddlDurum, reader("Durum").ToString())
                        SetDropdownSelectedValue(ddlSehir, reader("Sehir").ToString())

                        Dim resimYoluDB As String = reader("Resim_Yolu").ToString()
                        If Not String.IsNullOrEmpty(resimYoluDB) Then
                            imgMevcutResim.ImageUrl = "~/" & resimYoluDB
                            pnlMevcutResim.Visible = True
                        Else
                            pnlMevcutResim.Visible = False
                        End If

                    Else
                        lblMesaj.Text = "Hata: İlan bulunamadı. Lütfen ID'yi kontrol edin."
                        lblMesaj.Style("background-color") = "#f8d7da"
                        lblMesaj.Style("color") = "#721c24"
                        hfIlanID.Value = ""
                    End If

                    reader.Close()

                Catch ex As Exception
                    lblMesaj.Text = "İlan bilgileri yüklenirken veritabanı hatası oluştu: " & ex.Message
                    lblMesaj.Style("background-color") = "#f8d7da"
                    lblMesaj.Style("color") = "#721c24"
                End Try
            End Using
        End Using
    End Sub

    Private Sub SetDropdownSelectedValue(ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal value As String)
        Dim item As System.Web.UI.WebControls.ListItem = ddl.Items.FindByValue(value)
        If item IsNot Nothing Then
            ddl.SelectedValue = value
        End If
    End Sub

    Protected Sub CustomValidatorResim_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If fuResim.HasFile Then
            Dim dosyaUzantisi As String = Path.GetExtension(fuResim.FileName).ToLower()
            If Not (dosyaUzantisi = ".jpg" OrElse dosyaUzantisi = ".jpeg" OrElse dosyaUzantisi = ".png") Then
                args.IsValid = False
                source.ErrorMessage = "Sadece JPG veya PNG formatı kabul edilir."
            Else
                args.IsValid = True
            End If
        Else
            args.IsValid = True
        End If
    End Sub

    Protected Sub btnIlanGuncelle_Click(sender As Object, e As EventArgs) Handles btnIlanGuncelle.Click

        Page.Validate()

        If Not Page.IsValid Then
            lblMesaj.Text = "Lütfen formdaki tüm hataları düzeltin."
            lblMesaj.Style("background-color") = "#f8d7da"
            lblMesaj.Style("color") = "#721c24"
            Return
        End If

        Dim ilanID As Integer
        If Not Integer.TryParse(hfIlanID.Value, ilanID) Then
            lblMesaj.Text = "Hata: İlan kimliği bulunamadı, sayfa yeniden yüklenmeli."
            lblMesaj.Style("background-color") = "#f8d7da"
            lblMesaj.Style("color") = "#721c24"
            Return
        End If

        Dim resimYoluDB As String = GetExistingResimYolu(ilanID) ' Mevcut resim yolunu veritabanından çek
        Dim eskiResimYolu As String = resimYoluDB

        If fuResim.HasFile Then
            Try
                If Not String.IsNullOrEmpty(eskiResimYolu) AndAlso File.Exists(Server.MapPath("~/" & eskiResimYolu)) Then
                    File.Delete(Server.MapPath("~/" & eskiResimYolu))
                End If

                Dim dosyaUzantisi As String = Path.GetExtension(fuResim.FileName).ToLower()
                Dim imagesDir As String = Server.MapPath("~/Images/")
                If Not Directory.Exists(imagesDir) Then
                    Directory.CreateDirectory(imagesDir)
                End If

                Dim newFileName As String = Guid.NewGuid().ToString() & dosyaUzantisi
                Dim savePath As String = Path.Combine(imagesDir, newFileName)
                fuResim.SaveAs(savePath)

                resimYoluDB = "Images/" & newFileName

            Catch ex As Exception
                lblMesaj.Text = "Yeni resim yüklenirken bir hata oluştu: " & ex.Message
                lblMesaj.Style("background-color") = "#f8d7da"
                lblMesaj.Style("color") = "#721c24"
                Return
            End Try
        End If

        Dim updateQuery As String = "UPDATE Ilanlar SET Kategori = @kat, Baslik = @bas, Aciklama = @ack, Resim_Yolu = @res, Fiyat = @fiyat, Sehir = @sehir, Durum = @durum, Iletisim_Tel = @tel WHERE ilanID = @ilanID AND Kullanici_Adi = @kul"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(updateQuery, con)

                Dim fiyatValue As Decimal
                If Not Decimal.TryParse(txtFiyat.Text, NumberStyles.Any, CultureInfo.InvariantCulture, fiyatValue) Then
                    fiyatValue = 0D
                End If

                cmd.Parameters.AddWithValue("@ilanID", ilanID)
                cmd.Parameters.AddWithValue("@kul", Session("Kullanici").ToString())
                cmd.Parameters.AddWithValue("@kat", ddlKategori.SelectedValue)
                cmd.Parameters.AddWithValue("@bas", txtBaslik.Text)
                cmd.Parameters.AddWithValue("@ack", txtAciklama.Text)
                cmd.Parameters.AddWithValue("@res", resimYoluDB)
                cmd.Parameters.AddWithValue("@fiyat", fiyatValue)
                cmd.Parameters.AddWithValue("@sehir", ddlSehir.SelectedValue)
                cmd.Parameters.AddWithValue("@durum", ddlDurum.SelectedValue)
                cmd.Parameters.AddWithValue("@tel", txtTelefon.Text)

                Try
                    con.Open()
                    Dim affectedRows As Integer = cmd.ExecuteNonQuery()

                    If affectedRows > 0 Then
                        lblMesaj.Text = "İlanınız başarıyla güncellendi! Ana sayfaya yönlendiriliyorsunuz..."
                        lblMesaj.Style("background-color") = "#d4edda"
                        lblMesaj.Style("color") = "#155724"

                        Dim redirectScript As String = "setTimeout(function() { window.location.href = 'Anasayfa.aspx'; }, 2500);"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RedirectToHome", redirectScript, True)
                    Else
                        lblMesaj.Text = "Güncelleme sırasında bir sorun oluştu veya bu ilanı düzenleme yetkiniz yok."
                        lblMesaj.Style("background-color") = "#f8d7da"
                        lblMesaj.Style("color") = "#721c24"
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

    Private Function GetExistingResimYolu(ByVal ilanID As Integer) As String
        Dim existingPath As String = ""
        Dim selectQuery As String = "SELECT Resim_Yolu FROM Ilanlar WHERE ilanID = @ilanID AND Kullanici_Adi = @kul"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(selectQuery, con)
                cmd.Parameters.AddWithValue("@ilanID", ilanID)
                cmd.Parameters.AddWithValue("@kul", Session("Kullanici").ToString())
                Try
                    con.Open()
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                        existingPath = result.ToString()
                    End If
                Catch ex As Exception
                    ' Hata: Mevcut resim yolu çekilemedi.
                    Return ""
                End Try
            End Using
        End Using
        Return existingPath
    End Function

End Class