

using System;

public class GlobalDefine 
{
    public const string PREFS_SELECTEDCHARACTER = "SELECTED_CHARACTER";
    //Logs
    public const string LOG_PLAYERJOINED = "<b><color=green>[Game] Player Joined Game :</color></b> ";
    public const string LOG_PLAYERLEFT = "<b><color=orange>[Game] Player Left Game : </color></b>";
    public const string LOG_CONNECTEDTOSERVERSUCCESS = "<b><color=green>[Game] Connected to Server : </color></b>";
    public const string LOG_CONNECTEDTOSERVERFAILURE = "<b><color=red>[Game] Could not connect to PHOTON Server : </color></b>";
    public const string LOG_JOINEDLOBBYSUCCESS = "<b><color=green>[Game] Joined Lobby : </color></b>";

    public const string LOG_ROOMJOINSUCCESS = "<b><color=green>[Game] Successfully Joined Room : </color></b>";
    public const string LOG_ROOMJOINFAILURE = "<b><color=red>[Game] Could Not Join Room : </color></b>";



    //Messages
    public const string MESSAGE_JOININGLOBBY = "Joining Lobby";

    public static string LOG_INVALIDSENDERMESSAGE(Type expected, Type found) => $"Invalid Sender!. Expected {expected.Name}. Found {found.Name}";

}
public enum HWLoggerMessageType
{
    None,
    Info,
    Warning,
    Error
}

