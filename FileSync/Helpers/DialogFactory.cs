namespace FileSync.Helpers
{
    public static class DialogFactory
    {
        public static IDialog New()
        {
            return new Dialog();
        }
    }
}
