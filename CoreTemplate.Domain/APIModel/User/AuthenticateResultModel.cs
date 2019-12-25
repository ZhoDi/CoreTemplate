using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Domain.APIModel.User
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        //public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }//有效期

        public long UserId { get; set; }//用户ID
    }
}
