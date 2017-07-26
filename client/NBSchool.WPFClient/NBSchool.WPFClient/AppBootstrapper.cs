﻿using Caliburn.Micro;
using NBSchool.Interface;
using NBSchool.Library;
using NBSchool.WPFClient.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NBSchool.WPFClient
{

    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void BuildUp(object instance)
        {
            this.container.SatisfyImportsOnce(instance);
        }

        /// <summary>
        ///     By default, we are configured to use MEF
        /// </summary>
        protected override void Configure()
        {
            try
            {
                foreach (var file in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), "NBSchool.*.dll"))
                {
                    AssemblySource.Instance.Add(Assembly.LoadFile(file));
                }
                var catalog = new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>());

                this.container = new CompositionContainer(catalog);

                var batch = new CompositionBatch();

                batch.AddExportedValue<IWindowManager>(new WindowManager());
                batch.AddExportedValue<IEventAggregator>(new EventAggregator());
                batch.AddExportedValue(this.container);
                batch.AddExportedValue(catalog);

                this.container.Compose(batch);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            try
            {
                var exports = this.container.GetExportedValues<object>(contract);

                if (exports.Any())
                {
                    return exports.First();
                }
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                var startupTasks =
                    GetAllInstances(typeof(StartupTask))
                    .Cast<ExportedDelegate>()
                    .Select(exportedDelegate => (StartupTask)exportedDelegate.CreateDelegate(typeof(StartupTask)));

                startupTasks.Apply(s => s());

                DisplayRootViewFor<ILogin>();

            }
            catch (Exception ex)
            {
                Log.Error("AppBootstrapper  异常！", ex);
            }
            //
        }
    }
}
