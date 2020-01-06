using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReachargeBtnClicked()
    {
        Application.OpenURL("http://182.18.139.143/crypto/load/load.html");
    }

    public void ClaimBtnClicked()
    {
        Application.OpenURL("http://182.18.139.143/crypto/return/return.html");
    }
    public void BackBtnClicked()
    {
        GameSceneManager.LoadScene("HomeScreen");
    }
}
