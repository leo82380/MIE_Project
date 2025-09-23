using MIE.BoardSystem.Slot;
using UnityEngine;

namespace MIE.BoardSystem
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BaseSlot boardPrefab;
        [SerializeField] private GameObject itemPrefab;
    }
}
