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

            public const string ReactionArgumentInvalid =
                "Invalid arguments. There should be 3 arguments for each valid reaction, and there can be multiple of these. i.e. " +
                "```!role reaction [emoji],[roleToJoin],[gameName]``` or ```!role reaction [emoji1],[roleToJoin1],[gameName1] [emoji2],[roleToJoin2],[gameName2]```" +
                "NB: Each valid reaction uses a comma-separated list for its parameters, so don't add commas anywhere else.";
        }
    }
}
