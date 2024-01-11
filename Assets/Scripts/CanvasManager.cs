using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public ButtonInfo buttonOneInfo, buttonTwoInfo, buttonThreeInfo, buttonFourInfo;
    Button buttonOne, buttonTwo, buttonThree, buttonFour;
    Image buttonOneImage, buttonTwoImage, buttonThreeImage, buttonFourImage;
    public RawImage buttonOneCrownImage, buttonTwoCrownImage, buttonThreeCrownImage, buttonFourCrownImage;

    public GameManager gameManager;

    void Awake()
    {
        buttonOneImage = buttonOneInfo.GetComponent<Image>();
        buttonTwoImage = buttonTwoInfo.GetComponent<Image>();
        buttonThreeImage = buttonThreeInfo.GetComponent<Image>();
        buttonFourImage = buttonFourInfo.GetComponent<Image>();

        buttonOne = buttonOneInfo.GetComponent<Button>();
        buttonTwo = buttonTwoInfo.GetComponent<Button>();
        buttonThree = buttonThreeInfo.GetComponent<Button>();
        buttonFour = buttonFourInfo.GetComponent<Button>();
    }

    private void OnEnable()
    {
        buttonOne.onClick.AddListener(() => {OnButtonClick(1, buttonOneInfo.interactable); });
        buttonTwo.onClick.AddListener(() => {OnButtonClick(2, buttonTwoInfo.interactable); });
        buttonThree.onClick.AddListener(() => {OnButtonClick(3, buttonThreeInfo.interactable); });
        buttonFour.onClick.AddListener(() => {OnButtonClick(4, buttonFourInfo.interactable); });
    }

    private void OnDisable()
    {
        buttonOne.onClick.RemoveListener(() => {OnButtonClick(1, buttonOneInfo.interactable); });
        buttonTwo.onClick.RemoveListener(() => {OnButtonClick(2, buttonTwoInfo.interactable); });
        buttonThree.onClick.RemoveListener(() => {OnButtonClick(3, buttonThreeInfo.interactable); });
        buttonFour.onClick.RemoveListener(() => {OnButtonClick(4, buttonFourInfo.interactable); });
    }

    public void OnButtonClick(int characterNumber, bool interactable)
    {
        if(interactable) 
            ManageCharacter(characterNumber);
    }

    public void ManageCharacter(int characterNumber)
    {
        switch (characterNumber)
        {
            case 1: ManageCharacterInternal(gameManager.characterOne, buttonOneInfo, buttonOneImage, buttonOneCrownImage); break;
            
            case 2: ManageCharacterInternal(gameManager.characterTwo, buttonTwoInfo, buttonTwoImage, buttonTwoCrownImage); break;
            
            case 3: ManageCharacterInternal(gameManager.characterThree, buttonThreeInfo, buttonThreeImage, buttonThreeCrownImage); break;
            
            case 4: ManageCharacterInternal(gameManager.characterFour, buttonFourInfo, buttonFourImage, buttonFourCrownImage); break;
            
            default: Debug.LogError("Invalid character number."); break;            
        }
    }

    public void ManageCharacterInternal(CharacterInfo clickedCharacter, ButtonInfo customButton, Image buttonImage, RawImage crownImage)
    {
        if (clickedCharacter.isSelected)
        {
            customButton.interactable = false;

            UnselectCharacter(clickedCharacter);
            gameManager.RemoveCharacterFromPotentialGroup(clickedCharacter);

            if (clickedCharacter.isPotentialLeader && gameManager.newGroupBuffor.Count != 0) 
                gameManager.MakeNextMemberInPotentialGroupALeader();

            if (clickedCharacter.isPotentialLeader)
            {
                clickedCharacter.isPotentialLeader = false;
                if (crownImage != null & crownImage.gameObject.activeSelf) 
                    DisableLeaderIcon(crownImage);
            }

            MakeButtonWhite(buttonImage);
            customButton.interactable = true;
        }
        else if (!clickedCharacter.isSelected)
        {
            customButton.interactable = false;

            SelectCharacter(clickedCharacter);
            gameManager.AddCharacterToPotentialGroup(clickedCharacter);

            if (gameManager.newGroupBuffor.Count == 1)
            {
                clickedCharacter.isPotentialLeader = true;
                EnableLeaderIcon(crownImage);
            }

            MakeButtonGreen(buttonImage);
            customButton.interactable = true;
        }
    }

    public void ShowCrownForNextPotentialLeader(CharacterInfo potentialLeader)
    {
        switch (potentialLeader.number)
        {
            case 1: EnableLeaderIcon(buttonOneCrownImage); break;

            case 2: EnableLeaderIcon(buttonTwoCrownImage); break;
            
            case 3: EnableLeaderIcon(buttonThreeCrownImage); break;

            case 4: EnableLeaderIcon(buttonFourCrownImage); break;

            default: Debug.LogError("Invalid potential leader number."); break;
        }
    }

    public void SelectCharacter(CharacterInfo characterInfo)
    {
        characterInfo.isSelected = true;
    }

    public void UnselectCharacter(CharacterInfo characterInfo)
    {
        characterInfo.isSelected = false;
    }

    public void MakeButtonWhite(Image buttonImage)
    {
        buttonImage.color = Color.white;
    }
    
    public void MakeButtonGreen(Image buttonImage)
    {
        buttonImage.color = Color.green;
    }

    public void EnableLeaderIcon(RawImage crownImage)
    {
        crownImage.gameObject.SetActive(true);
    }

    public void DisableLeaderIcon(RawImage crownImage)
    {
        crownImage.gameObject.SetActive(false);
    }
}