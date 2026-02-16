namespace ServerStatusChecker
{
    public static class Settings
    {
        public const string ConnectionString = "Data Source=usersdata.db";
        public const string EndPointPLM = $"http://union-test/Health"; // возвращается просто 200 статус с телом {"status":"Server is running"}
    }
}
