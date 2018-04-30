using System;
using System.Collections.Generic;
using System.Text;

namespace Elton.AqaraCloud
{
    public partial class AqaraApi : Elton.OAuth2.ApiClient
    {
        public AqaraApi(AqaraConfiguration config) : base(config) { }
    }
}
