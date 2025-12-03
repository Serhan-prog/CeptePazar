<%@ Page Language="VB" AutoEventWireup="false" 
         CodeBehind="IlanDuzenle.aspx.vb" 
         Inherits="Serhan_Satis.IlanDuzenle"
         UnobtrusiveValidationMode="None" 
%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>İlan Düzenle</title>
    <style>
        body {
            font-family: 'Poppins', sans-serif;
            background-color: #f0f2f5;
            margin: 0;
            padding: 20px;
            display: flex;
            justify-content: center;
            align-items: flex-start;
            min-height: 100vh;
        }

        .ilan-form-container {
            width: 100%;
            max-width: 1100px;
            background: white;
            padding: 30px 40px;
            border-radius: 10px;
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.1); 
            display: flex;
            flex-direction: column; 
            gap: 25px; 
        }

        .form-row {
            display: flex;
            gap: 20px;
            flex-wrap: wrap; 
            align-items: flex-start;
        }

        h2 {
            text-align: center;
            color: #3b5998;
            margin-bottom: 20px;
            font-weight: 700;
        }

        .form-group {
            margin-bottom: 0; 
        }

        .form-row.two-col .form-group {
            flex: 1 1 48%; 
        }

        .form-row.three-col .form-group {
            flex: 1 1 30%;
        }

        .ilan-form-container > .form-group {
            margin-bottom: 20px;
        }

        label {
            display: block;
            margin-bottom: 8px;
            font-weight: 600;
            color: #495057;
        }

        input[type=text], input[type=number], textarea, select {
            width: 100%;
            padding: 12px;
            border: 1px solid #ced4da;
            border-radius: 5px;
            box-sizing: border-box;
            transition: border-color 0.3s, box-shadow 0.3s;
            font-size: 16px;
        }

        input[type=text]:focus, input[type=number]:focus, textarea:focus, select:focus {
            border-color: #3b5998;
            box-shadow: 0 0 0 3px rgba(59, 89, 152, 0.2);
            outline: none;
        }
        
        textarea {
            resize: vertical;
            min-height: 120px;
        }

        /* Mevcut Resim Gösterimi için Stil */
        .mevcut-resim-container {
            display: flex;
            align-items: center;
            gap: 20px;
            margin-bottom: 15px;
            padding: 10px;
            border: 1px solid #e9ecef;
            border-radius: 5px;
        }
        .mevcut-resim-container img {
            width: 100px;
            height: 100px;
            object-fit: cover;
            border-radius: 5px;
            border: 1px solid #ddd;
        }
        .mevcut-resim-text {
            color: #6c757d;
            font-size: 14px;
        }


        .file-upload-wrapper {
            display: inline-block;
            width: 100%;
        }

        #<%= fuResim.ClientID %> { 
            display: none; 
        }
        
        .custom-file-upload {
            display: flex;
            align-items: center;
            justify-content: space-between;
            cursor: pointer;
            padding: 12px;
            border: 1px solid #ced4da;
            border-radius: 5px;
            background-color: #ffffff;
            transition: all 0.3s;
        }

        .custom-file-upload:hover {
            border-color: #3b5998;
            box-shadow: 0 0 0 3px rgba(59, 89, 152, 0.1);
        }

        .custom-file-upload-button {
            background-color: #3b5998;
            color: white;
            padding: 6px 15px;
            border-radius: 4px;
            font-weight: 600;
            white-space: nowrap; 
        }

        .file-name-display {
            color: #6c757d;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            margin-right: 15px;
            flex-grow: 1; 
            font-size: 16px;
        }


        .btn-guncelle {
            width: 100%;
            padding: 15px;
            background-color: #28a745; /* Yeşil tonu */
            color: white;
            border: none;
            border-radius: 5px;
            font-weight: 700;
            font-size: 18px;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.1s;
            margin-top: 10px;
        }

        .btn-guncelle:hover {
            background-color: #1e7e34;
            transform: translateY(-1px);
        }
        
        #lblMesaj {
            display: block;
            text-align: center;
            padding: 12px;
            margin-top: 20px;
            border-radius: 5px;
            font-weight: 600;
        }
        
        .geri-don {
            text-align: center;
            margin-bottom: 15px; 
            margin-top: 5px;
        }

        .geri-don a {
            color: #3b5998;
            text-decoration: none;
            font-weight: 500;
            padding: 5px 10px;
            border-bottom: 2px solid transparent;
            transition: border-bottom-color 0.3s;
        }

        .geri-don a:hover {
            border-bottom-color: #3b5998;
        }
        
        .error-message {
            display: block;
            color: #dc3545;
            font-size: 14px;
            margin-top: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ilan-form-container">
            <h2>İlan Düzenle</h2>

            <%-- Geri Dön Bağlantısı --%>
            <div class="geri-don" style="margin-top: -15px;">
                <a href="Anasayfa.aspx">← Ana Sayfaya Geri Dön</a>
            </div>
            
            <%-- HiddenField ile İlan ID'sini Sakla --%>
            <asp:HiddenField ID="hfIlanID" runat="server" />
            
            <%-- Başlık (Tek Sütun, tam genişlikte kalmalı) --%>
            <div class="form-group">
                <label for="<%= txtBaslik.ClientID %>">İlan Başlığı:</label>
                <asp:TextBox ID="txtBaslik" runat="server" placeholder="Ürününüzü en iyi anlatan başlık"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorBas" runat="server" ControlToValidate="txtBaslik" ErrorMessage="Başlık zorunludur." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            
            <%-- ÜÇ SÜTUNLU GRUP: Kategori, Durum, Fiyat --%>
            <div class="form-row three-col">
                
                <%-- Kategori (Sütun 1) --%>
                <div class="form-group">
                    <label for="<%= ddlKategori.ClientID %>">Kategori Seçin:</label>
                    <asp:DropDownList ID="ddlKategori" runat="server">
                        <asp:ListItem Value="" Text="-- Kategori Seçiniz --"></asp:ListItem>
                        <%-- KATEGORİLER --%>
                        <asp:ListItem Value="Elektronik">Elektronik</asp:ListItem>
                        <asp:ListItem Value="Emlak">Emlak</asp:ListItem>
                        <asp:ListItem Value="Vasıta">Vasıta</asp:ListItem>
                        <asp:ListItem Value="Ev_ve_Yaşam">Ev ve Yaşam</asp:ListItem>
                        <asp:ListItem Value="Giyim_ve_Aksesuarlar">Giyim ve Aksesuarlar</asp:ListItem>
                        <asp:ListItem Value="Koleksiyon">Koleksiyon</asp:ListItem>
                        <asp:ListItem Value="Hizmetler">Hizmetler</asp:ListItem>
                        <asp:ListItem Value="Is_Makinalari">İş Makinaları ve Sanayi</asp:ListItem>
                        <asp:ListItem Value="Yedek_Parca">Yedek Parça, Aksesuar ve Donanım</asp:ListItem>
                        <asp:ListItem Value="Kitap_Dergi">Kitap, Dergi ve Film</asp:ListItem>
                        <asp:ListItem Value="Spor_ve_Outdoor">Spor ve Outdoor</asp:ListItem>
                        <asp:ListItem Value="Oyuncak_ve_Hobi">Oyuncak ve Hobi</asp:ListItem>
                        <asp:ListItem Value="Hayvanlar_Alemi">Hayvanlar Alemi</asp:ListItem>
                        <asp:ListItem Value="Özel_Ders">Özel Ders Verenler</asp:ListItem>
                        <asp:ListItem Value="Diğer">Diğer</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorKat" runat="server" ControlToValidate="ddlKategori" InitialValue="" ErrorMessage="Kategori zorunludur." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>

                <%-- Durum (Sütun 2) --%>
                <div class="form-group">
                    <label for="<%= ddlDurum.ClientID %>">Ürün Durumu:</label>
                    <asp:DropDownList ID="ddlDurum" runat="server">
                        <asp:ListItem Value="" Text="-- Seçiniz --"></asp:ListItem>
                        <%-- DURUM SEÇENEKLERİ --%>
                        <asp:ListItem Value="Sıfır">Sıfır / Hiç Kullanılmamış</asp:ListItem>
                        <asp:ListItem Value="Ikinci_El_Cok_Iyi">İkinci El (Çok İyi Durumda)</asp:ListItem>
                        <asp:ListItem Value="Ikinci_El_Normal">İkinci El (Normal Kullanım)</asp:ListItem>
                        <asp:ListItem Value="Ikinci_El_Hasarli">İkinci El (Tamir Gerektiren/Hasarlı)</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorDurum" runat="server" ControlToValidate="ddlDurum" InitialValue="" ErrorMessage="Durum zorunludur." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                
                <%-- Fiyat (Sütun 3) --%>
                <div class="form-group">
                    <label for="<%= txtFiyat.ClientID %>">Fiyat (TL):</label>
                    <asp:TextBox ID="txtFiyat" runat="server" TextMode="Number" placeholder="Örn: 1500" min="0"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorFiyat" runat="server" ControlToValidate="txtFiyat" ErrorMessage="Fiyat zorunludur." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtFiyat" Operator="DataTypeCheck" Type="Double" ErrorMessage="Lütfen geçerli bir sayı giriniz." CssClass="error-message" Display="Dynamic"></asp:CompareValidator>
                </div>
            </div>

            <%-- İKİ SÜTUNLU GRUP: Şehir ve Telefon --%>
            <div class="form-row two-col">
                
                <%-- Şehir (Sol Sütun) --%>
                <div class="form-group">
                    <label for="<%= ddlSehir.ClientID %>">Şehir:</label>
                    <asp:DropDownList ID="ddlSehir" runat="server">
                        <asp:ListItem Value="" Text="-- Şehir Seçiniz --"></asp:ListItem>
                        <%-- 81 İLİN TAMAMI ALFABETİK SIRAYLA --%>
                        <asp:ListItem Value="Adana">Adana</asp:ListItem>
                        <asp:ListItem Value="Adıyaman">Adıyaman</asp:ListItem>
                        <asp:ListItem Value="Afyonkarahisar">Afyonkarahisar</asp:ListItem>
                        <asp:ListItem Value="Ağrı">Ağrı</asp:ListItem>
                        <asp:ListItem Value="Amasya">Amasya</asp:ListItem>
                        <asp:ListItem Value="Ankara">Ankara</asp:ListItem>
                        <asp:ListItem Value="Antalya">Antalya</asp:ListItem>
                        <asp:ListItem Value="Artvin">Artvin</asp:ListItem>
                        <asp:ListItem Value="Aydın">Aydın</asp:ListItem>
                        <asp:ListItem Value="Balıkesir">Balıkesir</asp:ListItem>
                        <asp:ListItem Value="Bilecik">Bilecik</asp:ListItem>
                        <asp:ListItem Value="Bingöl">Bingöl</asp:ListItem>
                        <asp:ListItem Value="Bitlis">Bitlis</asp:ListItem>
                        <asp:ListItem Value="Bolu">Bolu</asp:ListItem>
                        <asp:ListItem Value="Burdur">Burdur</asp:ListItem>
                        <asp:ListItem Value="Bursa">Bursa</asp:ListItem>
                        <asp:ListItem Value="Çanakkale">Çanakkale</asp:ListItem>
                        <asp:ListItem Value="Çankırı">Çankırı</asp:ListItem>
                        <asp:ListItem Value="Çorum">Çorum</asp:ListItem>
                        <asp:ListItem Value="Denizli">Denizli</asp:ListItem>
                        <asp:ListItem Value="Diyarbakır">Diyarbakır</asp:ListItem>
                        <asp:ListItem Value="Edirne">Edirne</asp:ListItem>
                        <asp:ListItem Value="Elazığ">Elazığ</asp:ListItem>
                        <asp:ListItem Value="Erzincan">Erzincan</asp:ListItem>
                        <asp:ListItem Value="Erzurum">Erzurum</asp:ListItem>
                        <asp:ListItem Value="Eskişehir">Eskişehir</asp:ListItem>
                        <asp:ListItem Value="Gaziantep">Gaziantep</asp:ListItem>
                        <asp:ListItem Value="Giresun">Giresun</asp:ListItem>
                        <asp:ListItem Value="Gümüşhane">Gümüşhane</asp:ListItem>
                        <asp:ListItem Value="Hakkâri">Hakkâri</asp:ListItem>
                        <asp:ListItem Value="Hatay">Hatay</asp:ListItem>
                        <asp:ListItem Value="Isparta">Isparta</asp:ListItem>
                        <asp:ListItem Value="Mersin">Mersin (İçel)</asp:ListItem>
                        <asp:ListItem Value="İstanbul">İstanbul</asp:ListItem>
                        <asp:ListItem Value="İzmir">İzmir</asp:ListItem>
                        <asp:ListItem Value="Kars">Kars</asp:ListItem>
                        <asp:ListItem Value="Kastamonu">Kastamonu</asp:ListItem>
                        <asp:ListItem Value="Kayseri">Kayseri</asp:ListItem>
                        <asp:ListItem Value="Kırklareli">Kırklareli</asp:ListItem>
                        <asp:ListItem Value="Kırşehir">Kırşehir</asp:ListItem>
                        <asp:ListItem Value="Kocaeli">Kocaeli</asp:ListItem>
                        <asp:ListItem Value="Konya">Konya</asp:ListItem>
                        <asp:ListItem Value="Kütahya">Kütahya</asp:ListItem>
                        <asp:ListItem Value="Malatya">Malatya</asp:ListItem>
                        <asp:ListItem Value="Manisa">Manisa</asp:ListItem>
                        <asp:ListItem Value="Kahramanmaraş">Kahramanmaraş</asp:ListItem>
                        <asp:ListItem Value="Mardin">Mardin</asp:ListItem>
                        <asp:ListItem Value="Muğla">Muğla</asp:ListItem>
                        <asp:ListItem Value="Muş">Muş</asp:ListItem>
                        <asp:ListItem Value="Nevşehir">Nevşehir</asp:ListItem>
                        <asp:ListItem Value="Niğde">Niğde</asp:ListItem>
                        <asp:ListItem Value="Ordu">Ordu</asp:ListItem>
                        <asp:ListItem Value="Rize">Rize</asp:ListItem>
                        <asp:ListItem Value="Sakarya">Sakarya</asp:ListItem>
                        <asp:ListItem Value="Samsun">Samsun</asp:ListItem>
                        <asp:ListItem Value="Siirt">Siirt</asp:ListItem>
                        <asp:ListItem Value="Sinop">Sinop</asp:ListItem>
                        <asp:ListItem Value="Sivas">Sivas</asp:ListItem>
                        <asp:ListItem Value="Tekirdağ">Tekirdağ</asp:ListItem>
                        <asp:ListItem Value="Tokat">Tokat</asp:ListItem>
                        <asp:ListItem Value="Trabzon">Trabzon</asp:ListItem>
                        <asp:ListItem Value="Tunceli">Tunceli</asp:ListItem>
                        <asp:ListItem Value="Şanlıurfa">Şanlıurfa</asp:ListItem>
                        <asp:ListItem Value="Uşak">Uşak</asp:ListItem>
                        <asp:ListItem Value="Van">Van</asp:ListItem>
                        <asp:ListItem Value="Yozgat">Yozgat</asp:ListItem>
                        <asp:ListItem Value="Zonguldak">Zonguldak</asp:ListItem>
                        <asp:ListItem Value="Aksaray">Aksaray</asp:ListItem>
                        <asp:ListItem Value="Bayburt">Bayburt</asp:ListItem>
                        <asp:ListItem Value="Karaman">Karaman</asp:ListItem>
                        <asp:ListItem Value="Kırıkkale">Kırıkkale</asp:ListItem>
                        <asp:ListItem Value="Batman">Batman</asp:ListItem>
                        <asp:ListItem Value="Şırnak">Şırnak</asp:ListItem>
                        <asp:ListItem Value="Bartın">Bartın</asp:ListItem>
                        <asp:ListItem Value="Ardahan">Ardahan</asp:ListItem>
                        <asp:ListItem Value="Iğdır">Iğdır</asp:ListItem>
                        <asp:ListItem Value="Yalova">Yalova</asp:ListItem>
                        <asp:ListItem Value="Karabük">Karabük</asp:ListItem>
                        <asp:ListItem Value="Kilis">Kilis</asp:ListItem>
                        <asp:ListItem Value="Osmaniye">Osmaniye</asp:ListItem>
                        <asp:ListItem Value="Düzce">Düzce</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorSehir" runat="server" ControlToValidate="ddlSehir" InitialValue="" ErrorMessage="Şehir zorunludur." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                
                <%-- Telefon (Sağ Sütun) --%>
                <div class="form-group">
                    <label for="<%= txtTelefon.ClientID %>">İletişim Telefonu:</label>
                    <asp:TextBox ID="txtTelefon" runat="server" placeholder="Örn: 5xx xxx xx xx" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorTel" runat="server" ControlToValidate="txtTelefon" ErrorMessage="Telefon zorunludur." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>

            <%-- Açıklama (Tek Sütun - Geniş kalmalı) --%>
            <div class="form-group">
                <label for="<%= txtAciklama.ClientID %>">Açıklama:</label>
                <asp:TextBox ID="txtAciklama" runat="server" TextMode="MultiLine" placeholder="Ürünün durumu, özellikleri, fiyatı hakkında detay verin"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAcik" runat="server" ControlToValidate="txtAciklama" ErrorMessage="Açıklama zorunludur." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            
            <%-- Mevcut Resim ve Yükleme Bölümü --%>
            <div class="form-group">
                <label>Ürün Resmi:</label>

                <%-- Mevcut Resmi Gösterim Alanı --%>
                <asp:Panel ID="pnlMevcutResim" runat="server" CssClass="mevcut-resim-container">
                    <asp:Image ID="imgMevcutResim" runat="server" />
                    <span class="mevcut-resim-text">Mevcut Resim. Değiştirmek isterseniz aşağıdan yeni dosya seçiniz.</span>
                </asp:Panel>
                
                <label style="margin-top: 15px;">Resmi Değiştir (İsteğe Bağlı):</label>
                <div class="file-upload-wrapper">
                    <%-- Gerçek FileUpload kontrolü gizlenecek --%>
                    <asp:FileUpload ID="fuResim" runat="server" />
                    
                    <%-- Özelleştirilmiş görünüm --%>
                    <label for="<%= fuResim.ClientID %>" class="custom-file-upload">
                        <span id="fileName" class="file-name-display">Yeni dosya seçilmedi...</span>
                        <span class="custom-file-upload-button">Dosya Seç</span>
                    </label>
                </div>
                
                <%-- CustomValidator sadece dosya yüklendiğinde format kontrolü yapacak --%>
                <asp:CustomValidator ID="CustomValidatorResim" runat="server" ErrorMessage="Sadece JPG veya PNG formatı kabul edilir." OnServerValidate="CustomValidatorResim_ServerValidate" CssClass="error-message" Display="Dynamic"></asp:CustomValidator>
            </div>

            <%-- İlanı Güncelle Butonu --%>
            <asp:Button ID="btnIlanGuncelle" runat="server" Text="İlanı Güncelle" CssClass="btn-guncelle" OnClick="btnIlanGuncelle_Click" />

            <%-- Mesaj --%>
            <asp:Label ID="lblMesaj" runat="server"></asp:Label>
        </div>
    </form>
    
    <%-- Dosya adını göstermek için JavaScript bloğu --%>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var fileInput = document.getElementById('<%= fuResim.ClientID %>');
            var fileNameDisplay = document.getElementById('fileName');

            if (fileInput) {
                fileInput.addEventListener('change', function () {
                    if (this.files && this.files.length > 0) {
                        fileNameDisplay.textContent = this.files[0].name;
                    } else {
                        fileNameDisplay.textContent = 'Yeni dosya seçilmedi...';
                    }
                });
            }
        });
    </script>
</body>
</html>