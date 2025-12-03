<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Anasayfa.aspx.vb" Inherits="Serhan_Satis.Anasayfa" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ana Sayfa - Orion</title>
    <link rel="icon" href="Iconlar/Alış Ve Satışlar ICO.ico" type="image/x-icon" />

    <style>
        :root {
            /* Açık Mod Varsayılan Değerler */
            --background-color: #f0f2f5;
            --card-background: #ffffff;
            --header-background: #ffffff;
            --text-color: #333;
            --secondary-text: #888;
            --border-color: #e0e2e5;
            --shadow-color: rgba(0, 0, 0, 0.1);
            --header-blue: #3b5998;
            --footer-bg: #f7f7f7;
            --search-border: #ced4da;
        }

        .dark-mode {
            /* Karanlık Mod Değerleri */
            --background-color: #121212;
            --card-background: #1e1e1e;
            --header-background: #1e1e1e;
            --text-color: #ffffff;
            --secondary-text: #b0b3b8;
            --border-color: #333333;
            --shadow-color: rgba(255, 255, 255, 0.08);
            --header-blue: #70a4ff;
            --footer-bg: #282828;
            --search-border: #444;
        }

        body {
            font-family: 'Poppins', sans-serif;
            background-color: var(--background-color);
            margin: 0;
            padding: 0;
            line-height: 1.6;
            transition: background-color 0.3s, color 0.3s;
        }

        .header {
            background: var(--header-background);
            border-bottom: 1px solid var(--border-color);
            padding: 10px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            box-shadow: 0 1px 4px var(--shadow-color);
            position: sticky;
            top: 0;
            z-index: 100;
            flex-wrap: wrap;
        }

        .header .logo {
            font-size: 28px;
            font-weight: 700;
            color: var(--header-blue);
            margin-right: 20px;
        }

        .search-area {
            display: flex;
            align-items: center;
            gap: 5px;
            flex-grow: 2;
            max-width: 350px;
            margin: 5px 20px;
        }

        .search-box {
            padding: 8px 15px;
            border: 1px solid var(--search-border);
            border-radius: 5px;
            font-size: 15px;
            width: 70%;
            box-sizing: border-box;
            background-color: var(--card-background);
            color: var(--text-color);
        }

        .search-btn {
            background-color: #28a745;
            color: white;
            border: 1px solid #28a745;
            padding: 8px 15px;
            border-radius: 5px;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.3s;
            flex-shrink: 0;
        }

        .search-btn:hover {
            background-color: #1e7e34;
            border-color: #1e7e34;
            transform: translateY(-1px);
        }

        .header .user-info {
            color: var(--text-color);
            font-size: 16px;
            font-weight: 500;
            flex-shrink: 0;
            margin-left: 20px;
        }


        .header .nav-buttons {
            display: flex;
            gap: 10px;
            flex-shrink: 0;
            flex-wrap: wrap;
            justify-content: flex-end;
        }

        .action-btn {
            padding: 10px 20px;
            border-radius: 5px;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.1s;
            border: 1px solid transparent;
            text-decoration: none;
            text-align: center;
            display: inline-block;
        }

        #theme-toggle {
            background-color: var(--background-color);
            color: var(--text-color);
            border: 1px solid var(--border-color);
        }

        .dark-mode #theme-toggle {
            background-color: #333;
            border-color: #444;
            color: #fff;
        }

        #btnFavorilerim { background-color: #ffc107; color: #333; border-color: #ffc107; }
        #btnFavorilerim:hover { background-color: #e0a800 !important; border-color: #e0a800 !important; transform: translateY(-1px); }
        #btnIlanYukle, #btnIlanlarim, #btnHesapDetay { background-color: #007bff; color: white; border-color: #007bff; }
        #btnIlanYukle:hover, #btnIlanlarim:hover, #btnHesapDetay:hover { background-color: #0056b3; border-color: #0056b3; transform: translateY(-1px); }
        #btnCikis { background-color: #dc3545; color: white; border-color: #dc3545; }
        #btnCikis:hover { background-color: #c82333; border-color: #c82333; transform: translateY(-1px); }

        .content-container { padding: 40px 30px; max-width: 1300px; margin: 0 auto; }
        h3 { 
            color: var(--text-color); 
            border-bottom: 2px solid var(--header-blue); 
            padding-bottom: 10px; 
            margin-bottom: 30px; 
            font-size: 22px; 
            font-weight: 600; 
        }


        .ilan-listesi {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 25px;
            width: 100%;
        }
        .ilan-listesi a { text-decoration: none; color: inherit; }
        .ilan-card {
            background: var(--card-background);
            border-radius: 8px;
            box-shadow: 0 4px 12px var(--shadow-color);
            overflow: hidden;
            transition: transform 0.3s, box-shadow 0.3s;
            height: 100%;
        }
        .ilan-listesi a:hover .ilan-card { transform: translateY(-5px); box-shadow: 0 8px 20px var(--shadow-color); }
        .ilan-card-img { width: 100%; height: 220px; object-fit: cover; }
        .ilan-card-body { padding: 15px; padding-bottom: 5px; }
        .ilan-card-body h4 { margin: 0 0 10px; color: var(--text-color); font-size: 17px; display: flex; justify-content: space-between; }
        .ilan-card-body h4 .fiyat { font-size: 20px; color: #007bff; font-weight: 700; }
        .ilan-card-body h4 .sehir { font-size: 14px; font-weight: normal; color: var(--header-blue); }
        .ilan-card-body p { color: var(--text-color); font-size: 16px; font-weight: 600; margin-bottom: 10px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis; }
        .ilan-card-footer { 
            padding: 12px 15px; 
            background-color: var(--footer-bg); 
            font-size: 13px; 
            color: var(--secondary-text); 
            display: flex; 
            justify-content: space-between; 
            border-top: 1px solid var(--border-color); 
        }

        #lblCikisMesaj {
            display: block;
            text-align: center;
            padding: 15px;
            margin: 10px auto 30px auto;
            max-width: 600px;
            background-color: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
            border-radius: 5px;
            font-weight: bold;
        }

        /* Yeni Eklenen Stil: İlan Bulunamadı Mesajı */
        #lblSonucMesaj {
            display: block;
            text-align: center;
            padding: 20px;
            margin-top: 20px;
            background-color: var(--card-background);
            color: var(--text-color);
            border: 2px dashed var(--border-color);
            border-radius: 8px;
            font-size: 18px;
            font-weight: 600;
        }


        .filtre-alani {
            display: flex;
            gap: 15px;
            margin-bottom: 25px;
        }
        .filtre-alani select, .filtre-alani input {
            padding: 8px 12px;
            border: 1px solid var(--search-border);
            border-radius: 5px;
            font-size: 15px;
            background-color: var(--card-background);
            color: var(--text-color);
        }
        .filtre-btn {
            background-color: #3b5998;
            color: white;
            border: none;
            padding: 8px 15px;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        .filtre-btn:hover { background-color: #2d4373; }


        @media (max-width: 1200px) {
            .header {
                padding: 10px 20px;
            }

            .header .logo {
                order: -1;
                margin-right: 15px;
            }

            .header .user-info {
                order: 0;
                margin-left: auto;
            }

            .search-area {
                order: 1;
                flex-grow: 1;
                max-width: 100%;
                margin: 10px 0;
            }

            .search-box {
                width: 100%;
            }

            .header .nav-buttons {
                order: 2;
                flex-grow: 1;
                justify-content: space-around;
                gap: 8px;
                margin-top: 5px;
            }

            .action-btn {
                flex-grow: 1;
                min-width: 100px;
                padding: 8px 15px;
                font-size: 14px;
            }


            .filtre-alani {
                flex-wrap: wrap;
            }
            .filtre-alani select, .filtre-alani input {
                flex-grow: 1;
                min-width: 120px;
            }
        }

    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="logo">Orion Satış</div>

            <div class="search-area">
                <asp:TextBox ID="txtArama" runat="server" CssClass="search-box" placeholder="İlan Başlığı Ara..."></asp:TextBox>
                <asp:Button ID="btnAra" runat="server" Text="Ara" CssClass="search-btn" OnClick="btnAra_Click" />
            </div>

            <div class="user-info">
                Hoşgeldin, <asp:Label ID="lblKullanici" runat="server"></asp:Label>!
            </div>

            <div class="nav-buttons">
                <button id="theme-toggle" type="button" class="action-btn">
                    <span id="toggle-icon">🌙</span> Mod
                </button>
                <asp:Button ID="btnFavorilerim" runat="server" Text="Favorilerim" CssClass="action-btn" OnClick="btnFavorilerim_Click" />
                <asp:Button ID="btnIlanlarim" runat="server" Text="İlanlarım" CssClass="action-btn" OnClick="btnIlanlarim_Click" />
                <asp:Button ID="btnIlanYukle" runat="server" Text="İlan Yükle" CssClass="action-btn" OnClick="btnIlanYukle_Click" />
                <asp:Button ID="btnHesapDetay" runat="server" Text="Hesap Detay" CssClass="action-btn" OnClick="btnHesapDetay_Click" />
                <asp:Button ID="btnCikis" runat="server" Text="Çıkış Yap" CssClass="action-btn" OnClick="btnCikis_Click" />
            </div>
        </div>

        <div class="content-container">
            <h3>Öne Çıkan İlanlar</h3>

            <div class="filtre-alani">
                <asp:DropDownList ID="ddlSehir" runat="server">
                    <asp:ListItem Text="Tüm Şehirler" Value=""></asp:ListItem>
                </asp:DropDownList>

                <asp:DropDownList ID="ddlKategori" runat="server">
                    <asp:ListItem Text="Tüm Kategoriler" Value=""></asp:ListItem>
                </asp:DropDownList>

                <asp:TextBox ID="txtMinFiyat" runat="server" placeholder="Min Fiyat"></asp:TextBox>
                <asp:TextBox ID="txtMaxFiyat" runat="server" placeholder="Max Fiyat"></asp:TextBox>

                <asp:Button ID="btnFiltrele" runat="server" Text="Filtrele" CssClass="filtre-btn" OnClick="btnFiltrele_Click" />
            </div>

            <asp:Label ID="lblCikisMesaj" runat="server" Visible="false"></asp:Label>
            
            <%-- YENİ EKLENEN KONTROL: İlan Bulunamadı Mesajı --%>
            <asp:Label ID="lblSonucMesaj" runat="server" Text="Aradığınız Kriterde İlan Yok" Visible="false"></asp:Label>


            <asp:Repeater ID="rpIlanlar" runat="server">
                <HeaderTemplate><div class="ilan-listesi"></HeaderTemplate>
                <ItemTemplate>
                    <a href='IlanDetay.aspx?ilanID=<%# Eval("ilanID") %>'>
                        <div class="ilan-card">
                            <img src='<%# Eval("Resim_Yolu") %>' alt='<%# Eval("Baslik") %>' class="ilan-card-img" onerror="this.onerror=null; this.src='placeholder.jpg';" />
                            <div class="ilan-card-body">
                                <h4><span class="fiyat"><%# FormatFiyat(Eval("Fiyat")) %></span><span class="sehir"><%# Eval("Sehir") %></span></h4>
                                <p><%# Eval("Baslik") %></p>
                            </div>
                            <div class="ilan-card-footer">
                                <span>Satıcı: <%# Eval("Kullanici_Adi") %></span>
                                <span><%# Eval("Tarih", "{0:dd.MM.yyyy}") %></span>
                            </div>
                        </div>
                    </a>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
    
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const toggleButton = document.getElementById('theme-toggle');
            const toggleIcon = document.getElementById('toggle-icon');
            const htmlElement = document.documentElement;

            function applyTheme(theme) {
                if (theme === 'dark') {
                    htmlElement.classList.add('dark-mode');
                    toggleIcon.textContent = '☀️';
                    toggleButton.title = 'Açık Moda Geç';
                } else {
                    htmlElement.classList.remove('dark-mode');
                    toggleIcon.textContent = '🌙';
                    toggleButton.title = 'Karanlık Moda Geç';
                }
            }

            const savedTheme = localStorage.getItem('theme') || 'light';
            applyTheme(savedTheme);

            toggleButton.addEventListener('click', function () {
                const currentTheme = htmlElement.classList.contains('dark-mode') ? 'dark' : 'light';
                let newTheme = currentTheme === 'light' ? 'dark' : 'light';

                applyTheme(newTheme);
                localStorage.setItem('theme', newTheme);
            });
        });
    </script>
</body>
</html>