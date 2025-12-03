'------------------------------------------------------------------------------
' <otomatik üretildi>
'     Bu kod bir araç tarafından oluşturuldu.
'
'     Bu dosyada yapılacak değişiklikler yanlış davranışa neden olabilir ve
'     kod tekrar üretildi. 
' </otomatik üretildi>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class IlanYukle

    '''<summary>
    '''form1 denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents form1 As Global.System.Web.UI.HtmlControls.HtmlForm

    '''<summary>
    '''txtBaslik denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents txtBaslik As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''RequiredFieldValidatorBas denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents RequiredFieldValidatorBas As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''ddlKategori denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents ddlKategori As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''RequiredFieldValidatorKat denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents RequiredFieldValidatorKat As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''ddlDurum denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents ddlDurum As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''RequiredFieldValidatorDurum denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents RequiredFieldValidatorDurum As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''txtFiyat denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents txtFiyat As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''RequiredFieldValidatorFiyat denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents RequiredFieldValidatorFiyat As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''CompareValidator1 denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents CompareValidator1 As Global.System.Web.UI.WebControls.CompareValidator

    '''<summary>
    '''ddlSehir denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents ddlSehir As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''RequiredFieldValidatorSehir denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents RequiredFieldValidatorSehir As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''txtTelefon denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents txtTelefon As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''RequiredFieldValidatorTel denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents RequiredFieldValidatorTel As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''txtAciklama denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents txtAciklama As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''RequiredFieldValidatorAcik denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents RequiredFieldValidatorAcik As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''fuResim denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents fuResim As Global.System.Web.UI.WebControls.FileUpload

    '''<summary>
    '''CustomValidatorResim denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents CustomValidatorResim As Global.System.Web.UI.WebControls.CustomValidator

    '''<summary>
    '''btnIlanEkle denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents btnIlanEkle As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''lblMesaj denetimi.
    '''</summary>
    '''<remarks>
    '''Otomatik üretilmiş alan.
    '''Değiştirmek için, alan bildirimini tasarımcı dosyasından arka plan kod dosyasına taşıyın.
    '''</remarks>
    Protected WithEvents lblMesaj As Global.System.Web.UI.WebControls.Label
End Class
