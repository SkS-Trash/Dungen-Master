using UnityEngine;

namespace Settings.AudioVideoOptionsMenu
{
    public class TogglePanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject dropDownPanelObj;

        public void Button_ToggleDropDownPanel()
        {
            dropDownPanelObj.SetActive(!dropDownPanelObj.activeSelf);
        }
    }
}
