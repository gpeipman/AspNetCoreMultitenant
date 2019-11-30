namespace AspNetCoreMultitenant.Web.Commands
{
    public interface ICommand<T>
    {
        void Execute(T parameter);
    }
}