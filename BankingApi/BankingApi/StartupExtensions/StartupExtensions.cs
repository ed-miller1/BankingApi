using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;

namespace BankingApi.Utilities
{
    public static class StartupExtensions
    {
        public static void AddMediatrToProject(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = executingAssembly.GetReferencedAssemblies().ToList();
            var assemblies = new List<Assembly> { executingAssembly };
            assemblies.AddRange(from x in referencedAssemblies let atype = x.GetType() where !string.IsNullOrWhiteSpace(x.Name) select Assembly.Load(x.Name));
            services.AddMediatR(assemblies.ToArray());
        }
    }
}
