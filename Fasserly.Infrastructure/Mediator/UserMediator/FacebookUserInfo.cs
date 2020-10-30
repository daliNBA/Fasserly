namespace Fasserly.Infrastructure.Mediator.UserMediator
{
    public class FacebookUserInfo
    {
        public string  Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public FacebokkPictureData Picture { get; set; }
    }

    public class FacebokkPictureData
    {
        public FacebookPicture Data { get; set; }
    }

    public class FacebookPicture
    {
        public string Url { get; set; }
    }
}
