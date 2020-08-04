using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FloatDisplay : MonoBehaviour
{
    private Text _textField;

    private void Awake()
    {
        _textField = GetComponent<Text>();
    }

    public void SetValue(float newValue)
    {
        _textField.text = newValue.ToString();
    }
}
