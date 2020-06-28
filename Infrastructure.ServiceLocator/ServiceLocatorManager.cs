// <copyright file="ServiceLocatorManager.cs" company="Beckman Coulter">
// Copyright (c) Beckman Coulter. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Authoring.Infrastructure.ServiceLocator
{
    /// <summary>
    /// The creator and maintainer of instances of <see cref="IServiceLocator"/>.
    /// </summary>
    public class ServiceLocatorManager : IDisposable
    {
        /// <summary>
        /// The service provider.
        /// </summary>
        private IServiceProvider rootServiceProvider;

        /// <summary>
        /// The singleton <see cref="ServiceLocatorManager"/> instance.
        /// </summary>
        private static ServiceLocatorManager instance;

        /// <summary>
        /// The dictionary of <see cref="ServiceLocator"/>.
        /// </summary>
        private readonly ConcurrentDictionary<int, ServiceLocator> serviceLocators = new ConcurrentDictionary<int, ServiceLocator>();

        /// <summary>
        /// The global service locator.
        /// </summary>
        private ServiceLocator globalServiceLocator;

        /// <summary>
        /// The global service scope id.
        /// </summary>
        public const int GlobalServiceScopeId = int.MinValue;

        /// <summary>
        /// Flag to indicate whether Dispose method has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The delegate to create a new <see cref="IServiceScope"/> instance from <see cref="IServiceProvider"/>.
        /// </summary>
        internal Func<IServiceProvider, IServiceScope> CreateScopeDelegate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorManager"/> class.
        /// </summary>
        private ServiceLocatorManager()
        {
            CreateScopeDelegate = sp => sp.CreateScope();
        }

        /// <summary>
        /// Check if the instance is initialized or not.
        /// </summary>
        public bool IsInitialized => rootServiceProvider != null;

        /// <summary>
        /// The singleton <see cref="ServiceLocatorManager"/> instance.
        /// </summary>
        public static ServiceLocatorManager Instance => instance ?? (instance = new ServiceLocatorManager());

        /// <summary>
        /// The global service locator with the same lifetime as application.
        /// </summary>
        public IServiceLocator GlobalServiceLocator => globalServiceLocator ?? (globalServiceLocator = (ServiceLocator)GetServiceLocator(GlobalServiceScopeId));

        /// <summary>
        /// Set the service provider. It must be called when application startup.
        /// </summary>
        /// <param name="sp">The service provider.</param>
        public void Initialize(IServiceProvider sp)
        {
            rootServiceProvider = sp;
        }

        /// <summary>
        /// Get/Add a instance of <see cref="IServiceLocator"/>.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns>A instance of <see cref="IServiceLocator"/>.</returns>
        public IServiceLocator GetServiceLocator(int scopeId)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Initialize method must be called before calling GetServiceLocator");
            }

            return serviceLocators.GetOrAdd(scopeId, key => new ServiceLocator(scopeId, CreateScopeDelegate(rootServiceProvider)));
        }

        /// <summary>
        /// Get/Add a instance of <see cref="IServiceLocator"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="IServiceLocator"/>.</returns>
        public IServiceLocator GetServiceLocator()
        {
            var id = GenerateNewScopeId();
            var locator = GetServiceLocator(id);
            return locator;
        }

        /// <summary>
        /// Remove a <see cref="IServiceLocator"/> instance from system.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        public void RemoveServiceLocator(int scopeId)
        {
            if (scopeId == GlobalServiceScopeId)
            {
                // The global service locator cannot be removed.
                return;
            }

            if (serviceLocators.TryRemove(scopeId, out var current))
            {
                current?.Dispose();
            }
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Indicates whether to free managed objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                var scopeIds = new List<int>(serviceLocators.Keys);
                foreach (var id in scopeIds)
                {
                    RemoveServiceLocator(id);
                }
            }

            disposed = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ServiceLocatorManager"/> class.
        /// </summary>
        ~ServiceLocatorManager()
        {
            Instance.Dispose(false);
        }

        private int GenerateNewScopeId()
        {
            var id = new Random(Guid.NewGuid().GetHashCode()).Next();
            while (serviceLocators.ContainsKey(id))
            {
                id = new Random(Guid.NewGuid().GetHashCode()).Next();
            }

            return id;
        }
    }
}