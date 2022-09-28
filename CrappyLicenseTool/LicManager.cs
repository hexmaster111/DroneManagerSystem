using System.Reflection;
using Newtonsoft.Json;

namespace CrappyLicenseTool;

public class LicManager
{
    //exe path + filename
    public string LicenseFile =
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
    }

    private License _license;

    public LicManager()
    {
        //Check if license file exists
        if (!File.Exists(LicenseFile))
        {
            //If not, create it
            File.Create(LicenseFile);
        }

        //Deserialize license file with neutonsoft.json
        var license = JsonConvert.DeserializeObject<License>(File.ReadAllText(LicenseFile));
        //check if license is null
        if (license == null)
        {
            //if it is, create a new one
            license = new License(DateTime.Now, true);
        }

        //set the license
        _license = license;

        //save the license
        SaveLicense();
    }

    private void SaveLicense()
    {
        //serialize the license
        var license = JsonConvert.SerializeObject(_license);
        //write the license to the file
        File.WriteAllText(LicenseFile, license);
    }

    public bool IsValid()
    {
        //check if the license is a trial
        if (!_license.IsTrial) return true;
        //check if RegDate + trial length is greater than now
        return _license.RegDate.AddDays(30) > DateTime.Now;
    }
}