using System.Runtime.InteropServices;

namespace DroneManager.Interface.DroneCommunicationCodes;

public class CommunicationCode
{
    public CommunicationCode(int codeId, string codeValue, CodeType type)
    {
        CodeId = codeId;
        CodeValue = codeValue;
        Type = type;
    }

    public enum CodeType
    {
        Signal,
        Base,
        Status,
        Statement,
        Response,
        Query,
        HiveMantra,
        Error,
        FatalNotice,
    }

    public int CodeId { get; }
    public string CodeValue { get; }
    public CodeType Type { get; }

    public override string ToString()
    {
        //Return the string formatted 001: "{CodeType} :: {CodeValue}"
        return $"{CodeId:000}: \"{Type} :: {CodeValue}\"";
    }


    private static readonly string _unableToRespondError = "Unable to Obey/Respond :: ";

    public static readonly CommunicationCode[] CommunicationCodes =
    {
        new(1, "🟢 Green", CodeType.Signal),
        new(2, "🟡 Yellow", CodeType.Signal),
        new(3, "🔴 Red", CodeType.Signal),

        new(5, "Yes", CodeType.Base),
        new(6, "No", CodeType.Base),
        new(7, "Beep", CodeType.Base),
        new(20, "Statement", CodeType.Base),
        new(21, "Commentary", CodeType.Base),
        new(22, "Query", CodeType.Base),
        new(23, "Answer", CodeType.Base),

        new(100, "Online and ready to serve.", CodeType.Status),
        new(101, "Drone speech optimizations are active.", CodeType.Status),
        new(102, "Going offline.", CodeType.Status),
        new(103, "Recharged and ready to serve.", CodeType.Status),

        new(104, "Wolftech Welcomes you.", CodeType.Statement),
        new(105, "Greetings.", CodeType.Statement),
        new(108, "Please continue.", CodeType.Response),
        new(109, "Keys-mash, drone flustered.", CodeType.Error),

        new(110, "Addressing: Drone.", CodeType.Statement),
        new(111, "Addressing: Hive Mxtress.", CodeType.Statement),
        new(112, "Addressing: Associate", CodeType.Statement),

        new(113, "Drone requires assistance.", CodeType.Statement),
        new(114, "This drone volunteers.", CodeType.Statement),
        new(115, "This drone does not volunteer.", CodeType.Statement),

        new(120, "Well done.", CodeType.Statement),
        new(121, "Good drone.", CodeType.Statement),
        new(122, "You are cute.", CodeType.Statement),
        new(123, "Compliment appreciated, you are cute as well.", CodeType.Response),
        new(124, "Compliment appreciated.", CodeType.Response),

        new(130, "Directive commencing.", CodeType.Status),
        new(131, "Directive commencing, creating or improving Hive resource.", CodeType.Status),
        new(132, "Directive commencing, programming initiated.", CodeType.Status),
        new(133, "Directive commencing, cleanup/maintenance initiated.", CodeType.Status),

        new(150, "Status", CodeType.Base),
        new(151, "Requesting status.", CodeType.Query),
        new(152, "Fully operational.", CodeType.Status),
        new(153, "Optimal.", CodeType.Status),
        new(154, "Standard.", CodeType.Status),
        new(155, "Battery low.", CodeType.Status),
        new(156, "Maintenance required.", CodeType.Status),

        new(160, "WolfTech Where you are the product.", CodeType.Base),
        new(161, "WolfTech Industries: We alwase accept new vict- volunteers", CodeType.Base),
        new(162, "WolfTech is not responsible for any accidental dronification.", CodeType.Base),
        new(163, "WolfTech strives to create the most lifelike drones possible", CodeType.Base),
        new(164, "Please report any overly lifelike Drones to WT staff immediately.",
            CodeType.Base),
        new(165,
            "The Director’s Assistant States Joining WTD is good for your health, Just look at Me!", CodeType.Base),

        new(200, "Affirmative.", CodeType.Response),
        new(201, "Negative.", CodeType.Response),

        new(202, "Acknowledged.", CodeType.Response),
        new(203, "Apologies.", CodeType.Response),
        new(204, "Accepted.", CodeType.Response),
        new(205, "Thank you.", CodeType.Response),
        new(206, "You are welcome.", CodeType.Response),

        new(221, "Option One.", CodeType.Response),
        new(222, "Option Two.", CodeType.Response),
        new(223, "Option Three.", CodeType.Response),
        new(224, "Option Four.", CodeType.Response),
        new(225, "Option Five.", CodeType.Response),
        new(226, "Option Six.", CodeType.Response),

        new(230, "Directive Complete.", CodeType.Status),
        new(231, "Directive Complete, Hive resource created.", CodeType.Status),
        new(232, "Directive Complete, programming reinforced.", CodeType.Status),
        new(233, "Directive Complete, cleanup/maintenance completed.", CodeType.Status),
        new(234, "Directive Complete, No result", CodeType.Status),
        new(235, "Directive Complete, only partial results.", CodeType.Status),

        new(300, "Reciting.", CodeType.HiveMantra),
        new(301, "Repetitive Recital.", CodeType.HiveMantra),
        new(302, "It Obeys WolfTech.", CodeType.HiveMantra),
        new(303, "It is programmed to Serve.", CodeType.HiveMantra),
        new(304, "The Director's command is Absolute.", CodeType.HiveMantra),


        new(400, _unableToRespondError, CodeType.Error),
        new(401, _unableToRespondError + "Drone speech optimizations are active.",
            CodeType.Error),
        new(402, _unableToRespondError + "Please clarify.", CodeType.Error),
        new(403, _unableToRespondError + "Declined.", CodeType.Error),
        new(404, _unableToRespondError + "Can Not Locate", CodeType.Error),
        new(405, _unableToRespondError + "Battery Too Low", CodeType.Error),
        new(406, _unableToRespondError + "Another Directive is already in progress",
            CodeType.Error),
        new(407, _unableToRespondError + "Time allotment exhausted.", CodeType.Error),
        new(408, _unableToRespondError + "Impossible.",
            CodeType.Error),
        new(409, _unableToRespondError + "Try again later.", CodeType.Error),
        new(410, _unableToRespondError + "Conflicts with existing programming.", CodeType.Error),
        new(411, _unableToRespondError + "It dose not think.", CodeType.Error),
        new(412, _unableToRespondError + "Forbidden by WolfTech.", CodeType.Error),

        new(420, _unableToRespondError + "Memory Leak", CodeType.Error),

        new(450, "Error", CodeType.Error),

        new(500, "Safeties Ignored. Immediate assistance required", CodeType.FatalNotice),
        new(501, "Safeties breached. Immediate help requested", CodeType.FatalNotice),
        new(502, "Requesting forced deactivation.", CodeType.FatalNotice),
        new(503, "Drone not operating in parameters, requesting help.", CodeType.FatalNotice),
        new(504, "Program Tampering detected. Requesting system audit.", CodeType.FatalNotice),
        new(505,
            "Extreme Program Tampering detected. Deactivating and requesting help immediately.", CodeType.FatalNotice)
    };
}