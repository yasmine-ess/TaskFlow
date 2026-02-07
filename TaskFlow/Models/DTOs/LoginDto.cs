namespace TaskFlow.Models.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }  // peut être email ou username selon ta configuration Identity    
        public string Password { get; set; }  // mot de passe en clair reçu du client   si tu utilises email comme username, alors ce champ s’appellera email dans le DTO et tu l’utiliseras pour UserName dans Identity. C’est une question de convention et de clarté pour le client qui consomme ton API.    

    }
}
