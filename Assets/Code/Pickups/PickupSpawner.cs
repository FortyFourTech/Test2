using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PickupSpawner : MonoBehaviourPun
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private PickupBase _pickupPrefab;
    [SerializeField] private Transform _spawnTransform;

    private PickupBase _spawned;

    private void OnEnable()
    {
        _Respawn();
    }

    private void _Respawn()
    {
        if (_spawned)
        {
            _spawned.onConsumed -= OnConsume;
        }

        StartCoroutine(_SpawnDelayed(_spawnDelay));
    }

    private void OnConsume(GamePlayerController byPlayer)
    {
        _Respawn();
    }

    private IEnumerator _SpawnDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (PhotonNetwork.IsMasterClient) // проверка перенесена на конец корутины на случай host migration
        {
            _SpawnPickup_Master();
        }
    }

    private void _SpawnPickup_Master()
    {
        var spawnedGO = PhotonNetwork.InstantiateSceneObject(_pickupPrefab.gameObject.name, _spawnTransform.position, _spawnTransform.rotation);
        var spawned = spawnedGO.GetComponent<PickupBase>();
        var spawnedPv = spawned.GetComponent<PhotonView>();

        _spawned = spawned;
        _spawned.onConsumed += OnConsume;

        var spawnedViewId = spawnedPv.ViewID;
        photonView.RPC("PickupSpawned_Slave", RpcTarget.OthersBuffered, spawnedViewId);
    }

    [PunRPC]
    public void PickupSpawned_Slave(int pvId)
    {
        var spawnedPv = PhotonView.Find(pvId);

        if (spawnedPv)
        {
            _spawned = spawnedPv.GetComponent<PickupBase>();
            _spawned.onConsumed += OnConsume;
        }
    }
}
