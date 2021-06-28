using Assets.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    [Serializable]
    public class UserDetails
    {
        public string username;
        //public string localId;
       // public string idToken;
        
        public UserDetails(string username)
        //public UserDetails(string username, string idToken)
        //public UserDetails(string username, string localId, string idToken)
        {
            this.username = username;
            //this.localId = localId;
           // this.idToken = idToken;
        }

        //// Setter for localId.
        //public void SetLocalId(string localId)
        //{
        //    this.localId = localId;
        //}

        //// Setter for idToken.
        //public void SetIdToken(string idToken)
        //{
        //    this.idToken = idToken;
        //}

        //// Reset the user details fiels.
        //public void ResetDetails()
        //{
        //    username = "";
        //    localId = "";
        //    idToken = "";
        //}
    }
}
