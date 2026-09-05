using System;

namespace authservice.Data
{

    public class ClsPersrsonnDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public ClsPersrsonnDTO(string userName, string email, string phoneNumber, string address)
        {
            this.UserName = userName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
        }
    }

    public class ClsPesrsonnDataAccess
    {
       public static bool GetUser(int userId,ref ClsPersrsonnDTO user)
        {
            if (userId == 1)
            {
                user.UserName = "John Doe";
                user.Email  = "John@gmail.com";
                user.PhoneNumber = "123-456-7890";
                user.Address = "123 Main St, Anytown, USA";
                return true; // User found
            }
            else if (userId == 2)
            {
              user.UserName = "ayoub leet";
                user.Email  = "Ayoub@gmail.com";
                user.PhoneNumber = "111-2234-7765";
                user.Address = "123 Main St, Anytown, USA";
                return true; // User found
            }
            else
            {
                return false; // User not found
            }
        }
    }
}
