<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="IlanDetay.aspx.vb" Inherits="Serhan_Satis.IlanDetay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="litBaslikTitle" runat="server">İlan Detayı</asp:Literal></title>
    <style>
        body {
            font-family: 'Poppins', sans-serif;
            background-color: #f0f2f5;
            margin: 0;
            padding: 0;
            line-height: 1.6;
        }

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

        .detay-container {
            max-width: 900px;
            margin: 40px auto;
            background: white;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
            overflow: hidden;
            display: flex;
        }
        
        .resim-alani {
            flex: 1;
            max-width: 50%;
            overflow: hidden;
        }

        .resim-alani img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            display: block;
        }

        .bilgi-alani {
            flex: 1;
            padding: 30px;
        }

        .bilgi-alani h1 {
            color: #333;
            margin-top: 0;
            font-size: 32px;
            border-bottom: 2px solid #3b5998;
            padding-bottom: 10px;
        }

        .bilgi-alani .meta {
            margin-top: 15px;
            color: #6c757d;
            font-size: 14px;
        }

        .bilgi-alani .meta span {
            display: block;
            margin-bottom: 5px;
        }
        
        .bilgi-alani .meta h2 {
            font-size: 24px;
            color: #28a745; 
            margin: 15px 0 25px 0;
        }

        .bilgi-alani .aciklama {
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #eee;
        }

        .bilgi-alani p {
            font-size: 16px;
            color: #495057;
        }

        /* YENİ: Favori Buton Stilleri */
        .favori-alan {
            margin: 20px 0;
        }

        #btnFavori {
            padding: 12px 25px;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.1s;
        }

        /* Favoriye Ekle butonu stili */
        .btn-ekle {
            background-color: #ffc107; /* Sarı */
            color: #333;
        }
        .btn-ekle:hover {
            background-color: #e0a800;
            transform: translateY(-1px);
        }

        /* Favoriden Kaldır butonu stili */
        .btn-kaldir {
            background-color: #dc3545; /* Kırmızı */
            color: white;
        }
        .btn-kaldir:hover {
            background-color: #c82333;
            transform: translateY(-1px);
        }

        /* Mesaj Stili */
        .mesaj-basari {
            color: #155724;
            background-color: #d4edda;
            border: 1px solid #c3e6cb;
            padding: 10px;
            border-radius: 5px;
            margin-bottom: 10px;
            text-align: center;
        }
        .mesaj-hata {
            color: #721c24;
            background-color: #f8d7da;
            border: 1px solid #f5c6cb;
            padding: 10px;
            border-radius: 5px;
            margin-bottom: 10px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <a href="Anasayfa.aspx">&lt; Tüm İlanlara Geri Dön</a>
        </div>
        
        <asp:Panel ID="pnlDetay" runat="server" CssClass="detay-container" Visible="false">
            <div class="resim-alani">
                <asp:Image ID="imgIlan" runat="server" />
            </div>
            
            <div class="bilgi-alani">
                <h1><asp:Literal ID="litBaslik" runat="server" /></h1>
                
                <div class="favori-alan">
                    <%-- Favori ekleme/kaldırma mesajı buraya gelecek --%>
                    <asp:Literal ID="litFavoriMesaj" runat="server" /> 

                    <%-- YENİ: Favori Butonu --%>
                    <asp:Button ID="btnFavori" runat="server" Text="Favoriye Ekle" OnClick="btnFavori_Click" />
                </div>
                
                <div class="meta">
                    <h2>Fiyat: <asp:Literal ID="litFiyat" runat="server" /> TL</h2>
                    
                    <span>Kategori: <asp:Literal ID="litKategori" runat="server" /></span>
                    <span>Durum: <asp:Literal ID="litDurum" runat="server" /></span>
                    <span>Şehir: <asp:Literal ID="litSehir" runat="server" /></span>
                    
                    <hr style="margin: 10px 0; border: none; border-top: 1px solid #eee;"/>
                    
                    <span>Satıcı: <asp:Literal ID="litKullanici" runat="server" /></span>
                    <span>Yayınlanma Tarihi: <asp:Literal ID="litTarih" runat="server" /></span>
                    <span style="font-weight: bold; color: #3b5998;">Telefon: <asp:Literal ID="litTelefon" runat="server" /></span>
                </div>
                
                <div class="aciklama">
                    <h3>Açıklama</h3>
                    <asp:Literal ID="litAciklama" runat="server" />
                </div>
            </div>
        </asp:Panel>
        
        <asp:Literal ID="litHataMesaj" runat="server" />

    </form>
</body>
</html>