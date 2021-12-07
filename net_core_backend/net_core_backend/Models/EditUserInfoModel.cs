using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class EditUserInfoModel
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Birthdate { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String PostalCode { get; set; }
        public String Country { get; set; }

        public EditUserInfoModel()
        {

        }

        public EditUserInfoModel(Users user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            if (user.Birthdate.Month < 10)
            {
                this.Birthdate = $"{user.Birthdate.Year}-0{user.Birthdate.Month}-{user.Birthdate.Day}";
            }
            else
            {
                this.Birthdate = $"{user.Birthdate.Year}-{user.Birthdate.Month}-{user.Birthdate.Day}";
            }
            this.Address = user.Address;
            this.City = user.City;
            this.PostalCode = user.PostalCode;
            this.Country = user.Country;
        }
    }
}
