using FluentValidation;
using FluentValidation.Results;
using AdApp.Models;
using AdApp.DAL;
using AdApp.Core.Services.Auth;

public class AuthService: IAuthService
{
    private readonly JsonStorage _jsonStorage = new JsonStorage();
    private readonly string _filePath;
    private readonly UserValidator _userValidator = new UserValidator();

    public AuthService()
    {
        // Define the folder path
        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA");

        // Ensure the folder exists
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Combine the folder path with the file name
        string filePath = Path.Combine(folderPath, "users.json");
      
        // Create the file if it does not exist (initialize with an empty array)
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
        _filePath = filePath;

    }

    // Method to retrieve all users from the storage
    public List<User> GetAllUsers()
    {
        var users = _jsonStorage.Load<List<User>>(_filePath);
        return users ?? new List<User>();
    }

    // Validate user using the UserValidator
    public ValidationResult ValidateUser(User user)
    {
        return _userValidator.Validate(user);
    }

    // Register a new user
    public Task<User> RegisterAsync(User newUser)
    {
        var user = new User();
        // Validate the user data
        var validationResult = ValidateUser(newUser);
        if (!validationResult.IsValid)
        {
            // If validation fails, print the error messages
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
            }

            return   Task.FromResult(user);
        }

        var users = GetAllUsers();

        // Check if the username already exists
        if (users.Any(u => u.UserName == newUser.UserName))
        {
            Console.WriteLine("User already exists.");
            return   Task.FromResult(user);
        }
        var maxId = users.Any() ? users.Max(x => x.Id) : 0;
        newUser.Id = maxId+1;
        // Add the new user and save the updated list
        users.Add(newUser);
        _jsonStorage.Save(_filePath, users);
        return  Task.FromResult(newUser);
    }

    // Login a user by username and password
    public  Task<User?> Login(string email, string password)
    {
        var users = GetAllUsers();

        // Validate if the username and password match
        var user = users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Password == password);
        return Task.FromResult(user);
    }

    // Delete a user by username
    public bool DeleteUser(string userName)
    {
        var users = GetAllUsers();

        // Find the user to delete
        var userToRemove = users.FirstOrDefault(u => u.UserName == userName);
        if (userToRemove != null)
        {
            // Remove the user and save the updated list
            users.Remove(userToRemove);
            _jsonStorage.Save(_filePath, users);
            return true; // User deleted
        }

        return false; // User not found
    }

    public bool AuthorizeUser(string userName, string password) { 
        var res = false;
        var users = GetAllUsers();
        if(users.Exists(x=> x.UserName == userName && x.Password == password))
        {
            res = true;
        }

        return res;

    }
}
