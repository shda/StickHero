using TMPro;
using UnityEngine;

namespace CoffeeShopGame.Scripts.Common.Ui
{
    public class VersionLabel : MonoBehaviour
    {
        [SerializeField] private TextAsset versionAsset;
        [SerializeField] private string beforeText;
        [SerializeField] private TextMeshProUGUI versionLabel;

        private void Awake()
        {
            versionLabel.text = beforeText + versionAsset.text;
        }
    }
}