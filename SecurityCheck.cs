using CreditAgricoleSdk.Entity;

namespace CreditAgricoleSdk;

public class SecurityCheck 
{
    private readonly string _username;
    private readonly string _password;
    private readonly Keypad _keypad;

    public SecurityCheck(string username, string password, Keypad keypad)
    {
        _username = username;
        _password = password;
        _keypad = keypad;
    }
    
    public IEnumerable<KeyValuePair<string, string>> PrepareFormData(string regionalBankUrlPrefix)
    {
        var data =  new[]
        {
            new KeyValuePair<string, string>("j_password", MapPassword()),
            new KeyValuePair<string, string>("path", "/content/npc/start"),
            new KeyValuePair<string, string>("j_path_resource", $"{regionalBankUrlPrefix}particulier/operations/synthese.html"),
            new KeyValuePair<string, string>("j_username", _username),
            new KeyValuePair<string, string>("keypadId", _keypad.KeypadId),
            new KeyValuePair<string, string>("j_validate", "true"),
        };

        return data;
    }
    
    private string MapPassword()
    {
        var indices = _password.Select(digit => Array.FindIndex(_keypad.KeyLayout, i => i == int.Parse(digit.ToString()))).ToList();

        return string.Join(",", indices.Select(i => i.ToString()));
    }
}