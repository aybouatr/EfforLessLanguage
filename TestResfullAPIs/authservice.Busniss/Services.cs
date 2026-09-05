using System;
using authservice.Data;
namespace LayerBusnessLogic;

public class ClsPerson
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    private ClsPerson(string userName, string email, string phoneNumber, string address)
    {
        this.UserName = userName;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.Address = address;
    }

    public ClsPerson()
    {
        this.UserName = string.Empty;
        this.Email = string.Empty;
        this.PhoneNumber = string.Empty;
        this.Address = string.Empty;
    }



    private bool GetUser(int userId, ClsPersrsonnDTO user)
    {
        return ClsPesrsonnDataAccess.GetUser(userId, ref user);
    }

    public static ClsPerson Find(int IdUser)
    {
        string userName = string.Empty;
        string email = string.Empty;
        string phoneNumber = string.Empty;
        string address = string.Empty;

        ClsPersrsonnDTO user = new ClsPersrsonnDTO(userName, email, phoneNumber, address);

        bool userFound = ClsPesrsonnDataAccess.GetUser(IdUser, ref user);

        if (userFound)
        {
            return new ClsPerson(user.UserName, user.Email, user.PhoneNumber, user.Address);
        }

        return null; // User not found
    }

}
