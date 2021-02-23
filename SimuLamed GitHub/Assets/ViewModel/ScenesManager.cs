using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using SimuLamed.SimuLamed GitHub.Assets.Model.FirebaseManager;

public class ScenesManager : MonoBehaviour
{
    public void SignUpScene()
    {
        //User u = new User();
        User user = new User("Nili", "1234", "myEmail");
        FirebaseManager.PostUser(user, "1", () =>
        {
            Debug.Log($"{user.username} {user.password} {user.email}");
        });
        SceneManager.LoadScene("SignUpScene");


    }

    public void SignInScene()
    {

        //FirebaseManager.PostUser
    }
}



