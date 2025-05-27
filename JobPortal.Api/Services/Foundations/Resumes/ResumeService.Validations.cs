namespace JobPortal.Api.Services.Foundations.Resumes
{
    public partial class ResumeService
    {
        private void ValidateResumeId(int id)
        {
            Validate((Rule: IsInvalid(id), Parameter: nameof(Resume.Id)));
        }

        private void ValidateResumeOnAdd(Resume resume)
        {
            ValidateResumeNotNull(resume);

            Validate(
                (Rule: IsInvalid(resume.FullName), Parameter: nameof(Resume.FullName)),
                (Rule: IsInvalid(resume.Email), Parameter: nameof(Resume.Email)),
                (Rule: IsInvalid(resume.PhoneNumber), Parameter: nameof(Resume.PhoneNumber)),
                (Rule: IsInvalid(resume.Skills), Parameter: nameof(Resume.Skills)),
                (Rule: IsInvalid(resume.Experience), Parameter: nameof(Resume.Experience))
            );
        }

        private void ValidateResumeOnModify(Resume resume)
        {
            ValidateResumeNotNull(resume);

            Validate(
                (Rule: IsInvalid(resume.Id), Parameter: nameof(Resume.Id)),
                (Rule: IsInvalid(resume.FullName), Parameter: nameof(Resume.FullName)),
                (Rule: IsInvalid(resume.Email), Parameter: nameof(Resume.Email)),
                (Rule: IsInvalid(resume.PhoneNumber), Parameter: nameof(Resume.PhoneNumber)),
                (Rule: IsInvalid(resume.Skills), Parameter: nameof(Resume.Skills)),
                (Rule: IsInvalid(resume.Experience), Parameter: nameof(Resume.Experience))
            );
        }

        private static void ValidateAgainstStorageResumeOnModify(
            Resume inputResume,
            Resume storageResume)
        {
            ValidateStorageResume(storageResume, inputResume.Id);

            Validate(
                (Rule: IsNotSame(inputResume.Id, storageResume.Id, nameof(Resume.Id)),
                 Parameter: nameof(Resume.Id))
            );
        }

        private static void ValidateStorageResume(
            Resume maybeResume, int resumeId)
        {
            if (maybeResume is null)
            {
                throw new NotFoundResumeException(resumeId);
            }
        }

        private void ValidateResumeNotNull(Resume resume)
        {
            if (resume is null)
            {
                throw new NullResumeException();
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

        private static dynamic IsNotSame(int firstId, int secondId, string parameterName) => new
        {
            Condition = firstId != secondId,
            Message = $"Resume {parameterName} does not match."
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidResumeException = new InvalidResumeException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidResumeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidResumeException.ThrowIfContainsErrors();
        }
    }
}
