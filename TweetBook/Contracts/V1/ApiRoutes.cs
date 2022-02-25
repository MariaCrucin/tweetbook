﻿namespace TweetBook.Contracts.V1
{
    public static class ApiRoutes
    {
        // static class -> clasa nu va fi instantiata vre-odata

        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = $"{Root}/{Version}";
        // public const string Base = Root + "/" + Version;

        public static class Posts
        {
            public const string GetAll = $"{Base}/posts";
            public const string Get = $"{Base}/posts/{{postId}}";
            public const string Create = $"{Base}/posts";
            public const string Update = $"{Base}/posts/{{postId}}";
            public const string Delete = $"{Base}/posts/{{postId}}";
        }

        public static class Identity
        {
            public const string Login = $"{Base}/identity/login";
            public const string Register = $"{Base}/identity/register";
        }
    }
}
