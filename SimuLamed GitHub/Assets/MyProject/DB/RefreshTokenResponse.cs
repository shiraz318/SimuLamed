using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MyProject.DB
{
    [Serializable]
    public class RefreshTokenResponse
    {
        public string refresh_token;
        public string id_token;
    }

}
