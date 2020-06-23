namespace DiscordBotTesting
{
    public static class ErrorMessages
    {
        public static class Role
        {
            public const string NoArguments = "The role command requires at least one argument from [add,leave,list]";

            public const string AddArgumentInvalid =
                "The `role add` command requires a single argument, e.g. `role add epicrole`";

            public const string LeaveArgumentInvalid =
                "The `role leave` command requires a single argument, e.g. `role leave epicrole`";
        }
    }
}
