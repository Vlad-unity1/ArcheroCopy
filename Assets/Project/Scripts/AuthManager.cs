using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logTxt;

    public async void SignIn()
    {
        await SignInAnonymous();
    }

    async Task SignInAnonymous()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            print("Sign in Success");
            print("Player Id:" + AuthenticationService.Instance.PlayerId);
            _logTxt.text = "Player id:" + AuthenticationService.Instance.PlayerId;
        }
        catch (AuthenticationException ex)
        {
            print("Sign in failed!!");
            Debug.LogException(ex);
        }

    }
}