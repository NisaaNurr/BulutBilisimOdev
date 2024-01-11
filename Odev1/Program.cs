public interface IUserAction
{
    void AddUser(User user);
    List<User> GetUserList();
    void DeleteUser(string phoneNumber);
    List<User> GetUserByFilter(string filter);
}

public class UserAction : IUserAction
{
    private List<User> userList;
    private string filePath = "users.txt";

    public UserAction()
    {
        userList = new List<User>();
        LoadUsersFromFile();
    }

    public void AddUser(User user)
    {
        if (!userList.Any(u => u.PhoneNumber == user.PhoneNumber))
        {
            userList.Add(user);
            SaveUsersToFile();
            Console.WriteLine("Kullanıcı başarıyla eklendi.");
        }
        else
        {
            Console.WriteLine("Bu telefon numarasına sahip bir kullanıcı zaten kayıtlı.");
        }
    }

    public void DeleteUser(string phoneNumber)
    {
        var userToRemove = userList.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        if (userToRemove != null)
        {
            userList.Remove(userToRemove);
            SaveUsersToFile();
            Console.WriteLine("Kullanıcı başarıyla silindi.");
        }
        else
        {
            Console.WriteLine("Belirtilen telefon numarasına sahip bir kullanıcı bulunamadı.");
        }
    }

    public List<User> GetUserByFilter(string filter)
    {
        return userList.Where(u => u.Name.Contains(filter) || u.Surname.Contains(filter) || u.Email.Contains(filter) || u.PhoneNumber.Contains(filter)).ToList();
    }

    public List<User> GetUserList()
    {
        return userList;
    }

    private void LoadUsersFromFile()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var userProperties = line.Split(' ');
                var user = new User
                {
                    Name = userProperties[0],
                    Surname = userProperties[1],
                    Email = userProperties[2],
                    PhoneNumber = userProperties[3],
                    IsAdmin = bool.Parse(userProperties[4])
                };
                userList.Add(user);
            }
        }
    }

    private void SaveUsersToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var user in userList)
            {
                writer.WriteLine($"{user.Name} {user.Surname} {user.Email} {user.PhoneNumber} {user.IsAdmin}");
            }
        }
    }
}

public interface INoteAction
{
    void AddNote(string noteText);
    List<string> GetNoteList();
}

public class NoteAction : INoteAction
{
    private List<string> noteList;
    private string filePath = "notes.txt";

    public NoteAction()
    {
        noteList = new List<string>();
        LoadNotesFromFile();
    }

    public void AddNote(string noteText)
    {
        string formattedNote = $"{noteText} {DateTime.UtcNow:dd.MM.yyyyTHH:mm:ssZ}";
        noteList.Add(formattedNote);
        SaveNotesToFile();
        Console.WriteLine("Not başarıyla eklendi.");
    }

    public List<string> GetNoteList()
    {
        return noteList;
    }

    private void LoadNotesFromFile()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            noteList.AddRange(lines);
        }
    }

    private void SaveNotesToFile()
    {
        File.WriteAllLines(filePath, noteList);
    }
}

public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsAdmin { get; set; }
}

class Program
{
    static void Main()
    {
        
        Console.WriteLine("Mail Giriniz:");
        string email = Console.ReadLine();

        Console.WriteLine("Şifre Giriniz:");
        string password = Console.ReadLine();

        bool isAdmin = email.ToLower() == "celiknisanur5768@gmail.com" && password == "admin";

        if (isAdmin)
        {
            AdminMenu(); 
        }
        else
        {
            UserMenu();
        }

        static void AdminMenu()
        {
            IUserAction userAction = new UserAction();
            while (true)
            {
                Console.WriteLine("1- Kullanıcı Ekle");
                Console.WriteLine("2- Kullanıcı Ara");
                Console.WriteLine("3- Kullanıcı Sil");
                Console.WriteLine("Seçiminizi Yapın");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("İsim:");
                        string name = Console.ReadLine();
                        Console.Write("Soyisim:");
                        string surname = Console.ReadLine();
                        Console.Write("Telefon:");
                        string phoneNumber = Console.ReadLine();
                        Console.Write("Email:");
                        string userEmail = Console.ReadLine();
                        Console.Write("İsAdmin True/False:");
                        bool isAdminUser = bool.Parse(Console.ReadLine());

                        User newUser = new User
                        {
                            Name = name,
                            Surname = surname,
                            PhoneNumber = phoneNumber,
                            Email = userEmail,
                            IsAdmin = isAdminUser
                        };
                        userAction.AddUser(newUser);
                        break;

                    case 2:
                        // Kullanıcı arama
                        Console.Write("Arama metni (en az 3 karakter): ");
                        string filter = Console.ReadLine();

                        List<User> searchResults = userAction.GetUserByFilter(filter);

                        if (searchResults.Count > 0)
                        {
                            foreach (var result in searchResults)
                            {
                                Console.WriteLine($"{result.Name} {result.Surname} {result.Email} {result.PhoneNumber} {result.IsAdmin}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Belirtilen kriterde kullanıcı bulunamadı.");
                        }
                        break;

                    case 3:
                        // Kullanıcı silme
                        Console.Write("Telefon numarası: ");
                        string deletePhoneNumber = Console.ReadLine();
                        userAction.DeleteUser(deletePhoneNumber);
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim.");
                        break;
                }
            }
            
        }

        static void UserMenu()
        {
            INoteAction noteAction = new NoteAction();
            while (true)
            {
                Console.WriteLine("1- Not Ekle");
                Console.WriteLine("2- Notlarımı Listele");

                Console.Write("Seçiminizi yapınız (1 veya 2): ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        // Not ekleme
                        Console.Write("Not: ");
                        string noteText = Console.ReadLine();
                        noteAction.AddNote(noteText);
                        break;

                    case 2:
                        // Not listeleme
                        List<string> notes = noteAction.GetNoteList();

                        if (notes.Count > 0)
                        {
                            foreach (var note in notes)
                            {
                                Console.WriteLine(note);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Henüz not eklenmemiş.");
                        }
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim.");
                        break;
                }
            }

        }
        
    }
}

