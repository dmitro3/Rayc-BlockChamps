using System.Collections;
using System.Collections.Generic;
using MoralisUnity.Platform.Objects;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    private MoralisUser user;

    void Start()
    {
        getUser();
    }

    async void getUser()
    {
        user = await MoralisUnity.Moralis.GetUserAsync();
    }

}
