using Autofac;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Core.UnitOfWorks;
using MovieRestful.Repository;
using MovieRestful.Repository.Repositories;
using MovieRestful.Repository.UnitOfWorks;
using MovieRestful.Service.Mapping;
using MovieRestful.Service.Redis;
using MovieRestful.Service.Services;
using System.Reflection;
using Module = Autofac.Module;// Module leri karıştırdığı için

namespace MovieRestful.API.Modules
{
    public class RepoServiceModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope(); //Generic classlarda 
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            //builder.RegisterType<RedisHelper>().As<IRedisHelper>();

            var apiAssembly=Assembly.GetExecutingAssembly(); // üzerinde çalışılan assembly
            var repoAssembly = Assembly.GetAssembly(typeof(DatabaseContext)); // Repository katmanındaki herhangi bi class verilebilir. 
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); // o class üzerinden bulunduğu yeri alır.

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")) // Belittiğim yerlerde sonu Repository olanları al
                .AsImplementedInterfaces() // interfacelerini de implemente et
                .InstancePerLifetimeScope(); // AddScoped


            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
