using System.Reflection;
using Newtonsoft.Json;

namespace CrappyLicenseTool;

public static class LicManager
{
    //exe path + filename
    public static string LicenseFile =
        System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\license.lic";


    public class License
    {
        public License(DateTime regDate, bool isTrial)
        {
            RegDate = regDate;
            isTrial = IsTrial;
        }

        public DateTime RegDate { get; set; } = DateTime.Now;
        public bool IsTrial { get; set; } = true;
        public string LicenseKey { get; set; } = "1234567890";
    }

    public static void Init()
    {
        //Check if license file exists
        if (!File.Exists(LicenseFile))
        {
            //If not, create it
            File.Create(LicenseFile);
        }

        //Deserialize license file with neutonsoft.json
        //check if license is null
        //if it is, create a new one
        var license = JsonConvert.DeserializeObject<License>(File.ReadAllText(LicenseFile)) ??
                      new License(DateTime.Now, true);


        //save the license
        SaveLicense();
    }

    
    private static License _license;

    private static void SaveLicense()
    {
        if(_license == null)
            throw new Exception("License manager was not initialized (Init())");
        //serialize the license
        var license = JsonConvert.SerializeObject(_license);
        //write the license to the file
        File.WriteAllText(LicenseFile, license);
    }

    public static bool IsValid()
    {
        if(_license == null)
            throw new Exception("License manager was not initialized (Init())");
        //check if the license is a trial
        if (!_license.IsTrial) return true;
        //check if RegDate + trial length is greater than now
        return _license.RegDate.AddDays(30) > DateTime.Now;
    }
}