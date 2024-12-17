using AdApp.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdApp.Core.Validators
{
    public class AdValidator : AbstractValidator<Ad>
    {
        public AdValidator()
        {
            // Validate that the title is not empty and has at least 3 characters
            RuleFor(ad => ad.title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.");

            // Validate that the content is not empty and has at least 10 characters
            RuleFor(ad => ad.description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.");

            // Validate that the category is not empty
            RuleFor(ad => ad.category)
                .NotEmpty().WithMessage("Category is required.");

            //// Validate that the contact email is not empty and has a valid email format
            //RuleFor(ad => ad.ContactEmail)
            //    .NotEmpty().WithMessage("Contact email is required.")
            //    .EmailAddress().WithMessage("Invalid email format.");

            //// Validate that the ad has a valid expiration date in the future
            //RuleFor(ad => ad.ExpirationDate)
            //    .GreaterThan(DateTime.Now).WithMessage("Expiration date must be in the future.");

        }
    }
}
