using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContainer : MonoBehaviour
{
    public void OpenAboutUsPopup() => App.Get<GUIManager>().ShowGui<AboutUsPopup>();
    public void OpenSettings()
    {
        var notiPopup = App.Get<GUIManager>().ShowGui<NotiPopup>();
        notiPopup.Tittle = "Opp!!";
        notiPopup.Content = "Feature not ready!!!!";
        notiPopup.OKAction = () =>
        {
            Debug.Log("Popup Setting Close!");
        };
    }
}
