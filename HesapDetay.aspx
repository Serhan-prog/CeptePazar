<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="HesapDetay.aspx.vb" Inherits="Serhan_Satis.HesapDetay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hesap Detayları - Orion</title>
    <link rel="icon" href="Iconlar/transaction ICO.ico" type="image/x-icon" />

    <script type="text/javascript">
        function ConfirmDelete() {
            // Silme işlemi için kullanıcıya uyarı göster ve onay al
            var result = confirm("UYARI: Hesabınızı kalıcı olarak silmek istediğinizden emin misiniz? Bu işlem geri alınamaz ve tüm ilanlarınız silinecektir.");
            return result; // Kullanıcı 'Tamam' derse true, 'İptal' derse false döner
        }
    </script>
    <style>
        body {
            font-family: 'Poppins', sans-serif;
            background-color: #f0f2f5; 
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            margin: 0;
            padding: 20px;
        }
        
        .container {
            background: #ffffff;
            padding: 30px;
            border-radius: 8px; 
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
            width: 100%;
            max-width: 850px;
        }
        
        .form-grid {
            display: flex;
            gap: 30px; 
            margin-bottom: 20px;
        }

        .form-column {
            flex: 1;
            padding: 20px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            background-color: #fdfdfd;
            display: flex; 
            flex-direction: column;
        }
        
        h2 {
            text-align: center;
            color: #3b5998; 
            margin-top: 0; 
            margin-bottom: 25px;
            font-weight: 700;
            font-size: 24px;
        }

        h3 {
            color: #333;
            font-size: 20px;
            font-weight: 700;
            margin-top: 0;
            margin-bottom: 20px;
            text-align: center;
            border-bottom: 2px solid #3b5998;
            padding-bottom: 5px;
        }
        
        .form-group {
            margin-bottom: 15px;
        }
        
        label {
            display: block;
            margin-bottom: 6px;
            font-weight: 600;
            color: #3b5998;
            font-size: 14px;
        }
        
        .text-input {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid #ced4da;
            border-radius: 5px; 
            box-sizing: border-box;
            font-size: 15px;
            transition: border-color 0.3s;
        }
        
        .text-input:focus {
            border-color: #3b5998;
            outline: none;
            box-shadow: 0 0 0 0.2rem rgba(59, 89, 152, 0.25);
        }

        .readonly-input {
            background-color: #e9ecef;
            color: #495057;
            cursor: default;
        }
        
        .action-btn {
            width: 100%;
            padding: 12px;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.1s;
            margin-top: auto;
        }
        
        #btnKullaniciAdiDegistir {
            background-color: #ffc107;
            color: #333;
            margin-top: 25px;
        }
        #btnKullaniciAdiDegistir:hover {
            background-color: #e0a800;
            transform: translateY(-1px);
        }

        #btnParolaDegistir {
            background-color: #3b5998;
            color: white;
            margin-top: 25px;
        }
        #btnParolaDegistir:hover {
            background-color: #2b4570;
            transform: translateY(-1px);
        }
        
        #btnHesabiSil {
            background-color: #dc3545; /* Kırmızı renk */
            color: white;
            margin-top: 20px; 
            display: block;
            width: 100%;
        }
        #btnHesabiSil:hover {
            background-color: #c82333;
            transform: translateY(-1px);
        }

        #btnAnasayfa {
            background-color: #6c757d;
            color: white;
            margin-top: 15px; /* Sil butonu ile ayırıldı */
            width: 100%;
            display: block;
        }
        #btnAnasayfa:hover {
            background-color: #5a6268;
            transform: translateY(-1px);
        }
        
        .message {
            display: block; 
            padding: 12px;
            margin-bottom: 25px; 
            margin-top: -10px;
            border-radius: 5px;
            text-align: center;
            font-weight: 600;
            font-size: 15px;
            line-height: 1.4;
        }
        
        .success {
            background-color: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        
        .error {
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
        
        @media (max-width: 768px) {
            .container {
                max-width: 100%;
            }
            .form-grid {
                flex-direction: column;
                gap: 20px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            
            <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="message"></asp:Label>

            <h2>Hesap Detayları</h2>
            
            <%-- MEVCUT KULLANICI ADI GÖSTERİMİ --%>
            <div class="form-group">
                <label for="<%= txtKullaniciAdi.ClientID %>">Mevcut Kullanıcı Adı:</label>
                <asp:TextBox ID="txtKullaniciAdi" runat="server" CssClass="text-input readonly-input" ReadOnly="true"></asp:TextBox>
            </div>

            <hr style="margin: 30px 0; border: 0; border-top: 1px solid #eee;" />

            <%-- İKİ SÜTUNLU GRİD BAŞLANGICI --%>
            <div class="form-grid">

                <%-- SOL SÜTUN: KULLANICI ADI DEĞİŞTİRME --%>
                <div class="form-column">
                    <h3>Kullanıcı Adı Değiştirme</h3>

                    <div class="form-group">
                        <label for="<%= txtYeniKullaniciAdi.ClientID %>">Yeni Kullanıcı Adı:</label>
                        <asp:TextBox ID="txtYeniKullaniciAdi" runat="server" CssClass="text-input"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="<%= txtKADDegisimParola.ClientID %>">Parolanız (Onay İçin):</label>
                        <asp:TextBox ID="txtKADDegisimParola" runat="server" TextMode="Password" CssClass="text-input"></asp:TextBox>
                    </div>
                    
                    <asp:Button ID="btnKullaniciAdiDegistir" runat="server" Text="Kullanıcı Adını Değiştir" CssClass="action-btn" OnClick="btnKullaniciAdiDegistir_Click" />
                </div>
                <%-- SOL SÜTUN BİTİŞİ --%>


                <%-- SAĞ SÜTUN: PAROLA DEĞİŞTİRME --%>
                <div class="form-column">
                    <h3>Parola Değiştirme</h3>

                    <div class="form-group">
                        <label for="<%= txtEskiParola.ClientID %>">Eski Parola:</label>
                        <asp:TextBox ID="txtEskiParola" runat="server" TextMode="Password" CssClass="text-input"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="<%= txtYeniParola.ClientID %>">Yeni Parola:</label>
                        <asp:TextBox ID="txtYeniParola" runat="server" TextMode="Password" CssClass="text-input"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="<%= txtYeniParolaTekrar.ClientID %>">Yeni Parola Tekrar:</label>
                        <asp:TextBox ID="txtYeniParolaTekrar" runat="server" TextMode="Password" CssClass="text-input"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnParolaDegistir" runat="server" Text="Parolayı Değiştir" CssClass="action-btn" OnClick="btnParolaDegistir_Click" />
                </div>
                <%-- SAĞ SÜTUN BİTİŞİ --%>

            </div>
            <%-- İKİ SÜTUNLU GRİD BİTİŞİ --%>

            <%-- HESABI SİL BUTONU (Yeni Eklendi) --%>
            <asp:Button ID="btnHesabiSil" runat="server" Text="Hesabımı Sil" CssClass="action-btn" OnClientClick="return ConfirmDelete();" OnClick="btnHesabiSil_Click" CausesValidation="false" />

            <%-- ANA SAYFAYA DÖN BUTONU --%>
            <asp:Button ID="btnAnasayfa" runat="server" Text="Ana Sayfaya Dön" CssClass="action-btn" OnClick="btnAnasayfa_Click" CausesValidation="false" />

        </div>
    </form>
</body>
</html>