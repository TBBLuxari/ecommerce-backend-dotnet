using System;
using System.Text.RegularExpressions;

public class User
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    private string _name = string.Empty;
    public required string Name
    {
        get => _name;
        set
        {
            ValidateName(value);
            _name = value.Trim(); 
        }
    }

    private string _email = string.Empty;
    public required string Email
    {
        get => _email;
        set
        {
            ValidateEmail(value);
            _email = value.Trim(); 
        }
    }

    private string _password = string.Empty;
    public required string Password
    {
        get => _password;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentException("The password hash cannot be empty."); }
            _password = value;
        }
    }

    public required string Rol { get; set; } 

    public string? VerificationToken { get; set; }
    public bool IsVerified { get; set; } = false;

    private void ValidateName(string name)
    {
        string? cleanName = name?.Trim();
        if (string.IsNullOrWhiteSpace(cleanName)) { throw new ArgumentException("The name cannot be empty.."); }
        if (cleanName.Length < 5 || cleanName.Length > 50) { throw new ArgumentException("The name must be between 5 and 50 actual characters long."); }
        if (!Regex.IsMatch(cleanName, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")){ throw new ArgumentException("The name can only contain letters."); }
    }

    private void ValidateEmail(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email)) { throw new Exception(); }
            var addr = new System.Net.Mail.MailAddress(email.Trim());
            if (addr.Address != email.Trim()) { throw new Exception(); }
        }
        catch
        {
            throw new ArgumentException("The email structure is invalid.");
        }
    }

}