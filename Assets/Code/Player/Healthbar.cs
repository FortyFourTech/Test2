using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Healthbar : MonoBehaviour
{
#region Fields
    [Header("Refs")]
    [SerializeField] protected PhotonView _photonView;
    [SerializeField] private Slider _healthSlider;

    protected Player _owner;
    protected Health _ownerHealth;
#endregion

#region Unity functions
    protected virtual void OnEnable()
    {
        _owner = _photonView.Owner;

        StartCoroutine(AwaitPlayerHealth());
    }

    private IEnumerator AwaitPlayerHealth()
    {
        for (
            _ownerHealth = _owner.References().m_Controller?.Health;
            _ownerHealth == null;
            _ownerHealth = _owner.References().m_Controller?.Health
        )
        {
            yield return null;
        }
        
        _ownerHealth.onChanged += UpdateView;

        UpdateView(0f);

        yield break;
    }

    protected virtual void OnDisable()
    {
        if (_ownerHealth)
        {
            _ownerHealth.onChanged -= UpdateView;

            _ownerHealth = null;
            _owner = null;
        }
    }
#endregion

    private void UpdateView(float change)
    {
        float curHealth = _ownerHealth.CurrentHealth;
        float maxHealth = _ownerHealth.MaxHealth;

        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = curHealth;
    }
}
