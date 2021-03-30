namespace LatenessManager.Application.Common
{
    public static class ErrorCode
    {
        public static class Player
        {
            public const string NotExist = "Player.NotExist";
        }
        
        public static class Penalty
        {
            public const string InvalidCount = "Penalty.InvalidCount";
        }
        
        public static class User
        {
            public const string EmailIsNotUnique = "User.EmailIsNotUnique";
        }
    }
}