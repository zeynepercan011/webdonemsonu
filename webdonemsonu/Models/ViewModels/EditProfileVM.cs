namespace webdonemsonu.Models.ViewModels
{
    public class EditProfileVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Optional: if you'd like to allow email change
        public string Email { get; set; }
    }
}
