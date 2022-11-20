using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI.Panels
{
    public class TeamCharactersPanel : MonoBehaviour
    {
        [SerializeField] private List<Image> _charactersImages;

        public List<Image> CharactersImages => _charactersImages;
    }
}