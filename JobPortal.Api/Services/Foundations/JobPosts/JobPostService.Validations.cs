namespace JobPortal.Api.Services.Foundations.JobPosts
{
    public partial class JobPostService
    {
        private void ValidateJobPostId(int id)
        {
            Validate((Rule: IsInvalid(id), Parameter: nameof(JobPost.Id)));
        }

        private void ValidateJobPostOnAdd(JobPost jobPost)
        {
            ValidateJobPostNotNull(jobPost);

            Validate(
                (Rule: IsInvalid(jobPost.Title), Parameter: nameof(JobPost.Title)),
                (Rule: IsInvalid(jobPost.Description), Parameter: nameof(JobPost.Description)),
                (Rule: IsInvalid(jobPost.Company), Parameter: nameof(JobPost.Company)),
                (Rule: IsInvalid(jobPost.Location), Parameter: nameof(JobPost.Location)),
                (Rule: IsInvalidSalary(jobPost.Salary), Parameter: nameof(JobPost.Salary)),
                (Rule: IsInvalid(jobPost.UserId), Parameter: nameof(JobPost.UserId))
            );
        }

        private void ValidateJobPostOnModify(JobPost jobPost)
        {
            ValidateJobPostNotNull(jobPost);

            Validate(
                (Rule: IsInvalid(jobPost.Id), Parameter: nameof(JobPost.Id)),
                (Rule: IsInvalid(jobPost.Title), Parameter: nameof(JobPost.Title)),
                (Rule: IsInvalid(jobPost.Description), Parameter: nameof(JobPost.Description)),
                (Rule: IsInvalid(jobPost.Company), Parameter: nameof(JobPost.Company)),
                (Rule: IsInvalid(jobPost.Location), Parameter: nameof(JobPost.Location)),
                (Rule: IsInvalidSalary(jobPost.Salary), Parameter: nameof(JobPost.Salary)),
                (Rule: IsInvalid(jobPost.UserId), Parameter: nameof(JobPost.UserId))
            );
        }

        private static void ValidateAgainstStorageJobPostOnModify(
            JobPost inputJobPost,
            JobPost storageJobPost)
        {
            ValidateStorageJobPost(storageJobPost, inputJobPost.Id);

            Validate(
                (Rule: IsNotSame(inputJobPost.Id, storageJobPost.Id, nameof(JobPost.Id)),
                 Parameter: nameof(JobPost.Id))
            );
        }

        private static void ValidateStorageJobPost(
            JobPost maybeJobPost, int jobPostId)
        {
            if (maybeJobPost is null)
            {
                throw new NotFoundJobPostException(jobPostId);
            }
        }

        private void ValidateJobPostNotNull(Models.Foundations.JobPosts.JobPost jobPost)
        {
            if (jobPost is null)
            {
                throw new NullJobPostException();
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

        private static dynamic IsInvalidSalary(decimal salary) => new
        {
            Condition = salary < 0,
            Message = "Salary cannot be negative."
        };

        private static dynamic IsNotSame(int firstId, int secondId, string parameterName) => new
        {
            Condition = firstId != secondId,
            Message = $"JobPost {parameterName} does not match."
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidJobPostException = new InvalidJobPostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidJobPostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidJobPostException.ThrowIfContainsErrors();
        }
    }
}
