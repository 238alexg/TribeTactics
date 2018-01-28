using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSignIn : MonoBehaviour {

    public Button[] P1ClassSelections;
    public Button[] P2ClassSelections;

    public InputField Player1Name;
    public InputField Player2Name;

    void Start()
    {
        for (int i = 0; i < (int)Tribe.Count; i++)
        {
            Tribe tribe = (Tribe)i;
            P1ClassSelections[i].onClick.AddListener(() => PlayerClassSelection(tribe, true));
            P2ClassSelections[i].onClick.AddListener(() => PlayerClassSelection(tribe, false));
        }

        Player1Name.onEndEdit.AddListener((string s) => ChangePlayerName(s, true));
        Player2Name.onEndEdit.AddListener((string s) => ChangePlayerName(s, false));
    }

    public void PlayerClassSelection(Tribe tribe, bool isPlayer1)
    {
        Player player = isPlayer1 ? GameplayManager.Inst.Player1 : GameplayManager.Inst.Player2;
        player.Tribe = tribe;
        print("Player 1's tribe is " + tribe);
    }

    public void ChangePlayerName(string name, bool isPlayer1)
    {
        Player player = isPlayer1 ? GameplayManager.Inst.Player1 : GameplayManager.Inst.Player2;
        player.Name = name;
        print("Player 1's name is " + name);
    }
}
