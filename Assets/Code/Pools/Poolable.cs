using System;
using System.Collections;
using UnityEngine;

namespace Dimar.Pools
{
	public class Poolable : MonoBehaviour
	{
		public Action onQueue;
		public Action onReturn;
		
		public GameObject m_prefab;
		public PrefabPooler m_pool;

		private Coroutine _delayCoroutine = null;

		public void QueueFromPool()
		{
			gameObject.SetActive(true);

			onQueue?.Invoke();
		}

		public void ReturnToPool()
		{
			if (_delayCoroutine != null)
				StopCoroutine(_delayCoroutine);
			
			gameObject.SetActive(false);
			m_pool.ReturnObject(this);

			onReturn?.Invoke();
		}

		public void ReturnWithDelay(float delay)
		{
			if (_delayCoroutine != null)
				StopCoroutine(_delayCoroutine);
			
			_delayCoroutine = StartCoroutine(_ReturnDelayed(delay));
		}

		private IEnumerator _ReturnDelayed(float delay)
		{
			yield return new WaitForSeconds(delay);
			ReturnToPool();

			_delayCoroutine = null;
		}
	}
}
