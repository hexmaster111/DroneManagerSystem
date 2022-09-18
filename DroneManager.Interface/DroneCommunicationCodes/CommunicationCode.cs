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
        Notice,
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

    public static CommunicationCode[] GetCommunicationCodes()
    {
        return new[]
        {
            new CommunicationCode(1, "🟢 Green", CodeType.Signal),
            new CommunicationCode(2, "🟡 Yellow", CodeType.Signal),
            new CommunicationCode(3, "🔴 Red", CodeType.Signal),

            new CommunicationCode(5, "Yes", CodeType.Base),
            new CommunicationCode(6, "No", CodeType.Base),
            new CommunicationCode(7, "Beep", CodeType.Base),
            new CommunicationCode(20, "Statement", CodeType.Base),
            new CommunicationCode(21, "Commentary", CodeType.Base),
            new CommunicationCode(22, "Query", CodeType.Base),
            new CommunicationCode(23, "Answer", CodeType.Base),

            new CommunicationCode(100, "Online and ready to serve.", CodeType.Status),
            new CommunicationCode(101, "Drone speech optimizations are active.", CodeType.Status),
            new CommunicationCode(102, "Going offline.", CodeType.Status),
            new CommunicationCode(103, "Recharged and ready to serve.", CodeType.Status),

            new CommunicationCode(104, "Wolftech Welcomes you.", CodeType.Statement),
            new CommunicationCode(105, "Greetings.", CodeType.Statement),
            new CommunicationCode(108, "Please continue.", CodeType.Response),
            new CommunicationCode(109, "Keys-mash, drone flustered.", CodeType.Error),

            new CommunicationCode(110, "Addressing: Drone.", CodeType.Statement),
            new CommunicationCode(111, "Addressing: Hive Mxtress.", CodeType.Statement),
            new CommunicationCode(112, "Addressing: Associate", CodeType.Statement),

            new CommunicationCode(113, "Drone requires assistance.", CodeType.Statement),
            new CommunicationCode(114, "This drone volunteers.", CodeType.Statement),
            new CommunicationCode(115, "This drone does not volunteer.", CodeType.Statement),

            new CommunicationCode(120, "Well done.", CodeType.Statement),
            new CommunicationCode(121, "Good drone.", CodeType.Statement),
            new CommunicationCode(122, "You are cute.", CodeType.Statement),
            new CommunicationCode(123, "Compliment appreciated, you are cute as well.", CodeType.Response),
            new CommunicationCode(124, "Compliment appreciated.", CodeType.Response),

            new CommunicationCode(130, "Directive commencing.", CodeType.Status),
            new CommunicationCode(131, "Directive commencing, creating or improving Hive resource.", CodeType.Status),
            new CommunicationCode(132, "Directive commencing, programming initiated.", CodeType.Status),
            new CommunicationCode(133, "Directive commencing, cleanup/maintenance initiated.", CodeType.Status),

            new CommunicationCode(150, "Status", CodeType.Base),
            new CommunicationCode(151, "Requesting status.", CodeType.Query),
            new CommunicationCode(152, "Fully operational.", CodeType.Status),
            new CommunicationCode(153, "Optimal.", CodeType.Status),
            new CommunicationCode(154, "Standard.", CodeType.Status),
            new CommunicationCode(155, "Battery low.", CodeType.Status),
            new CommunicationCode(156, "Maintenance required.", CodeType.Status),

            new CommunicationCode(160, "WolfTech Where you are the product.", CodeType.Base),
            new CommunicationCode(161, "WolfTech Industries: We alwase accept new vict- volunteers", CodeType.Base),
            new CommunicationCode(162, "WolfTech is not responsible for any accidental dronification.", CodeType.Base),
            new CommunicationCode(163, "WolfTech strives to create the most lifelike drones possible", CodeType.Base),
            new CommunicationCode(164, "Please report any overly lifelike Drones to WT staff immediately.",
                CodeType.Base),
            new CommunicationCode(165,
                "The Director’s Assistant States Joining WTD is good for your health, Just look at Me!", CodeType.Base),

            new CommunicationCode(200, "Affirmative.", CodeType.Response),
            new CommunicationCode(201, "Negative.", CodeType.Response),

            new CommunicationCode(202, "Acknowledged.", CodeType.Response),
            new CommunicationCode(203, "Apologies.", CodeType.Response),
            new CommunicationCode(204, "Accepted.", CodeType.Response),
            new CommunicationCode(205, "Thank you.", CodeType.Response),
            new CommunicationCode(206, "You are welcome.", CodeType.Response),

            new CommunicationCode(221, "Option One.", CodeType.Response),
            new CommunicationCode(222, "Option Two.", CodeType.Response),
            new CommunicationCode(223, "Option Three.", CodeType.Response),
            new CommunicationCode(224, "Option Four.", CodeType.Response),
            new CommunicationCode(225, "Option Five.", CodeType.Response),
            new CommunicationCode(226, "Option Six.", CodeType.Response),

            new CommunicationCode(230, "Directive Complete.", CodeType.Status),
            new CommunicationCode(231, "Directive Complete, Hive resource created.", CodeType.Status),
            new CommunicationCode(232, "Directive Complete, programming reinforced.", CodeType.Status),
            new CommunicationCode(233, "Directive Complete, cleanup/maintenance completed.", CodeType.Status),
            new CommunicationCode(234, "Directive Complete, No result", CodeType.Status),
            new CommunicationCode(235, "Directive Complete, only partial results.", CodeType.Status),

            new CommunicationCode(300, "Reciting.", CodeType.HiveMantra),
            new CommunicationCode(301, "Repetitive Recital.", CodeType.HiveMantra),
            new CommunicationCode(302, "It Obeys WolfTech.", CodeType.HiveMantra),
            new CommunicationCode(303, "It is programmed to Serve.", CodeType.HiveMantra),
            new CommunicationCode(304, "The Director's command is Absolute.", CodeType.HiveMantra),


            new CommunicationCode(400, _unableToRespondError, CodeType.Error),
            new CommunicationCode(401, _unableToRespondError + "Drone speech optimizations are active.",
                CodeType.Error),
            new CommunicationCode(402, _unableToRespondError + "Please clarify.", CodeType.Error),
            new CommunicationCode(403, _unableToRespondError + "Declined.", CodeType.Error),
            new CommunicationCode(404, _unableToRespondError + "Can Not Locate", CodeType.Error),
            new CommunicationCode(405, _unableToRespondError + "Battery Too Low", CodeType.Error),
            new CommunicationCode(406, _unableToRespondError + "Another Directive is already in progress",
                CodeType.Error),
            new CommunicationCode(407, _unableToRespondError + "Time allotment exhausted.", CodeType.Error),
            new CommunicationCode(408, _unableToRespondError + "Impossible.",
                CodeType.Error),
            new CommunicationCode(409, _unableToRespondError + "Try again later.", CodeType.Error),
            new CommunicationCode(410, _unableToRespondError + "Conflicts with existing programming.", CodeType.Error),
            new CommunicationCode(411, _unableToRespondError + "It dose not think.", CodeType.Error),
            new CommunicationCode(412, _unableToRespondError + "Forbidden by WolfTech.", CodeType.Error),

            new CommunicationCode(420, _unableToRespondError + "Memory Leak", CodeType.Error),
            
            new CommunicationCode(450, "Error", CodeType.Error),
            
            new CommunicationCode(500, "Safeties Ignored. Immediate assistance required", CodeType.Notice),
            new CommunicationCode(501, "Safeties breached. Immediate help requested", CodeType.Notice),
            new CommunicationCode(502, "Requesting forced deactivation.", CodeType.Notice),
            new CommunicationCode(503, "Drone not operating in parameters, requesting help.", CodeType.Notice),
            new CommunicationCode(504, "Program Tampering detected. Requesting system audit.", CodeType.Notice),
            new CommunicationCode(505,
                "Extreme Program Tampering detected. Deactivating and requesting help immediately.", CodeType.Notice)
        };
    }
}