namespace JobPortal.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Resume> InsertResumeAsync(Resume resume);
        IQueryable<Resume> SelectAllResumes();
        ValueTask<Resume> SelectResumeByIdAsync(int resumeId);
        ValueTask<Resume> UpdateResumeAsync(Resume resume);
        ValueTask<Resume> DeleteResumeAsync(Resume resume);
        ValueTask<Resume?> SelectResumeBySeekerIdAsync(int userId);
        ValueTask<Resume?> SelectResumeByIdAndSeekerIdAsync(int resumeId, int userId);

    }
}
