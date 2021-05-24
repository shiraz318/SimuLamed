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
    public struct UserDetails
    {
        public string username;
        public string email;
        public string localId;
        public string idToken;
        
        public UserDetails(string username, string email, string localId, string idToken)
        {
            this.username = username;
            this.email = email;
            this.localId = localId;
            this.idToken = idToken;
        }
        public void SetLocalId(string localId)
        {
            this.localId = localId;
        }
        public void SetIdToken(string idToken)
        {
            this.idToken = idToken;
        }
        public void ResetDetails()
        {
            username = "";
            email = "";
            localId = "";
            idToken = "";
        }
    }
}
