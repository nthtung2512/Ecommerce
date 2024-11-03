namespace MealMate.PL.Environment
{
    public static class SelectedEnvironment
    {
        public static AppEnvironment Value { get; } =
#if DEBUG
            AppEnvironment.Development;
#elif STAGINGINTERNAL
        AppEnvironment.StagingInternal;
#elif RELEASE
        AppEnvironment.Production;
#endif
    }
}
