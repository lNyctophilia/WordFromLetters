using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputField roundOfNumberInputField;
    [SerializeField] private Score score;

    [Space(10)]
    [SerializeField] private Dropdown languageDropdown;

    private void Awake()
    {
        int currentIndex = LocalizationSettings.AvailableLocales.Locales
            .IndexOf(LocalizationSettings.SelectedLocale);
        languageDropdown.value = currentIndex;

        SetLanguage(currentIndex);
        roundOfNumberInputField.text = score.scoreToWin.ToString();
    }

    public void SetRoundOfNumber(string roundOfNumberString)
    {
        int roundOfNumber = string.IsNullOrEmpty(roundOfNumberString) ? 3 : int.Parse(roundOfNumberString);
        if (roundOfNumber <= 0) roundOfNumber = 3;

        roundOfNumberInputField.text = roundOfNumber.ToString();
        score.scoreToWin = roundOfNumber;
    }

    public void SetLanguage(int index)
    {
        if (index >= 0 && index < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}
