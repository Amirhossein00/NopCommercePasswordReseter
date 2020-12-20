namespace Test_Example
{
    public static class PathFinder
    {
        public static string GetBackToRootPath(this string path)
        {
            path = path.Replace('\\', '/');
            var targetChar = '/';
            for (int i = 0; i <= 1; i++)
            {
                var lastIndex = path.LastIndexOf(targetChar);
                path = path.Substring(0, lastIndex);
            }

            return path.Replace('/', '\\') + "\\";
        }
    }
}
