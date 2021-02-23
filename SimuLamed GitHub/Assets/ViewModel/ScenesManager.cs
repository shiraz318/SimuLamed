using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using SimuLamed.SimuLamed GitHub.Assets.Model.FirebaseManager;

public class ScenesManager : MonoBehaviour
{
    private FirebaseManager firebaseManager = new FirebaseManager();
    
    public InputField inputUsername;
    public InputField inputPassword;
    public InputField inputEmail;

    //then drag and drop the Username_field


    public void OnClickNewUser()
    {
        SceneManager.LoadScene("SignUpScene");

    }

    public void OnClickSignUp()
    {
        string username = inputUsername.text.ToString();
        string password = inputPassword.text.ToString();
        string email = inputEmail.text.ToString();
        User user = new User(username, password, email);
        firebaseManager.SignUpUser(user);
        //firebaseManager.SignUpUser(new User(username, password, email));
        //string username  = Inpu
        //FirebaseManager.SignUpUser();
        //SceneManager.LoadScene("MenuScene");
    }

    public void SignInScene()
    {

        //FirebaseManager.PostUser
    }
}




