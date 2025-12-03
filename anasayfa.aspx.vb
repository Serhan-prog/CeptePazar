Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Data

Public Class Anasayfa
    Inherits System.Web.UI.Page

    Private ReadOnly connectionString As String = "Data Source=.\SQLEXPRESS;Initial Catalog=SerSatis;Persist Security Info=True;User ID=sa;Password=1"
    Private ReadOnly trCulture As New CultureInfo("tr-TR")

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("Kullanici") Is Nothing Then
            Response.Redirect("Login.aspx")
        Else
            lblKullanici.Text = Session("Kullanici").ToString()
            If Not IsPostBack Then
                SehirleriYukle()
                KategorileriYukle()
                IlanlariYukle()
            End If
        End If
    End Sub

    Private Sub SehirleriYukle()
        Dim tümSehirler() As String = {
            "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir",
            "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli",
            "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkâri",
            "Hatay", "Isparta", "Mersin", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir",
            "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir",
            "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat",
            "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman",
            "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye",
            "Düzce"
        }

        ddlSehir.Items.Clear()
        ddlSehir.Items.Add(New ListItem("Tüm Şehirler", ""))

        Array.Sort(tümSehirler)
        For Each sehir As String In tümSehirler
            ddlSehir.Items.Add(New ListItem(sehir, sehir))
        Next
    End Sub

    Private Sub KategorileriYukle()
        Dim popülerKategoriler() As String = {
            "Elektronik", "Emlak", "Vasıta", "Yedek Parça", "Giyim & Aksesuar", "Ev Eşyaları",
            "Hizmetler", "İş Makineleri", "Spor & Outdoor", "Kitap & Dergi", "Hayvanlar Alemi"
        }

        ddlKategori.Items.Clear()
        ddlKategori.Items.Add(New ListItem("Tüm Kategoriler", ""))

        For Each kategori As String In popülerKategoriler
            ddlKategori.Items.Add(New ListItem(kategori, kategori))
        Next

        Using con As New SqlConnection(connectionString)
            ' SQL Injection riskini azaltmak için bu kısım idealde Stored Procedure veya daha güvenli yöntemlerle yapılmalı.
            ' Ancak mevcut yapıya uygun olarak kategori listesi dinamik oluşturuluyor.
            Dim excludeList As String = "'" & String.Join("','", popülerKategoriler.Select(Function(s) s.Replace("'", "''")).ToArray()) & "'"

            Dim cmd As New SqlCommand("SELECT DISTINCT Kategori FROM Ilanlar WHERE Kategori NOT IN (" & excludeList & ") ORDER BY Kategori", con)
            con.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                Dim kategoriAdi As String = reader("Kategori").ToString()
                If ddlKategori.Items.FindByValue(kategoriAdi) Is Nothing Then
                    ddlKategori.Items.Add(New ListItem(kategoriAdi, kategoriAdi))
                End If
            End While
            reader.Close()
        End Using
    End Sub

    Protected Sub btnAra_Click(sender As Object, e As EventArgs) Handles btnAra.Click
        IlanlariYukle(txtArama.Text.Trim())
    End Sub

    Protected Sub btnFiltrele_Click(sender As Object, e As EventArgs) Handles btnFiltrele.Click
        IlanlariYukle(txtArama.Text.Trim())
    End Sub

    Private Sub IlanlariYukle(Optional ByVal arama As String = "")
        Dim query As String = "SELECT ilanID, Kullanici_Adi, Kategori, Baslik, Resim_Yolu, Tarih, Fiyat, Sehir FROM Ilanlar WHERE 1=1"
        Dim parameters As New List(Of SqlParameter)

        If Not String.IsNullOrEmpty(arama) Then
            query &= " AND Baslik LIKE @Arama"
            parameters.Add(New SqlParameter("@Arama", "%" & arama & "%"))
        End If

        If Not String.IsNullOrEmpty(ddlSehir.SelectedValue) Then
            query &= " AND Sehir = @Sehir"
            parameters.Add(New SqlParameter("@Sehir", ddlSehir.SelectedValue))
        End If

        If Not String.IsNullOrEmpty(ddlKategori.SelectedValue) Then
            query &= " AND Kategori = @Kategori"
            parameters.Add(New SqlParameter("@Kategori", ddlKategori.SelectedValue))
        End If

        Dim minFiyat As Decimal
        If Decimal.TryParse(txtMinFiyat.Text, minFiyat) Then
            query &= " AND Fiyat >= @MinFiyat"
            parameters.Add(New SqlParameter("@MinFiyat", minFiyat))
        End If

        Dim maxFiyat As Decimal
        If Decimal.TryParse(txtMaxFiyat.Text, maxFiyat) Then
            query &= " AND Fiyat <= @MaxFiyat"
            parameters.Add(New SqlParameter("@MaxFiyat", maxFiyat))
        End If

        query &= " ORDER BY Tarih DESC"

        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddRange(parameters.ToArray())
                con.Open()

                ' İlanları bir DataTable'a al
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                rpIlanlar.DataSource = dt
                rpIlanlar.DataBind()

                ' Kontrol kısmı: İlan listesi boşsa mesajı göster, doluysa gizle
                If dt.Rows.Count = 0 Then
                    lblSonucMesaj.Visible = True
                Else
                    lblSonucMesaj.Visible = False
                End If

            End Using
        End Using
    End Sub

    Public Function FormatFiyat(fiyat As Object) As String
        If IsDBNull(fiyat) OrElse fiyat Is Nothing Then Return "Fiyat Yok"
        Return String.Format(trCulture, "{0:N0}", Convert.ToDecimal(fiyat)) & " TL"
    End Function

    Protected Sub btnIlanlarim_Click(sender As Object, e As EventArgs) Handles btnIlanlarim.Click
        Response.Redirect("Ilanlarim.aspx")
    End Sub

    Protected Sub btnHesapDetay_Click(sender As Object, e As EventArgs) Handles btnHesapDetay.Click
        Response.Redirect("HesapDetay.aspx")
    End Sub

    Protected Sub btnCikis_Click(sender As Object, e As EventArgs) Handles btnCikis.Click
        Session.Abandon()

        lblCikisMesaj.Text = "Başarıyla çıkış yaptınız. Giriş sayfasına yönlendiriliyorsunuz..."
        lblCikisMesaj.Visible = True

        Dim redirectScript As String = "setTimeout(function() { window.location.href = 'Login.aspx'; }, 2000);"

        ClientScript.RegisterStartupScript(Me.GetType(), "GuaranteedRedirect", redirectScript, True)
    End Sub

    Protected Sub btnIlanYukle_Click(sender As Object, e As EventArgs) Handles btnIlanYukle.Click
        Response.Redirect("IlanYukle.aspx")
    End Sub

    Protected Sub btnFavorilerim_Click(sender As Object, e As EventArgs) Handles btnFavorilerim.Click
        Response.Redirect("Ilanlarim.aspx?tip=favoriler")
    End Sub
End Class