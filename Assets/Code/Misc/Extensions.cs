using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ExtensionMethods
{
    public static T GetOrAddComponent<T>(this Component compo) where T : Component
    {
        return compo.gameObject.GetOrAddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        var foundCompo = go.GetComponent<T>();

        if (foundCompo != null)
        {
            return foundCompo;
        }
        else
        {
            var addedCompo = go.AddComponent<T>();

            return addedCompo;
        }
    }

    public static T GetRandomElement<T>(this T[] array)
    {
        if (array.Length > 0)
            return array[Random.Range(0, array.Length)];
        else
            return default(T);
    }

    /// <param name="probabilities">Array of probabilities to get each element from the head of the array</param>
    /// <returns>Element from the array, if it's not empty. Default value for T type otherwise.</returns>
    public static T GetRandomElementWithProbs<T>(this T[] array, float[] probabilities)
    {
        T result; ;
        if (array.Length > 0)
        {
            var chooseValue = Random.value * 100;
            var choosedIndex = 0;
            var summ = 0f;
            for (int i = 0; i < probabilities.Length && i < array.Length; i++)
            {
                summ += probabilities[i];
                choosedIndex = i;
                if (chooseValue < summ)
                {
                    break;
                }
            }
            var choosedElement = array[choosedIndex];

            result = choosedElement;
        }
        else
        {
            result = default(T);
        }

        return result;
    }

    public static void DebugLog(this object obj, string debugText)
    {
        var possiblePathes = new string[] {
            "DebugCanvas/Text",
            "Canvas/Debug",
        };
        GameObject textObject = null;
        foreach (var path in possiblePathes)
        {
            textObject = GameObject.Find(path);
            if (textObject != null) break;
        }

        if (textObject != null)
        {
            var textComponent = textObject.GetComponent<UnityEngine.UI.Text>();
            textComponent.text = debugText;
        }

        Debug.Log(debugText);
    }

    public static Transform CreateChild(this MonoBehaviour mb, string name)
    {
        var go = new GameObject(name);
        var tr = go.transform;
        tr.SetParent(mb.transform);
        tr.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        return tr;
    }
}