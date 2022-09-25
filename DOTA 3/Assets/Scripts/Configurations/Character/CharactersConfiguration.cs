using System.Collections.Generic;
using UnityEngine;

namespace Configurations.Character
{
    [CreateAssetMenu(menuName = "Configurations/Characters configuration", order = 0)]
    public class CharactersConfiguration : ScriptableObject
    {
        [SerializeField] private List<CharacterInfo> _characterInfos;

        public List<CharacterInfo> CharacterInfos => _characterInfos;
    }
}