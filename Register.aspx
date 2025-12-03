<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Register.aspx.vb" Inherits="Serhan_Satis.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kayıt Ol - Orion</title>
    <link rel="icon" href="Iconlar/Login 32x32 ico.ico" type="image/x-icon" />
    <%-- İKONLAR İÇİN HARİCİ KÜTÜPHANE EKLEMESİ --%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />

    <style>

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #74ebd5 0%, #9face6 100%);
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }

        .register {
            width: 350px;
            background: rgba(255, 255, 255, 0.95);
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
            transition: transform 0.3s ease-in-out;
        }
        
        .register:hover {
            transform: translateY(-5px);
        }

        h2 {
            text-align: center;
            color: #333;
            margin-bottom: 25px;
            border-bottom: 2px solid #9face6; 
            padding-bottom: 10px;
        }

        /* Yeni: İKON GRUBU STİLLERİ */
        .input-icon-group {
            position: relative;
            margin-bottom: 15px; /* Margin'i buraya taşıdık */
        }
        
        /* Inputlar */
        input[type=text], input[type=password] {
            width: 100%;
            padding: 12px 15px;
            padding-left: 40px; /* İkon için boşluk eklendi */
            margin: 0; /* İç margin sıfırlandı */
            border-radius: 8px;
            border: 1px solid #ccc;
            box-sizing: border-box; 
            transition: border-color 0.3s;
        }

        /* Yeni: İKON Stili */
        .input-icon-group i {
            position: absolute;
            left: 15px;
            top: 50%;
            transform: translateY(-50%);
            color: #9face6; /* Tema rengi */
            font-size: 16px;
            z-index: 10;
        }


        input[type=text]:focus, input[type=password]:focus {
            border-color: #9face6; 
            outline: none;
        }

        .btn {
            width: 100%;
            padding: 12px;
            background: #9face6; 
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            margin-top: 15px;
            transition: background 0.3s, transform 0.1s;
        }

        .btn:hover {
            background: #74ebd5; 
            transform: translateY(-2px);
        }

        .login {
            text-align: center;
            margin-top: 20px;
        }
        
        .login a {
            color: #555;
            text-decoration: none;
            font-size: 14px;
            transition: color 0.3s;
        }

        .login a:hover {
            color: #9face6; 
            text-decoration: underline;
        }

        #lblMesaj {
            display: block;
            text-align: center;
            margin-top: 15px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register">

            <h2>Orion'a Hoşgeldiniz</h2>
            <h2>Kayıt Ol</h2>
            
            <%-- Kullanıcı Adı Inputu --%>
            <div class="input-icon-group">
                <i class="fas fa-user"></i> 
                <asp:TextBox ID="txtKullanici" runat="server" placeholder="Kullanıcı Adı" MaxLength="30"></asp:TextBox>
            </div>
            
            <%-- Parola Inputu --%>
            <div class="input-icon-group">
                <i class="fas fa-lock"></i> 
                <asp:TextBox ID="txtParola" runat="server" TextMode="Password" placeholder="Parola" MaxLength="30"></asp:TextBox>
            </div>
            
            <asp:Button ID="btnKayitOl" runat="server" Text="Kayıt Ol" CssClass="btn" />
            
            <div class="login">
                <a href="Login.aspx">Zaten hesabın var mı? Giriş Yap</a>
            </div>
            
            <asp:Label ID="lblMesaj" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>