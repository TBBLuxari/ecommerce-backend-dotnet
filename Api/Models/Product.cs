using System.Text.RegularExpressions;

namespace Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        string _name = string.Empty;
        public required string Name 
        {
            get => _name;
            set 
            {
                ValidateName(value);
                _name = value.Trim();
            } 
        }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }

        private void ValidateName(string name)
        {
            string? cleanName = name?.Trim();
            if (string.IsNullOrWhiteSpace(cleanName)) { throw new ArgumentException("The name cannot be empty."); }
            if (cleanName.Length < 5 || cleanName.Length > 50) { throw new ArgumentException("The name must be between 5 and 50 actual characters long."); }
            if (!Regex.IsMatch(cleanName, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$")) { throw new ArgumentException("The name can only contain letters."); }
        }

    }
}
