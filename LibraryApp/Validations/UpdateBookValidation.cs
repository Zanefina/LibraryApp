using FluentValidation;
using LibraryApp.Models.DTO;

namespace LibraryApp.Validations
{
    public class UpdateBookValidation : AbstractValidator<UpdateBookDTO>
    {
        public UpdateBookValidation()
        {
            RuleFor(model => model.BookId).NotEmpty().GreaterThan(0);
            RuleFor(model => model.Title).NotEmpty();
            RuleFor(model => model.Author).NotEmpty();
            RuleFor(model => model.PublishedOn).NotEmpty().Must(ValidDate); ;
            RuleFor(model => model.Genre).NotEmpty().MaximumLength(30);
            RuleFor(model => model.Description).NotEmpty().MaximumLength(500);
            RuleFor(model => model.Availability).NotNull();

        }
        private bool ValidDate(DateTime? date)
        {

            return date != null && date >= DateTime.MinValue && date <= DateTime.MaxValue;
        }
    }
}
