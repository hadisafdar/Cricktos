using System.Collections;
using UnityEngine;
using Aptos.Accounts;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Networking;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class WalletController : MonoBehaviour
{


    [ReadOnly]public string WalletAddress = "1J7mdg5rbQyUHENYdx39WVWK7fsLpEoXZy";
    [ReadOnly]public int Tokens;

    private static string faucetEndpoint = "https://faucet.testnet.aptoslabs.com";
    private static string RestClient = "https://api.testnet.aptoslabs.com/accounts/{address}/tokens";
    private Account myAccount;

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
        if (myAccount != null)
        {
            StartCoroutine(FundAliceAccount());
        }
        else
        {
            Debug.LogError("myAccount is not initialized. Generate a wallet first.");
        }


    }

   

    IEnumerator FundAliceAccount()
    {
      
        if (myAccount == null)
        {
            Debug.LogError("myAccount is null. Exiting coroutine.");
            yield break;  // Exit the coroutine
        }

        bool success = false;
        ResponseInfo responseInfo = new();

        Coroutine fundAccount = StartCoroutine(FaucetClient.Instance.FundAccount((bool _success, ResponseInfo _responseInfo) =>
        {
            success = _success;
            responseInfo = _responseInfo;
        }, myAccount.AccountAddress.ToString(), 100000000, faucetEndpoint));

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
            Tokens = 100000000;
            WalletButton.gameObject.SetActive(false);
            TokensText.SetText(Tokens.ToString());
            TokensTextWalletMenu.SetText("$"+Tokens.ToString());
        }
    }

    public void GenerateWallet()
    {
        Account alice = Account.Generate();
        myAccount = alice;
        WalletAddress = myAccount.AccountAddress.ToString();
        Debug.Log($"Generated Wallet Address: {WalletAddress}");
        WalletAddressText?.SetText(WalletAddress);
        ButtonText.SetText("Fund Wallet");
        WalletButton.onClick.RemoveAllListeners();
        WalletButton.onClick.AddListener(FundWallet);
    }


}