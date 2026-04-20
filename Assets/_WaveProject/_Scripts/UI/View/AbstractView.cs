using System;
using UnityEngine;

namespace WaveProject.UI.View
{
    public abstract class AbstractView : MonoBehaviour, IView
    {
        public void Close(Action callback = null)
        {
            gameObject.SetActive(false);
            callback?.Invoke();
        }

        public void Open(Action callback = null)
        {
            gameObject.SetActive(true);
            callback?.Invoke();
        }

        public virtual void Init()
        {
            Debug.Log($"{transform.name} was inited!");
        }
    }

    public abstract class AbstractPopup : AbstractView { }
}