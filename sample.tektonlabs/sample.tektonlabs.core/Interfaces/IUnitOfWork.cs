namespace sample.tektonlabs.core.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}

