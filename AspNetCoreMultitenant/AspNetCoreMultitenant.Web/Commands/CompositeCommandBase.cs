using System.Collections.Generic;

namespace AspNetCoreMultitenant.Web.Commands
{
    public class CompositeCommandBase<T> : ICommand<T>
    {
        protected IList<ICommand<T>> Children { get; private set; }

        public CompositeCommandBase()
        {
            Children = new List<ICommand<T>>();
        }

        public void Execute(T parameter)
        {
            foreach(var command in Children)
            {
                command.Execute(parameter);
            }
        }
    }
}
