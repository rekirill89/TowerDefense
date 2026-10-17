using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.MVP
{
    public class BasePanelView : MonoBehaviour
    { 
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}