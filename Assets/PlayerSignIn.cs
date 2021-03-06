﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSignIn : MonoBehaviour {

    public Button[] P1ClassSelections;
    public Button[] P2ClassSelections;

    public InputField Player1Name;
    public InputField Player2Name;

    public GameObject SignInScreen;
    public Button EnterGameButton;

    bool ReadyToEnterGame {
        get
        {
            return GameplayManager.Inst.Player1.IsSignedIn
                && GameplayManager.Inst.Player2.IsSignedIn;
        }
    }

    void Start()
    {
        for (int i = 0; i < (int)Tribe.Count; i++)
        {
            Tribe tribe = GetTribeFromButtonName(P1ClassSelections[i].name);

            P1ClassSelections[i].onClick.AddListener(() => PlayerClassSelection(tribe, true));
            P2ClassSelections[i].onClick.AddListener(() => PlayerClassSelection(tribe, false));
        }

        Player1Name.onValueChanged.AddListener((string s) => ChangePlayerName(s, true));
        Player2Name.onValueChanged.AddListener((string s) => ChangePlayerName(s, false));
    }

    public void PlayerClassSelection(Tribe tribe, bool isPlayer1)
    {
        
        Player player = isPlayer1 ? GameplayManager.Inst.Player1 : GameplayManager.Inst.Player2;
        player.Tribe = tribe;

        Button[] tribeButtons = isPlayer1 ? P1ClassSelections : P2ClassSelections;
        for (int i = 0; i < tribeButtons.Length; i++)
        {
            Color buttonColor = tribeButtons[i].image.color;
            buttonColor.a = (int)tribe == i ? 1 : 0.4f;
            tribeButtons[i].image.color = buttonColor;
        }
        CheckForEnterGameButtonReady();
    }

    void CheckForEnterGameButtonReady()
    {
        EnterGameButton.interactable = ReadyToEnterGame;
    }

    public void ChangePlayerName(string name, bool isPlayer1)
    {
        Player player = isPlayer1 ? GameplayManager.Inst.Player1 : GameplayManager.Inst.Player2;
        player.Name = name;
        CheckForEnterGameButtonReady();
    }

    public void EnterGame()
    {
        GameSetup.Inst.SetUpGame();
        SignInScreen.SetActive(false);
    }

    public Tribe GetTribeFromButtonName(string name)
    {
        switch (name)
        {
            case "Metal":
                return Tribe.Knight;
            case "Nomads":
                return Tribe.Desert;
            case "Pirates":
                return Tribe.Pirate;
            case "Nymphs":
                return Tribe.Nymph;
            case "Dwarves":
                return Tribe.Dwarf;
            default:
                return Tribe.Count;
        }
    }
}
