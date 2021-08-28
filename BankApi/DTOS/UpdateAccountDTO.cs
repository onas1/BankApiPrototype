using System;
using System.ComponentModel.DataAnnotations;

namespace BankApi.DTOS
{
    public class UpdateAccountDTO
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be more than four digits.")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pin does not match.")]
        public string ConfirmPin { get; set; }
        public DateTime DateLastUpdated { get; set; }

    }
}
