namespace JobPortal.Api.Services.Foundations.Applications
{
    public partial class ApplicationService
    {
        private void ValidateApplicationId(int id)
        {
            Validate(
                (Rule: IsInvalid(id), Parameter: nameof(Application.Id))
            );
        }

        private void ValidateApplicationOnCreate(Application application)
        {
            ValidateApplicationNotNull(application);

            Validate(
                (Rule: IsInvalid(application.UserId), Parameter: nameof(Application.UserId)),
                (Rule: IsInvalid(application.JobPostId), Parameter: nameof(Application.JobPostId)),
                (Rule: IsInvalid(application.ResumePath), Parameter: nameof(Application.ResumePath)),
                (Rule: IsInvalidApplicationStatus(application.Status), Parameter: nameof(Application.Status)),
                (Rule: IsInvalidDate(application.AppliedAt), Parameter: nameof(Application.AppliedAt))
            );
        }

        private void ValidateApplicationOnModify(Application application)
        {
            ValidateApplicationNotNull(application);

            Validate(
                (Rule: IsInvalid(application.Id), Parameter: nameof(Application.Id)),
                (Rule: IsInvalid(application.UserId), Parameter: nameof(Application.UserId)),
                (Rule: IsInvalid(application.JobPostId), Parameter: nameof(Application.JobPostId)),
                (Rule: IsInvalid(application.ResumePath), Parameter: nameof(Application.ResumePath)),
                (Rule: IsInvalidApplicationStatus(application.Status), Parameter: nameof(Application.Status)),
                (Rule: IsInvalidDate(application.AppliedAt), Parameter: nameof(Application.AppliedAt))
            );
        }

        private static void ValidateApplicationExists(Application maybeApplication, int applicationId)
        {
            if (maybeApplication is null)
            {
                throw new NotFoundApplicationException(applicationId);
            }
        }

        private void ValidateApplicationNotNull(Application application)
        {
            if (application is null)
            {
                throw new NullApplicationException();
            }
        }

        private static dynamic IsInvalid(int id) => new
        {
            Condition = id <= 0,
            Message = "Id must be greater than zero."
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required."
        };

        private static dynamic IsInvalidApplicationStatus(ApplicationStatus status) => new
        {
            Condition = !Enum.IsDefined(typeof(ApplicationStatus), status),
            Message = "Status is invalid."
        };

        private static dynamic IsInvalidDate(DateTime date) => new
        {
            Condition = date == default,
            Message = "Date is required and must be valid."
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidApplicationException = new InvalidApplicationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidApplicationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidApplicationException.ThrowIfContainsErrors();
        }
    }
}
