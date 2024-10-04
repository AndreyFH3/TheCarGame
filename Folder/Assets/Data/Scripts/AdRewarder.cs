public static class AdRewarder
{
    private static float savedTimeValue;
    public static float SavedTimeValue => savedTimeValue;

    public static void SetLastTimeShoed(float value)
    {
        savedTimeValue = value;
    }

    public static int CalculateResult(int value, float multiplayer)
    {
        var calculateResult = (int)(value * multiplayer);
        return calculateResult;
        
    }

}
