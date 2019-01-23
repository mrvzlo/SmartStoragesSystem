namespace SmartKitchen.Models
{
	public class LoginModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class RegisterModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Confirm { get; set; }
		public string Password { get; set; }
	}

}