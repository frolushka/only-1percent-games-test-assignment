namespace Only1PercentGames.TestAssignment
{
    public static class Utils
    {
        public static T Random<T>(this T[] source) 
            => source[UnityEngine.Random.Range(0, source.Length)];
    }
}