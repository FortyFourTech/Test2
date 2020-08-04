using UnityEngine;

/// <summary>
/// This static class generates a random nickname for each networked player that joins the server
/// </summary>
public static class NetworkPlayerUtils
{
    // Read-only property
    private static string _nickName = "PLAYER #";
    public static string NewNickName
    {
        // TO DO: Consider assigning numbers in order of player count
        get
        {
            int number = Random.Range(1, 1000);     // Can have identical player numbers, so range is kept large
            return _nickName + number.ToString();
        }
    }
}
