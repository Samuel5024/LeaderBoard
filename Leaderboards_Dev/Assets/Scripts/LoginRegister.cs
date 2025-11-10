using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;


public class LoginRegister : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public TextMeshProUGUI displayText;
    public UnityEvent onLoggedIn;

    [HideInInspector]
    public string playFabId;

    public static LoginRegister instance;
    private void Awake()
    {
        displayText.gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
    }
    public void OnRegister()
    {
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            DisplayName = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false

        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
            result =>
            {
                SetDisplayText(result.PlayFabId, Color.green);
            },
            error =>
            {
                SetDisplayText(error.ErrorMessage, Color.red);
            }
        );
    }

    public void OnLoginButton()
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest,
            result =>
            {
                Debug.Log("Logged in as: " + result.PlayFabId);
                
                if(onLoggedIn != null)
                {
                    onLoggedIn.Invoke();
                }

                playFabId = result.PlayFabId;
            },


            error => Debug.Log(error.ErrorMessage)
            );
    }

    void SetDisplayText(string text, Color color)
    {
        displayText.gameObject.SetActive(true);
        displayText.text = text;
        displayText.color = color;
    }
}
