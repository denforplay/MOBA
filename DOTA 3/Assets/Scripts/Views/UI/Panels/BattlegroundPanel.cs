using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.UI.Images;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Views.UI.Panels
{
    public class BattlegroundPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _leftTeamCounterText;
        [SerializeField] private TextMeshProUGUI _rightTeamCounterText;
        [SerializeField] private List<CharacterBattlegroundInfoImage> _leftTeamImages;
        [SerializeField] private List<CharacterBattlegroundInfoImage> _rightTeamImages;
        
        public void Initialize(List<CharacterInfo> leftTeamCharacters, List<CharacterInfo> rightTeamCharacters)
        {
            for (int i = 0; i < leftTeamCharacters.Count; i++)
            {
                _leftTeamImages[i].PlayerImage.sprite = leftTeamCharacters[i].CharacterIcon;
            }
            
            for (int i = 0; i < rightTeamCharacters.Count; i++)
            {
                _rightTeamImages[i].PlayerImage.sprite = rightTeamCharacters[i].CharacterIcon;
            }
        }
    }
}