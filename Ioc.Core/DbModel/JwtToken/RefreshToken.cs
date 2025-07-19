namespace Ioc.Core.DbModel.JwtToken
{
    public class RefreshToken
    {
        private Guid iD;
        private string refreshToken;

        public RefreshToken(Guid iD, string refreshToken)
        {
            this.iD = iD;
            this.refreshToken = refreshToken;
        }
    }
}
