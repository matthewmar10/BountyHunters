namespace RTSGame
{

    static class StateManager
    {
        public enum StartMode
        {
            GameLift,
            LocalServer,
            NoServer
        };


        public static StartMode startMode
        {
            get; set;
        }

        public static bool isServerSimulated
        {
            get { return startMode == StartMode.NoServer; }
        }

    }

}