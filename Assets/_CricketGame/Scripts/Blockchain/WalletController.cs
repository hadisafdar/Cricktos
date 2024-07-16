using System.Collections;
using UnityEngine;
//using Aptos.Accounts;
//using Aptos.Unity.Rest;
//using Aptos.Unity.Rest.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WalletController : MonoBehaviour
{


    public string WalletAddress = "1J7mdg5rbQyUHENYdx39WVWK7fsLpEoXZy";
    public int Tokens;

    private static string faucetEndpoint = "https://faucet.testnet.aptoslabs.com";
    private static string RestClient = "https://api.testnet.aptoslabs.com/accounts/{address}/tokens";
    //private Account MyAccount;

    public Button WalletButton;

    public HWText WalletAddressText;
    public HWText ButtonText;
    public HWText TokensText;
    public HWText TokensTextWalletMenu;

    void Start()
    {
        WalletButton.onClick.AddListener(GenerateWallet);
        ButtonText.SetText("Create Wallet");
    }
    public void UpdateFund()
    {
    }

    public void FundWallet()
    {
        /*if (MyAccount != null)
        {
            StartCoroutine(FundAliceAccount());
        }
        else
        {
            Debug.LogError("MyAccount is not initialized. Generate a wallet first.");
        }*/
        DemoFundAccount();


    }

    //Simulate a fund account;
    private void DemoFundAccount()
    {
        Tokens = 100000000;
        TokensText.SetText(Tokens.ToString());
        TokensTextWalletMenu.SetText("$" + Tokens);
        WalletButton.gameObject.SetActive(false);
    }

    IEnumerator FundAliceAccount()
    {
        yield break;
        /*if (MyAccount == null)
        {
            Debug.LogError("MyAccount is null. Exiting coroutine.");
            yield break;  // Exit the coroutine
        }

        bool success = false;
        ResponseInfo responseInfo = new();

        Coroutine fundAccount = StartCoroutine(FaucetClient.Instance.FundAccount((bool _success, ResponseInfo _responseInfo) =>
        {
            success = _success;
            responseInfo = _responseInfo;
        }, MyAccount.AccountAddress.ToString(), 100000000, faucetEndpoint));

        yield return fundAccount;

        if (!success)
        {
            Debug.LogError("Failed to fund account.");
        }
        else
        {
            Debug.Log("Successfully funded account.");
            var output = JsonUtility.ToJson(responseInfo, true);
            Debug.Log(output);
        }*/
    }

    public void GenerateWallet()
    {
        //Account alice = Account.Generate();
        //MyAccount = alice;
        //Debug.Log($"Generated Wallet Address: {alice.AccountAddress.ToString()}");
        WalletAddressText?.SetText(WalletAddress);
        ButtonText.SetText("Fund Wallet");
        WalletButton.onClick.RemoveAllListeners();
        WalletButton.onClick.AddListener(FundWallet);
    }


}