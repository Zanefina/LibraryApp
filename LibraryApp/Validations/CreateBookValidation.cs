using FluentValidation;
using LibraryApp.Models.DTO;

namespace LibraryApp.Validations
{
    public class CreateBookValidation : AbstractValidator<CreateBookDTO>
    {
        public CreateBookValidation() 
        {
            RuleFor(model => model.Title).NotEmpty().WithMessage("Please specify a title");
            RuleFor(model => model.Author).NotEmpty().WithMessage("Please specify an author");
            RuleFor(model => model.PublishedOn).NotEmpty().Must(ValidDate).WithMessage("Please enter a valid date.");
            RuleFor(model => model.Genre).NotEmpty().MaximumLength(30).WithMessage("Maximum length of a Gender should be no more than 30 characters.");
            RuleFor(model => model.Description).NotEmpty().MaximumLength(500).WithMessage("Max length of description should not be more than 500 characters");
            RuleFor(model => model.Availability).NotNull();

        }
        private bool ValidDate(DateTime? date)
        {
            
            return date != null && date >= DateTime.MinValue && date <= DateTime.MaxValue;
        }
    }
}
