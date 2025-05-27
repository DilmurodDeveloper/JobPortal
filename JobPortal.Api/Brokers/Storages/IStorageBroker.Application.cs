namespace JobPortal.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Application> InsertApplicationAsync(Application application);
        IQueryable<Application> SelectAllApplications();
        ValueTask<Application?> SelectApplicationByIdAsync(int applicationId);
        ValueTask<Application> UpdateApplicationAsync(Application application);
        ValueTask<Application> DeleteApplicationAsync(Application application);
    }
}
