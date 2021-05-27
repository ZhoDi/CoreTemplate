namespace CoreTemplate.Application.Model.User.Dto
{
    public class AuthenticateResultDto
    {
        public string AccessToken { get; set; }

        //public string EncryptedAccessToken { get; set; }

        public long ExpireInSeconds { get; set; }//有效期

        public long UserId { get; set; }//用户ID
    }
}
