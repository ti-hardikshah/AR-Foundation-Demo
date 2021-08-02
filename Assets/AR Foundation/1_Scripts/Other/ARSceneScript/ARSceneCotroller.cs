using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace com.intellify.ARSceneCotroller
{
    public class ARSceneCotroller : MonoBehaviour
    {

        public static ARSceneCotroller Instance;

        [SerializeField] ARSession m_Session;
        [SerializeField] GameObject errorDialogBox;

  
        [SerializeField] Text selectedItemInfo = default;

        public GameObject currentSelected;
        public GameObject ResetButton;

        //  public GameManager gameManager;

        public string[] modelName;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            #if !UNITY_EDITOR
                   StartCoroutine(checkingDeviceSupport());
            #endif
        }

        IEnumerator checkingDeviceSupport()
        {
            if ((ARSession.state == ARSessionState.None) ||
                (ARSession.state == ARSessionState.CheckingAvailability))
            {
                yield return ARSession.CheckAvailability();
            }

            if (ARSession.state == ARSessionState.Unsupported)
            {
                Debug.Log("Device not supported");
                errorDialogBox.SetActive(true);
            }
            else
            {
                m_Session.enabled = true;
               // GenrateScrollView(gameManager._PaintingInfo);
            }
        }



        public void OnClickItem(int index)
        {
            PlacementController.Instance.ResetPlacement();
            PlacementController.Instance.UpdateItemState(modelName[index]);
        }




        public void ResetScale() {
            if (currentSelected)
                currentSelected.GetComponent<PaitingHandler>().ResetScale();
        }


        public void DestroySelected() {
            if (currentSelected)
                Destroy(currentSelected);

        }


        public void  UpdatePaintingSelection(GameObject current, bool isSelected) {
            currentSelected = current;
            ResetButton.SetActive(isSelected);
        }
    }
}
