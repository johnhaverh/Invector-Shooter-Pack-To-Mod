using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.UI;

public class WallCoverController : MonoBehaviour
{
    [SerializeField] private Canvas _helpCanvas;
    [SerializeField] private Text _txtHelp;

    private void Start()
    {
        _helpCanvas.enabled = false;
        vThirdPersonController.OnCover += Cover;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _helpCanvas.enabled = true;
        }
    }

    private void Cover(GameObject wall, bool isNear)
    {
        if (gameObject.Equals(wall))
        {
            if (!isNear)
            {
                _txtHelp.text = "COVER";
            }
            else
            {
                _txtHelp.text = "UNCOVER";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _helpCanvas.enabled = false;
        }
    }

    private void OnDestroy()
    {
        vThirdPersonController.OnCover -= Cover;
    }
}
