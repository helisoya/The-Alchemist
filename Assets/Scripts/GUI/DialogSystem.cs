using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem instance;

    [SerializeField] private GameObject root;
    [SerializeField] private Image characterSprite;
    [SerializeField] private LocalizedText textName;
    [SerializeField] private LocalizedText textDialog;
    [SerializeField] private Image background;

    [SerializeField] private GameObject[] buttons;

    private TextMeshProUGUI textDialogReal;

    private Coroutine coroutine;

    public bool skipDialog;

    public bool inDialog { get { return coroutine != null; } }

    private int choice;

    private string[] choiceNext;


    void Awake()
    {
        textDialogReal = textDialog.GetText();
        choiceNext = new string[3];
        choice = -1;
        skipDialog = false;
        instance = this;
        HideDialog();
    }

    public void StartDialog(string fileName)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ProcessingDialog(fileName));
    }

    public void HideDialog()
    {
        root.SetActive(false);
    }


    public void SetChoice(int choiceMade)
    {
        choice = choiceMade;
    }

    public void SkipCurrentDialog()
    {
        skipDialog = true;
    }


    IEnumerator ProcessingDialog(string fileName)
    {
        List<string> fileContent = FileManager.ReadTextAsset(Resources.Load<TextAsset>("Dialogs/" + fileName));

        GameManager.instance.playerCanMove = false;

        for (int i = 0; i < fileContent.Count; i++)
        {
            string[] splited = fileContent[i].Replace("\t", "").Split("(");
            skipDialog = false;

            if (fileContent[i].Length >= 2)
            {
                splited[1] = splited[1].Split(")")[0];
            }

            switch (splited[0])
            {
                case "DIALOG":
                    string[] infos = splited[1].Split(",");
                    textName.SetNewKey(infos[0]);

                    textDialog.SetNewKey(infos[1]);
                    textDialogReal.maxVisibleCharacters = 0;
                    root.SetActive(true);
                    textDialog.gameObject.SetActive(true);
                    foreach (GameObject button in buttons)
                    {
                        button.SetActive(false);
                    }

                    while (textDialogReal.maxVisibleCharacters < textDialogReal.text.Length)
                    {
                        textDialogReal.maxVisibleCharacters++;

                        if (skipDialog)
                        {
                            textDialogReal.maxVisibleCharacters = textDialogReal.text.Length;
                        }
                        yield return new WaitForSeconds(0.01f);
                    }


                    while (!skipDialog)
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    break;
                case "MUGSHOT":
                    if (splited[1].Equals("null"))
                    {
                        characterSprite.color = new Color(0, 0, 0, 0);
                    }
                    else
                    {
                        characterSprite.sprite = Resources.Load<Sprite>("Characters/Mugshots/" + splited[1]);
                        characterSprite.color = Color.white;
                    }

                    break;

                case "INCREMENTQUEST":
                    GameManager.player.IncrementQuest(splited[1]);
                    PlayerInfo.instance.RefreshGold();
                    break;
                case "WAIT":
                    yield return new WaitForSeconds(float.Parse(splited[1], System.Globalization.CultureInfo.InvariantCulture));
                    break;
                case "HIDE":
                    HideDialog();
                    break;
                case "SHOW":
                    root.SetActive(true);
                    break;
                case "NEWGAME":
                    GameManager.instance.NewGame(splited[1].Equals("true"));
                    break;
                case "MAINMENU":
                    GameManager.instance.ToMainMenu();
                    break;
                case "BACKGROUND":
                    if (splited[1].Equals("null"))
                    {
                        background.sprite = null;
                        background.color = new Color(0, 0, 0, 0);
                    }
                    else
                    {
                        background.sprite = Resources.Load<Sprite>("Backgrounds/" + splited[1]);
                        background.color = Color.white;
                    }
                    break;
                case "MAP":
                    string[] parameters = splited[1].Split(",");
                    if (parameters.Length != 3)
                    {
                        GameManager.ChangeLevel(parameters[0]);
                    }
                    else
                    {
                        GameManager.ChangeLevel(parameters[0], new Vector2(
                            float.Parse(parameters[1], System.Globalization.CultureInfo.InvariantCulture),
                            float.Parse(parameters[2], System.Globalization.CultureInfo.InvariantCulture))
                        );
                    }

                    break;
                case "{":
                    textDialog.gameObject.SetActive(false);
                    foreach (GameObject button in buttons)
                    {
                        button.SetActive(false);
                    }

                    int currentBox = 0;
                    i++;
                    while (currentBox <= 3 && i < fileContent.Count && fileContent[i] != "}")
                    {
                        string[] choice = fileContent[i].Replace("\t", "").Split("(");
                        if (choice[0] == "CHOICE" && choice.Length > 1)
                        {
                            choice[1] = choice[1].Split(")")[0];
                            string[] splitedLine = choice[1].Split(",");
                            choiceNext[currentBox] = splitedLine[1];
                            buttons[currentBox].SetActive(true);
                            buttons[currentBox].GetComponentInChildren<LocalizedText>().SetNewKey(splitedLine[0]);
                            currentBox++;
                        }
                        i++;
                    }

                    choice = -1;
                    while (choice == -1)
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    StartDialog(choiceNext[choice]);
                    yield break;
            }

            yield return new WaitForEndOfFrame();
        }

        HideDialog();
        GameManager.instance.playerCanMove = true;
        coroutine = null;
    }

}
