using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
[ExecuteAlways]
public class QRGenrator : MonoBehaviour
{
    public static QRGenrator qRGenrator;
    public InputField qRlinkField;
    public Button createQRbutton;
    [Header("Pass your link or text")]
    public string qrValue;
    [Header("Pass a raw image")]
    public RawImage qrImg;
    [Header("Set Texture Hight and Width (recommended 256*256)")]
    public int hight = 256;
    public int width = 256;
    private Texture2D storeEncodedTexture;
    void Awake()
    {
        if(qRGenrator==null)
        {
            qRGenrator=this;
        }
    }
    private void Start()
    {
        createQRbutton.onClick.AddListener(()=>EncodeTextToQRCode(qRlinkField.text));
        storeEncodedTexture = new Texture2D(hight, width);
        EncodeTextToQRCode(qrValue);
    }
    public Color32[] Encode(string textForEncoding, int width, int hight)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = hight,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
    public void OnClickEncode()
    {
        EncodeTextToQRCode(qrValue);
    }
    public void EncodeTextToQRCode(string qrValue)
    {
        string textWrite = qrValue;
        Color32[] convertPixelTotexture = Encode(textWrite, storeEncodedTexture.width, storeEncodedTexture.height);
        storeEncodedTexture.SetPixels32(convertPixelTotexture);
        storeEncodedTexture.Apply();

        qrImg.texture = storeEncodedTexture;
    }

    [CustomEditor(typeof(QRGenrator))]
    public class QRGenratorButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Genrate QR"))
            {
                qRGenrator.OnClickEncode();
            }
            if (GUILayout.Button("More Tools"))
            {
                Application.OpenURL("https://sites.google.com/view/umarhyatttools/home");
            }
            GUIStyle style = new GUIStyle { fontSize = 20, fontStyle = FontStyle.BoldAndItalic };
            EditorGUILayout.LabelField("UmarHyatt QR Genrator v1.0", style);

        }
    }
}
