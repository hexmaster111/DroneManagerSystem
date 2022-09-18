namespace DroneManager.Interface.GenericTypes;

public enum DroneType
{
    DirectorAssistant, //Directors Assistant (-DA Designation)
    //Unit assigned to the head Hive assistant
    //who works directly with The Director in
    //overseeing Hive Operations. 

    SeniorAssistant, //Senior Assistant (-SA Designation)
    //WTDs that assist in the management
    //of Hive Operations.  Report directly
    //to -DA designation.

    ResearchAndDevelopment, //Research and Development(-RD) Designation
    //WTDs that have been programmed to assist
    //in developing new WolfTech technologies
    //and products for the Hive. Report to -SA
    //designations or higher. 

    Experimental, //Experimental (-X) Designation* – These are specialist
    //WTDs that have been chosen (or volunteered and assessed)
    //to work with and under units operating with the -RD designation.
    //-X models willing to undergo rigorous experimentation in the
    //field of mental programming, testing of new protocols, and
    //brainwashing method testing.  -X Designations report exclusively
    //to their direct -RD designation handler.

    HiveMaintenance, //Hive Maintenance (-HA Designation) –WTDs tasked with general
    //Hive maintenance and general upkeep.  Report to -SA or higher
    //designations as needed.

    ProgramRecruiting, //Program Restructuring (-PR Designation)
    //– WTDs tasked with monitoring and administering
    //programs to other Drones. Report to -SA or higher
    //designations as needed.

    ServiceModel, //Service Model (-SM Designation) –
    //WTDs programmed in acts of service and general submission.

    DomesticService, //Domestic Service (-DS Designation) –
    //WTDs programmed with classically domestic
    //skills and subroutines, such as cooking and cleaning.
}