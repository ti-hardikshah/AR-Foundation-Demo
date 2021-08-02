using System.Collections;
using System.Collections.Generic;
using com.intellify.ARSceneCotroller;
using UnityEngine;

public class PaitingHandler : MonoBehaviour
{

    public GameObject _FrameObject;
    public Vector2 paintingScale;

   
    public void UpdatePainting(Texture2D texture, float width, float height) {
      /*  transform.localScale = new Vector3(width, height, 1f);
        paintingScale.x = width;
        paintingScale.y = height;
        _FrameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = texture;*/
    }


    public void ResetScale() {
        transform.localScale = new Vector3(paintingScale.x, paintingScale.y, 1f);
    }


    public void UpdateSelected(bool selected) {

        ARSceneCotroller.Instance.UpdatePaintingSelection(this.gameObject, selected);
    }

}
