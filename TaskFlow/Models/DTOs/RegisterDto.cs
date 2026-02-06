namespace TaskFlow.Models.DTOs
{
    public class RegisterDto
    {
        public string FullName { get; set; }   // ton champ personnalisé dans ApplicationUser
        public string Email { get; set; }      // utilisé pour UserName et Email
        public string Password { get; set; }   // mot de passe en clair reçu du client
    }
}
