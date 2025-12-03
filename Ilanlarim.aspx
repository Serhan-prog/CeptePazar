<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Ilanlarim.aspx.vb" Inherits="Serhan_Satis.Ilanlarim" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>İlanlarım / Favorilerim</title>
        <link rel="icon" href="Iconlar/Ürünler ICO.ico" type="image/x-icon" />

    <style>
        /* Genel Stil Sıfırlamaları ve Font */
        body {
            font-family: 'Poppins', sans-serif;
            background-color: #f0f2f5;
            margin: 0;
            padding: 0;
            line-height: 1.6;
            color: #333;
        }

        /* Header Stili */
        .header {
            background: #ffffff;
            border-bottom: 1px solid #e0e2e5;
            padding: 15px 30px;
            box-shadow: 0 1px 4px rgba(0, 0, 0, 0.05);
        }

        .header a {
            color: #3b5998;
            text-decoration: none;
            font-weight: 600;
        }
        .header a:hover {
            text-decoration: underline;
        }

        /* Ana İçerik Kapsayıcı */
        .content-container {
            padding: 40px 30px;
            max-width: 1300px;
            margin: 0 auto;
        }

        /* Başlık Stili */
        h1 {
            color: #333;
            border-bottom: 2px solid #3b5998;
            padding-bottom: 10px;
            margin-bottom: 30px;
            font-size: 28px;
            font-weight: 600;
        }

        /* İlan Listesi Grid Ayarı */
        .ilan-listesi {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 25px;
        }
        
        /* HER BİR İLAN KARTINI VE BUTONLARINI KAPSAYAN YENİ DİV */
        .ilan-item-wrapper {
            display: flex;
            flex-direction: column; /* İçeriği dikey olarak hizala */
            border-radius: 8px;
            overflow: hidden;
            background: white; /* Kart ve buton alanının arkası beyaz olsun */
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1); /* Ortak gölge */
            transition: transform 0.3s, box-shadow 0.3s;
            height: 100%; /* Dikeyde eşit yükseklik */
        }
        /* Hover efekti tüm kapsayıcıya uygulandı */
        .ilan-item-wrapper:hover {
            transform: translateY(-5px); 
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
        }

        /* İlan Kartı Stili (Linkin içi) */
        .ilan-card-link {
            text-decoration: none; 
            color: inherit;
            display: block; /* A etiketinin tüm alanı kaplamasını sağlar */
            flex-grow: 1; /* Butonlar sabit kalırken kartın boş alanı doldurmasını sağlar */
        }

        .ilan-card {
            background: white;
            border-radius: 8px 8px 0 0; /* Sadece üst köşeler yuvarlak */
            overflow: hidden;
            height: 100%; /* Link içindeki kart yüksekliği */
        }

        .ilan-card-img {
            width: 100%;
            height: 220px;
            object-fit: cover;
            display: block;
        }
        
        .ilan-card-body {
            padding: 15px;
            min-height: 100px; /* Metin içeriği için minimum yükseklik */
        }

        .ilan-card-body h4 {
            margin-top: 0;
            margin-bottom: 10px;
            color: #333;
            font-size: 17px;
            white-space: nowrap; /* Tek satırda kalmasını sağlar */
            overflow: hidden;
            text-overflow: ellipsis; /* Taşmayı ... ile gösterir */
        }
        
        .ilan-card-body p {
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 10px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }
        .ilan-card-body .sehir-fiyat {
            display: flex;
            justify-content: space-between;
            align-items: baseline;
            font-size: 14px;
            color: #666;
            margin-bottom: 5px;
        }
        .ilan-card-body .fiyat {
            font-weight: bold;
            color: #007bff;
            font-size: 17px;
        }

        /* BUTONLAR İÇİN YENİ ALAN */
        .ilan-actions {
            padding: 12px 15px;
            background-color: #f7f7f7; /* Hafif gri arka plan */
            border-top: 1px solid #eee;
            text-align: right; /* Butonları sağa yasla */
            display: flex; /* Butonların yan yana durması için */
            justify-content: flex-end; /* İçeriği sağa hizala */
            gap: 8px; /* Butonlar arasında boşluk */
        }
        
        .action-btn {
            padding: 8px 15px;
            border-radius: 5px;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.1s;
            border: 1px solid transparent; 
            text-decoration: none;
            color: white;
            font-size: 14px;
        }
        .action-btn:hover {
            transform: translateY(-1px);
        }
        
        /* Favoriden Çıkart Butonu */
        .btn-unfavorite {
            background-color: #dc3545; /* Kırmızı */
            border-color: #dc3545;
        }
        .btn-unfavorite:hover {
            background-color: #c82333;
            border-color: #c82333;
        }

        /* Düzenle Butonu (Sadece Kendi İlanlarım için) */
        .btn-edit {
            background-color: #007bff; /* Mavi */
            border-color: #007bff;
        }
        .btn-edit:hover {
            background-color: #0056b3;
            border-color: #0056b3;
        }

        /* Sil Butonu (Sadece Kendi İlanlarım için) */
        .btn-delete {
            background-color: #6c757d; /* Gri */
            border-color: #6c757d;
        }
        .btn-delete:hover {
            background-color: #5a6268;
            border-color: #5a6268;
        }

        /* İlan Yok Mesajı */
        .no-ilan {
             text-align: center;
             padding: 50px;
             margin-top: 30px;
             background-color: white;
             border-radius: 8px;
             box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
             color: #555;
        }
        .no-ilan h3 {
             border-bottom: none;
             margin-bottom: 15px;
             color: #333;
        }
        .no-ilan p {
            font-size: 16px;
        }
        .no-ilan a {
            color: #3b5998;
            font-weight: bold;
            text-decoration: none;
        }
        .no-ilan a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <a href="Anasayfa.aspx">&lt; Ana Sayfaya Geri Dön</a>
        </div>

        <div class="content-container">
            <h1><asp:Literal ID="litBaslik" runat="server" /></h1>
            
            <asp:Literal ID="litIlanYok" runat="server" />

            <asp:Repeater ID="rptrIlanlar" runat="server" OnItemCommand="rptrIlanlar_ItemCommand">
                <HeaderTemplate>
                    <div class="ilan-listesi">
                </HeaderTemplate>
                
                <ItemTemplate>
                    <%-- Her bir ilan öğesini ve butonlarını kapsayan dış div --%>
                    <div class="ilan-item-wrapper">
                        <%-- İlan Detayına yönlendiren link, tüm kartı kapsar --%>
                        <a href='IlanDetay.aspx?ilanID=<%# Eval("ilanID") %>' class="ilan-card-link">
                            <div class="ilan-card">
                                <img src='<%# Eval("Resim_Yolu") %>' alt='<%# Eval("Baslik") %>' class="ilan-card-img" onerror="this.onerror=null; this.src='placeholder.jpg';" />
                                
                                <div class="ilan-card-body">
                                    <h4><%# Eval("Baslik") %></h4>
                                    <div class="sehir-fiyat">
                                        <span class="fiyat"><%# FormatFiyat(Eval("Fiyat")) %> TL</span>
                                        <span><%# Eval("Sehir") %></span>
                                    </div>
                                </div>
                            </div>
                        </a>

                        <%-- Butonların bulunduğu alan --%>
                        <div class="ilan-actions">
                            <%-- Favori Sayfası için: Sadece "Favoriden Çıkart" butonu --%>
                            <asp:PlaceHolder ID="phFavori" runat="server" Visible='<%# Not IsKendiIlanlarimPage() %>'>
                                <asp:LinkButton ID="btnCikart" runat="server" CommandName="Cikart" CommandArgument='<%# Eval("ilanID") %>' 
                                    CssClass="action-btn btn-unfavorite" Text="Favoriden Çıkart" />
                            </asp:PlaceHolder>

                            <%-- Kendi İlanlarım Sayfası için: "Düzenle" ve "Sil" butonları --%>
                            <asp:PlaceHolder ID="phKendiIlanlarim" runat="server" Visible='<%# IsKendiIlanlarimPage() %>'>
                                <asp:LinkButton ID="btnDuzenle" runat="server" CommandName="Duzenle" CommandArgument='<%# Eval("ilanID") %>' 
                                    CssClass="action-btn btn-edit" Text="Düzenle" PostBackUrl='<%# "IlanYukle.aspx?ilanID=" & Eval("ilanID") %>' />
                                <asp:LinkButton ID="btnSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ilanID") %>' 
                                    CssClass="action-btn btn-delete" Text="Sil" OnClientClick="return confirm('Bu ilanı kalıcı olarak silmek istediğinizden emin misiniz?')" />
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </ItemTemplate>
                
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            
        </div>
    </form>
</body>
</html>