using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizedTextSetter : MonoBehaviour
{
    public static void SetLocalizedText(string key, Text textField)
    {
        var localizedString = new LocalizedString("Localization", key);
        localizedString.StringChanged += (value) =>
        {
            textField.text = value;
        };
    }

}
public class LocalizedStringVar
{
    public string key;
    private LocalizedString localizedString;

    public LocalizedStringVar(string key)
    {
        this.key = key;
        localizedString = new LocalizedString { TableReference = "Localization", TableEntryReference = key };
    }

    public void Get(System.Action<string> callback)
    {
        localizedString.GetLocalizedStringAsync().Completed += (handle) =>
        {
            callback(handle.Result);
        };
    }
}